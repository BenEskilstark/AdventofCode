package.path = package.path .. ";../?.lua"
local utils = require("utils")

local str_meta = getmetatable ""
local prev_str_index = str_meta.__index
function str_meta:__index(key)
    if type(key) == "number" then return string.sub(self, key, key) end
    return prev_str_index[key]
end

local function replace(str, i, val)
    return str:sub(1, i - 1) .. val .. str:sub(i + 1)
end

local Grid
Grid = {
    neighbors = function(self, r, c)
        local n = { insert = function(s, val) table.insert(s, val) end }
        if r > 1 then
            if c > 1 then n:insert(self[r - 1][c - 1]) end
            n:insert(self[r - 1][c])
            if c < #self[r - 1] then n:insert(self[r - 1][c + 1]) end
        end
        if c > 1 then n:insert(self[r][c - 1]) end
        if c < #self[r] then n:insert(self[r][c + 1]) end
        if r < #self then
            if c > 1 then n:insert(self[r + 1][c - 1]) end
            n:insert(self[r + 1][c])
            if c < #self[r + 1] then n:insert(self[r + 1][c + 1]) end
        end
        return n
    end,

    neighbors2 = function(self, r, c)
        local n = { insert = function(s, val) table.insert(s, val) end }

        local rr = r
        while rr > 1 do
            rr = rr - 1
            if self[rr][c] ~= "." then
                n:insert(self[rr][c])
                break
            end
        end

        local cc = c
        rr = r
        while rr > 1 and cc > 1 do
            rr = rr - 1
            cc = cc - 1
            if self[rr][cc] ~= "." then
                n:insert(self[rr][cc])
                break
            end
        end

        rr = r
        cc = c
        while rr > 1 and cc < #self[rr] do
            rr = rr - 1
            cc = cc + 1
            if self[rr][cc] ~= "." then
                n:insert(self[rr][cc])
                break
            end
        end

        local cc = c
        while cc > 1 do
            cc = cc - 1
            if self[r][cc] ~= "." then
                n:insert(self[r][cc])
                break
            end
        end

        cc = c
        while cc < #self[r] do
            cc = cc + 1
            if self[r][cc] ~= "." then
                n:insert(self[r][cc])
                break
            end
        end

        rr = r
        while rr < #self do
            rr = rr + 1
            if self[rr][c] ~= "." then
                n:insert(self[rr][c])
                break
            end
        end

        rr = r
        cc = c
        while rr < #self and cc > 1 do
            rr = rr + 1
            cc = cc - 1
            if self[rr][cc] ~= "." then
                n:insert(self[rr][cc])
                break
            end
        end

        rr = r
        cc = c
        while rr < #self and cc < #self[rr] do
            rr = rr + 1
            cc = cc + 1
            if self[rr][cc] ~= "." then
                n:insert(self[rr][cc])
                break
            end
        end

        return n
    end,

    set = function(self, r, c, val)
        self[r] = replace(self[r], c, val)
    end,

    __eq = function(self, other)
        if #self ~= #other then return false end
        for i = 1, #self do
            if other[i] ~= self[i] then return false end
        end
        return true
    end,

    copy = function(self, into)
        local next = into or {}
        for r = 1, #self do
            local row = ""
            for c = 1, #self[r] do
                row = row .. self[r][c]
            end
            next[r] = row
        end
        return setmetatable(next, Grid)
    end,

    update = function(self)
        local next = self:copy()
        for r = 1, #self do
            for c = 1, #self[r] do
                local ns = self:neighbors(r, c)
                local num = 0
                for _, v in ipairs(ns) do
                    if v == "#" then num = num + 1 end
                end
                if num == 0 and self[r][c] == "L" then
                    next:set(r, c, "#")
                elseif self[r][c] == "#" and num >= 4 then
                    next:set(r, c, "L")
                end
            end
        end
        local didChange = self ~= next
        next:copy(self)
        return didChange
    end,

    update2 = function(self)
        local next = self:copy()
        for r = 1, #self do
            for c = 1, #self[r] do
                local ns = self:neighbors2(r, c)
                local num = 0
                for _, v in ipairs(ns) do
                    if v == "#" then num = num + 1 end
                end
                if num == 0 and self[r][c] == "L" then
                    next:set(r, c, "#")
                elseif self[r][c] == "#" and num >= 5 then
                    next:set(r, c, "L")
                end
            end
        end
        local didChange = self ~= next
        next:copy(self)
        return didChange
    end,

    numOccupied = function(self)
        local num = 0
        for r = 1, #self do
            for c = 1, #self[r] do
                if self[r][c] == "#" then num = num + 1 end
            end
        end
        return num
    end,
}
Grid.__index = Grid


local content = io.open("input.txt", "r"):read("*a")
local grid = setmetatable(utils.getLines(content), Grid)

local didChange = true
while didChange do
    didChange = grid:update()
end
print(grid:numOccupied())

grid = setmetatable(utils.getLines(content), Grid)
didChange = true
while didChange do
    didChange = grid:update2()
end
print(grid:numOccupied())
