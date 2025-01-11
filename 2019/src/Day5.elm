module Day5 exposing (..)

import Helpers exposing (nums)
import UI

import Browser
import Html exposing (div, text, span, h3, Html, textarea, input)
import Html.Attributes exposing (style, value)
import Html.Events exposing (onInput)
import Array exposing (Array, empty, fromList, map, toList, set)
import Maybe
import String exposing (toInt, fromInt, right, dropRight, split)

may : Maybe Int -> Int
may = Maybe.withDefault 0


main : Program () Model Msg
main = Browser.sandbox { init = init, view = view, update = update }

type alias Model = { 
        program: String, memory: Array Int, 
        input: Int, output: List Int,
        addr: Int, isDone: Bool, error: String
    }

type Msg = SetProgram String | SetInput String |
    StepProgram | RunProgram

type ParameterMode = Position | Immediate
type alias Parameter = (Int, ParameterMode)
type Instruction = 
    Add (Parameter, Parameter, Parameter) | Mult (Parameter, Parameter, Parameter) | 
    JumpIfTrue (Parameter, Parameter) | JumpIfFalse (Parameter, Parameter) |
    LessThan (Parameter, Parameter, Parameter) | Equals (Parameter, Parameter, Parameter) |
    Input Parameter | Output Parameter | Halt | Error String 

init : Model
init = Model "" empty 5 [] 0 False ""

view : Model -> Html Msg
view ({addr, program, isDone, memory, output} as model) = UI.fullscreen
    <| UI.centered
    <| UI.column [
        UI.card (UI.title "Int Code Emulator Day 5"),
        UI.card (span [] [
            h3 [ style "margin" "0"] [ text "Program Source: "],
            textarea [ onInput SetProgram, style "width" "500px", style "height" "100px" ] [],
            div [] [
                text "Input: ",
                input [ value (fromInt model.input), onInput SetInput, style "width" "80px"] []
            ]
        ]),
        UI.optionCard [
            UI.Clickable "Reset" (SetProgram program) UI.Reject,
            UI.Clickable "Step" StepProgram UI.Default,
            UI.Clickable "Run" RunProgram UI.Default
        ] ( div [] [
            div [] [ h3 [ style "margin" "0" ] [ text "Program State: " ] ],
            div [] [ text ("Execution Pointer: " ++ (String.fromInt addr))],
            viewInstruction (getCurrentInstruction model) model,
            div [] [ text ("Output: " ++ if isDone then " (DONE)" else "")],
            div [] [ text (String.join ", " (List.map fromInt output)) ],
            div [] [ text (if model.error == "" then "" else "Error: " ++ model.error)]
        ]),
        UI.card (div [] [
            div [] [ text "Program memory: "],
            div [] [ text (String.join ", " (toList (map fromInt memory))) ]
        ])
    ]

viewInstruction : Instruction -> Model -> Html Msg
viewInstruction ins _ = case ins of 
    Add (a, b, o) -> div [] [ text "Add: ", viewParam a, viewParam b, viewParam o ]
    Mult (a, b, o) -> div [] [ text "Mult: " , viewParam a, viewParam b, viewParam o ]
    Input p -> div [] [ text "Input To: ", viewParam p]
    Output p -> div [] [ text "Output From: ", viewParam p]
    JumpIfTrue (a, b) -> div [] [ text "JumpIfTrue: ", viewParam a, viewParam b]
    JumpIfFalse (a, b) -> div [] [ text "JumpIfFalse: ", viewParam a, viewParam b]
    LessThan (a, b, o) -> div [] [ text "LessThan: ", viewParam a, viewParam b, viewParam o ]
    Equals (a, b, o) -> div [] [ text "Equals: ", viewParam a, viewParam b, viewParam o ]
    Halt -> div [] [ text "Halt" ]
    Error str -> div [] [ text ("Error " ++ str)]

viewParam : Parameter -> Html Msg
viewParam (val, mode) = case mode of 
    Position -> text (" (Position: " ++ (fromInt val) ++ ")")
    Immediate -> text (" (Immediate: " ++ (fromInt val) ++ ")")


update : Msg -> Model -> Model
update msg ({program, input, isDone} as model) = case msg of 
    SetProgram str -> Model str (nums str |> fromList) input [] 0 False ""
    SetInput str -> Model program (nums program |> fromList) (may <| toInt str) [] 0 False ""
    StepProgram -> if isDone then model else step model
    RunProgram -> run model

run : Model -> Model
run ({isDone} as model) = if isDone then model else run (step model)

step : Model -> Model
step ({memory, addr, input, output} as model) =
    case getCurrentInstruction model of
        Add (a, b, o) -> { model |
            memory = set (getVal o memory) ((getVal a memory) + (getVal b memory)) memory,
            addr = addr + 4
            }
        Mult (a, b, o) -> { model |
            memory = set (getVal o memory) ((getVal a memory) * (getVal b memory)) memory,
            addr = addr + 4
            }
        Input p -> { model | 
            memory = set (getVal p memory) input memory, 
            addr = addr + 2
            }
        Output p -> { model | 
            output = List.append output [getVal p memory],
            addr = addr + 2
            }
        JumpIfTrue (a, b) -> { model |
            addr = if (getVal a memory) /= 0 then getVal b memory else addr + 3
            }
        JumpIfFalse (a, b) -> { model |
            addr = if (getVal a memory) == 0 then getVal b memory else addr + 3
            }
        LessThan (a, b, o) -> { model |
            memory = set (getVal o memory) (if (getVal a memory) < (getVal b memory) then 1 else 0) memory,
            addr = addr + 4
            }
        Equals (a, b, o) -> { model |
            memory = set (getVal o memory) (if (getVal a memory) == (getVal b memory) then 1 else 0) memory,
            addr = addr + 4
            }
        Halt -> { model | isDone = True }
        Error str -> { model | isDone = True, error = str}


getCurrentInstruction : Model -> Instruction
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

get : Int -> Array Int -> Int
get index array = may <| Array.get index array