//////////////////////////////////////////////
// GameMode_dungonsopfdeth/scripts/items.cs //
//////////////////////////////////////////////

//table of contents
// #1. functionality
//   #1.1 global vars
//   #1.2 helper/readability functions
//   #1.3 server commands
// #2. package
// #3. item enumeration

// #1.

// #1.1
$dod::Melee = mPow(2, 0); //001
$dod::Ranged = mPow(2, 1); //010
$dod::Magic = mPow(2, 2); //100

// #1.2
function getItemType(%str)
{
	return getField(%str, 0);
}

function getQuantity(%str)
{
	return getField(%str, 1);
}
function setQuantity(%str, %amt)
{
	return setField(%str, 1, %amt);
}

function getBonusDamage(%str)
{
	return getField(%str, 1);
}
function incBonusDamage(%str, %amt)
{
	if(!strLen(%amt))
	{
		%amt = 1;
	}
	return setField(%str, 1, getField(%str, 1) + %amt);
}

function getBonusRange(%str)
{
	return getField(%str, 2);
}
function incBonusRange(%str, %amt)
{
	if(!strLen(%amt))
	{
		%amt = 1;
	}
	return setField(%str, 2, getField(%str, 2) + %amt);
}

function getBonusMastery(%str)
{
	return getField(%str, 3);
}
function incBonusMastery(%str, %amt)
{
	if(!strLen(%amt))
	{
		%amt = 1;
	}
	return setField(%str, 3, getField(%str, 3) + %amt);
}

function getExperience(%str)
{
	return getField(%str, 4);
}
function incExperience(%str, %amt)
{
	if(!strLen(%amt))
	{
		%amt = 1;
	}
	return setField(%str, 4, getField(%str, 4) + %amt);
}

function getLevel(%str)
{
	return getField(%str, 5);
}
function incLevel(%str, %amt)
{
	if(!strLen(%amt))
	{
		%amt = 1;
	}
	return setField(%str, 5, getField(%str, 5) + %amt);
}

function getPotential(%str)
{
	return getField(%str, 6);
}
function incPotential(%str, %amt)
{
	if(!strLen(%amt))
	{
		%amt = 1;
	}
	return setField(%str, 6, getField(%str, 6) + %amt);
}

// #1.3

function serverCmdToPack(%client, %w1, %w2, %w3, %w4, %w5)
{
	%w = trim(%w1 SPC %w2 SPC %w3 SPC %w4 SPC %w5);
	if(!strLen(%w))
	{
		//try to move the currently equipped item
		if(isObject(%obj = %client.player))
		{
			%w = %obj.getMountedImage(0).item.uiName;
		}
	}
	if(!strLen(%w))
	{
		messageClient(%client, 'MsgError', "\c0You must enter an item name!");
		return;
	}
	%acc = %client.dodAccount;
	%found = false;
	for(%i = 0; %i < 5; %i++)
	{
		if(striPos(%name = getItemType(%acc.equip[%i]).name, %w) != -1)
		{
			%found = true;
			break;
		}
	}
	if(!%found)
	{
		messageClient(%client, 'MsgError', "\c0Could not find an item named \"" @ %w @ "\" in your inventory.");
		return;
	}
	%room = false;
	for(%j = 0; %j < %acc.maxItems; %j++)
	{
		if(!strLen(%acc.item[%j]))
		{
			%room = true;
			break;
		}
	}
	if(!%room)
	{
		messageClient(%client, 'MsgError', "\c0No room to move the " @ %name @ " to your pack.");
		return;
	}
	%acc.item[%j] = %acc.equip[%i];
	%acc.equip[%i] = "";
	if(isObject(%obj = %client.player))
	{
		if(%obj.getMountedImage(0).dodItem.name $= %name)
		{
			serverCmdUnuseTool(%client);
		}
		%obj.tool[%i] = "";
		messageClient(%client, 'MsgItemPickup', '', %i, 0);
	}
	messageClient(%client, '', "\c2Moved \c3" @ %name @ "\c2 to your pack.");
}
function serverCmdtp(%client, %w1, %w2, %w3, %w4, %w5)
{
	serverCmdToPack(%client, %w1, %w2, %w3, %w4, %w5);
}

function serverCmdFromPack(%client, %w1, %w2, %w3, %w4, %w5)
{
	%w = trim(%w1 SPC %w2 SPC %w3 SPC %w4 SPC %w5);
	if(!strLen(%w))
	{
		messageClient(%client, 'MsgError', "\c0You must enter an item name!");
		return;
	}
	%acc = %client.dodAccount;
	%found = false;
	for(%i = 0; %i < %acc.maxItems; %i++)
	{
		if(striPos(%name = getItemType(%acc.item[%i]).name, %w) != -1)
		{
			%found = true;
			break;
		}
	}
	if(!%found)
	{
		messageClient(%client, 'MsgError', "\c0Could not find an item named \"" @ %w @ "\" in your pack.");
		return;
	}
	%room = false;
	for(%j = 0; %j < 5; %j++)
	{
		if(!strLen(%acc.equip[%j]))
		{
			%room = true;
			break;
		}
	}
	if(!%room)
	{
		messageClient(%client, 'MsgError', "\c0No room in your equipment for the \"" @ %w @ "\".");
		return;
	}
	%acc.equip[%j] = %acc.item[%i];
	%acc.item[%i] = "";
	if(isObject(%obj = %client.player))
	{
		%itemID = getItemType(%acc.equip[%j]).item.getID();
		%obj.tool[%j] = %itemID;
		messageClient(%client, 'MsgItemPickup', '', %j, %itemID);
	}
	messageClient(%client, '', "\c2Retrieved \c3" @ %name @ "\c2 from your pack.");
}
function serverCmdfp(%client, %w1, %w2, %w3, %w4, %w5)
{
	serverCmdFromPack(%client, %w1, %w2, %w3, %w4, %w5);
}

// #2.
package dod_Items
{
	function armor::onCollision(%this, %obj, %col, %norm, %speed)
	{
		if(isObject(%dodItem = %col.getDatablock().dodItem) && isObject(%acc = %obj.client.dodAccount))
		{
			if(%acc.hasItem(%dodItem))
			{
				return;
			}
			%space = false;
			for(%i = 0; %i < %this.maxTools; %i++)
			{
				if(!isObject(%obj.tool[%i]))
				{
					%space = true;
					break;
				}
			}
			if(%space)
			{
				%p = parent::onCollision(%this, %obj, %col, %norm, %speed);
				for(%i = 0; %i < %obj.getDatablock().maxTools; %i++)
				{
					if(%obj.tool[%i] == %itemData)
					{
						%acc.addItem(%dodItem, %i);
					}
				}
				return %p;
			}
			else
			{
				//add to pack
				if(%acc.addItem(%dodItem))
				{
					messageClient(%obj.client, '', "\c3" @ %dodItem.name @ "\c2 added to pack.");
					//respawn/delete the item
					if(isObject(%col.spawnBrick))
					{
						%col.respawn();
					}
					else
					{
						%col.deleteSched = %col.schedule(1, delete);
					}
				}
			}
		}
		return parent::onCollision(%this, %obj, %col, %norm, %speed);
	}

	function GameConnection::SpawnPlayer(%this)
	{
		%p = parent::SpawnPlayer(%this);
		if(isObject(%obj = %this.player))
		{
			%acc = %this.dodAccount;
			%obj.clearTools();
			for(%i = 0; %i < 5; %i++)
			{
				if(strLen(%item = getField(%acc.equip[%i], 0)))
				{
					if(%item.class $= "dodItem")
					{
						%item = %item.item.getID();
					}
					else
					{
						%item = %item.getID();
					}
					%obj.tool[%i] = %item;
					messageClient(%this, 'MsgItemPickup', "", %i, %item);
				}
			}
		}
		return %p;
	}
};
activatePackage(dod_Items);

// #3.
new ScriptObject(dodItem_CopperSword)
{
	class = "dodItem";
	isWeapon = true;
	weaponclass = $dod::Melee;
	item = copperSwordItem;
	tier = 1;
	name = "Copper Sword";

	//base stats
	damage = 20;
	range = 5;
	mastery = 15;
	potential = 0;

	upgrades = 1;
	upgrade[0] = dodItem_IronSword;
};

new ScriptObject(dodItem_BlacksmithHammer)
{
	class = "dodItem";
	isWeapon = true;
	weaponclass = $dod::Melee;
	item = blacksmithHammerItem;
	tier = 1;
	name = "Blacksmith's Hammer";

	//base stats
	damage = 25;
	range = 5;
	mastery = 15;
	potential = 0;

	upgrades = 1;
	upgrade[0] = dodItem_IronSword;
};

new ScriptObject(dodItem_AshBow)
{
	class = "dodItem";
	isWeapon = true;
	weaponclass = $dod::Ranged;
	item = ashBowItem;
	tier = 1;
	name = "Ash Bow";

	//base stats
	damage = 25;
	range = 40;
	mastery = 0;
	potential = 0;

	upgrades = 1;
	upgrade[1] = dodItem_CedarBow;
};