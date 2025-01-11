module Day3 exposing (..)

import String exposing (left, dropLeft, split, lines, fromInt, toInt)
import Maybe exposing (withDefault)
import List exposing (head, tail, sortBy, map)
import Set exposing (intersect, toList, fromList, Set)

type alias Point = (Int, Int, Int)
type alias Dir = {dx: Int, dy: Int, dist: Int}


solve: String -> (String, String)
solve input = 
    let
        (line1, line2) = parseLines input
    in
        (solve1 line1 line2, solve2 line1 line2)

parseLines : String -> (String, String)
parseLines input = 
    let
        line1 = lines input 
            |> head |> withDefault ""
        line2 = lines input 
            |> tail |> withDefault [] 
            |> head |> withDefault ""
    in 
        (line1, line2)


solve2 : String -> String -> String
solve2 line1 line2 = 
    intersections2 line1 line2
    |> head |> withDefault (0, 0, 0)
    |> (\ (_, _, s) -> s)
    |> fromInt

    
intersections2 : String -> String -> List Point
intersections2 line1 line2 = intersect2
    (lineToDirs line1 |> enumAllDirs (0, 0, 0) |> fromList)
    (lineToDirs line2 |> enumAllDirs (0, 0, 0) |> fromList)
    |> toList
    |> sortBy (\ (_, _, s) -> s)

intersect2 : Set Point -> Set Point -> Set Point
intersect2 set1 set2 = 
    let 
        overlaps = intersect 
            (Set.map (\ (x, y, _) -> (x, y, 0)) set1)
            (Set.map (\ (x, y, _) -> (x, y, 0)) set2)
    in 
        Set.map (\ (x, y, _) -> (x, y, ((stepsAtPoint x y set1) + (stepsAtPoint x y set2)))) overlaps


stepsAtPoint : Int -> Int -> Set Point -> Int 
stepsAtPoint x y set = Set.filter (\ (x1, y1, _) -> x == x1 && y == y1) set
    |> toList
    |> sortBy (\ (_, _, s) -> s)
    |> head |> withDefault (0,0,0)
    |> (\ (_, _, s) -> s)


solve1 : String -> String -> String
solve1 line1 line2 = 
    intersections line1 line2
    |> head |> withDefault (0, 0, 0)
    |> (\ (x, y, _) -> abs x + abs y)
    |> fromInt

intersections : String -> String -> List Point
intersections line1 line2 = intersect
    (lineToDirs line1 |> enumAllDirs (0, 0, 0) |> map (\ (x, y, _) -> (x, y, 0)) |> fromList)
    (lineToDirs line2 |> enumAllDirs (0, 0, 0) |> map (\ (x, y, _) -> (x, y, 0)) |> fromList)
    |> toList
    |> sortBy (\ (x, y, _) -> abs x + abs y)


lineToDirs : String -> List Dir
lineToDirs line = split "," line |> map parseDir

enumAllDirs : Point -> List Dir -> List Point
enumAllDirs pos dirs = case dirs of
    [] -> []
    d :: ds -> (enumDir pos d) ++ (enumAllDirs (enumToPoint pos d) ds)


parseDir : String -> Dir
parseDir str = 
    let
        dir = left 1 str
        dist = dropLeft 1 str |> toInt |> Maybe.withDefault 0
    in 
        case dir of 
            "U" -> { dx = 0, dy = -1, dist = dist }
            "D" -> { dx = 0, dy = 1, dist = dist }
            "L" -> { dx = -1, dy = 0, dist = dist }
            "R" -> { dx = 1, dy = 0, dist = dist }
            _ -> { dx = 0, dy = 0, dist = 0 }


enumDir : Point -> Dir -> List Point
enumDir (x, y, s) ({dx, dy, dist} as dir) = 
    let 
        next = (x + dx, y + dy, s + 1)
    in
        if dist == 0 then [] else
        next :: enumDir next {dir | dist = dist - 1}

enumToPoint : Point -> Dir -> Point
enumToPoint (x, y, s) ({dx, dy, dist} as dir) = 
    let 
        next = (x + dx, y + dy, s + 1)
    in
        if dist == 0 then (x, y, s) else
        enumToPoint next {dir | dist = dist - 1}