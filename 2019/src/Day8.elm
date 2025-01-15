module Day8 exposing (..)

import String exposing (left, dropLeft, indices, split, join)
import Debug exposing (toString)
import List exposing (sortBy, head, tail, length, map, foldl, repeat, take, drop)

solve : String -> (String, String)
solve input = (solve1 input, solve2 input)

type alias Layer = List String

imgWidth : Int
imgWidth = 25
imgHeight : Int
imgHeight = 6
imgSize : Int
imgSize = imgWidth * imgHeight

-- part 2

solve2 : String -> String
solve2 input = toLayers input 
    |> map (\ l -> split "" l)
    |> foldl (\ l m -> mergeLayers m l) (repeat imgSize "2")
    |> map (\ c -> if c == "0" then "_" else "O")
    |> (\ l -> "\n" ++ toImgString l)

toImgString : Layer -> String
toImgString layer = if layer == [] then "" else 
    ((take imgWidth layer) |> (join "")) ++ "\n" ++ ((drop imgWidth layer) |> toImgString)

mergeLayers : Layer -> Layer -> Layer
mergeLayers topLayer botLayer = if topLayer == [] || botLayer == [] then []
    else if first topLayer /= "2"
        then first topLayer :: mergeLayers (rest topLayer) (rest botLayer)
        else first botLayer :: mergeLayers (rest topLayer) (rest botLayer)

first : Layer -> String
first layer = head layer |> Maybe.withDefault "2"
rest : Layer -> Layer
rest layer = tail layer |> Maybe.withDefault []

-- part 1

solve1 : String -> String
solve1 input = toLayers input 
    |> sortBy (\ l -> indices "0" l |> length)
    |> head |> Maybe.withDefault ""
    |> (\ l -> (indices "1" l |> length) * (indices "2" l |> length))
    |> toString

toLayers : String -> List String
toLayers input = if input == "" then [] else
    left imgSize input :: toLayers (dropLeft imgSize input)


