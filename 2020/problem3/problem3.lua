package.path = package.path .. ";../?.lua"
local utils = require("utils")
local Grid = require("grid")

local file = io.open("input.txt", "r")
if file == nil then
    print("couldn't find file")
    return
end
local content = file:read("*a")
file:close()

local grid = Grid:fromString(content)

local function checkNumTrees(xStep, yStep)
    yStep = yStep or 1
    local x = 1 + xStep
    local numTrees = 0
    for y = (1 + yStep), grid.height, yStep do
        if grid:at(x, y) == "#" then
            numTrees = numTrees + 1
        end
        x = (x + (xStep - 1)) % grid.width + 1
    end
    return numTrees
end

-- part 1
print(checkNumTrees(3))

-- part 2
print(checkNumTrees(1))
print(checkNumTrees(3))
print(checkNumTrees(5))
print(checkNumTrees(7))
print(checkNumTrees(1, 2))
print(
    checkNumTrees(1) * checkNumTrees(3) * checkNumTrees(5) *
    checkNumTrees(7) * checkNumTrees(1, 2)
)
