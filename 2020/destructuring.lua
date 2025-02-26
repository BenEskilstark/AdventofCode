
setfenv = setfenv or function(f, t)
    f = (type(f) == 'function' and f or debug.getinfo(f + 1, 'f').func)
    local name
    local up = 0
    repeat
        up = up + 1
        name = debug.getupvalue(f, up)
    until name == '_ENV' or name == nil
    if name then
debug.upvaluejoin(f, up, function() return name end, 1) -- use unique upvalue
        debug.setupvalue(f, up, t)
    end
end

local function destruct(obj, ...)
  local res = {}
  for _, k in ipairs({...}) do
    res[k] = obj[k]
  end
  setmetatable(res, {__index = _G})
  setfenv(2, res)
end

local function getArea(rect)
  destruct(rect, "width", "height")
  return width * height
end

print(getArea({width = 2, height = 13}))
print(width)
