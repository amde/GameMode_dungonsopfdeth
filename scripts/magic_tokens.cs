/////////////////////////////////////////////////////
// GameMode_dungonsopfdeth/scripts/magic_tokens.cs //
/////////////////////////////////////////////////////
// Brick-based magic token system thing

//#1.
//	#1.1
function TokensCanStack(%a, %b)
{
	if(%a $= %b || strLen(%a @ %b) != 2)
	{
		return 0;
	}
	return 1;
}

function GetTokenBitmask(%tok)
{
	for(%i = 0; %i < strLen(%tok); %i++)
	{
		%bitmask = %bitmask | $dod::Bitmask[getSubStr(%tok, %i, 1)];
	}
	return %bitmask;
}

function GameConnection::AddToken(%this, %tok)
{
	if(strLen(%this.tokens) >= 3 || %this.clearTokens || %tok $= "-1")
	{
		%this.clearTokens = 0;
		%this.tokens = "";
	}
	if(strLen(%this.tokens) > 0 && %tok !$= "-1")
	{
		if(TokensCanStack(getSubStr(%this.tokens, strLen(%this.tokens) - 1, 1), %tok))
		{
			if(strLen(%this.tokens) > 1)
			{
				if(TokensCanStack(getSubStr(%this.tokens, 0, 1), %tok))
				{
					%this.tokens = %this.tokens @ %tok;
				}
			}
			else
			{
				%this.tokens = %this.tokens @ %tok;
			}
		}
	}
	else
	{
		%this.tokens = %this.tokens @ %tok;
	}
	%this.displayTokens();
}

function GameConnection::DisplayTokens(%this)
{
	%str = "";
	%len = strLen(%this.tokens);
	%bitmask = getTokenBitmask(%this.tokens);
	if(%len == 0)
	{
		%this.bottomPrint("\c6No tokens");
		return;
	}
	for(%i = 0; %i < %len; %i++)
	{
		%ch = getSubStr(%this.tokens, %i, 1);
		switch$(%ch)
		{
			case "F": %ch = "<color:FF0000>F";
			case "W": %ch = "<color:0000FF>W";
			case "E": %ch = "<color:804000>E";
			case "N": %ch = "<color:80FF80>N";
			case "L": %ch = "<color:FFFF00>L";
			case "T": %ch = "<color:FFFF80>T";
			case "D": %ch = "<color:404040>D";
			case "A": %ch = "<color:C5A5FF>A";
		}
		%str = %str @ %ch;
	}
	%name = $Spell[%bitmask];
	%str = %str @ "\c6: " @ (strLen(%name) ? %name : "???");
	%str = %str @ "   <font:Courier New:14>\c6Bitmask: " @ %bitmask;
	%this.bottomPrint(%str);
}

//#2.0
//	#2.1
datablock fxDTSBrickData(FireToken)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Special";
	subCategory = "Magic";
	uiName = "Fire";
	iconName = "Add-Ons/GameMode_dungonsopfdeth/icons/Fire";
};

function FireToken::onUse(%this, %obj, %slot)
{
	%obj.client.addToken("F");
}

datablock fxDTSBrickData(WaterToken)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Special";
	subCategory = "Magic";
	uiName = "Water";
	iconName = "Add-Ons/GameMode_dungonsopfdeth/icons/Water";
};

function WaterToken::onUse(%this, %obj, %slot)
{
	%obj.client.addToken("W");
}

datablock fxDTSBrickData(EarthToken)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Special";
	subCategory = "Magic";
	uiName = "Earth";
	iconName = "Add-Ons/GameMode_dungonsopfdeth/icons/Earth";
};

function EarthToken::onUse(%this, %obj, %slot)
{
	%obj.client.addToken("E");
}

datablock fxDTSBrickData(LightningToken)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Special";
	subCategory = "Magic";
	uiName = "Lightning";
	iconName = "Add-Ons/GameMode_dungonsopfdeth/icons/Lightning";
};

function LightningToken::onUse(%this, %obj, %slot)
{
	%obj.client.addToken("L");
}

datablock fxDTSBrickData(WindToken)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Special";
	subCategory = "Magic";
	uiName = "Wind";
	iconName = "Add-Ons/GameMode_dungonsopfdeth/icons/Wind";
};

function WindToken::onUse(%this, %obj, %slot)
{
	%obj.client.addToken("N");
}

datablock fxDTSBrickData(LightToken)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Special";
	subCategory = "Magic";
	uiName = "Light";
	iconName = "Add-Ons/GameMode_dungonsopfdeth/icons/Light";
};

function LightToken::onUse(%this, %obj, %slot)
{
	%obj.client.addToken("T");
}

datablock fxDTSBrickData(DarkToken)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Special";
	subCategory = "Magic";
	uiName = "Dark";
	iconName = "Add-Ons/GameMode_dungonsopfdeth/icons/Darkness";
};

function DarkToken::onUse(%this, %obj, %slot)
{
	%obj.client.addToken("D");
}

datablock fxDTSBrickData(ArcaneToken)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Special";
	subCategory = "Magic";
	uiName = "Arcane";
	iconName = "Add-Ons/GameMode_dungonsopfdeth/icons/Arcane";
};

function ArcaneToken::onUse(%this, %obj, %slot)
{
	%obj.client.addToken("A");
}

datablock fxDTSBrickData(NullToken)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Special";
	subCategory = "Magic";
	uiName = "Clear";
	iconName = "Add-Ons/GameMode_dungonsopfdeth/icons/Null";
};

function NullToken::onUse(%this, %obj, %slot)
{
	%obj.client.tokens = "";
}

//#2.
//	#2.1
function AddSpell(%bitmask, %name)
{
	$Spell[%bitmask] = %name;
}

//	#2.2

$dod::Bitmask["F"] = 1;
$dod::Bitmask["W"] = 2;
$dod::Bitmask["E"] = 4;
$dod::Bitmask["N"] = 8;
$dod::Bitmask["L"] = 16;
$dod::Bitmask["T"] = 32;
$dod::Bitmask["D"] = 64;
$dod::Bitmask["A"] = 128;

AddSpell(0, "Null");

AddSpell(1, "Fireball");
AddSpell(2, "Waterball");
AddSpell(4, "Stones");
AddSpell(8, "Wind");
AddSpell(16, "Lightningbolt");
AddSpell(32, "Light Bolt");
AddSpell(64, "Shadow Bolt");
AddSpell(128, "Arcane Missiles");

//	#2.3
AddSpell(1 | 2, "Steam");
AddSpell(1 | 4, "Magma");
AddSpell(1 | 8, "Firestorm");
AddSpell(1 | 16, "Chain Lightning");
AddSpell(1 | 32, "Holyfire");
AddSpell(1 | 64, "Shadowflame");
AddSpell(1 | 128, "Flame Arrow");

AddSpell(2 | 4, "Mudball");
AddSpell(2 | 8, "Mist");
AddSpell(2 | 16, "Storm");
AddSpell(2 | 32, "Healing Stream");
AddSpell(2 | 64, "Bloodbolt");
AddSpell(2 | 128, "Nullify");

AddSpell(4 | 8, "Dust");
AddSpell(4 | 16, "Metalbolt");
AddSpell(4 | 32, "Consecration");
AddSpell(4 | 64, "Desecration");
AddSpell(4 | 128, "Stone Shield");

AddSpell(8 | 16, "Thunderstorm");
AddSpell(8 | 32, "Righteousness");
AddSpell(8 | 64, "Shadowstorm");
AddSpell(8 | 128, "Interrupt");

AddSpell(16 | 32, "Word of Glory");
AddSpell(16 | 64, "Teeth");
AddSpell(16 | 128, "Blink");

AddSpell(32 | 64, "Twilight Bolt");
AddSpell(32 | 128, "Cleanse");

AddSpell(64 | 128, "Cloak");

AddSpell(1 | 2 | 4, "Geyser");
AddSpell(1 | 2 | 8, "Typhoon");
AddSpell(1 | 2 | 16, "Static Spray");
AddSpell(1 | 2 | 32, "Godbreath");
AddSpell(1 | 2 | 64, "Shroud");
AddSpell(1 | 2 | 128, "Arcane Barrage");

AddSpell(1 | 4 | 8, "Emberstorm");
AddSpell(1 | 4 | 16, "Slagball");
AddSpell(1 | 4 | 32, "Meteor Smite");
AddSpell(1 | 4 | 64, "");
AddSpell(1 | 4 | 128, "Lava Fissure");

AddSpell(1 | 8 | 16, "Zephyr");
AddSpell(1 | 8 | 32, "Meteor Shower");
AddSpell(1 | 8 | 64, "Chaos Blast");
AddSpell(1 | 8 | 128, "Dragon's Breath");

AddSpell(1 | 16 | 32, "Holyshock");
AddSpell(1 | 16 | 64, "Shadowfury");
AddSpell(1 | 16 | 128, "Haste");

AddSpell(1 | 32 | 64, "Twilight Inferno");
AddSpell(1 | 32 | 128, "Pillar of Truth");

AddSpell(1 | 64 | 128, "Crucio");

AddSpell(2 | 4 | 8, "Mudslide");
AddSpell(2 | 4 | 16, "Disruption");
AddSpell(2 | 4 | 32, "Glacier");
AddSpell(2 | 4 | 64, "Voodoo Bolt");
AddSpell(2 | 4 | 128, "Stoneskin");

AddSpell(2 | 8 | 16, "Thunderstorm");
AddSpell(2 | 8 | 32, "Healing Mist");
AddSpell(2 | 8 | 64, "Toxic Spray");
AddSpell(2 | 8 | 128, "Galefoot");

AddSpell(2 | 16 | 32, "Blizzard");
AddSpell(2 | 16 | 64, "Horrorcane");
AddSpell(2 | 16 | 128, "Asalraalaikum");

AddSpell(2 | 32 | 64, "Corrupt");
AddSpell(2 | 32 | 128, "Confusion");

AddSpell(2 | 64 | 128, "Edema");

AddSpell(4 | 8 | 16, "Voltaic Field");
AddSpell(4 | 8 | 32, "Sandblast");
AddSpell(4 | 8 | 64, "Blackout");
AddSpell(4 | 8 | 128, "Mana Dust");

AddSpell(4 | 16 | 32, "Artifact");
AddSpell(4 | 16 | 64, "Poison Dart");
AddSpell(4 | 16 | 128, "Lightning Rod");

AddSpell(4 | 32 | 64, "Sanctioned Ground");
AddSpell(4 | 32 | 128, "Mana Field");

AddSpell(4 | 64 | 128, "Raise Zombie");

AddSpell(8 | 16 | 32, "Light of Dawn");
AddSpell(8 | 16 | 64, "Ominous Wind");
AddSpell(8 | 16 | 128, "Improved Blink");

AddSpell(8 | 32 | 64, "Psychic Square");
AddSpell(8 | 32 | 128, "Arcane Blast");

AddSpell(8 | 64 | 128, "Counterspell");

AddSpell(16 | 32 | 64, "Thunderous Twilight");
AddSpell(16 | 32 | 128, "Smite");

AddSpell(16 | 64 | 128, "Self-Destruct");

AddSpell(32 | 64 | 128, "Spirit Bomb");