module Helpers exposing (..)

import Regex
import String exposing (toInt)

nums : String -> List Int
nums line = Regex.find numRegex line
    |> List.map .match
    |> List.map (\ s -> Maybe.withDefault 0 (toInt s)) 

numRegex : Regex.Regex
numRegex = Maybe.withDefault Regex.never
    <| Regex.fromString "-?\\d+"