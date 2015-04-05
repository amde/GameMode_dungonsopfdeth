////////////////////////////////////////////////
// GameMode_dungonsopfdeth/scripts/effects.cs //
////////////////////////////////////////////////

// table of contents
// #1.0 arcane
//   #1.1 arcane barrage
//   #1.2 arcane blast
//   #1.3 arcane missiles
// #2.0
//   #2.1 shadowflame
//   #2.2 twilight
//   #2.3 mud
//   #2.4 spirit bom


// #1.0
// #1.1
datablock AudioProfile(ArcaneBarrage_Loop)
{
   filename    = "Add-Ons/GameMode_dungonsopfdeth/sounds/ArcaneBarrage_Loop.wav";
   description = AudioCloseLooping3d;
   preload     = true;
};
datablock AudioProfile(ArcaneBarrage_Impact)
{
   filename    = "Add-Ons/GameMode_dungonsopfdeth/sounds/ArcaneBarrage_Impact.wav";
   description = AudioClose3d;
   preload     = true;
};

datablock ParticleData(ABarrageParticleA)
{
	dragCoefficient = 10;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 750;
	lifetimeVarianceMS = 50;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "1 1 1 1";
	colors[1] = "1 0.3 1 0.8";
	colors[2] = "0.5 0.25 0 0.1";
	sizes[0] = 1;
	sizes[1] = 0.9;
	sizes[2] = 0.3;
	times[0] = 0;
	times[1] = 0.25;
	times[2] = 1;
};

datablock ParticleData(ABarrageParticleB : ABarrageParticleA)
{
	colors[1] = "0.5 0.3 1 0.8";
	colors[2] = "0 0.25 0.5 0.1";
};

datablock ParticleEmitterData(ArcaneBarrageEmitterA)
{
	ejectionPeriodMS = 25;
	periodVarianceMS = 5;
	ejectionVelocity = 12;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 75;
	thetaMax = 90;
	phiReferenceVel = 1000;
	phiVariance = 2;
	overrideAdvance = false;
	particles = "ABarrageParticleA";
	uiName = "Arcane Barrage A";
};

datablock ParticleEmitterData(ArcaneBarrageEmitterB : ArcaneBarrageEmitterA)
{
	ejectionVelocity = -12;
	particles = "ABarrageParticleB";
	uiName = "Arcane Barrage B";
};

datablock ParticleData(ArcaneExplosionParticle)
{
	dragCoefficient = 10;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0.5;
	lifetimeMS = 100;
	lifetimeVarianceMS = 0;
	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
	useInvAlpha = 0;
	textureName = "Add-Ons/GameMode_dungonsopfdeth/models/Arcane";
	colors[0] = "0.8 0.4 1 0.5";
	colors[1] = "0.8 0.4 1 0.5";
	colors[2] = "0.8 0.4 1 0.5";
	sizes[0] = 4.9;
	sizes[1] = 5;
	sizes[2] = 4.9;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData(ArcaneExplosionEmitter)
{
	ejectionPeriodMS = 30;
	periodVarianceMS = 0;
	ejectionVelocity = 0.5;
	velocityVariance = 0.3;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	orientParticles = 0;
	orientOnVelocity = 1;
	particles = "ArcaneExplosionParticle";
	uiName = "Arcane Explosion";
};


datablock ParticleData(SparkleParticle)
{
	dragCoefficient = 10;
	windCoefficient = 0;
	gravityCoefficient = 0.1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 250;
	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
	useInvAlpha = false;
	textureName = "Add-Ons/GameMode_dungonsopfdeth/models/flare";
	colors[0] = "1 0.75 0 1";
	colors[1] = "1 0.75 0 0";
	sizes[0] = 0.5;
	sizes[1] = 0;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(SparkleEmitter)
{
	ejectionPeriodMS = 20;
	periodVarianceMS = 10;
	ejectionVelocity = 5;
	velocityVariance = 3;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	particles = "SparkleParticle";
	uiName = "Sparkles";
};

datablock ExplosionData(ArcaneExplosion)
{
	//soundProfile = "";

	lifeTimeMS = 300;

	particleEmitter = SparkleEmitter;
	particleDensity = 20;
	particleRadius = 1;

	emitter[0] = ArcaneExplosionEmitter;

	faceViewer = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "10 11 10";
	camShakeAmp = "1 1 1";
	camShakeDuration = 0.5;
	camShakeRadius = 10.0;

	lightStartRadius = 5;
	lightEndRadius = 0;
	lightStartColor = "0.8 0.4 1";
	lightEndColor = "0.8 0.1 1";
};

datablock ProjectileData(arcaneBarrageProjectileA)
{
	projectileShapeName = "base/data/shapes/empty.dts";

	brickExplosionRadius = 1;
	brickExplosionImpact = true;
	brickExplosionForce = 30;
	brickExplosionMaxVolume = 4;
	brickExplosionMaxVolumeFloating = 7;

	impactImpulse = 400;
	verticalImpulse = 200;
	explosion = ArcaneExplosion;
	sound = ArcaneBarrage_Loop;
	particleEmitter = "ArcaneBarrageEmitterA";

	muzzleVelocity = 200;
	velInheritFactor = 0;

	armingDelay = 0;
	lifetime = 10000;
	fadeDelay = 9500;
	bounceElasticity = 0.5;
	bounceFriction = 0.2;
	isBallistic = false;
	gravityMod = 0;

	hasLight = true;
	lightRadius = 2;
	lightColor = "0.8 0.4 1";

	uiName = "Arcane Barrage A";
};
function arcaneBarrageProjectileA::onExplode(%this, %obj, %fade, %pos)
{
	if(%obj.doSound)
	{
		serverPlay3D(ArcaneBarrage_Impact, %pos);
	}
	parent::onExplode(%this, %obj, %fade, %pos);
}

function arcaneBarrageProjectileA::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	InitContainerRadiusSearch(%pos, 2, $TypeMasks::PlayerObjectType);
	while(%hit = containerSearchNext())
	{
		if(minigameCanDamage(%caster, %hit))
		{
			%hit.damage(%obj.sourceObject, %pos, 30 * (1 + %obj.sourceObject.ABstacks * 0.3), $DamageType::Direct);
			%obj.sourceObject.ABstacks = 0;
		}
	}
}
datablock ProjectileData(arcaneBarrageProjectileB : arcaneBarrageProjectileA)
{
	explosion = "";
	particleEmitter = "ArcaneBarrageEmitterB";
	sound = "";
	hasLight = false;
	uiName = "Arcane Barrage B";
};
function arcaneBarrageProjectileB::Damage()
{
}

// #1.2

// #1.3

datablock AudioProfile(ArcaneMissile_Loop)
{
   filename    = "Add-Ons/GameMode_dungonsopfdeth/sounds/ArcaneMissile_Loop.wav";
   description = AudioCloseLooping3d;
   preload     = true;
};
datablock AudioProfile(ArcaneMissile_Impact)
{
   filename    = "Add-Ons/GameMode_dungonsopfdeth/sounds/ArcaneMissile_Impact.wav";
   description = AudioClose3d;
   preload     = true;
};

datablock ParticleData(ArcaneMissileTrailParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0.1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 0;
	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
	useInvAlpha = false;
	textureName = "base/data/particles/dot";
	colors[0] = "0.8 0.1 1 0.8";
	colors[1] = "0.8 0.1 1 0";
	sizes[0] = 0.2;
	sizes[1] = 0;
	times[0] = 0;
	times[1] = 1;
};
datablock ParticleEmitterData(ArcaneMissileTrailEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	velocityVariance = 0;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	particles = "ArcaneMissileTrailParticle";
	uiName = "Arcane Missile Trail";
};

datablock ProjectileData(arcaneMissileProjectileA)
{
	projectileShapeName = "Add-Ons/GameMode_dungonsopfdeth/models/arcaneMissile.dts";

	brickExplosionRadius = 1;
	brickExplosionImpact = true;
	brickExplosionForce = 10;
	brickExplosionMaxVolume = 2;
	brickExplosionMaxVolumeFloating = 3;

	impactImpulse = 400;
	verticalImpulse = 200;
	explosion = "";
	sound = ArcaneMissile_Loop;
	particleEmitter = "ArcaneMissileTrailEmitter";

	muzzleVelocity = 200;
	velInheritFactor = 0;

	armingDelay = 0;
	lifetime = 8000;
	fadeDelay = 7500;
	bounceElasticity = 0.999;
	bounceFriction = 0.001;
	isBallistic = false;
	gravityMod = 0;

	hasLight = true;
	lightRadius = 1.5;
	lightColor = "0.8 0.1 1";

	uiName = "Arcane Missile A";
};
function arcaneMissileProjectileA::onExplode(%this, %obj, %fade, %pos)
{
	parent::onExplode(%this, %obj, %fade, %pos);
	%p = new Projectile()
	{
		datablock = ArcaneBarrageProjectileA;
		initialPosition = %pos;
		intiialVelocity = "0 0 0";
		scale = vectorScale(%obj.getScale(), 0.4);
	};
	%p.explode();
	serverPlay3D(ArcaneMissile_Impact, %pos);
}

function arcaneMissileProjectileA::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
	cancel(%obj.homeSched);
	%obj.explode();
}

function arcaneMissileProjectileA::Damage(%this, %obj, %col, %fade, %pos, %normal)
{
	%col.damage(%obj.sourceObject, %pos, (20/3) * (1 + %obj.sourceObject.ABstacks * 0.3), $DamageType::Direct);
	schedule(100, 0, eval, %obj.sourceObject @ ".ABstacks = 0;");
}

datablock ProjectileData(arcaneMissileProjectileB : arcaneMissileProjectileA)
{
	explosion = "";
	particleEmitter = "SparkleEmitter";
	sound = "";
	hasLight = false;
	uiName = "Arcane Missile B";
};
function arcaneMissileProjectileB::Damage()
{
}

// #2.0
// #2.1
datablock AudioProfile(Shadowflame_Cast)
{
   filename    = "Add-Ons/GameMode_dungonsopfdeth/sounds/Shadowflame_Cast.wav";
   description = AudioClose3d;
   preload     = true;
};

datablock ParticleData(ShadowflameParticle)
{
	dragCoefficient = 8;
	windCoefficient = 0;
	gravityCoefficient = 0.5;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0.5;
	lifetimeMS = 500;
	lifetimeVarianceMS = 100;
	spinSpeed = 0;
	spinRandomMin = -500;
	spinRandomMax = 500;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	animTexName[0] = "base/data/particles/cloud";
	colors[0] = "0 0 1 1";
	colors[1] = "0.1 0 0.8 0.4";
	colors[2] = "0.8 0.1 1 0";
	sizes[0] = 0.5;
	sizes[1] = 1;
	sizes[2] = 0.5;
	times[0] = 0;
	times[1] = 0.1;
	times[2] = 1;
};
datablock ParticleEmitterData(ShadowflameEmitter)
{
   ejectionPeriodMS = 3;
   periodVarianceMS = 1;
   ejectionVelocity = 25;
   velocityVariance = 5;
   ejectionOffset = 0;
   thetaMin = 0;
   thetaMax = 45;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvance = 0;
   particles = "ShadowflameParticle";
   uiName = "Shadowflame Breath";
};

datablock ShapeBaseImageData(ShadowflameBreathImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = $HeadSlot;
	offset = "0 0.5 0.3";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = "";
	ammo = " ";
	projectile = " ";
	projectileType = Projectile;

	melee = false;
	armReady = false;

	stateName[0]                = "Activate";
	stateEmitter[0]             = ShadowflameEmitter;
	stateEmitterTime[0]         = 10;
	stateSound[0]               = Shadowflame_Cast;
	stateTimeoutValue[0]        = 10;
	stateTransitionOnTimeout[0] = "Activate";
};

datablock AudioProfile(Shadow_Loop)
{
   filename    = "Add-Ons/GameMode_dungonsopfdeth/sounds/Shadow_Loop.wav";
   description = AudioCloseLooping3d;
   preload     = true;
};

// #2.2
datablock AudioProfile(Twilight_Impact)
{
   filename    = "Add-Ons/GameMode_dungonsopfdeth/sounds/Twilight_Impact.wav";
   description = AudioClose3d;
   preload     = true;
};

datablock ParticleData(TwilightParticle)
{
	dragCoefficient = 1;
	windCoefficient = 0;
	gravityCoefficient = -0.4;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 250;
	spinSpeed = 0;
	spinRandomMin = -500;
	spinRandomMax = 500;
	useInvAlpha = 0;
	textureName = "base/data/particles/cloud";
	colors[0] = "1 0 0 1";
	colors[1] = "1 0 0.2 0.7";
	colors[2] = "0 0 1 1";
	sizes[0] = 0.3;
	sizes[1] = 1;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.4;
	times[2] = 1;
};
datablock ParticleEmitterData(TwilightEmitter)
{
	ejectionPeriodMS = 15;
	periodVarianceMS = 3;
	ejectionVelocity = 1;
	velocityVariance = 0.5;
	ejectionOffset = 0.1;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	particles = "TwilightParticle";
	uiName = "Twilight Flame";
};

datablock ExplosionData(TwilightExplosion : ArcaneExplosion)
{
	soundProfile = Twilight_Impact;

	particleEmitter = TwilightEmitter;
	emitter[0] = TwilightEmitter;

	lightStartColor = "1 0 0";
	lightEndColor = "0 0 1";
};

datablock ProjectileData(TwilightBoltProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";

	brickExplosionRadius = 1;
	brickExplosionImpact = true;
	brickExplosionForce = 10;
	brickExplosionMaxVolume = 2;
	brickExplosionMaxVolumeFloating = 3;

	impactImpulse = 400;
	verticalImpulse = 200;
	explosion = TwilightExplosion;
	sound = Shadow_Loop;
	particleEmitter = "TwilightEmitter";

	muzzleVelocity = 200;
	velInheritFactor = 0;

	armingDelay = 0;
	lifetime = 8000;
	fadeDelay = 7500;
	bounceElasticity = 0.999;
	bounceFriction = 0.001;
	isBallistic = false;
	gravityMod = 0;

	hasLight = true;
	lightRadius = 2;
	lightColor = "1 0 0.5";

	uiName = "Twilight Bolt";
};
function TwilightBoltProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	InitContainerRadiusSearch(%pos, 2, $TypeMasks::PlayerObjectType);
	while(%hit = containerSearchNext())
	{
		if(minigameCanDamage(%caster, %hit))
		{
			%dmg = 10 + %hit.getDamagePercent() * 40; //10-50 damage, higher as the victim takes more damage
			%dmg*= %obj.dmgMod;
			%hit.damage(%obj.sourceObject, %pos, %dmg, $DamageType::Direct);
		}
	}
}

// #2.3

datablock AudioProfile(Stone_Impact)
{
   filename    = "Add-Ons/GameMode_dungonsopfdeth/sounds/Stone_Impact.wav";
   description = AudioDefault3d;
   preload     = true;
};

datablock ParticleEmitterData(HeavyFogEmitter : FogEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 1;
	thetaMax = 180;
	uiName = "Fog B (Heavy)";
};

datablock ParticleData(StoneParticle)
{
	dragCoefficient = 0.25;
	windCoefficient = 0;
	gravityCoefficient = 1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1100;
	lifetimeVarianceMS = 300;
	spinSpeed = 0;
	spinRandomMin = -500;
	spinRandomMax = 500;
	useInvAlpha = true;
	textureName = "base/data/particles/cloud";
	colors[0] = "0.35 0.15 0 1";
	colors[1] = "0.35 0.15 0 1";
	colors[2] = "0.35 0.15 0 0";
	sizes[0] = 0.6;
	sizes[1] = 0.5;
	sizes[2] = 0.3;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData(StoneEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 4;
	ejectionOffset = 0.25;
	thetaMin = 1;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	particles = "StoneParticle";
	useEmitterSizes = false;
	useEmitterColors = true;
	uiName = "Stone Explosion";
};

datablock ParticleEmitterData(MudEmitter : StoneEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 2;
	ejectionVelocity = 1.5;
	velocityVariance = 0.3;
	ejectionOffset = 0;
	uiName = "Mud Ball";
};

datablock ProjectileData(MudProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";

	brickExplosionRadius = 1;
	brickExplosionImpact = true;
	brickExplosionForce = 10;
	brickExplosionMaxVolume = 2;
	brickExplosionMaxVolumeFloating = 3;

	impactImpulse = 400;
	verticalImpulse = 200;
	explosion = "";
	sound = "";
	particleEmitter = "MudEmitter";

	muzzleVelocity = 200;
	velInheritFactor = 0;

	armingDelay = 0;
	lifetime = 8000;
	fadeDelay = 7500;
	bounceElasticity = 0.999;
	bounceFriction = 0.001;
	isBallistic = true;
	gravityMod = 1;

	hasLight = false;
	lightRadius = 2;
	lightColor = "0 0 0";

	uiName = "Mudball";
};

function MudProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	InitContainerRadiusSearch(posFromRaycast(%ray), 3, $TypeMasks::PlayerObjectType);
	while(%hit = containerSearchNext())
	{
		if(minigameCanDamage(%obj.sourceObject, %hit))
		{
			%hit.damage(%obj.sourceObject, %pos, 20, $DamageType::Direct);
		}
	}
	if(%obj.huge)
	{
		return;
	}
	%color = getClosestPaintColor("0.35 0.15 0 1");
	if(%obj.tiny)
	{
		%brick = new FxDTSbrick()
		{
			datablock = Brick8xCubeData;
			position = %pos;
			rotation = "0 0 1 " @ 1 * 90;
			colorID = %color;
			scale = "1 1 1";
			angleID = 1;
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
		%brick.schedule(10000, fakeKillBrick, "0 0 10", 1);
		%brick.schedule(11000, delete);
		return;
	}
	%offset[0] = "0 0 1";
	%offset[1] = "0 0 -1";
	%offset[2] = "1 0 0";
	%offset[3] = "-1 0 0";
	%offset[4] = "0 1 0";
	%offset[5] = "0 -1 0";
	for(%i = 0; %i < 6; %i++)
	{
		%brick = new FxDTSbrick()
		{
			datablock = Brick4xCubeData;
			position = vectorAdd(%pos, vectorScale(%offset[%i], 2));
			rotation = "0 0 1 " @ 1 * 90;
			colorID = %color;
			scale = "1 1 1";
			angleID = 1;
			colorFxID = 0;
			shapeFxID = 0;
			isPlanted = 1;
			magic = 1;
		};
		%brick.plant();
		if(%i == 0)
		{
			%brick.setEmitter(StoneEmitter);
			serverPlay3d(Stone_Impact, %pos);
			%brick.schedule(40, setEmitter, HeavyFogEmitter);
			%brick.schedule(100, setEmitter, 0);
		}
		%brick.schedule(10000, fakeKillBrick, vectorScale(%offset[%i], 10), 1);
		%brick.schedule(11000, delete);
	}
}

function MudProjectile::onExplode(%this, %obj, %pos, %fade)
{
	parent::onExplode(%this, %obj, %pos, %fade);
	if(%obj.huge)
	{
		serverPlay3d(Stone_Impact, %pos);
		%vec[0] = vectorScale(vectorNormalize(getRandom()-0.5 SPC getRandom()-0.5 SPC getRandom()-1), 20);
		%vec[1] = vectorScale(vectorNormalize(getRandom()-0.5 SPC getRandom()-0.5 SPC getRandom()-1), 20);
		%vec[2] = vectorScale(vectorNormalize(getRandom()-0.5 SPC getRandom()-0.5 SPC getRandom()-0.8), 20);
		for(%i = 0; %i < 3; %i++)
		{
			%p = new Projectile()
			{
				datablock = MudProjectile;
				initialPosition = vectorAdd(%pos, vectorScale(vectorNormalize(getRandom()-0.5 SPC getRandom()-0.5 SPC getRandom() + 0.1), 2));
				initialVelocity = %vec[%i];
				sourceObject = %caster;
				sourceSlot = 0;
				scale = "1 1 1";
				huge = false;
				tiny = getRandom(0, 1);
			};
		}
	}
}

// #2.4

datablock AudioProfile(Shadow_Impact)
{
   filename    = "Add-Ons/GameMode_dungonsopfdeth/sounds/Shadow_Impact.wav";
   description = AudioDefault3d;
   preload     = true;
};

datablock ParticleData(spiritBombParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 100;
	lifetimeVarianceMS = 15;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = false;
	textureName = "base/data/particles/dot";
	animTexName[0] = "base/data/particles/dot";
	colors[0] = "0.5 0.5 0.5 0.5";
	colors[1] = "0.8 0.7 0.6 0.5";
	colors[2] = "1.0 0.1 0.1 0.5";
	colors[3] = "1.0 1.0 1.0 1.0";
	sizes[0] = 0.2;
	sizes[1] = 0.9;
	sizes[2] = 0.3;
	sizes[3] = 1;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
	times[3] = 2;
};

datablock ParticleEmitterData(spiritBombEmitter)
{
   ejectionPeriodMS = 12;
   periodVarianceMS = 0;
   ejectionVelocity = 0;
   velocityVariance = 0;
   ejectionOffset = 0;
   thetaMin = 0;
   thetaMax = 0;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvance = 0;
   orientParticles = 0;
   orientOnVelocity = 1;
   particles = "spiritBombParticle";
   lifetimeMS = 0;
   lifetimeVarianceMS = 0;
   useEmitterSizes = false;
   useEmitterColors = false;
   uiName = "Spirit Bomb";
};

datablock ParticleData(spiritBombExplosionParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = -5;
	lifetimeMS = 700;
	lifetimeVarianceMS = 100;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = false;
	textureName = "base/data/particles/thinRing";
	colors[0] = "1 0.8 0.7 0";
	colors[1] = "1 0.7 0.6 0.8";
	colors[2] = "1 0.6 0.5 0.7";
	sizes[0] = 2;
	sizes[1] = 1.5;
	sizes[2] = 0.5;
	sizes[3] = 1;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData(spiritBombExplosionEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0;
	ejectionOffset = 3.3;
	thetaMin = 85;
	thetaMax = 95;
	phiReferenceVel = 0;
	phiVariance = 360;
	particles = "spiritBombExplosionParticle";
	useEmitterSizes = false;
	useEmitterColors = false;
	uiName = "Spirit Bomb Explosion";
};

datablock ExplosionData(spiritBombExplosion)
{
	soundProfile = Shadow_Impact;

	lifeTimeMS = 500;

	//particleEmitter = spiritBombExplosionEmitter;
	//particleDensity = 20;
	//particleRadius = 3.3;

	emitter[0] = spiritBombExplosionEmitter;

	faceViewer = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "10 11 10";
	camShakeAmp = "1 1 1";
	camShakeDuration = 0.5;
	camShakeRadius = 10.0;

	lightStartRadius = 8;
	lightEndRadius = 5;
	lightStartColor = "0.7 0.6 0.5";
	lightEndColor = "0 0 0";
};

datablock ProjectileData(spiritBombProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";

	brickExplosionRadius = 3;
	brickExplosionImpact = true;
	brickExplosionForce = 30;
	brickExplosionMaxVolume = 30;
	brickExplosionMaxVolumeFloating = 60;

	impactImpulse = 400;
	verticalImpulse = 200;
	explosion = spiritBombExplosion;
	sound = Shadow_Loop;
	particleEmitter = spiritBombEmitter;

	muzzleVelocity = 10;
	velInheritFactor = 0;

	armingDelay = 0;
	lifetime = 8000;
	fadeDelay = 7500;
	bounceElasticity = 0.999;
	bounceFriction = 0.001;
	isBallistic = false;
	gravityMod = 0;

	hasLight = true;
	lightRadius = 2;
	lightColor = "0.7 0.6 0.5";

	uiName = "Spirit Bomb";
};

function spiritBombProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if(minigameCanDamage(%obj.sourceObject, %hit) && %hit.getType() & $Typemasks::PlayerObjectType)
	{
		%hit.schedule(500, damage, %obj.sourceObject, %pos, %obj.damage, $DamageType::Direct);
	}
}