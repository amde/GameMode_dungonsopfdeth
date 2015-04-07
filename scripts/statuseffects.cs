//blind
function Player::Blind(%this, %dur, %dr)
{
	if(%this.getClassName() $= "AIplayer")
	{
	}
	else if(%this.BlindDR < 3)
	{
		if(%dr)
		{
			%dur/= mClamp(%this.BlindDR + 1, 1, 4);
			%this.BlindDR++;
			%this.schedule(15000 + %dur, BlindDRdec);
		}
		%this.mountImage(DarkBlindPlayerImage, 1);
		%this.schedule(%dur, endBlind);
	}
}

function Player::EndBlind(%this)
{
	if(%this.getMountedImage(1) == nameToID(DarkBlindPlayerImage))
	{
		%this.unmountImage(1);
		return true;
	}
	return false;
}

function Player::BlindDRDec(%this)
{
	%this.BlindDR--;
}

//stun
function Player::Stun(%this, %dur, %dr)
{
	if(%this.getClassName() $= "AIplayer")
	{
	}
	else if(%this.StunDR <= 3)
	{
		if(%dr)
		{
			%dur/= mClamp(%this.StunDR + 1, 1, 4);
			%this.StunDR++;
			%this.schedule(15000 + %dur, StunDRDec);
		}
		%this.stunned = true;
		if(isEventPending(%this.unstunSched))
		{
			cancel(%this.unstunSched);
		}
		%this.unstunSched = %this.schedule(%dur, Unstun);
		if(isEventPending(%client.stunSched))
		{
			cancel(%client.stunSched);
		}
		StunSched(%this.client, %this.unstunSched);
	}
}

function StunSched(%client, %sched)
{
	%dur = getTimeRemaining(%sched) / 1000;
	if(!isObject(%client))
	{
		return;
	}
	if(!isObject(%obj = %client.player) || %obj.getState() $= "Dead") //dead part is better done in a package
	{
		return;
	}
	if(%client.getControlObject() != (%cam = %client.camera))
	{
		%client.setControlObject(%cam);
		%cam.setMode("Corpse", %obj);
	}
	commandToClient(%client, 'centerPrint', "<color:FF0000>Stunned! " @ mFloatLength(%dur, 1), 0.2);
	if(%dur > 0.1)
	{
		%client.stunSched = schedule(100, 0, StunSched, %client, %sched);
	}
}

function Player::StunDRDec(%this)
{
	%this.StunDR--;
}

function Player::Unstun(%this)
{
	if(isObject(%client = %this.client) && %client.getControlObject() == %client.camera)
	{
		%client.setControlObject(%this);
	}
	%this.stunned = false;
}