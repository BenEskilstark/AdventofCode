module Day2 exposing (..)

import UI
import Helpers exposing (nums)

import Browser
import Html exposing (Html, div, span, textarea, h3, text)
import Html.Attributes exposing (style)
import Html.Events exposing (onInput)
import String exposing (fromInt)
import List exposing (map)


main : Program () Model Msg
main = Browser.sandbox { init = init, view = view, update = update }

type alias Model = { input: String, memory: List Int, addr: Int, done: Bool }
type Msg = SetInput String | StepProgram | RunProgram | DoPart2

init : Model
init = Model "" [] 0 False

update : Msg -> Model -> Model
update msg model = case msg of 
    SetInput str -> Model str (nums str) 0 False
    StepProgram -> step model
    RunProgram -> run model
    DoPart2 -> iterate 19690720 0 0 model

view : Model -> Html Msg
view {input, memory, addr, done} = UI.fullscreen
    <| UI.centered
    <| UI.column [
        UI.card (UI.title "Int Code Emulator"),
        UI.card (span [] [
            div []  [text "Paste Input: "],
            textarea [ onInput SetInput, style "width" "500px", style "height" "100px" ] []
        ]),
        UI.optionCard [
            UI.Clickable "Reset" (SetInput input) UI.Reject,
            UI.Clickable "Step" StepProgram UI.Default,
            UI.Clickable "Run" RunProgram UI.Default,
            UI.Clickable "Do Part 2" DoPart2 UI.Default
        ] ( div [] [
            div [] [ h3 [ style "margin" "0" ] [ text "Program State: " ] ],
            div [] [ text ("Execution Pointer: " ++ (String.fromInt addr))],
            div [] [ text ("Program memory: " ++ if done then " (DONE)" else "")],
            div [] [ text (String.join ", " (map fromInt memory)) ]
        ])
    ]

initialize : Int -> Int -> Model -> Model
initialize a b ({memory} as model) = 
    { model | memory = set 1 a memory |> set 2 b }

evaluate : Int -> Int -> Model -> Int
evaluate a b model = initialize a b model
    |> run 
    |> .memory 
    |> get 0

iterate : Int -> Int -> Int -> Model -> Model
iterate target a b model = if evaluate a b model == target then initialize a b model
    else (if b == 99 then iterate target (a + 1) 0 model else iterate target a (b + 1) model)

run : Model -> Model
run ({done} as model) = if done then model else run (step model)

step : Model -> Model
step ({memory, addr} as model) = let opcode = get addr memory in
    case opcode of 
        1 -> { model | memory = doOp addr sumOp memory, addr = addr + 4}
        2 -> { model | memory = doOp addr multOp memory, addr = addr + 4}
        99 -> { model | done = True}
        _ -> { model | memory = [-1], done = True} -- should be unreachable


doOp : Int -> (Int -> Int -> List Int -> Int) -> List Int -> List Int
doOp addr op memory = set (get (addr + 3) memory) (op (addr + 1) (addr + 2) memory) memory

sumOp : Int -> Int -> List Int -> Int
sumOp i j memory = (ptr i memory) + (ptr j memory)

multOp : Int -> Int -> List Int -> Int
multOp i j memory = (ptr i memory) * (ptr j memory)


ptr : Int -> List Int -> Int
ptr i lst = get (get i lst) lst

get : Int -> List Int -> Int
get i lst = case lst of 
    [] -> 99
    [x] -> if i == 0 then x else 99
    x :: xs -> if i == 0 then x else get (i - 1) xs

set : Int -> Int -> List Int -> List Int
set i val lst = case lst of 
    [] -> lst
    [_] -> if i == 0 then [val] else lst
    x :: xs -> if i == 0 then val :: xs else x :: (set (i - 1) val xs)