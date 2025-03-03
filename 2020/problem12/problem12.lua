package.path = package.path .. ";../?.lua"
local utils = require("utils")

local content = io.open("input.txt", "r"):read("*a")

local facing = { x = 1, y = 0 }
local x, y = 0, 0
for _, line in ipairs(utils.getLines(content)) do
    local dir, num = utils.charAt(line, 1), utils.getNums(line)[1]
    if dir == "F" then
        x = x + facing.x * num
        y = y + facing.y * num
    elseif dir == "N" then
        y = y - num
    elseif dir == "S" then
        y = y + num
    elseif dir == "E" then
        x = x + num
    elseif dir == "W" then
        x = x - num
    elseif dir == "L" then
        for _ = 1, num / 90 do
            facing = { x = facing.y, y = -1 * facing.x }
        end
    elseif dir == "R" then
        for _ = 1, num / 90 do
            facing = { x = -1 * facing.y, y = facing.x }
        end
    end
end
print(x .. " + " .. y .. " = " .. (x + y))

x, y = 0, 0
local wx, wy = 10, -1
for _, line in ipairs(utils.getLines(content)) do
    local dir, num = utils.charAt(line, 1), utils.getNums(line)[1]
    if dir == "F" then
        x = x + wx * num
        y = y + wy * num
    elseif dir == "N" then
        wy = wy - num
    elseif dir == "S" then
        wy = wy + num
    elseif dir == "E" then
        wx = wx + num
    elseif dir == "W" then
        wx = wx - num
    elseif dir == "L" then
        for _ = 1, num / 90 do
            wx, wy = wy, -1 * wx
        end
    elseif dir == "R" then
        for _ = 1, num / 90 do
            wx, wy = -1 * wy, wx
        end
    end
end
print(x .. " + " .. y .. " = " .. (x + y))
