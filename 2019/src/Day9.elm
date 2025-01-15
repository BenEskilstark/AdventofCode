module Day9 exposing (..)

import Helpers exposing (nums)
import UI
import IntCode exposing (
    IntCode, Instruction, run, step, getCurrentInstruction, 
    initIntCode, setInput
    )

import Browser
import Html exposing (div, text, span, h3, Html, textarea, input)
import Html.Attributes exposing (style, value)
import Html.Events exposing (onInput)
import Maybe
import Debug exposing (toString)
import String exposing (fromInt)
import Dict

may : Maybe Int -> Int
may = Maybe.withDefault 0


main : Program () IntCode Msg
main = Browser.sandbox { init = init, view = view, update = update }

type Msg = SetProgram String | SetInput String |
    StepProgram | RunProgram

init : IntCode
init = initIntCode "" []

update : Msg -> IntCode -> IntCode
update msg ({isDone} as intCode) = case msg of 
    SetProgram str -> initIntCode str []
    SetInput str -> setInput (nums str) intCode
    StepProgram -> if isDone then intCode else step intCode
    RunProgram -> run intCode

view : IntCode -> Html Msg
view ({addr, program, isDone, memory, output} as intCode) = UI.fullscreen
    <| UI.centered
    <| UI.column [
        UI.card (UI.title "Int Code Emulator Day 9"),
        UI.card (span [] [
            h3 [ style "margin" "0"] [ text "Program Source: "],
            textarea [ onInput SetProgram, style "width" "500px", style "height" "100px" ] [],
            div [] [
                text "Input: ",
                input [ 
                    value (List.map fromInt intCode.input |> String.join ","), 
                    onInput SetInput, style "width" "80px"
                ] []
            ]
        ]),
        UI.optionCard [
            UI.Clickable "Reset" (SetProgram program) UI.Reject,
            UI.Clickable "Step" StepProgram UI.Default,
            UI.Clickable "Run" RunProgram UI.Default
        ] ( div [] [
            div [] [ h3 [ style "margin" "0" ] [ text "Program State: " ] ],
            div [] [ text ("Execution Pointer: " ++ (String.fromInt addr))],
            div [] [ text ("Relative Base: " ++ (String.fromInt intCode.relativeBase))],
            viewInstruction (getCurrentInstruction intCode) intCode,
            div [] [ text ("Output: " ++ if isDone then " (DONE)" else "")],
            div [] [ text (String.join ", " (List.map fromInt output)) ],
            div [] [ text (if intCode.error == "" then "" else "Error: " ++ intCode.error)]
        ]),
        UI.card (div [style "overflow-wrap" "break-word"] [
            div [] [ text "Program memory: "],
            div [] [ text (toString (Dict.values memory)) ]
        ])
    ]

viewInstruction : Instruction -> IntCode -> Html Msg
viewInstruction ins _ = case ins of 
    IntCode.Add (a, b, o) -> div [] [ text "Add: ", viewParam a, viewParam b, viewParam o ]
    IntCode.Mult (a, b, o) -> div [] [ text "Mult: " , viewParam a, viewParam b, viewParam o ]
    IntCode.Input p -> div [] [ text "Input To: ", viewParam p]
    IntCode.Output p -> div [] [ text "Output From: ", viewParam p]
    IntCode.JumpIfTrue (a, b) -> div [] [ text "JumpIfTrue: ", viewParam a, viewParam b]
    IntCode.JumpIfFalse (a, b) -> div [] [ text "JumpIfFalse: ", viewParam a, viewParam b]
    IntCode.LessThan (a, b, o) -> div [] [ text "LessThan: ", viewParam a, viewParam b, viewParam o ]
    IntCode.Equals (a, b, o) -> div [] [ text "Equals: ", viewParam a, viewParam b, viewParam o ]
    IntCode.SetRelativeBase a -> div [] [ text "Set Relative Base: ", viewParam a]
    IntCode.Halt -> div [] [ text "Halt" ]
    IntCode.Error str -> div [] [ text ("Error " ++ str)]

viewParam : IntCode.Parameter -> Html Msg
viewParam (val, mode) = case mode of 
    IntCode.Position -> text (" (Position: " ++ (fromInt val) ++ ")")
    IntCode.Immediate -> text (" (Immediate: " ++ (fromInt val) ++ ")")
    IntCode.Relative -> text (" (Relative: " ++ (fromInt val) ++ ")")

