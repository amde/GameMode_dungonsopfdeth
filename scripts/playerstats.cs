function getDamageMultiplier(%obj)
{
	if(isObject(%obj.client))
	{
		return (1 + %obj.client.dodAccount.damage / 100);
	}
	return 1;
}

function getRangeMultiplier(%obj)
{
	if(isObject(%obj.client))
	{
		return (1 + %obj.client.dodAccount.range / 100);
	}
	return 1;
}

function getMasteryMultiplier(%obj)
{
	if(isObject(%obj.client))
	{
		return (1 + %obj.client.dodAccount.mastery / 100);
	}
	return 1;
}

function getProjectileCount(%obj, %baseNum)
{
	if(isObject(%obj.client))
	{
		%baseNum *= (1 + %obj.client.dodAccount.mastery / 100);
		%rem = %baseNum - mFloor(%baseNum);
		%baseNum = mFloor(%baseNum);
		if(getRandom() < %rem)
		{
			%baseNum++;
		}
	}
	return %baseNum;
}