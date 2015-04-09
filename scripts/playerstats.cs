////////////////////////////////////////////////////
// GameMode_dungonsopfdeth/scripts/playerstats.cs //
////////////////////////////////////////////////////

//table of contents
// #1. getDamageMultiplier
// #2. getRangeMultiplier
// #3. getMasteryMultiplier
//   #3.1 getProjectileCount

//returns an item string (or 0) corresponding to the inventory item the player is using in image slot 0
function getCurrentDodItem(%obj)
{
	return %obj.client.dodAccount.getItem(%obj.getMountedImage(0).dodItem.getName());
}

// #1.
function getDamageMultiplier(%obj)
{
	if(isObject(%acc = %obj.client.dodAccount))
	{
		return (1 + %acc.damage / 100 + getBonusDamage(getCurrentDodItem(%obj)) / 100);
	}
	return 1;
}

// #2.
function getRangeMultiplier(%obj)
{
	if(isObject(%obj.client))
	{
		return (1 + %obj.client.dodAccount.range / 100 + getBonusRange(getCurrentDodItem(%obj)) / 100);
	}
	return 1;
}

// #3.
function getMasteryMultiplier(%obj)
{
	if(isObject(%obj.client))
	{
		return (1 + %obj.client.dodAccount.mastery / 100 + getBonusMastery(getCurrentDodItem(%obj)) / 100);
	}
	return 1;
}

// #3.1
function getProjectileCount(%obj, %baseNum)
{
	if(isObject(%obj.client))
	{
		%baseNum *= getMasteryMultiplier(%obj);
		%rem = %baseNum - mFloor(%baseNum);
		%baseNum = mFloor(%baseNum);
		if(getRandom() < %rem)
		{
			%baseNum++;
		}
	}
	return %baseNum;
}