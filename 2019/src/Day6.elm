module Day6 exposing (..)

import Dict exposing (Dict, empty, insert, keys, values)
import String exposing (String, lines, split, fromInt)
import List exposing (map, head, foldl, sum)
import Maybe exposing (withDefault)
import Set exposing (Set)

type alias ChildToParent = Dict String String
type alias DistChildToParent = Dict String Int
type alias Graph = Dict String (List String)
type alias Visited = Set String

solve : String -> (String, String)
solve input = (solve1 input, solve2 input)

solve2 : String -> String
solve2 input = linesToGraph input
    |> distToSAN "YOU" Set.empty
    |> Maybe.withDefault 999 -- random wrong value (for debugging)
    |> (\ res -> res - 2) -- problem doesn't count start/end
    |> fromInt


distToSAN : String -> Visited -> Graph -> Maybe Int
distToSAN cur visited graph = 
    let
        edges = Maybe.withDefault [] (Dict.get cur graph) 
    in
        if cur == "SAN" then Just 0
        else if Set.member cur visited then Nothing
        else foldl (\ e m -> 
                min (distToSAN e (Set.insert cur visited) graph) m
            ) Nothing edges
            |> Maybe.map (\ v -> v + 1)


linesToGraph : String -> Graph
linesToGraph input = input |> lines
    |> map (split ")")
    |> foldl (\ s dict -> -- add both ways
        addEdge (first s) (second s) dict |> addEdge (second s) (first s)
    ) empty

-- helpers

addEdge : String -> String -> Graph -> Graph
addEdge key val graph = 
    let 
        cur = Maybe.withDefault [] (Dict.get key graph)
    in
        insert key (val :: cur) graph

min : Maybe Int -> Maybe Int -> Maybe Int
min a b = case a of 
    Nothing -> Maybe.map (\ bi -> bi) b
    Just ai -> case b of 
        Nothing -> Just ai
        Just bi -> Just (Basics.min ai bi)



-- Part 1:

solve1 : String -> String
solve1 input = linesToChildToParent input
    |> populateDists 
    |> values
    |> sum 
    |> fromInt


populateDists : ChildToParent -> DistChildToParent
populateDists graph = keys graph 
    |> foldl (\ k dists -> distToCom k graph dists) empty


linesToChildToParent : String -> ChildToParent
linesToChildToParent input = input |> lines
    |> map (split ")")
    |> foldl (\ s dict -> insert (second s) (first s) dict) empty

distToCom : String -> ChildToParent -> DistChildToParent -> DistChildToParent
distToCom cur graph dists = 
    let 
        next = getS cur graph
    in
        case Dict.get cur dists of
            Just _ -> dists
            Nothing -> if next == "COM" 
                then insert cur 1 dists 
                else insert cur (1 + (getI next (distToCom next graph dists))) dists


-- helpers

getS : String -> ChildToParent -> String
getS key graph = Dict.get key graph |> withDefault ""

getI : String -> DistChildToParent -> Int
getI key graph = Dict.get key graph |> withDefault 0

first : List String -> String
first lst = head lst |> withDefault ""
second : List String -> String
second lst = case lst of
    [] -> ""
    _ :: [] -> ""
    _::v::_ -> v
