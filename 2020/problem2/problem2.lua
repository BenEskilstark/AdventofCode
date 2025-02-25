package.path = package.path .. ";../?.lua"
local utils = require("utils")

local file = io.open("input.txt", "r")
if file == nil then
    print("couldn't find file")
    return
end
local content = file:read("*a")
file:close()

local Policy = {}
function Policy.new(min, max, char, pass)
    local self = setmetatable(
        { min = min, max = max, char = char, password = pass },
        { __index = Policy }
    )
    return self
end

function Policy:isValid1()
    local matches = 0
    for i = 1, #self.password do
        if utils.charAt(self.password, i) == self.char then
            matches = matches + 1
        end
    end
    return matches >= self.min and matches <= self.max
end

function Policy:isValid2()
    local pos1 = 0
    if utils.charAt(self.password, self.min) == self.char then
        pos1 = 1
    end
    local pos2 = 0
    if utils.charAt(self.password, self.max) == self.char then
        pos2 = 1
    end
    return pos1 + pos2 == 1
end

function Policy:print()
    print(self.min .. "-" .. self.max .. " of " .. self.char .. " in " .. self.password)
end

local lines = utils.getLines(content)
local totalValid1 = 0
local totalValid2 = 0
for _, line in ipairs(lines) do
    local nums = utils.getNums(line)
    local sp = utils.splitString(line, ":")
    local char = utils.charAt(sp[1], #sp[1])
    local pass = utils.trim(sp[2])

    local policy = Policy.new(nums[1], nums[2], char, pass)
    if policy:isValid1() then totalValid1 = totalValid1 + 1 end
    if policy:isValid2() then totalValid2 = totalValid2 + 1 end
end

print(totalValid1)
print(totalValid2)
