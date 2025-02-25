local utils = require("utils")

local Grid = {}

function Grid:new(grid)
    local self = setmetatable({}, { __index = Grid })
    self.width = grid.width or nil
    self.height = grid.height or nil
    self.default = grid.default or nil
    return self
end

function Grid:fromString(str)
    local grid = Grid:new({})
    for y, line in ipairs(utils.getLines(str)) do
        for x = 1, #line do
            grid:set(x, y, utils.charAt(line, x))
        end
        grid.width = #line
        grid.height = y
    end
    return grid
end

function Grid:at(x, y)
    return self[x .. "_" .. y]
end

function Grid:set(x, y, val)
    self[x .. "_" .. y] = val
end

return Grid
