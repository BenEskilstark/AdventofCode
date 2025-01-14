module Day7 exposing (..)

import List exposing (map, partition, foldl, append)
import Tuple exposing (first)
import Debug exposing (toString)

solve : String -> (String, String)
solve input = (solve1 input, solve2 input)

solve2 : String -> String
solve2 input = ""

solve1 : String -> String
solve1 input = range 5 
    |> combos 
    |> toString


-- part 1 helpers

combos : List a -> List (List a)
combos lst = if lst == [] then [[]] else
    foldl (\ e res -> consAll e (combos (remove e lst)) |> append res) [] lst

remove : a -> List a -> List a
remove elem lst = first <| partition (\ e -> e /= elem) lst

consAll : a -> List (List a) -> List (List a)
consAll n lsts = map (\ lst -> n :: lst) lsts

range : Int -> List Int
range n = if n == 0 then [0] else
    n :: range (n - 1)