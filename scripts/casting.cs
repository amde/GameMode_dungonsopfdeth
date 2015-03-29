////////////////////
//Magicks: Casting//
////////////////////

////////////
//CONTENTS//
////////////
//#1. Functionality
//	#1.1 Package starts
//	#1.2 armor::onTrigger
//	#1.3 Package ends
//	#1.4 conal raycasts
//	#1.5 player::Blind
//	#1.6 getVectorAngle
//#2. Spell casts
//   #2.1 Shadowflame
//   #2.2 Arcane Barrage
//   #2.3 Arcane Missiles
//   #2.4 Twilight Bolt
//   #2.5 Burning Twilight
//   #2.6 Twilight Barrage
//   #2.7 Blink
//   #2.8 Stone Shield
//   #2.9 Mudball
//   #2.10 Disruption

//#1.
//	#1.1
package Magicks_Casting
{
	//	#1.2
	function armor::onTrigger(%this, %obj, %slot, %val)
	{
		if(%slot == 0 && %val)
		{
			if(isObject(%client = %obj.client) && %client.getClassName() $= "GameConnection")
			{
				if(%tok = strLen(%client.tokens))
				{
					if(isFunction(%fn = strReplace($SpellName[%client.tokens], " ", "")))
					{
						if(getSimTime() > %obj.GCD)
						{
							%manaCost = %tok * 15 + (%tok > 2 ? 15 : 0);
							if((%lev = %obj.getEnergyLevel()) >= %manaCost)
							{
								%result = call(%fn, %obj);
								if(%result != 1)
								{
									%obj.setEnergyLevel(%lev - %manaCost);
									%client.clearTokens = 1;
									%obj.GCD = getSimTime() + 1000;
								}
							}
							else
							{
								%client.centerPrint("Not enough mana!<br>" @ mFloatLength(%lev, 0) @ "/" @ %manaCost, 1);
							}
						}
						else
						{
							%client.centerPrint("Not ready yet!", 1);
						}
					}
					else
					{
						%client.centerPrint("Spell unscripted", 2);
						%client.clearTokens = 1;
					}
				}
			}
		}
		parent::onTrigger(%this, %obj, %slot, %val);
	}
};
//	#1.3
activatePackage(Magicks_Casting);
//	#1.4
function conalRaycast(%center, %forwardVector, %radius, %angle, %typemasks, %exclude)
{
	InitContainerRadiusSearch(%center, %radius, %typemasks);
	while(%hit = containerSearchNext())
	{
		if(%hit == %exclude)
		{
			continue;
		}
		%vec1 = vectorNormalize(VectorSub(strPos(%hit.getClassName(), "Player") != -1 ? %hit.getHackPosition() : %hit.getPosition(), %center));
		%vec2 = %forwardVector;
		%ang1 = getVectorAngle(%vec2, %vec1);
		if(%ang1 <= %angle)
		{
			break;
		}
	}
	if(isObject(%hit))
	{
		%raycast = containerRaycast(%center, strPos(%hit.getClassName(), "Player") != -1 ? %hit.getHackPosition() : %hit.getPosition(), %typemasks, %exclude);
	}
	return setWord(%raycast, 0, %hit);
}
function conalRaycastM(%center, %forwardVector, %radius, %angle, %typemasks, %exclude)
{
	InitContainerRadiusSearch(%center, %radius, %typemasks);
	while(%hit = containerSearchNext())
	{
		if(%hit == %exclude)
		{
			continue;
		}
		%vec1 = vectorNormalize(VectorSub(strPos(%hit.getClassName(), "Player") != -1 ? %hit.getHackPosition() : %hit.getPosition(), %center));
		%vec2 = %forwardVector;
		%ang1 = getVectorAngle(%vec2, %vec1);
		if(%ang1 <= %angle)
		{
			%list = %list SPC %hit;
		}
	}
	return trim(%list);
}

//	#1.5
function DarkBlindPlayerImage::onMount(%this, %obj, %slot)
{
	serverPlay3d(DarkBlindSound, %obj.getEyePoint());
	parent::onMount(%this, %obj, %slot);
}
function DarkBlindPlayerImage::Dismount(%this, %obj, %slot)
{
}

function Player::Blind(%this, %dur, %dr)
{
	if(%this.BlindDR < 3 && %dr)
	{
		%this.mountImage(DarkBlindPlayerImage, 1);
		%duration = %dur / (%dr ? mClamp(%this.BlindDR + 1, 1, 4) : 1);
		%this.BlindDR++;
		%this.schedule(%duration, unmountImage, 1);
		if(!isEventPending(%this.BlindDRclear))
		{
			%this.BlindDRclear = %this.schedule(15000 + %duration, BlindDRdec);
		}
	}
}

//	#1.6
function getVectorAngle(%vec1, %vec2) //http://www.torquepowered.com/community/forums/viewthread/12632/1#comment-596037
{
	%vec1n = VectorNormalize(%vec1);
	%vec2n = VectorNormalize(%vec2);

	%vdot = VectorDot(%vec1n, %vec2n);
	%angle = mACos(%vdot);

	// convert to degrees and return
	%degangle = mRadToDeg(%angle);
	return %degangle;
}

//#2
//	#2.1
function Shadowflame(%caster)
{
	%caster.mountImage(ShadowflameBreathImage, 0);
	%caster.schedule(2000, unmountImage, ShadowflameBreathImage);
	for(%i = 1; %i <= 4; %i++)
	{
		schedule(%i * 500, 0, Shadowflame_Spray, %caster, %i == 1);
	}
}
function Shadowflame_Spray(%caster, %bool)
{
	%list = conalRaycastM(%caster.getEyePoint(), %caster.getEyeVector(), 4, 45, $Typemasks::PlayerObjectType, %caster);
	for(%col = firstWord(%list); isObject(%col); %col = getWord(%list, %i++))
	{
		%r = getRandom() >= 0.5;
		if(MinigameCanDamage(%caster, %col) == 1)
		{
			%col.damage(%obj, %pos, 25 / 4, $DamageType::Direct);
			if(%bool)
			{
				if(%r)
				{
					schedule(1000, 0, Shadowflame_Tick, %col, %caster, 4);
				}
				else
				{
					%col.blind(4, 1);
				}
			}
		}
	}
}
function Shadowflame_Tick(%obj, %sObj, %remain)
{
	cancel(%obj.IgnitionSched);
	%obj.damage(%sObj, %obj.getPosition(), 25 / 4, $DamageType::Direct);
	if(%remain-- > 0)
	{
		%obj.IngitionSched = schedule(1000, 0, Shadowflame_Tick, %obj, %sObj, %remain);
	}
}

// #2.2
function ArcaneBarrage(%caster)
{
	%pos = %caster.getEyePoint();
	%vel = vectorScale(%caster.getEyeVector(), 150);
	new Projectile()
	{
		datablock = ArcaneBarrageProjectileA;
		initialPosition = %pos;
		initialVelocity = %vel;
		sourceObject = %caster;
		sourceSlot = 0;
		doSound = 1;
	};
	new Projectile()
	{
		datablock = ArcaneBarrageProjectileB;
		initialPosition = %pos;
		initialVelocity = %vel;
		sourceObject = %caster;
		sourceSlot = 0;
	};
}

function ArcaneBlast(%caster)
{
	%list = conalRaycastM(%caster.getEyePoint(), %caster.getEyeVector(), 32, 15, $Typemasks::PlayerObjectType, %caster);
	for(%col = firstWord(%list); isObject(%col); %col = getWord(%list, %i++))
	{
		if(minigameCanDamage(%caster, %col) == 1)
		{
			%col.damage(%caster, %col.getPosition(), 23 * (1 + %caster.ABstacks * 0.3), $DamageType::Direct);
			%caster.ABstacks++;
			cancel(%caster.ABstackDec);
			%caster.ABstackDec = %caster.schedule(6000, ABstackDec);
			%col.spawnExplosion(ArcaneBarrageProjectileA, 1);
			serverPlay3D(ArcaneMissile_Impact, %col.getHackPosition());
			return 0;
		}
	}
	return 1;
}

function Player::ABstackDec(%obj)
{
	%obj.ABstacks = 0;
}

// #2.3

function ArcaneMissiles(%caster)
{
	%pos = %caster.getEyePoint();
	%end = vectorAdd(%pos, vectorScale(%caster.getEyeVector(), 300));
	%types = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::InteriorObjectType | $Typemasks::TerrainObjectType | $TypeMasks::FxBrickObjectType;
	%ray = containerRaycast(%pos, %end, %types, %caster);
	%tpos = isObject(firstWord(%ray)) ? posFromRaycast(%ray) : %end;
	%vec[0] = %caster.getRelativeVector("0.1 1 0", "eye");
	%vec[1] = %caster.getRelativeVector("0 1 0.05", "eye");
	%vec[2] = %caster.getRelativeVector("-0.1 1 0", "eye");
	for(%i = 0; %i < 3; %i++)
	{
		%vec = %vec[%i];
		%vel = VectorScale(%vec[%i], 70);
		%p = new Projectile()
		{
			datablock = ArcaneMissileProjectileB;
			initialPosition = %pos;
			initialVelocity = %vel;
			creationTime = getSimTime();
		};
		%p2 = new Projectile()
		{
			datablock = ArcaneMissileProjectileA;
			initialPosition = %pos;
			initialVelocity = %vel;
			sourceObject = %caster;
			sourceSlot = 0;
			creationTime = getSimTime();
			sparkles = %p;
		};
		%p2.homeSched = ArcaneMissileProjectileA.schedule(150, home, %p2, %tpos);
	}
}
function ArcaneMissileProjectileA::Home(%this, %p, %d)
{
	if(!isObject(%p))
	{
		return;
	}
	%vel = vectorScale(vectorNormalize(vectorSub(%d, %p.getPosition())), 70);
	%p2 = new Projectile()
	{
		datablock = ArcaneMissileProjectileB;
		initialPosition = %p.getPosition();
		initialVelocity = %vel;
		sourceSlot = 0;
	};
	%p3 = new Projectile()
	{
		datablock = ArcaneMissileProjectileA;
		initialPosition = %p.getPosition();
		initialVelocity = %vel;
		sourceObject = %p.caster;
		sourceSlot = 0;
		sparkles = %p2;
	};
	%p.sparkles.delete();
	%p.delete();
}
function ArcaneMissileProjectileA::Home2(%this, %p)
{
	%obj = %p.sourceObject;
	if(!isObject(%p) || !isObject(%obj))
	{
		return;
	}
	if(getSimTime() - %p.creationTime >= 8000)
	{
		%p.explode();
		return;
	}
	%vec1 = %obj.getEyeVector();
	%tpos = vectorScale(%vec1, vectorDist(%obj.getEyePoint(), %p.getPosition()) + 70);
	%types = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::InteriorObjectType | $Typemasks::TerrainObjectType | $TypeMasks::FxBrickObjectType;
	%ray = containerRaycast(%obj.getEyePoint(), %tpos, %types, %obj);
	%tpos = isObject(firstWord(%ray)) ? posFromRaycast(%ray) : %tpos;
	%vec2 = vectorScale(vectorNormalize(vectorSub(%tpos, %p.getPosition())), 70);
	%p3 = new Projectile()
	{
		dataBlock = ArcaneMissileProjectileB;
		initialVelocity = %vec2;
		initialPosition = %p.getPosition();
		sourceObject = %obj;
		sourceSlot = %slot;
		client = %obj.client;
	};
	%p2 = new Projectile()
	{
		dataBlock = ArcaneMissileProjectileB;
		initialVelocity = %vec2;
		initialPosition = %p.getPosition();
		sourceObject = %obj;
		sourceSlot = %slot;
		creationTime = %p.creationTime;
		client = %obj.client;
		sparkles = %p3;
	};
	%p.sparkles.delete();
	%p.delete();
	%p2.homeSched = ArcaneMissileProjectileA.schedule(100, Home, %p2);
}

// #2.4

function TwilightBolt(%caster)
{
	%pos = %caster.getEyePoint();
	%vel = vectorScale(%caster.getEyeVector(), 140);
	new Projectile()
	{
		datablock = TwilightBoltProjectile;
		initialPosition = %pos;
		initialVelocity = %vel;
		sourceObject = %caster;
		sourceSlot = 0;
		dmgMod = 1;
	};
}

// #2.5

function BurningTwilight(%caster) //not done
{
	%pos = %caster.getEyePoint();
	%vel = vectorScale(%caster.getEyeVector(), 140);
	new Projectile()
	{
		datablock = TwilightBoltProjectile;
		initialPosition = %pos;
		initialVelocity = %vel;
		sourceObject = %caster;
		sourceSlot = 0;
		dmgMod = 0.8;
		ignition = 1;
	};
}

// #2.6

function TwilightBarrage(%caster)
{
	%pos = %caster.getEyePoint();
	%eyeVec = %caster.getEyeVector();
	%add[0] = vectorScale(vectorNormalize(getRandom()-0.5 SPC getRandom()-0.5 SPC getRandom()-0.5), 0.1);
	%add[1] = vectorScale(vectorNormalize(getRandom()-0.5 SPC getRandom()-0.5 SPC getRandom()-0.5), 0.1);
	%add[2] = vectorScale(vectorNormalize(getRandom()-0.5 SPC getRandom()-0.5 SPC getRandom()-0.5), 0.1);
	for(%i = 0; %i < 3; %i++)
	{
		%vec = vectorNormalize(vectorAdd(%eyeVec, %add[%i]));
		%vel = VectorScale(%vec, 125);
		%p = new Projectile()
		{
			datablock = TwilightBoltProjectile;
			initialPosition = %pos;
			initialVelocity = %vel;
			sourceObject = %caster;
			sourceSlot = 0;
			scale = "0.75 0.75 0.75";
			dmgMod = 0.5;
		};
	}
}

// #2.7

function Blink(%caster)
{
	%p = new Projectile()
	{
		datablock = ArcaneBarrageProjectileA;
		initialVelocity = "0 0 0";
		initialPosition = %caster.getHackPosition();
	};
	%p.explode();
	serverPlay3D(ArcaneMissile_Impact, %caster.getHackPosition());
	%rot = rotFromTransform(%caster.getTransform());
	%start = %caster.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%caster.getEyeVector(), 50));
	%types = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::InteriorObjectType | $Typemasks::TerrainObjectType | $TypeMasks::FxBrickObjectType;
	%ray = containerRaycast(%start, %end, %types, %caster);
	%start = isObject(firstWord(%ray)) ? vectorSub(posFromRaycast(%ray), %caster.getEyeVector()) : %end;
	%end = vectorAdd(%start, "0 0 -50");
	%ray = containerRaycast(%start, %end, %types);
	if(isObject(firstWord(%ray)) && vectorDist(%caster.getEyePoint(), posFromRaycast(%ray)) <= 10)
	{
		%caster.client.centerPrint("Not enough room!", 2);
		return 1;
	}
	%end = isObject(firstWord(%ray)) ? posFromRaycast(%ray) : %end;
	%caster.addVelocity("0 0 0.5");
	%caster.setTransform(%end SPC %rot);
}

// #2.8

function StoneShield(%caster)
{
	%start = %caster.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%caster.getEyeVector(), 12));
	%types = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::InteriorObjectType | $Typemasks::TerrainObjectType | $TypeMasks::FxBrickObjectType;
	%ray = containerRaycast(%start, %end, %types, %caster);
	if(isObject(firstWord(%ray)))
	{
		InitContainerRadiusSearch(posFromRaycast(%ray), 64, $TypeMasks::FxBrickObjectType);
		while(%hit = containerSearchNext())
		{
			if(%hit.getDatablock().category $= "Baseplates" && %hit.getDatablock().subCategory !$= "Water" && getWord(getColorIDtable(%hit.colorID), 3) == 1)
			{
				%color = %hit.colorID;
				break;
			}
		}
		%color = strLen(%color) ? %color : getClosestPaintColor("0.35 0.15 0 1");
		%angleID = getAngleIDfromPlayer(%caster);
		%pos = vectorAdd(posFromRaycast(%ray), "0 0 0.9");
		InitContainerRadiusSearch(%pos, 2, $TypeMasks::PlayerObjectType);
		while(%hit = containerSearchNext())
		{
			if(minigameCanDamage(%caster, %hit))
			{
				%hit.damage(%caster, %pos, 20, $DamageType::Direct);
			}
		}
		for(%i = 0; %i < 2; %i++)
		{
			%brick = new FxDTSbrick()
			{
				datablock = Brick2x6x3data;
				position = %pos;
				rotation = "0 0 1 " @ %angleID * 90;
				colorID = %color;
				scale = "1 1 1";
				angleID = %angleID;
				colorFxID = 0;
				shapeFxID = 0;
				isPlanted = 1;
				magic = 1;
			};
			%brick.plant();
			%brick.setEmitter(StoneEmitter);
			serverPlay3d(Stone_Impact, %pos);
			%brick.schedule(40, setEmitter, HeavyFogEmitter);
			%brick.schedule(100, setEmitter, 0);
			%brick.schedule(5000, fakeKillBrick, "0 0 10", 1);
			%brick.schedule(6000, delete);
			%pos = vectorAdd(%pos, "0 0 1.8");
		}
	}
	else
	{
		%caster.client.centerPrint("Too far!", 2);
		return 1;
	}
}

// #2.9

function Mudball(%caster)
{
	%pos = %caster.getEyePoint();
	%vel = vectorScale(%caster.getEyeVector(), 50);
	new Projectile()
	{
		datablock = MudProjectile;
		initialPosition = %pos;
		initialVelocity = %vel;
		sourceObject = %caster;
		sourceSlot = 0;
		tiny = false;
		huge = false;
	};
}

// #2.10

function Disruption(%caster)
{
	%pos = %caster.getEyePoint();
	%vel = vectorScale(%caster.getEyeVector(), 25);
	%p = new Projectile()
	{
		datablock = MudProjectile;
		initialPosition = %pos;
		initialVelocity = %vel;
		sourceObject = %caster;
		sourceSlot = 0;
		scale = "1.5 1.5 1.5";
		huge = true;
		tiny = false;
	};
	%p.dump();
	%p.schedule(1000, explode);
}