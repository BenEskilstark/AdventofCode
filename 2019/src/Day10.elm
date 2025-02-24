module Day10 exposing (..)

import Dict exposing (Dict)

type alias Grid a = Dict (Int, Int) a
type alias CharGrid = Grid String