package.path = package.path .. ";../?.lua"
local utils = require("utils")

local c = io.open("input.txt", "r"):read("*a")
c = "return {\n"..c
c = string.gsub(c, " bags contain", "\"] = {")
c = string.gsub(c, "bag[s]?", "")
c = string.gsub(c, "\n", "\n[\"")
c = string.gsub(c, "(%d) (%w+ %w+) ([,%.])", "[\"%2\"] = %1%3 ")
c = string.gsub(c, "%.", "},")
c = string.gsub(c, "%[\"$", "")
c = string.gsub(c, "no other ", "")
c = c .. "}"

local rules = load(c)()
local color = "shiny gold"

local colors = {}
local function pathTo(rules, curColor, color)
  if curColor == nil then return false end
  if colors[curColor] ~= nil then
    return curColor
  end
  if rules[curColor][color] ~= nil then
    colors[curColor] = 1
    return curColor
  end
  for nextColor in pairs(rules[curColor]) do
    if pathTo(rules, nextColor, color) then
      colors[curColor] = 1
      return curColor
    end
  end
  return false
end

local total1 = 0
for curColor in pairs(rules) do
  if pathTo(rules, curColor, color) then
    total1 = total1 + 1
  end
end

print(total1)
-- utils.printTable(colors)

local bags = {}
local function numBags(rules, curColor)
  if bags[curColor] then return bags[curColor] end
  total = 1
  for nextColor, num in pairs(rules[curColor]) do
    total = total + num * numBags(rules, nextColor)
  end
  bags[curColor] = total
  return total
end

local total2 = numBags(rules, color) - 1

-- utils.printTable(bags)
print(total2)
