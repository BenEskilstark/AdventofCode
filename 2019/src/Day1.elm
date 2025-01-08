module Day1 exposing (..)

import String exposing (lines, toInt, fromInt)

solve: String -> (String, String)
solve input = (solve1 input, solve2 input)

solve1 : String -> String
solve1 input = lines input
    |> List.map (\ s -> Maybe.withDefault 0 (toInt s))
    |> List.map fuel
    |> List.sum
    |> fromInt

fuel : Int -> Int
fuel mass = max (floor (toFloat mass / 3) - 2) 0


solve2 : String -> String
solve2 input = lines input
    |> List.map (\ s -> Maybe.withDefault 0 (toInt s))
    |> List.map (\ mass -> recFuel (fuel mass))
    |> List.sum
    |> fromInt

recFuel : Int -> Int
recFuel mass = case mass of 
    0 -> mass
    _ -> mass + recFuel (fuel mass)

