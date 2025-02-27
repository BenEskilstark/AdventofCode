package.path = package.path .. ";../?.lua"
local utils = require("utils")

local c = io.open("input.txt", "r"):read("*a")
local jolts = {}
for _, j in ipairs(utils.getNums(c)) do jolts[j] = 1 end

local max = 0
for j in pairs(jolts) do if j > max then max = j end end

local num1s, num3s = 0, 1
local joltage = 0
while joltage < max do
    if jolts[joltage + 1] then
        joltage = joltage + 1
        num1s = num1s + 1
    elseif jolts[joltage + 2] then
        joltage = joltage + 2
    elseif jolts[joltage + 3] then
        joltage = joltage + 3
        num3s = num3s + 1
    end
end

print(num1s .. " x " .. num3s .. " = " .. (num1s * num3s))

local numWays = {}
local function getNumWays(jolt)
    if jolt ~= 0 and jolts[jolt] == nil then return 0 end
    if numWays[jolt] then return numWays[jolt] end

    if jolt == max then
        numWays[jolt] = 1
        return numWays[jolt]
    end

    numWays[jolt] = getNumWays(jolt + 1)
        + getNumWays(jolt + 2)
        + getNumWays(jolt + 3)
    return numWays[jolt]
end

print(getNumWays(0))
