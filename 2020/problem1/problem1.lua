package.path = package.path .. ";../?.lua"
local utils = require("utils")

local file = io.open("test_input.txt", "r")
local content = file:read("*a")
file:close()

local nums = utils.getNums(content)

-- part 1:
for i = 1, (#nums - 1) do
  for j = (i + 1), #nums do
    if nums[i] + nums[j] == 2020 then
      print(nums[i] * nums[j])
    end
  end
end


-- part 2:
for i = 1, (#nums - 2) do
  for j = (i + 1), (#nums - 1) do
    for k = (j + 1), #nums do
      if nums[i] + nums[j] + nums[k] == 2020 then
        print(nums[i] * nums[j] * nums[k])
      end
    end
  end
end
