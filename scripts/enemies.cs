///////////////////////////////////////////////
// GameMode_dungonsopfdth/scripts/enemies.cs //
///////////////////////////////////////////////

// table of contents

// #1. basic AI functionality
//   #1.1 AIPlayer::dodAI_Init & dodAI_InitDefault
//   #1.2 dodAI_call
//   #1.3 dodAI_Sched (main functionality)
// #2. default AI type



// #1.
// #1.1
function AIPlayer::dodAI_Init(%this, %canmove, %sightRange, %wRanged, %wMelee, %mode)
{
	%this.canMove = %canmove;
	%this.sightRange = %sightRange;
	%this.fov = 180;
	%this.item[0] = %wMelee;
	%this.item[1] = %wRanged;

	%this.effectiveRange = 35;
	%this.safeRange = 10;

	switch(%mode)
	{
		case 0: %this.prefersRanged = false; %this.rangedOnly = false;
		case 1: %this.prefersRanged = true; %this.rangedOnly = false;
		case 2: %this.prefersRanged = true; %this.rangedOnly = true;
	}

	%this.setMoveSlowdown(false);
	%this.setMoveTolerance(2);

	%this.type = "Default";

	%this.dodAI_Sched();
}

function AIPlayer::dodAI_InitDefault(%this)
{
	%this.dodAI_Init(true, 80, bowImage, swordImage, getRandom(0, 2));
}

// #1.2
function dodAI_call(%fn, %bot, %arg)
{
	%fn = isFunction("dodAI_" @ %bot.type @ "_" @ %fn) ? "dodAI_" @ %bot.type @ "_" @ %fn : "dodAI_Default_" @ %fn;
	return call(%fn, %bot, %arg);
}

// #1.3
function AIPlayer::dodAI_Sched(%this)
{
	if(!isObject(%this) || %this.getState() $= "Dead")
	{
		return;
	}
	cancel(%this.jumpsched);
	cancel(%this.dod_AISched);
	%schedlen = getRandom(750, 1500);
	%this.dodAI_Sched = %this.schedule(%schedlen, dodAI_Sched);
	%tepos = %this.getEyePoint();
	%tlpos = %this.getPosition();
	%hasTarget = (isObject(%target = %this.target) && %target.getState() !$= "Dead" && vectorDist(%tepos, %targpos = %target.getPosition()) <= %this.sightrange * 2);
	if(%hasTarget)
	{
		if(dodAI_call("CanSee", %this, %target))
		{
			//weapon selection
			%dist = vectorDist(%tlpos, %target.getPosition());
			if(%dist > 5)
			{
				//ranged
				if(isObject(%image = nameToID(%this.item[1])) && %this.getMountedImage(0) != %image)
				{
					%this.updateArm(%image);
					%this.mountImage(%image, 0);
				}
			}
			else if(!%this.rangedOnly)
			{
				//melee
				if(isObject(%image = nameToID(%this.item[0])) && %this.getMountedImage(0) != %image)
				{
					%this.updateArm(%image);
					%this.mountImage(%image, 0);
				}
				//strafing
				%this.setMoveX(getRandom(-1, 1));
				%this.schedule(%schedlen / 2, clearMoveX);
			}
			if(%this.canMove)
			{
				//chase behavior
				if(%this.rangedOnly || (%dist > 5 && %this.prefersRanged))
				{
					//ranged chase behavior
					if(%dist > %this.effectiveRange + 5)
					{
						//too far away: advance
						%this.setMoveObject(%target);
						switch$(dodAI_call("Pathfind", %this, %target))
						{
							case "Jump": %this.setImageTrigger(2, 1); %this.schedule(%schedlen / 2, setImageTrigger, 2, 0);
							case "Crouch": %this.setImageTrigger(3, 1); %this.schedule(%schedlen / 2, setImageTrigger, 3, 0);
						}
						//attack
						%this.setAimObject(%target);
					}
					else if(%dist < %this.safeRange)
					{
						//too close: retreat
						%this.setMoveObject(0);
						%this.clearAim();
						%run = vectorNormalize(vectorSub(setWord(%tlpos, 2, 0), setWord(%target.getPosition(), 2, 0)));
						%run = vectorAdd(%tlpos, vectorScale(%run, 50));
						%this.setMoveDestination(%run);
						return;
					}
					else
					{
						//sweet spot: backpedal
						%this.setMoveObject(0);
						%this.stop();
						if(%dist + 5 < %this.effectiveRange)
						{
							%this.setMoveY(-1);
							%this.schedule(%schedlen - 1, clearMoveY);
						}
						//attack
						%this.setAimObject(%target);
					}
				}
				else
				{
					//melee chase behavior
					%this.setMoveObject(%target);
					%this.setAimObject(%target);
					switch$(dodAI_call("Pathfind", %this, %target))
					{
						case "Jump": %this.setImageTrigger(2, 1); %this.schedule(%schedlen / 2, setImageTrigger, 2, 0);
						case "Crouch": %this.setImageTrigger(3, 1); %this.schedule(%schedlen / 2, setImageTrigger, 3, 0);
					}
				}
			}
			//attack
			%this.setImageTrigger(0, 1);
			%this.schedule(%schedlen * 0.3, setImageTrigger, 1, 1);
			%this.schedule(%schedlen * 0.7, setImageTrigger, 0, 0);
			%this.schedule(%schedlen - 1, setImageTrigger, 1, 0);
			//store target info
			%this.lastSeenTarget = getSimTime();
			%this.lastSeenTargetPos = %targpos;
			%noWander = true;
		}
		else
		{
			//can't see target
			%this.setImageTrigger(0, 0);
			%this.setImageTrigger(1, 0);
			%this.setMoveObject(0);
			%this.clearAim();
			if(%this.lastSeenTarget + 5000 < getSimTime())
			{
				//lost target
				%hasTarget = false;
				if(isObject(%this.target))
				{
					//OnBotLoseTarget(%this, %this.target);
				}
			}
			else if(%this.canMove)
			{
				%this.setMoveDestination(%this.lastSeenTargetPos);
				%noWander = true;
			}
		}
	}
	if(!%hasTarget)
	{
		//clear attacks
		%this.setImageTrigger(0, 0);
		%this.setImageTrigger(1, 0);
		//clear target info
		%this.lastSeenTarget = 0;
		%this.lastSeenTargetPos = 0;
		%this.target = 0;
		//clear movement
		%this.setMoveObject(0);
		%this.clearMoveX();
		%this.clearAim();
	}
	//this search runs even if it has a target, just in case there's a better target around
	InitContainerRadiusSearch(%tepos, %this.sightrange, $Typemasks::PlayerObjectType);
	while(%hit = ContainerSearchNext())
	{
		if(dodAI_call("CanSee", %this, %hit) && %hit.getState() !$= "Dead")
		{
			%hmini = isObject(%hit.client) ? %hit.client.minigame : %hit.brickGroup.client.minigame;
			if(%hmini == %this.client.minigame)
			{
				%hpos = %hit.getHackPosition();
				%eyevec = %this.getEyeVector();
				%dvec = vectorNormalize(vectorSub(%hpos, %tepos));
				%nFOV = getVectorAngle(%eyevec, %dvec);
				if(%nFOV <= %this.fov)
				{
					if(dodAI_call("ShouldAttack", %this, %hit))
					{
						//OnBotDetectEnemy(%this, %hit);
						if(!%haveenemy)
						{
							%this.target = %hit;
							if(%this.canMove && (vectorDist(%hpos, %tepos) > 5 && isObject(%this.item[0])))
							{
								%this.setMoveObject(%hit);
							}
							%this.setAimObject(%hit);
							//OnBotChaseTarget(%this, %hit);
							%haveenemy = true;
						}
					}
					else
					{
						//OnBotDetectAlly(%this, %hit);
					}
				}
			}
		}
	}
	if(!%noWander && %this.canMove)
	{
		%dest = vectorAdd(%tepos, (getRandom() - 0.5) * 50 SPC (getRandom() - 0.5) * 50 SPC 0);
		while(isObject(firstWord(containerRaycast(%tlpos, %dest, $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::VehicleBlockerObjectType, %this))))
		{
			%dest = vectorAdd(%tepos, (getRandom() - 0.5) * 20 SPC (getRandom() - 0.5) * 20 SPC 0);
			if(%i++ > 10) //This is here so if the object is encased in bricks or something the server doesn't crash
			{
				break;
			}
		}
		if(%this.canMove)
		{
			%this.setMoveDestination(%dest);
		}
		else
		{
			%this.setAimLocation(%dest);
		}
	}
}

// #2.
function dodAI_Default_Pathfind(%bot, %target)
{
	%oh = %bot.getEyePoint();
	%th = %target.getEyePoint();
	%tl = vectorAdd(%target.getPosition(), "0 0 0.1");
	%typemasks = $Typemasks::InteriorObjectType | $Typemasks::TerrainObjectType;
	%types = $Typemasks::PlayerObjectType | $Typemasks::FXbrickAlwaysObjectType;
	%headRaycast = containerRaycast(%oh, %th, %typemasks | %types, %bot);
	%hit = firstWord(%headRaycast);
	%headCanSee = %hit == %target;
	%legRaycast = containerRaycast(%oh, %tl, %typemasks | %types, %bot);
	%hit = firstWord(%legRaycast);
	%legsCanSee = %hit == %target;
	if(%legsCanSee && !%headCanSee)
	{
		return "Crouch";
	}
	if(%headCanSee && !%legsCanSee)
	{
		return "Jump";
	}
	%oz = getWord(%bot.getPosition(), 2) + 0.1;
	%tz = getWord(%tl, 2);
	%ow = %bot.getWaterCoverage() > 0.5;
	%tw = %target.getWaterCoverage() > 0.5;
	if(%ow && %tw)
	{
		if(%oz < %tz)
		{
			return "Crouch";
		}
		if(%tz < %oz)
		{
			return "Jump";
		}
		return "None";
	}
	if(%tz > %oz + 0.6)
	{
		return "Jump";
	}
	return "None";
}

function dodAI_Default_ShouldAttack(%bot)
{
	return true;
}

function dodAI_Default_CanSee(%bot, %target)
{
	%opos = %bot.getEyePoint();
	%th = %target.getEyePoint();
	%tl = %target.getPosition();
	%typemasks = $Typemasks::InteriorObjectType | $Typemasks::TerrainObjectType;
	%types = $Typemasks::PlayerObjectType | $Typemasks::FXbrickAlwaysObjectType;
	%headRaycast = containerRaycast(%opos, %th, %typemasks | %types, %bot);
	%hit = firstWord(%headRaycast);
	if(isObject(%hit) && (%type = %hit.getType()) & %types)
	{
		if(%hit == %target)
		{
			return true;
		}
		else if(%type & $Typemasks::PlayerObjectType)
		{
			return false;
		}
		else //Has to be a brick
		{
			if(!%hit.isRendering())
			{
				return true;
			}
			if(getWord(getColorIDtable(%hit.colorID), 3) < 1)
			{
				return true;
			}
		}
	}
	%legRaycast = containerRaycast(%opos, %tl, %typemasks, %bot);
	%hit = firstWord(%legRaycast);
	if(isObject(%hit) && (%type = %hit.getType()) & %types)
	{
		if(%hit == %target)
		{
			return true;
		}
		else if(%type & $Typemasks::PlayerObjectType)
		{
			return false;
		}
		else
		{
			if(!%hit.isRendering())
			{
				return true;
			}
			if(getWord(getColorIDtable(%hit.colorID), 3) < 1)
			{
				return true;
			}
		}
	}
	return false;
}