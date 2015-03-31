
//blind
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