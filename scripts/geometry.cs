//table of contents
//#1.
//  #1.1 getVectorAngle
//  #1.2 player::getRelativePosition
//  #1.3 player::getRelativeVector
//  #1.4 conal raycasts

// #1.1
function getVectorAngle(%vec1, %vec2)
{
	%vec1n = VectorNormalize(%vec1);
	%vec2n = VectorNormalize(%vec2);

	%vdot = VectorDot(%vec1n, %vec2n);
	%angle = mACos(%vdot);

	// convert to degrees and return
	%degangle = mRadToDeg(%angle);
	return %degangle;
}

// #1.2
function Player::getRelativePosition(%player,%vec,%slot)
{
	if(getWordCount(%vec) != 3 || !isObject(%player))
	{
		return;
	}
	// X is left/right
	// Y is forward/back
	// Z is up/down
	%x = getWord(%vec,0);
	%y = getWord(%vec,1);
	%z = getWord(%vec,2);

	%scale = %player.getScale();
	%sx = getWord(%scale,0);
	%sy = getWord(%scale,1);
	%sz = getWord(%scale,2);

	%x *= %sx;
	%y *= %sy;
	%z *= %sz;

	if(%slot $= "eye")
	{
		%yv = vectorNormalize(%player.getEyeVector());
	} else {
		%yv = vectorNormalize(%player.getForwardVector());
	}
	%zv = vectorNormalize(%player.getUpVector());
	%xv = vectorNormalize(vectorCross(%yv,%zv));

	if(%slot $= "eye")
	{
		%pos = %player.getEyePoint();
	} else if(%slot !$= "") {
		%pos = getWords(%player.getSlotTransform(%slot),0,2);
	} else {
		%pos = %player.getHackPosition();
	}

	%pos = vectorAdd(%pos,vectorScale(%xv,%x));
	%pos = vectorAdd(%pos,vectorScale(%yv,%y));
	%pos = vectorAdd(%pos,vectorScale(%zv,%z));

	return %pos;
}

// #1.3
function Player::getRelativeVector(%player,%vec,%slot)
{
	%pos2 = %player.getRelativePosition(%vec,%slot);
	if(%slot $= "eye")
	{
		%pos = %player.getEyePoint();
	} else if(%slot !$= "") {
		%pos = getWords(%player.getSlotTransform(%slot),0,2);
	} else {
		%pos = %player.getHackPosition();
	}
	return vectorNormalize(vectorSub(%pos2,%pos));
}

// #1.4
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