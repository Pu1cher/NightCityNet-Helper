-- Optional approximate speech motion. Requires the Boneware API already used by NightCityNet.
-- One jaw entry only; never clears the actor's full pose or touches other actors.
local Pose = require('anim/Pose')
local M = {}
local speakers = {}
local warned = false
local function push(s, weight)
    if not IsDefined(s.body) then return false end
    local angle = -math.rad(12.0) * weight * 0.5
    local transform = Pose.Additive({
        rot = Quaternion.new(0.0, 0.0, math.sin(angle), math.cos(angle)),
        trans = Vector4.new(-0.00357 * weight, -0.00657 * weight, 0.0, 0.0),
    })
    if not transform then return false end
    local ok = pcall(function()
        s.body:AddPoseAdditiveTransformLS(CName.new('mid_J_jaw_JNT'), transform)
    end)
    return ok
end
function M.Start(ev, body)
    if not IsDefined(body) or not ev.npcId or ev.npcId == '' then return end
    local hasJaw = false
    local ok = pcall(function()
        for _, bone in ipairs(body:GetBoneNames() or {}) do
            if tostring(bone.value or bone) == 'mid_J_jaw_JNT' then hasJaw = true; break end
        end
    end)
    if not ok or not hasJaw then
        if not warned then print('[PalcherBTW][mouth] no supported jaw/Boneware; fallback skipped'); warned = true end
        return
    end
    speakers[ev.npcId] = { body = body, turn = ev.requestId, t = 0.0, age = 0.0 }
    print('[PalcherBTW][mouth] approximate speech motion started: ' .. tostring(ev.npcId))
end
function M.Stop(ev)
    local s = speakers[ev.npcId]
    if s and (not ev.requestId or ev.requestId == s.turn) then
        push(s, 0.0)
        speakers[ev.npcId] = nil
    end
end
function M.Tick(dt, active)
    dt = math.min(math.max(tonumber(dt) or 0.0, 0.0), 0.1)
    for id, s in pairs(speakers) do
        s.age = s.age + dt
        if not IsDefined(s.body) or (active and not active[id]) or s.age > 90.0 then
            push(s, 0.0)
            speakers[id] = nil
        else
            s.t = s.t + dt
            local wave = (0.5 + 0.5 * math.sin(s.t * 24.5))
                * (0.55 + 0.45 * math.sin(s.t * 6.7)^2)
            if not push(s, wave) then speakers[id] = nil end
        end
    end
end
-- No game calls during save teardown: native NightCityNet releases its face poses there.
function M.Forget() speakers = {} end
return M
