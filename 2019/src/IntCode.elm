module IntCode exposing (..)

import Helpers exposing (nums)

import Array exposing (Array, empty, fromList, map, toList, set)
import Maybe
import String exposing (toInt, fromInt, right, dropRight, split)
import List exposing (head, tail, append, length)

type alias IntCode = { 
        program: String, memory: Array Int, 
        input: List Int, output: List Int,
        addr: Int, isDone: Bool, error: String
    }

type ParameterMode = Position | Immediate
type alias Parameter = (Int, ParameterMode)
type Instruction = 
    Add (Parameter, Parameter, Parameter) | Mult (Parameter, Parameter, Parameter) | 
    JumpIfTrue (Parameter, Parameter) | JumpIfFalse (Parameter, Parameter) |
    LessThan (Parameter, Parameter, Parameter) | Equals (Parameter, Parameter, Parameter) |
    Input Parameter | Output Parameter | Halt | Error String 

initIntCode : String -> List Int -> IntCode
initIntCode program inputs = {
    program = program, memory = (nums program |> fromList),
    input = inputs, output = [], addr = 0, isDone = False, error = ""
    }

addInput : Int -> IntCode -> IntCode
addInput next ({input} as intCode) = { intCode | input = (append input [next])}

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
            memory = set (getVal o memory) ((getVal a memory) + (getVal b memory)) memory,
            addr = addr + 4
            }
        Mult (a, b, o) -> { intCode |
            memory = set (getVal o memory) ((getVal a memory) * (getVal b memory)) memory,
            addr = addr + 4
            }
        Input p -> { intCode | 
            memory = set (getVal p memory) (may <| head input) memory, 
            addr = addr + 2,
            input = Maybe.withDefault [] (tail input)
            }
        Output p -> { intCode | 
            output = List.append output [getVal p memory],
            addr = addr + 2
            }
        JumpIfTrue (a, b) -> { intCode |
            addr = if (getVal a memory) /= 0 then getVal b memory else addr + 3
            }
        JumpIfFalse (a, b) -> { intCode |
            addr = if (getVal a memory) == 0 then getVal b memory else addr + 3
            }
        LessThan (a, b, o) -> { intCode |
            memory = set (getVal o memory) (if (getVal a memory) < (getVal b memory) then 1 else 0) memory,
            addr = addr + 4
            }
        Equals (a, b, o) -> { intCode |
            memory = set (getVal o memory) (if (getVal a memory) == (getVal b memory) then 1 else 0) memory,
            addr = addr + 4
            }
        Halt -> { intCode | isDone = True }
        Error str -> { intCode | isDone = True, error = str}


getCurrentInstruction : IntCode -> Instruction
getCurrentInstruction {memory, addr} = 
    let 
        curStr = get addr memory |> fromInt
        top = right 2 curStr
        op = if String.length top == 2 then top else "0" ++ top
        modes = dropRight 2 curStr |> split "" |> List.reverse |> fromList
    in 
        case op of
            "01" -> Add (
                (get (addr + 1) memory, getMode 0 modes),
                (get (addr + 2) memory, getMode 1 modes),
                (get (addr + 3) memory, Immediate))
            "02" -> Mult (
                (get (addr + 1) memory, getMode 0 modes),
                (get (addr + 2) memory, getMode 1 modes),
                (get (addr + 3) memory, Immediate))
            "03" -> Input (get (addr + 1) memory, Immediate)
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
                (get (addr + 3) memory, Immediate))
            "08" -> Equals (
                (get (addr + 1) memory, getMode 0 modes),
                (get (addr + 2) memory, getMode 1 modes),
                (get (addr + 3) memory, Immediate))
            "99" -> Halt
            _ -> Error curStr


getVal : Parameter -> Array Int -> Int
getVal (val, mode) memory = case mode of
    Position -> get val memory
    Immediate -> val

getMode : Int -> Array String -> ParameterMode
getMode index modes = 
    case Array.get index modes |> Maybe.withDefault "0" of 
        "0" -> Position
        "1" -> Immediate
        _ -> Position


-- helpers

get : Int -> Array Int -> Int
get index array = may <| Array.get index array

may : Maybe Int -> Int
may = Maybe.withDefault 0
