
return {
  splitString = function (str, delimiter)
      delimiter = delimiter or "%s"
      local result = {}
      for match in (str..delimiter):gmatch("(.-)"..delimiter) do
        table.insert(result, match)
      end
      return result
    end,

  getNums = function (str)
      local numbers = {}
      for num in str:gmatch("%d+") do
          table.insert(numbers, tonumber(num))
      end
      return numbers
    end,

  printTable = function(table)
      local str = ""
      for key, value in pairs(table) do
        str = str .. key .. ": " .. value .. " "
        if #str > 50 then
          print(str)
          str = ""
        end
      end
      print(str)
    end
}
