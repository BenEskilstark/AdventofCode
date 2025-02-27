package.path = package.path .. ";../?.lua"
local utils = require("utils")

local c = io.open("test_input.txt", "r"):read("*a")
c = string.gsub(c, "(%a%a%a) ([%+%-]%d+)", "{\"%1\", %2},")
c = string.gsub(c, "%+", "")
c = "return {\n" .. c .. "\n}"

local CPU = {}
CPU = {
    new = function(program)
        local self = {
            isDone = false,
            isInfinite = true,
            accumulator = 0,
            pointer = 1,
            program = program,
            alreadyExecuted = {},
        }
        return setmetatable(self, { __index = CPU })
    end,

    step = function(self)
        if self.alreadyExecuted[self.pointer] then
            self.isDone = true
            return
        end
        local instruction = self.program[self.pointer]
        self.alreadyExecuted[self.pointer] = true
        if instruction == nil then
            self.isDone = true
            self.isInfinite = false
            return
        end
        if instruction[1] == "nop" then
            self.pointer = self.pointer + 1
        end
        if instruction[1] == "acc" then
            self.accumulator = self.accumulator + instruction[2]
            self.pointer = self.pointer + 1
        end
        if instruction[1] == "jmp" then
            self.pointer = self.pointer + instruction[2]
        end
    end,

    checkIsInfinite = function(self)
        while self.isDone == false do
            self:step()
        end
        return self.isInfinite
    end,
}

local program = loadstring(c)()
local cpu = CPU.new(program)
cpu:checkIsInfinite()
print(cpu.accumulator)


for _, ins in ipairs(program) do
    local prev = ins[1]
    if ins[1] == "jmp" then
        ins[1] = "nop"
    elseif ins[1] == "nop" then
        ins[1] = "jmp"
    end
    if prev ~= "acc" then
        cpu = CPU.new(program)
        if cpu:checkIsInfinite() == false then
            print(cpu.accumulator)
            break
        end
        ins[1] = prev
    end
end
