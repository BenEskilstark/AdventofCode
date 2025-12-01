package.path = package.path .. ";../?.lua"
local utils = require("utils")

local content = io.open("input.txt", "r"):read("*a")
local lines = utils.getLines(content)

local target = utils.getNums(lines[1])[1]
local times = utils.getNums(lines[2])

local minTime = 0
local minutes = target
for _, time in ipairs(times) do
    local min = time - (target % time)
    if min < minutes then
        minTime = time
        minutes = time - (target % time)
    end
end

print(minTime .. " x " .. minutes .. " = " .. (minTime * minutes))

local function modInverse(a, m)
    local m0, x0, x1 = m, 0, 1
    while a > 1 do
        local q = math.floor(a / m)
        m, a = a % m, m
        x0, x1 = x1 - q * x0, x0
    end
    if x1 < 0 then
        x1 = x1 + m0
    end
    return x1
end


local function chineseRemainder(n, a)
    local prod = 1
    for i = 1, #n do
        prod = prod * n[i]
    end

    local sum = 0
    for i = 1, #n do
        local p = prod / n[i]
        sum = sum + a[i] * p * modInverse(p, n[i])
    end

    return sum % prod
end


local function solvePartTwo(buses)
    local n = {}
    local a = {}

    for index, bus in ipairs(buses) do
        if bus ~= 'x' then
            local id = tonumber(bus)
            table.insert(n, id)
            table.insert(a, (id - index) % id) -- Adjust offset for the remainder
        end
    end

    return chineseRemainder(n, a) + 1
end


-- The first line is no longer relevant for part 2; we discard it
-- Parse the second line of input, which gives us bus IDs and their offsets
local rawBuses = utils.splitString(lines[2], ",")
print(string.format("%.0f", solvePartTwo(rawBuses)))
