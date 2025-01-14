module Day7 exposing (..)

import IntCode exposing (initIntCode, run, runUntilOutput, addInput, IntCode)

import String exposing (fromInt)
import List exposing (map, filter, foldl, append, head, maximum, all)
import Maybe exposing (withDefault)

solve : String -> (String, String)
solve input = (solve1 input, solve2 input)

-- part 2

solve2 : String -> String
solve2 program = 
    range 5 |> map (\ i -> i + 5) |> combos 
    |> map (\ combo -> computeCombo2 program combo)
    |> maximum |> withDefault 0
    |> fromInt

computeCombo2 : String -> List Int -> Int
computeCombo2  program combo = map (\ c -> initIntCode program [c]) combo
    |> loopIntCodes 0
    |> map (\ {output} -> last output)
    |> last 

loopIntCodes : Int -> List IntCode -> List IntCode
loopIntCodes inSignal intCodes = if all .isDone intCodes then intCodes else
    foldl (\ curIC prevICs -> 
        if prevICs == [] then 
            addInput inSignal curIC 
                |> runUntilOutput |> (\ c -> append prevICs [c])
        else
            addInput (lastIC prevICs |> .output |> last) curIC 
                |> runUntilOutput |> (\ c -> append prevICs [c])
    ) [] intCodes
    |> (\ nextIntCodes -> 
        loopIntCodes (lastIC nextIntCodes |> .output |> last) nextIntCodes) 


-- part 1

solve1 : String -> String
solve1 program = range 5 |> combos 
    |> map (\ combo -> computeCombo program combo)
    |> maximum |> withDefault 0
    |> fromInt

computeCombo : String -> List Int -> Int
computeCombo program combo = foldl 
    (\ c s -> 
        initIntCode program [c, s] |> run |> .output |> last
    ) 0 combo


-- helpers

lastIC : List IntCode -> IntCode
lastIC lst = case lst of
    [] -> initIntCode "" []
    [x] -> x
    _ :: rst -> lastIC rst

last : List Int -> Int
last lst = case lst of
    [] -> 0
    [x] -> x
    _ :: rst -> last rst

combos : List a -> List (List a)
combos lst = if lst == [] then [[]] else
    foldl (\ e res -> consAll e (combos (remove e lst)) |> append res) [] lst

remove : a -> List a -> List a
remove elem lst = filter (\ e -> e /= elem) lst

consAll : a -> List (List a) -> List (List a)
consAll n lsts = map (\ lst -> n :: lst) lsts

range : Int -> List Int
range n = rangeImpl 0 n

rangeImpl : Int -> Int -> List Int
rangeImpl i n = if i == n - 1 then [i] else
    i :: rangeImpl (i + 1) n