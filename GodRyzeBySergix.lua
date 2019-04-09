
class "Ryze"

local LastSpellCast = Game.Timer()
local forcedTarget

if myHero.charName ~= "Ryze" then  return end
Callback.Add("Load",
function() 	
	Ryze()
end)

function Ryze:__init()
	self:LoadSpells()
	self:CreateMenu()
	Callback.Add("Tick", function() self:Tick() end)
	Callback.Add("Draw", function() self:Draw() end)
	Callback.Add("WndMsg",function(Msg, Key) self:WndMsg(Msg, Key) end)
end

function Ryze:CreateMenu()
	Menu = MenuElement({type = MENU, id = myHero.charName, name = "[God Ryze by Sergix]"})
	
	Menu:MenuElement({id = "Drawing", name = "Drawing", type = MENU})
	Menu.Drawing:MenuElement({id = "DrawQ", name = "Draw Q Range", value = true})

	
end

function Ryze:LoadSpells()

	Q = {Range = 1000, Width = 50,Delay = 0.25, Speed = 1700,  Sort = "line"}
	W = {Range = 615, Delay = 0.25, Speed = math.huge}
	E = {Range = 615,Delay = 0.75, Speed = 2000}
end

function Ryze:Draw()		


	if KnowsSpell(_Q) and Menu.Drawing.DrawQ:Value() then
		Draw.Circle(myHero.pos, Q.Range, Draw.Color(150, 50, 50,50))
	end
end



function Ryze:Tick()
	if IsRecalling() then return end
	
	--Should work to turn off the orbwalker and auto skills while evade is dodging
	if self:IsEvading() then
		self:DisableOrbWalk()
		return
	else
		self:EnableOrbWalk()
	end
	
	--Orbwalker says we're in combo mode
	if self:IsComboActive() then 
		self:Combo()		
	end	
	
	if self:IsHarassActive() then 
		self:Harass()		
	end
	

end



function Ryze:Harass()	

    local targetQ = GOS:GetTarget(Q.Range)
    local targetE = GOS:GetTarget(E.Range)
    if(targetQ)
    
        then
            if Ready(_Q) 
            then
             if targetQ:GetCollision(Q.width,Q.speed,Q.delay) == 0 then
                Control.CastSpell(HK_Q, targetQ) 
             end
              


            end	
        end
    if(targetE)
    then
            if Ready(_E)
            then
         
                Control.CastSpell(HK_E, targetE)
          
            end 
            if Ready(_W )            
            then
                if Ready(_E) 
                then
                else
                Control.CastSpell(HK_W, targetE)
                end
            end 
    end
	
end


function Ryze:Combo()


    --Pick the highest hitchance target within Q range to cast on.	
    local target = GOS:GetTarget(E.Range)
    if(target)
        then
            if Ready(_Q) 
            then
              --  local Qpos = GetBestCastPosition(targetQ, Q)--
                Control.CastSpell(HK_Q, target) 
              


            end	
            if Ready(_E)
            then
                if Ready(_Q)  
                then 
                else               
                Control.CastSpell(HK_E, target)
                end
            end 
            if Ready(_W )            
            then
                if Ready(_E) and Ready(_Q) 
                then
                else
                Control.CastSpell(HK_W, target)
                end
            end 
    end
	
	--self:EBounceMinions() --
end



function Ryze:IsHarassActive()
	if _G.SDK and _G.SDK.Orbwalker then		
		if _G.SDK.Orbwalker.Modes[_G.SDK.ORBWALKER_MODE_HARASS] then 
			if myHero.activeSpell and 
				myHero.activeSpell.valid and 
				myHero.activeSpell.startTime + myHero.activeSpell.windup - Game.Timer() > 0 
			then
				return false
			else
				return true
			end
		end
	end	
	if _G.GOS and _G.GOS.GetMode() == "Harass" and not _G.GOS:IsAttacking() then
		return true
	end	
end

function Ryze:IsComboActive()
	if _G.SDK and _G.SDK.Orbwalker then		
		if _G.SDK.Orbwalker.Modes[_G.SDK.ORBWALKER_MODE_COMBO] then 
			if myHero.activeSpell and 
				myHero.activeSpell.valid and 
				myHero.activeSpell.startTime + myHero.activeSpell.windup - Game.Timer() > 0 
			then
				return false
			else
				return true
			end
		end
	end	
	if _G.GOS and _G.GOS.GetMode() == "Combo" and not _G.GOS:IsAttacking() then
		return true
	end	
end
function Ryze:CastSpell(spell,pos)
	if Game.Timer() - LastSpellCast < Menu.General.SpellDelay:Value() then return end
	LastSpellCast = Game.Timer()
	self:DisableOrbWalk()
	self:DisableOrbAttack()
	DelayAction(function() Control.CastSpell(spell, pos) end,0.05)	
	DelayAction(function() self:EnableOrbWalk() end,0.1)
	DelayAction(function() self:EnableOrbAttack() end,Menu.General.SpellDelay:Value())
end

function Ryze:EnableOrbAttack()
	if _G.SDK and _G.SDK.Orbwalker then
		_G.SDK.Orbwalker:SetAttack(true)
	end
	if _G.GOS then
		_G.GOS.BlockAttack  = false
	end
end

function Ryze:EnableOrbWalk()
	if _G.SDK and _G.SDK.Orbwalker then
		_G.SDK.Orbwalker:SetMovement(true)
	end
	if _G.GOS then
		_G.GOS.BlockMovement = false
	end
end

function Ryze:DisableOrbAttack()
	if _G.SDK and _G.SDK.Orbwalker then
		_G.SDK.Orbwalker:SetAttack(false)
	end
	if _G.GOS then
		_G.GOS.BlockAttack  = true
	end
end


function Ryze:DisableOrbWalk()
	if _G.SDK and _G.SDK.Orbwalker then
		_G.SDK.Orbwalker:SetMovement(false)
	end
	if _G.GOS then
		_G.GOS.BlockMovement = true
	end
end


function Ryze:GetTarget(range)
	if self.forcedTarget and self:GetDistance(myHero.pos, self.forcedTarget.pos) <= range then
		return self.forcedTarget		
	end
	if _G.SDK then
		return _G.SDK.TargetSelector:GetTarget(range, _G.SDK.DAMAGE_TYPE_PHYSICAL);
	elseif _G.EOW then
		return _G.EOW:GetTarget(range)
	else
		return _G.GOS:GetTarget(range,"AD")
	end
end


function Ryze:WndMsg(msg,key)
	if msg == 513 then
		local starget = nil
		for i  = 1,Game.HeroCount(i) do
			local enemy = Game.Hero(i)
			if enemy.alive and enemy.isEnemy and self:GetDistance(mousePos, enemy.pos) < 250 then
				starget = enemy
				break
			end
		end
		if starget then
			self.forcedTarget = starget
		else
			self.forcedTarget = nil
		end
	end	
end

function Ryze:IsEvading()	
    if ExtLibEvade and ExtLibEvade.Evading then return true end
	return false
end






function Ryze:GetDistanceSqr(p1, p2)
	return (p1.x - p2.x) ^ 2 + ((p1.z or p1.y) - (p2.z or p2.y)) ^ 2
end
function Ryze:GetDistance(p1, p2)
	return math.sqrt(self:GetDistanceSqr(p1, p2))
end
function Ready(spellSlot)
	return IsReady(spellSlot)
end

function IsReady(spell)
	return Game.CanUseSpell(spell) == 0
end

function IsRecalling()
	for K, Buff in pairs(GetBuffs(myHero)) do
		if Buff.name == "recall" and Buff.duration > 0 then
			return true
		end
	end
	return false
end

function KnowsSpell(spell)
	local spellInfo = myHero:GetSpellData(spell)
	if spellInfo and spellInfo.level > 0 then
		return true
	end
	return false
end
 		

