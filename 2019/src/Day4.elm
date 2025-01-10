module Day4 exposing (..)

import List exposing (map, foldl)
import String exposing (fromInt, split, toInt)
import Maybe exposing (withDefault)

solve : (String, String)
solve = (findAllValid validNum 367479 |> fromInt, findAllValid validNum2 367479 |> fromInt)

validNum2 : Int -> Bool
validNum2 num = 
    let 
        strs = fromInt num |> split "" 
        nums = fromInt num |> split "" |> map (\ c -> toInt c |> withDefault 0)
    in 
        hasAdjacent "" strs && neverDecrease 0 nums && has2Adjacent strs

has2Adjacent : List String -> Bool
has2Adjacent strs = sameAsPrev "" strs |> loneTrue False

loneTrue : Bool -> List Bool -> Bool
loneTrue prev bools = case bools of 
    [] -> False
    [_] -> False
    c :: nxt :: rst -> if prev == False && c == True && nxt == False then True
        else loneTrue c (nxt :: rst)

sameAsPrev : String -> List String -> List Bool
sameAsPrev cur strs = case strs of 
    [] -> [False]
    c :: rst -> if cur == c then True :: (sameAsPrev c rst) else False :: (sameAsPrev c rst)


findAllValid : (Int -> Bool) -> Int -> Int
findAllValid f num = case num of 
    893698 -> 0
    _ -> if f num then 1 + findAllValid f (num + 1) else findAllValid f (num + 1)

validNum : Int -> Bool
validNum num =
    let 
        strs = fromInt num |> split "" 
        nums = fromInt num |> split "" |> map (\ c -> toInt c |> withDefault 0)
    in 
        hasAdjacent "" strs && neverDecrease 0 nums

hasAdjacent : String -> List String -> Bool
hasAdjacent cur strs = case strs of 
    [] -> False
    c :: rst -> if cur == c then True else hasAdjacent c rst

neverDecrease : Int -> List Int -> Bool
neverDecrease cur nums = case nums of 
    [] -> True
    n :: rst -> if cur > n then False else neverDecrease n rst