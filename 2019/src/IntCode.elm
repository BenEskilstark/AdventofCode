module IntCode exposing (..)

import Helpers exposing (nums)

import Array exposing (Array, fromList)
import Dict exposing (Dict, empty, insert)
import Maybe
import String exposing (fromInt, right, dropRight, split)
import List exposing (head, tail, append, length)

type alias IntCode = { 
        program: String, memory: Dict Int Int, 
        relativeBase: Int,
        input: List Int, output: List Int,
        addr: Int, isDone: Bool, error: String
    }

type ParameterMode = Position | Immediate | Relative
type alias Parameter = (Int, ParameterMode)
type Instruction = 
    Add (Parameter, Parameter, Parameter) | Mult (Parameter, Parameter, Parameter) | 
    JumpIfTrue (Parameter, Parameter) | JumpIfFalse (Parameter, Parameter) |
    LessThan (Parameter, Parameter, Parameter) | Equals (Parameter, Parameter, Parameter) |
    SetRelativeBase (Parameter) |
    Input Parameter | Output Parameter | Halt | Error String 

initIntCode : String -> List Int -> IntCode
initIntCode program inputs = {
    program = program, memory = (nums program |> fromList),
    relativeBase = 0,
    input = inputs, output = [], addr = 0, isDone = False, error = ""
    }

addInput : Int -> IntCode -> IntCode
addInput next ({input} as intCode) = { intCode | input = (append input [next])}

setInput : List Int -> IntCode -> IntCode
setInput nextInput intCode = { intCode | input = nextInput}

run : IntCode -> IntCode
run ({isDone} as intCode) = if isDone then intCode else run (step intCode)

runUntilOutput : IntCode -> IntCode
runUntilOutput ({output} as intCode) = runUntilOutputImpl (length output) intCode

runUntilOutputImpl : Int -> IntCode -> IntCode
runUntilOutputImpl outputLen ({output, isDone} as intCode) = 
    if (length output) > outputLen || isDone then
        intCode
    else runUntilOutputImpl outputLen (step intCode)

step : IntCode -> IntCode
step ({memory, addr, input, output} as intCode) =
    case getCurrentInstruction intCode of
        Add (a, b, o) -> { intCode |
            memory = insert (getInsertionVal o intCode) ((getVal a intCode) + (getVal b intCode)) memory,
            addr = addr + 4
            }
        Mult (a, b, o) -> { intCode |
            memory = insert (getInsertionVal o intCode) ((getVal a intCode) * (getVal b intCode)) memory,
            addr = addr + 4
            }
        Input p -> { intCode | 
            memory = insert (getInsertionVal p intCode) (may <| head input) memory, 
            addr = addr + 2,
            input = Maybe.withDefault [] (tail input)
            }
        Output p -> { intCode | 
            output = List.append output [getVal p intCode],
            addr = addr + 2
            }
        JumpIfTrue (a, b) -> { intCode |
            addr = if (getVal a intCode) /= 0 then getVal b intCode else addr + 3
            }
        JumpIfFalse (a, b) -> { intCode |
            addr = if (getVal a intCode) == 0 then getVal b intCode else addr + 3
            }
        LessThan (a, b, o) -> { intCode |
            memory = insert (getInsertionVal o intCode) (if (getVal a intCode) < (getVal b intCode) then 1 else 0) memory,
            addr = addr + 4
            }
        Equals (a, b, o) -> { intCode |
            memory = insert (getInsertionVal o intCode) (if (getVal a intCode) == (getVal b intCode) then 1 else 0) memory,
            addr = addr + 4
            }
        SetRelativeBase a -> { intCode |
            relativeBase = intCode.relativeBase + (getVal a intCode),
            addr = addr + 2
            }
        Halt -> { intCode | isDone = True }
        Error str -> { intCode | isDone = True, error = str}


getCurrentInstruction : IntCode -> Instruction
getCurrentInstruction {memory, addr} = 
    let 
        curStr = get addr memory |> fromInt
        top = right 2 curStr
        op = if String.length top == 2 then top else "0" ++ top
        modes = dropRight 2 curStr |> split "" |> List.reverse |> Array.fromList
    in 
        case op of
            "01" -> Add (
                (get (addr + 1) memory, getMode 0 modes),
                (get (addr + 2) memory, getMode 1 modes),
                (get (addr + 3) memory, getMode 2 modes))
            "02" -> Mult (
                (get (addr + 1) memory, getMode 0 modes),
                (get (addr + 2) memory, getMode 1 modes),
                (get (addr + 3) memory, getMode 2 modes))
            "03" -> Input (get (addr + 1) memory, getMode 0 modes)
            "04" -> Output (get (addr + 1) memory, getMode 0 modes)
            "05" -> JumpIfTrue (
                (get (addr + 1) memory, getMode 0 modes),
                (get (addr + 2) memory, getMode 1 modes))
            "06" -> JumpIfFalse (
                (get (addr + 1) memory, getMode 0 modes),
                (get (addr + 2) memory, getMode 1 modes))
            "07" -> LessThan (
                (get (addr + 1) memory, getMode 0 modes),
                (get (addr + 2) memory, getMode 1 modes),
                (get (addr + 3) memory, getMode 2 modes))
            "08" -> Equals (
                (get (addr + 1) memory, getMode 0 modes),
                (get (addr + 2) memory, getMode 1 modes),
                (get (addr + 3) memory, getMode 2 modes))
            "09" -> SetRelativeBase (get (addr + 1) memory, getMode 0 modes)
            "99" -> Halt
            _ -> Error curStr


getVal : Parameter -> IntCode -> Int
getVal (val, mode) {memory, relativeBase} = case mode of
    Position -> get val memory
    Immediate -> val
    Relative -> get (val + relativeBase) memory

getInsertionVal : Parameter -> IntCode -> Int
getInsertionVal (val, mode) {relativeBase} = case mode of
    Position -> val
    Immediate -> val
    Relative -> val + relativeBase

getMode : Int -> Array String -> ParameterMode
getMode index modes = 
    case Array.get index modes |> Maybe.withDefault "0" of 
        "0" -> Position
        "1" -> Immediate
        "2" -> Relative
        _ -> Position


-- helpers

fromList : List Int -> Dict Int Int 
fromList nums = fromListImpl 0 nums empty

fromListImpl : Int -> List Int -> Dict Int Int -> Dict Int Int
fromListImpl index nums dict = case nums of 
    [] -> dict
    n :: ns -> insert index n dict |> fromListImpl (index + 1) ns


get : Int -> Dict Int Int -> Int
get index array = may <| Dict.get index array

may : Maybe Int -> Int
may = Maybe.withDefault 0
