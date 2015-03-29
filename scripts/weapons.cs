////////////////////////////////////////////////
// GameMode_dungonsopfdeth/scripts/weapons.cs //
////////////////////////////////////////////////

// table of contents
// #2.0 player::dodDamage
// #3.0 sword demonstration
//   #3.1 swordImage::onFire

// #2.0

function Player::dodDamage(%obj, %sObj, %pos, %amt, %type, %wpn)
{
	
}

// #3.0

switch(forceRequiredAddOn(Weapon_Sword))
{
   case $Error::AddOn_Disabled: sworditem.uiName = "";
   case $Error::AddOn_NotFound: echo("\c2CRITICAL ERROR: Weapon_Sword.zip is needed for this add-on!"); %error = 1;
}

// #3.1

$swordAngle = 45;
$swordSize = 1.0;

function swordImage::onFire(%this, %obj, %slot)
{
	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), GlobalStorage.KnifeRange * %scale));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%targets = conalRaycastM(%start, %obj.getEyeVector(), $swordSize * 5 * %scale, $swordangle, $Typemasks::PlayerObjectType, %obj);
	for(%i = 0; (%hit = getWord(%targets, %i)) !$= ""; %i++)
	{
		%hit.spawnExplosion(swordProjectile, %scale);
		if(minigameCanDamage(%obj, %hit))
		{
			%hit.damage(%obj, %hit.getPosition(), swordProjectile.directDamage, $DamageType::Sword);
		}
	}
}