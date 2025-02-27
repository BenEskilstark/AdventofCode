module Main exposing (..)

import Browser
import UI
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (onInput)
import Tuple exposing (..)

import Day1
import Day3
import Day4
import Day6
import Day7
import Day8

main : Program () Model Msg
main = Browser.element { 
        init = init, view = view, update = update, subscriptions = subscriptions 
    }

type alias Model = { day: Int, input: String, result: (String, String)}
type Msg = SetInput String | SolveDay

init : () -> (Model, Cmd Msg)
init _ = (Model 9 "" ("", ""), Cmd.none)

subscriptions : Model -> Sub Msg
subscriptions _ = Sub.none 

update : Msg -> Model -> (Model, Cmd Msg)
update msg ({day, input} as model) = case msg of 
    SetInput str -> ({ model | input = str }, Cmd.none)
    SolveDay -> case day of 
        1 -> ({ model | result = Day1.solve input}, Cmd.none)
        3 -> ({ model | result = Day3.solve input}, Cmd.none)
        4 -> ({ model | result = Day4.solve}, Cmd.none)
        6 -> ({ model | result = Day6.solve input}, Cmd.none)
        7 -> ({ model | result = Day7.solve input}, Cmd.none)
        8 -> ({ model | result = Day8.solve input}, Cmd.none)
        _ -> (model, Cmd.none)
    

view : Model -> Html Msg
view {day, result} = UI.fullscreen
    <| UI.centered
    <| UI.column [
        UI.card (
            div [ style "width" "500px" ] 
                [text ("Advent of Code Day: " ++ (String.fromInt day))]
        ),
        UI.optionCard 
            [ UI.Clickable "Solve" SolveDay UI.Default ] 
            (span [] [
                div []  [text "Paste Input: "],
                textarea [ onInput SetInput, style "width" "500px", style "height" "200px" ] []
            ]),
        UI.card (
            div [ style "width" "500px", style "white-space" "pre" ] 
            [
                div [] [text "Part 1: ", viewPart (first result)],
                div [style "margin-top" "6px"] [text "Part 2: ", viewPart (second result)]
            ]
        )
    ]


viewPart : String -> Html Msg
viewPart resultPart = span [ style "width" "435px" ] [text resultPart]

