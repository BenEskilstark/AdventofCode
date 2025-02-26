package.path = package.path .. ";../?.lua"
local utils = require("utils")

local content = io.open("input.txt", "r"):read("*a")

local function numKeys(t)
    local num = 0
    for k in pairs(t) do num = num + 1 end
    return num
end

local function numKeysOfSize(t, size)
    local num = 0
    for _, v in pairs(t) do
        if v >= size then num = num + 1 end
    end
    return num
end

local Set = {
    __index = function() return 0 end
}

local total1 = 0
local total2 = 0
local groups = utils.splitString(content, "\n\n")
for _, group in pairs(groups) do
    local size = #utils.getLines(group)
    group = string.gsub(group, "[ \n]", "")
    local set = setmetatable({}, Set)
    for i = 1, #group do
        set[utils.charAt(group, i)] = set[utils.charAt(group, i)] + 1
    end
    total1 = total1 + numKeys(set)
    total2 = total2 + numKeysOfSize(set, size)
end
print(total1)
print(total2)
