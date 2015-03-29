//#1.
//	#1.1
function TokensCanStack(%a, %b)
{
	if(%a $= %b || strLen(%a @ %b) != 2)
	{
		return 0;
	}
	return 1;
	switch$(%a)
	{
		case "F": %r = %b $= "w" || %b $= "n";
		case "W": %r = %b $= "f" || %b $= "l";
		case "E": %r = %b $= "n" || %b $= "l";
		case "N": %r = %b $= "f" || %b $= "e";
		case "L": %r = %b $= "e" || %b $= "w";
		case "T": %r = %b $= "d";
		case "D": %r = %b $= "t";
	}
	return !%r;
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
	%name = $SpellName[%this.tokens];
	%str = %str @ "\c6: " @ (strLen(%name) ? %name : "???");
	%str = %str @ "   <font:Courier New:14>\c6Bitmask: " @ $Spell[%this.tokens];
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
	iconName = "Add-Ons/Gamemode_Magicks/icons/Fire";
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
	iconName = "Add-Ons/Gamemode_Magicks/icons/Water";
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
	iconName = "Add-Ons/Gamemode_Magicks/icons/Earth";
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
	iconName = "Add-Ons/Gamemode_Magicks/icons/Lightning";
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
	iconName = "Add-Ons/Gamemode_Magicks/icons/Wind";
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
	iconName = "Add-Ons/Gamemode_Magicks/icons/Light";
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
	iconName = "Add-Ons/Gamemode_Magicks/icons/Darkness";
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
	iconName = "Add-Ons/Gamemode_Magicks/icons/Arcane";
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
	iconName = "Add-Ons/Gamemode_Magicks/icons/Null";
};

function NullToken::onUse(%this, %obj, %slot)
{
	%obj.client.tokens = "";
	%obj.client.displayTokens();
}

//#2.
//	#2.1
function AddSpell(%bitmask, %t, %name)
{
	%tA = getSubStr(%t, 0, 1);
	if(strLen(%t > 1))
	{
		%tB = getSubStr(%t, 1, 1);
		if(strLen(%t > 2))
		{
			%tC = getSubStr(%t, 2, 1);
		}
	}
	if(strLen(%tB))
	{
		if(strLen(%tC))
		{
			$Spell[%tA @ %tB @ %tC] = %bitmask;
			$Spell[%tA @ %tC @ %tB] = %bitmask;
			$Spell[%tB @ %tA @ %tC] = %bitmask;
			$Spell[%tB @ %tC @ %tA] = %bitmask;
			$Spell[%tC @ %tB @ %tA] = %bitmask;
			$Spell[%tC @ %tA @ %tB] = %bitmask;
			$SpellName[%tA @ %tB @ %tC] = %name;
			$SpellName[%tA @ %tC @ %tB] = %name;
			$SpellName[%tB @ %tA @ %tC] = %name;
			$SpellName[%tB @ %tC @ %tA] = %name;
			$SpellName[%tC @ %tB @ %tA] = %name;
			$SpellName[%tC @ %tA @ %tB] = %name;
		}
		else
		{
			$Spell[%tA @ %tB] = %bitmask;
			$Spell[%tB @ %tA] = %bitmask;
			$SpellName[%tA @ %tB] = %name;
			$SpellName[%tB @ %tA] = %name;
		}
	}
	else
	{
		$Spell[%tA @ %tB] = %bitmask;
		$SpellName[%tA @ %tB] = %name;
	}
}

//	#2.2

//BOLTS
//SPRAYS
//AOES
//BEAMS
//SELF/SPECIAL

AddSpell(0, "", "Null");      //00000000

AddSpell(1, "F", "Fireball"); //00000001
AddSpell(2, "W", "Waterball"); //00000010
AddSpell(4, "E", "Stones"); //00000100
AddSpell(8, "N", "Wind"); //00001000
AddSpell(16, "L", "Lightningbolt"); //00010000
AddSpell(32, "T", "Light Bolt"); //00100000
AddSpell(64, "D", "Shadow Bolt"); //01000000
AddSpell(128, "A", "Arcane Missiles"); //10000000

//	#2.3
AddSpell(1 | 2, "FW", "Steam");
AddSpell(1 | 4, "FE", "Magma");
AddSpell(1 | 8, "FN", "Firestorm");
AddSpell(1 | 16, "FL", "Chain Lightning");
AddSpell(1 | 32, "FT", "Holyfire");
AddSpell(1 | 64, "FD", "Shadowflame");
AddSpell(1 | 128, "FA", "Flame Arrow");

AddSpell(2 | 4, "WE", "Mudball");
AddSpell(2 | 8, "WN", "Mist");
AddSpell(2 | 16, "WL", "Storm");
AddSpell(2 | 32, "WT", "Holy Water");
AddSpell(2 | 64, "WD", "Bloodbolt");
AddSpell(2 | 128, "WA", "Nullify");

AddSpell(4 | 8, "EN", "Dust");
AddSpell(4 | 16, "EL", "Metal");
AddSpell(4 | 32, "ET", "Holystone");
AddSpell(4 | 64, "ED", "Darkstone");
AddSpell(4 | 128, "EA", "Stone Shield");

AddSpell(8 | 16, "NL", "Thunderstorm");
AddSpell(8 | 32, "NT", "Holystorm");
AddSpell(8 | 64, "ND", "Shadowstorm");
AddSpell(8 | 128, "NA", "Spellstorm");

AddSpell(16 | 32, "LT", "Divine Thunder");
AddSpell(16 | 64, "LD", "Shadowthunder");
AddSpell(16 | 128, "LA", "Blink");

AddSpell(32 | 64, "TD", "Twilight Bolt");
AddSpell(32 | 128, "TA", "Cleanse");

AddSpell(64 | 128, "DA", "Cloak");

//	#2.4
AddSpell(1 | 2 | 4, "FWE", "Geyser");
AddSpell(1 | 2 | 8, "FWN", "Mist");
AddSpell(1 | 2 | 16, "FWL", "Static Spray");
AddSpell(1 | 2 | 32, "FWT", "Godbreath");
AddSpell(1 | 2 | 64, "FWD", "Shroud");
AddSpell(1 | 2 | 128, "FWA", "Arcane Barrage");

AddSpell(1 | 4 | 8, "FEN", "Emberstorm");
AddSpell(1 | 4 | 16, "FEL", "Slagball");
AddSpell(1 | 4 | 32, "FET", "Meteor Smite");
AddSpell(1 | 4 | 64, "FED", "Shadowmagma");
AddSpell(1 | 4 | 128, "FEA", "Lava Fissure");

AddSpell(1 | 8 | 16, "FNL", "Firethunderstorm");
AddSpell(1 | 8 | 32, "FNT", "Holyfirestorm");
AddSpell(1 | 8 | 64, "FND", "Chaos Blast");
AddSpell(1 | 8 | 128, "FNA", "Spellfirestorm");

AddSpell(1 | 16 | 32, "FLT", "Consecration");
AddSpell(1 | 16 | 64, "FLD", "Shadowflamestrike");
AddSpell(1 | 16 | 128, "FLA", "Haste");

AddSpell(1 | 32 | 64, "FTD", "Burning Twilight");
AddSpell(1 | 32 | 128, "FTA", "Pillar of Truth");

AddSpell(1 | 64 | 128, "FDA", "Shadowspellfire");

AddSpell(2 | 4 | 8, "WEN", "Mudslide");
AddSpell(2 | 4 | 16, "WEL", "Disruption");
AddSpell(2 | 4 | 32, "WET", "Watermelon Bolt");
AddSpell(2 | 4 | 64, "WED", "Encasement");
AddSpell(2 | 4 | 128, "WEA", "Stoneskin");

AddSpell(2 | 8 | 16, "WNL", "Thunderstorm");
AddSpell(2 | 8 | 32, "WNT", "Holymist");
AddSpell(2 | 8 | 64, "WND", "Toxic Spray");
AddSpell(2 | 8 | 128, "WNA", "Galefoot");

AddSpell(2 | 16 | 32, "WLT", "");
AddSpell(2 | 16 | 64, "WLD", "");
AddSpell(2 | 16 | 128, "WLA", "Blinkpool");

AddSpell(2 | 32 | 64, "WTD", "Corrupt");
AddSpell(2 | 32 | 128, "WTA", "Confusion");

AddSpell(2 | 64 | 128, "WDA", "Edema");

AddSpell(4 | 8 | 16, "ENL", "");
AddSpell(4 | 8 | 32, "ENT", "");
AddSpell(4 | 8 | 64, "END", "Blackout");
AddSpell(4 | 8 | 128, "ENA", "");

AddSpell(4 | 16 | 32, "ELT", "Artifact");
AddSpell(4 | 16 | 64, "ELD", "Poison Dart");
AddSpell(4 | 16 | 128, "ELA", "Lightning Rod");

AddSpell(4 | 32 | 64, "ETD", "Twilight Meteor");
AddSpell(4 | 32 | 128, "ETA", "eat");

AddSpell(4 | 64 | 128, "EDA", "Darkstone Bolt");

AddSpell(8 | 16 | 32, "NLT", "Light of Dawn");
AddSpell(8 | 16 | 64, "NLD", "Ominous Wind");
AddSpell(8 | 16 | 128, "NLA", "Improved Blink");

AddSpell(8 | 32 | 64, "NTD", "");
AddSpell(8 | 32 | 128, "NTA", "Arcane Blast");

AddSpell(8 | 64 | 128, "NDA", "Counterspell");

AddSpell(16 | 32 | 64, "LTD", "Thunderous Twilight");
AddSpell(16 | 32 | 128, "LTA", "");

AddSpell(16 | 64 | 128, "LDA", "Haunt");

AddSpell(32 | 64 | 128, "TDA", "Twilight Barrage");