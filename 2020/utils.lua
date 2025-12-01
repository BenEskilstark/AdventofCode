local function splitString(str, delimiter)
  delimiter = delimiter or "%s"
  local result = {}
  for match in (str .. delimiter):gmatch("(.-)" .. delimiter) do
    table.insert(result, match)
  end
  return result
end


local function toString(table)
  local str = ""
  for key, value in pairs(table) do
    if type(value) == "string" or type(value) == "number" then
      str = str .. key .. ": " .. value .. " "
    elseif type(value) == "table" then
      str = str .. toString(value)
    end

    str = str .. "\n"
  end
  return str
end


local function trim(str)
  if str == "" then return "" end
  local inContent = false
  local ftrimmed = ""

  -- trim from the front
  for c in str:gmatch(".") do
    if inContent then
      ftrimmed = ftrimmed .. c
    elseif c ~= " " and c ~= "\t" and c ~= "\n" and c ~= "\r\n" then
      inContent = true
      ftrimmed = ftrimmed .. c
    end
  end

  if ftrimmed == "" then return "" end

  inContent = false
  local trimmed = ""
  -- trim from the end
  for i = 0, #ftrimmed - 1 do
    local j = #ftrimmed - i
    local c = ftrimmed:sub(j, j)
    if inContent then
      trimmed = c .. trimmed
    elseif c ~= " " and c ~= "\t" and c ~= "\n" and c ~= "\r\n" then
      inContent = true
      trimmed = c .. trimmed
    end
  end

  return trimmed
end


return {
  splitString = splitString,
  toString = toString,

  charAt = function(str, i)
    return str:sub(i, i)
  end,

  trim = trim,

  getLines = function(str)
    local lines = splitString(str, "\n")
    if #lines == 1 then
      lines = splitString(str, "\r\n")
    end
    if lines[#lines] == "" then
      lines[#lines] = nil
    end
    return lines
  end,

  getNums = function(str)
    local numbers = {}
    for num in str:gmatch("%d+") do
      table.insert(numbers, tonumber(num))
    end
    return numbers
  end,

  printTable = function(table)
    print(trim(toString(table)))
  end
}
