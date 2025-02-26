package.path = package.path .. ";../?.lua"
local utils = require("utils")

local content = io.open("input.txt", "r"):read("*a")

local function partition(parts)
  local rowPart = string.sub(parts, 1, 7)
  rowPart = string.gsub(rowPart, "F", "0")
  rowPart = string.gsub(rowPart, "B", "1")
  local row = tonumber(rowPart, 2)

  local seatPart = string.sub(parts, 8, 10)
  seatPart = string.gsub(seatPart, "R", "1")
  seatPart = string.gsub(seatPart, "L", "0")
  local seat = tonumber(seatPart, 2)

  return row * 8 + seat
end

local maxID = 0
local seats = {}
for _, line in ipairs(utils.getLines(content)) do
  -- print(line)
  local id = partition(line)
  if id > maxID then maxID = id end
  seats[id] = ""
end

print(maxID)
utils.printTable(seats)
