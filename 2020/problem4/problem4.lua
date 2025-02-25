package.path = package.path .. ";../?.lua"
local utils = require("utils")
local Grid = require("grid")

local content = io.open("input.txt", "r"):read("*a")

local Set = {
    __eq = function(a, b)
        for k in pairs(a) do
            if b[k] == nil and k ~= "cid" then return false end
        end
        for k in pairs(b) do
            if a[k] == nil and k ~= "cid" then return false end
        end
        return true
    end,
    isValid = function(self)
        self.byr = tonumber(self.byr)
        self.iyr = tonumber(self.iyr)
        self.eyr = tonumber(self.eyr)
        if self.byr < 1920 or self.byr > 2002 then
            return false
        elseif self.iyr < 2010 or self.iyr > 2020 then
            return false
        elseif self.eyr < 2020 or self.eyr > 2030 then
            return false
        elseif string.find(self.hcl, "^#[%da-f][%da-f][%da-f][%da-f][%da-f][%da-f]$") == nil then
            return false
        elseif ({ amb = 1, blu = 1, brn = 1, gry = 1, grn = 1, hzl = 1, oth = 1 })[self.ecl] == nil then
            return false
        elseif string.find(self.pid, "^%d%d%d%d%d%d%d%d%d$") == nil then
            return false
        else
            if string.find(self.hgt, "^%d+cm$") ~= nil then
                local hgt = utils.getNums(self.hgt)[1]
                if hgt < 150 or hgt > 193 then return false end
            elseif string.find(self.hgt, "^%d+in$") ~= nil then
                local hgt = utils.getNums(self.hgt)[1]
                if hgt < 59 or hgt > 76 then return false end
            else
                return false
            end
        end
        return true
    end
}
Set.__index = Set

local keys = { byr = 1, iyr = 1, eyr = 1, hgt = 1, hcl = 1, ecl = 1, pid = 1, cid = 1 }
setmetatable(keys, Set)

local numAllPresent = 0
local numValid = 0
local passports = utils.splitString(content, "\n\n")
for i, pass in pairs(passports) do
    pass = "{" .. string.gsub(pass, ":", "=\"") .. "\"}"
    pass = string.gsub(pass, "[ \n]", "\", ")
    passports[i] = loadstring("return " .. pass)()
    setmetatable(passports[i], Set)
    if passports[i] == keys then
        numAllPresent = numAllPresent + 1
        if passports[i]:isValid() then numValid = numValid + 1 end
    end
end
print(numAllPresent)
print(numValid)
