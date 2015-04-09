///////////////////////////////////////////////
// GameMode_dungonsopfdeth/scripts/ranged.cs //
///////////////////////////////////////////////

//table of contents
// #1. base datablocks
// #2. base functionality
// #3. bows
//   #3.1 ash bow

//sounds
datablock AudioProfile(arrowHitSound)
{
	filename = "Add-Ons/Weapon_Bow/arrowHit.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(bowFireSound)
{
	filename = "Add-Ons/Weapon_Bow/bowFire.wav";
	description = AudioClosest3d;
	preload = true;
};

//particles
datablock ParticleData(arrowTrailParticle)
{
	dragCoefficient = 3.0;
	windCoefficient = 0.0;
	gravityCoefficient = 0.0;
	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;
	lifetimeMS = 200;
	lifetimeVarianceMS = 0;
	spinSpeed = 10.0;
	spinRandomMin = -50.0;
	spinRandomMax = 50.0;
	useInvAlpha = false;
	animateTexture = false;

	textureName = "base/data/particles/dot";

	colors[0] = "1 1 1 0.2";
	colors[1] = "1 1 1 0.0";
	sizes[0] = 0.2;
	sizes[1] = 0.01;
	times[0] = 0.0;
	times[1] = 1.0;
};

datablock ParticleEmitterData(arrowTrailEmitter)
{
	ejectionPeriodMS = 2;
	periodVarianceMS = 0;

	ejectionVelocity = 0;
	velocityVariance = 0;

	ejectionOffset = 0;

	thetaMin = 0;
	thetaMax = 90;

	particles = arrowTrailParticle;

	useEmitterColors = true;
	uiName = "Arrow Trail";
};

//effects
datablock ParticleData(arrowStickExplosionParticle)
{
	dragCoefficient = 5;
	gravityCoefficient = 0.1;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	textureName = "base/data/particles/chunk";
	spinSpeed = 10.0;
	spinRandomMin = -50.0;
	spinRandomMax = 50.0;
	colors[0] = "0.9 0.9 0.6 0.9";
	colors[1] = "0.9 0.5 0.6 0.0";
	sizes[0] = 0.25;
	sizes[1] = 0.0;
};

datablock ParticleEmitterData(arrowStickExplosionEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0.0;
	ejectionOffset = 0.0;
	thetaMin = 80;
	thetaMax = 80;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "arrowStickExplosionParticle";
	useEmitterColors = true;
	uiName = "Arrow Stick";
};

datablock ExplosionData(arrowStickExplosion)
{
	soundProfile = arrowHitSound;

	lifeTimeMS = 150;

	particleEmitter = arrowStickExplosionEmitter;
	particleDensity = 10;
	particleRadius = 0.2;

	emitter[0] = "";

	faceViewer = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10.0;

	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0 0 0";
	lightEndColor = "0 0 0";
};

datablock ParticleData(arrowExplosionParticle)
{
	dragCoefficient = 8;
	gravityCoefficient = -0.3;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	textureName = "base/data/particles/cloud";
	spinSpeed = 10.0;
	spinRandomMin = -50.0;
	spinRandomMax = 50.0;
	colors[0] = "0.5 0.5 0.5 0.9";
	colors[1] = "0.5 0.5 0.5 0.0";
	sizes[0] = 0.45;
	sizes[1] = 0.0;
	useInvAlpha = true;
};
datablock ParticleEmitterData(arrowExplosionEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	velocityVariance = 0.0;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax  = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "arrowExplosionParticle";

	useEmitterColors = true;
	uiName = "Arrow Vanish";
};
datablock ExplosionData(arrowExplosion)
{
	soundProfile = "";

	lifeTimeMS = 50;

	emitter[0] = arrowExplosionEmitter;

	faceViewer = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10.0;

	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0 0 0";
	lightEndColor = "0 0 0";
};

AddDamageType("ArrowDirect", '<bitmap:add-ons/Weapon_Bow/CI_arrow> %1', '%2 <bitmap:add-ons/Weapon_Bow/CI_arrow> %1', 0.5, 1);

datablock ProjectileData(arrowProjectile)
{
	projectileShapeName = "Add-Ons/Weapon_Bow/arrow.dts";

	directDamage = 30;
	directDamageType = $DamageType::ArrowDirect;

	radiusDamage = 0;
	damageRadius = 0;
	radiusDamageType = $DamageType::ArrowDirect;

	explosion = arrowExplosion;
	stickExplosion = arrowStickExplosion;
	bloodExplosion = arrowStickExplosion;
	particleEmitter = arrowTrailEmitter;
	explodeOnPlayerImpact = true;
	explodeOnDeath = true;  

	armingDelay = 60000;
	lifetime = 60000;
	fadeDelay = 60000;

	isBallistic = true;
	bounceAngle = 170;
	minStickVelocity = 10;
	bounceElasticity = 0.2;
	bounceFriction = 0.01;   
	gravityMod = 0.3;

	hasLight = false;
	lightRadius = 3.0;
	lightColor = "0 0 0.5";

	muzzleVelocity = 65;
	velInheritFactor = 1;

	uiName = "Arrow";
};

// #2.

function applySpread(%vec, %amt)
{
	%vec = vectorScale(vectorNormalize(%vec), 100 / %amt);
	%rVec = getRandom()-0.5 SPC getRandom()-0.5 SPC getRandom()-0.5;
	return vectorNormalize(vectorAdd(%vec, vectorNormalize(%rVec)));
}

function DoBowLoose(%obj, %slot, %proj, %speed, %damage, %projCount, %spread)
{
	if(%projCount < 1)
	{
		%projCount = 1;
	}
	%projCount = getProjectileCount(%obj, %projCount);
	%damage *= getDamageMultiplier(%obj);
	%speed *= getRangeMultiplier(%obj);
	new Projectile()
	{
		datablock = %proj;
		initialPosition = vectorAdd(%obj.getMuzzlePoint(%slot), %obj.getMuzzleVector(%slot));
		initialVelocity = vectorScale(%obj.getMuzzleVector(%slot), %speed);
		scale = "1 1 1";
		sourceSlot = %slot;
		sourceObject = %obj;
		damage = %damage;
	};
	for(%i = 1; %i < %projCount; %i++)
	{
		new Projectile()
		{
			datablock = %proj;
			initialPosition = vectorAdd(%obj.getMuzzlePoint(%slot), %obj.getMuzzleVector(%slot));
			initialVelocity = vectorScale(applySpread(%obj.getMuzzleVector(%slot), %spread), %speed);
			scale = "1 1 1";
			sourceSlot = %slot;
			sourceObject = %obj;
			damage = %damage;
		};
	}
}


// #3.

// #3.1
datablock ItemData(ashBowItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "Add-Ons/Weapon_Bow/bow.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Ash Bow";
	iconName = "Add-Ons/Weapon_Bow/icon_bow";
	doColorShift = true;
	colorShiftColor = "0.82 0.71 0.53 1";

	image = ashBowImage;
	canDrop = true;

	//dod stuff
	dodItem = dodItem_AshBow;
};

datablock ShapeBaseImageData(ashBowImage)
{
	shapeFile = ashBowItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = eulerToMatrix( "0 0 10" );

	correctMuzzleVector = true;

	className = "WeaponImage";

	item = ashBowItem;
	dodItem = ashBowItem.dodItem;
	ammo = " ";
	projectile = arrowProjectile;
	projectileType = Projectile;

	melee = false;
	armReady = true;

	doColorShift = true;
	colorShiftColor = ashBowItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = weaponSwitchSound;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = true;

	stateName[2] = "Fire";
	stateTransitionOnTimeout[2] = "Reload";
	stateTimeoutValue[2] = 0.05;
	stateFire[2] = true;
	stateAllowImageChange[2] = false;
	stateSequence[2] = "Fire";
	stateScript[2] = "onFire";
	stateWaitForTimeout[2] = true;
	stateSound[2] = bowFireSound;

	stateName[3] = "Reload";
	stateSequence[3] = "Reload";
	stateAllowImageChange[3] = false;
	stateTimeoutValue[3] = 0.5;
	stateWaitForTimeout[3] = true;
	stateTransitionOnTimeout[3] = "Check";

	stateName[4] = "Check";
	stateTransitionOnTriggerUp[4] = "StopFire";
	stateTransitionOnTriggerDown[4] = "Fire";

	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.2;
	stateAllowImageChange[5] = false;
	stateWaitForTimeout[5] = true;
	stateScript[5] = "onStopFire";
};

function ashBowImage::onFire(%this, %obj, %slot)
{
	DoBowLoose(%obj, %slot, %this.projectile, 25, 40, 1, 10);
}