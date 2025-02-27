package.path = package.path .. ";../?.lua"
local utils = require("utils")

local c = io.open("input.txt", "r"):read("*a")
local nums = utils.getNums(c)

local preamble = 25
local badNum = -1
for i = preamble + 1, #nums do
    local doesSum = false
    for jj = 0, preamble - 1 do
        local j = i - (preamble - jj)
        for k = j + 1, j + preamble do
            if nums[i] == nums[j] + nums[k] then
                doesSum = true
                break
            end
        end
        if doesSum then break end
    end
    if not doesSum then
        badNum = nums[i]
        break
    end
end
print(badNum)

for i = 1, #nums do
    local total = 0
    local j = i
    local smallest, largest = nums[i], nums[i]
    while total < badNum do
        total = total + nums[j]
        if nums[j] < smallest then smallest = nums[j] end
        if nums[j] > largest then largest = nums[j] end
        j = j + 1
    end
    if total == badNum then
        print(largest + smallest)
        break
    end
end
