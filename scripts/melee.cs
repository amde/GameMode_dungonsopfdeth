//////////////////////////////////////////////
// GameMode_dungonsopfdeth/scripts/melee.cs //
//////////////////////////////////////////////

// table of contents
// #1. general melee functionality
// #2. swords
//    #2.1 copper sword
// #4. hammers
//    #4.1 blacksmith's hammer

// #1.0

function DoMeleeStrike(%obj, %slot, %angle, %range, %damage, %proj, %sound)
{
	if(isObject(%acc = %obj.client.dodAccount))
	{
		%angle *= (1 + %acc.mastery / 100);
		%range *= (1 + %acc.range / 100);
		%damage *= (1 + %acc.damage / 100);
	}
	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%targets = conalRaycastM(%start, %obj.getMuzzleVector(%slot), %range * %scale, %angle, $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType, %obj);
	for(%i = 0; (%hit = getWord(%targets, %i)) !$= ""; %i++)
	{
		if(minigameCanDamage(%obj, %hit))
		{
			if(isObject(%proj))
			{
				%hit.spawnExplosion(%proj, %scale);
			}
			if(isObject(%sound))
			{
				serverPlay3D(%sound, %hit.getPosition());
			}
			%hit.damage(%obj, %hit.getPosition(), %damage, %proj.directDamageType);
			%hitcount++;
		}
	}
	if(%hitcount < 1)
	{
		%typemasks = $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
		%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), %range * %scale));
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		if(isObject(%hit = getWord(%raycast, 0)))
		{
			if(isObject(%proj))
			{
				%p = new Projectile()
				{
					datablock = %proj;
					initialPosition = posFromRaycast(%raycast);
					initialVelocity = normalFromRaycast(%raycast);
					scale = %scale SPC %scale SPC %scale;
					sourceSlot = %slot;
					sourceObject = %obj;
				};
				%p.explode();
			}
			if(isObject(%sound))
			{
				serverPlay3D(%sound, posFromRaycast(%raycast));
			}
		}
	}
}

// #2.0

datablock AudioProfile(swordDrawSound)
{
	filename = "Add-Ons/Weapon_Sword/swordDraw.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(swordHitSound)
{
	filename = "Add-Ons/Weapon_Sword/swordHit.wav";
	description = AudioClosest3d;
	preload = true;
};

AddDamageType("Sword", '<bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', '%2 <bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', 0.75, 1);

datablock ParticleData(swordExplosionParticle)
{
	dragCoefficient = 4.5;
	gravityCoefficient = 3.5;
	inheritedVelFactor = 0.4;
	constantAcceleration = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	lifetimeMS = 350;
	lifetimeVarianceMS = 300;
	textureName = "base/data/particles/chunk";
	colors[0] = "0.88 0.76 0.0 1.0";
	colors[1] = "0.88 0.76 0.0 1.0";
	sizes[0] = 0.1;
	sizes[1] = 0.0;
};

datablock ParticleEmitterData(swordExplosionEmitter)
{
	ejectionPeriodMS = 8;
	periodVarianceMS = 0;
	ejectionVelocity = 15;
	velocityVariance = 7.5;
	ejectionOffset = 0.0;
	thetaMin = 0;
	thetaMax = 50;
	phiReferenceVel= 0;
	phiVariance= 360;
	overrideAdvance = false;
	particles = "swordExplosionParticle";

	uiName = "Sword Hit";
};

datablock ExplosionData(swordExplosion)
{
	lifeTimeMS = 200;

	soundProfile = swordHitSound;

	particleEmitter = swordExplosionEmitter;
	particleDensity = 25;
	particleRadius = 0.2;

	faceViewer = true;
	explosionScale = "1 1 1";

	shakeCamera = true;
	camShakeFreq = "20.0 22.0 20.0";
	camShakeAmp = "0.2 0.2 0.2";
	camShakeDuration = 0.5;
	camShakeRadius = 5.0;

	lightStartRadius = 3;
	lightEndRadius = 0;
	lightStartColor = "0.88 0.76 0.21";
	lightEndColor = "0 0 0";
};

datablock ProjectileData(swordProjectile)
{
	directDamage = 35;
	directDamageType = $DamageType::Sword;
	radiusDamageType = $DamageType::Sword;
	explosion = swordExplosion;

	muzzleVelocity = 50;
	velInheritFactor = 1;

	armingDelay = 0;
	lifetime = 100;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = false;
	gravityMod = 0;

	hasLight = false;
	lightRadius = 3;
	lightColor= "0.88 0.76 0.21";

	uiName = "Sword Hit";
};

// #2.1

datablock ItemData(copperSwordItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "Add-Ons/Weapon_Sword/sword.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Copper Sword";
	iconName = "Add-Ons/Weapon_Sword/icon_sword";
	doColorShift = true;
	colorShiftColor = "0.52 0.38 0.22 1";

	image = copperSwordImage;
	canDrop = true;

	//dod stuff
	dodItem = true;
	//base stats
	damage = 15;
	range = 5;
	mastery = 15; //cone angle
	potential = 0;
};

datablock ShapeBaseImageData(copperSwordImage)
{
	shapeFile = "Add-Ons/Weapon_Sword/sword.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = false;

	//eyeOffset = "0.7 1.2 -0.25";
	eyeOffset = "0 0 0";

	className = "WeaponImage";

	item = copperSwordItem;
	ammo = " ";
	projectile = swordProjectile;
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = true;
	colorShiftColor = copperSwordItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0]= swordDrawSound;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1] = true;

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "Fire";

	stateName[3] = "Fire";
	stateTransitionOnTimeout[3] = "CheckFire";
	stateTimeoutValue[3]= 0.2;
	stateFire[3] = true;
	stateAllowImageChange[3] = false;
	stateSequence[3] = "Fire";
	stateScript[3] = "onFire";
	stateWaitForTimeout[3] = true;

	stateName[4] = "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4] = "Fire";

	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.2;
	stateAllowImageChange[5] = false;
	stateWaitForTimeout[5] = true;
	stateSequence[5] = "StopFire";
	stateScript[5] = "onStopFire";
};

function copperSwordImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function copperSwordImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function copperSwordImage::onFire(%this, %obj, %slot)
{
	DoMeleeStrike(%obj, %slot, %this.item.damage, %this.item.range, %this.item.mastery, %this.projectile);
}

// #4.

//   #4.1

datablock ItemData(blacksmithHammerItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "base/data/shapes/Hammer.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Blacksmith's Hammer";
	iconName = "base/client/ui/itemIcons/Hammer";
	doColorShift = true;
	colorShiftColor = "0.23 0.21 0.18 1";

	image = blacksmithHammerImage;
	canDrop = true;

	//dod stuff
	dodItem = true;
	//base stats
	damage = 25;
	range = 5;
	mastery = 15; //cone angle
	potential = 0;
};

datablock ShapeBaseImageData(blacksmithHammerImage)
{
	shapeFile = "base/data/shapes/Hammer.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = false;

	//eyeOffset = "0.7 1.2 -0.25";
	eyeOffset = "0 0 0";

	className = "WeaponImage";

	item = blacksmithHammerItem;
	ammo = " ";
	projectile = hammerProjectile;
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = true;
	colorShiftColor = blacksmithHammerItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "Ready";
	//stateSound[0] = swordDrawSound;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1] = true;

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "Fire";

	stateName[3] = "Fire";
	stateTransitionOnTimeout[3] = "CheckFire";
	stateTimeoutValue[3] = 0.2;
	stateFire[3] = true;
	stateAllowImageChange[3] = false;
	stateSequence[3] = "Fire";
	stateScript[3] = "onFire";
	stateWaitForTimeout[3] = true;

	stateName[4] = "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4] = "PreFire";

	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.2;
	stateAllowImageChange[5] = false;
	stateWaitForTimeout[5] = true;
	stateSequence[5] = "StopFire";
};

function blacksmithHammerImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
	%obj.schedule(200, playThread, 2, root);
}

function blacksmithHammerImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function blacksmithHammerImage::onFire(%this, %obj, %slot)
{
	DoMeleeStrike(%obj, %slot, %this.item.damage, %this.item.range, %this.item.mastery, %this.projectile, hammerHitSound);
}