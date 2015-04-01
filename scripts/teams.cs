//////////////////////////////////////////////
// GameMode_dungonsopfdeth/scripts/teams.cs //
//////////////////////////////////////////////

// table of contents
// #1. global vars
// #2. new functions
//   #2.1 server commands for joining/leaving a team
//   #2.2 server commands for enabling/disabling pvp
// #3. dodTeams package
//   #3.1 gameConnection::autoAdminCheck
//	#3.2 gameConnection::spawnPlayer
//   #3.3 minigameCanDamage
// #4. function overwrites
//   #4.1 serverCmdMessageSent
//   #4.2 serverCmdTeamMessageSent


// #1.0
$dod::Teams = 6;
$dod::Team[-1]="White";
$dod::TeamColor[-1]="<color:B0B0B0>";
$dod::TeamColorScript[-1]="0.7 0.7 0.7";
$dod::Team[0]="Red";
$dod::TeamColor[0]="<color:FF0000>";
$dod::TeamColorScript[0]="1 0 0";
$dod::Team[1]="Blue";
$dod::TeamColor[1]="<color:1010FF>";
$dod::TeamColorScript[1]="0.06 0.06 1";
$dod::Team[2]="Green";
$dod::TeamColor[2]="<color:00FF00>";
$dod::TeamColorScript[2]="0 1 0";
$dod::Team[3]="Yellow";
$dod::TeamColor[3]="<color:FFF000>";
$dod::TeamColorScript[3]="1 0.94 0";
$dod::Team[4]="Magenta";
$dod::TeamColor[4]="<color:FF00FF>";
$dod::TeamColorScript[4]="1 0 1";
$dod::Team[5]="Cyan";
$dod::TeamColor[5]="<color:00FFFF>";
$dod::TeamColorScript[5]="0 1 1";

// #2.

// #2.1
function serverCmdJoinTeam(%client, %team)
{
	if(!%client.player.inCombat)
	{
		%foundteam = false;
		for(%i = -1; %i < $dod::Teams; %i++)
		{
			if(strPos($dod::Team[%i], %team) != -1)
			{
				%foundteam = true;
				break;
			}
		}
		if(!%foundteam)
		{
			%i = -1;
		}
		if(%i != %client.dodTeam)
		{
			if(%client.dodTeam $= "")
			{
				%client.dodTeam = -1;
			}
			messageAll('', $dod::TeamColor[%client.dodTeam] @ %client.name @ "\c7 has joined the " @ $dod::TeamColor[%i] @ $dod::Team[%i] @ "\c7 team.");
			%client.dodTeam = %i;
			%client.dodAccount.dodTeam = %i;
			if(isObject(%client.player))
			{
				%client.player.setShapeNameColor($dod::TeamColorScript[%client.dodTeam]);
			}
		}
		else
		{
			messageClient(%client, '', "\c0You're already on the " @ $dod::TeamColor[%i] @ $dodTeam[%i] @ "\c0 team.");
		}
	}
	else
	{
		messageClient(%client, '', "\c0Not while you're in combat!");
	}
}

function serverCmdLeaveTeam(%client)
{
	serverCmdJoinTeam(%client, "White");
}

// #2.2
function serverCmdEnablePvp(%client)
{
	messageAll('', $dod::TeamColor[%client.dodTeam] @ %client.name @ "\c7 has enabled PvP!");
	%client.pvp = true;
}

function serverCmdDisablePvp(%client)
{
	if(!%client.player.inCombat)
	{
		messageAll('', $dod::TeamColor[%client.dodTeam] @ %client.name @ "\c7 has disabled PvP!");
		%client.pvp = false;
	}
	else
	{
		messageClient(%client, '', "\c0Not while you're in combat!");
	}
}

function serverCmdTogglePvp(%client)
{
	if(%client.pvp)
	{
		serverCmdDisablePvp(%client);
	}
	else
	{
		serverCmdEnablePvp(%client);
	}
}

// #3.0
package dodTeams
{
	// #3.1
	function GameConnection::AutoAdminCheck(%client)
	{
		%p = parent::AutoAdminCheck(%client);
		serverCmdJoinTeam(%client, "White");
		return %p;
	}

	// #3.2
	function GameConnection::SpawnPlayer(%client)
	{
		%p = parent::SpawnPlayer(%client);
		%client.player.setShapeNameColor($dod::TeamColorScript[%client.dodTeam]);
		return %p;
	}

	// #3.3
	function minigameCanDamage(%a, %b)
	{
		%p = parent::minigameCanDamage(%a, %b);
		if(!%p)
		{
			//resolve %a and %b to clients if possible
			if(%a.getClassName() $= "GameConnection")
			{
			}
			else if(%a.getClassName() $= "Player")
			{
				if(isObject(%a.client))
				{
					%a = %a.client;
				}
				else
				{
					return %p;
				}
			}
			else
			{
				return %p;
			}
			if(%b.getClassName() $= "GameConnection")
			{
			}
			else if(%b.getClassName() $= "Player")
			{
				if(isObject(%b.client))
				{
					%b = %b.client;
				}
				else
				{
					return %p;
				}
			}
			else
			{
				return %p;
			}
			if( (%a.dodTeam != %b.dodTeam || %a.dodTeam == -1 || %b.dodTeam == -1 || %a == %b) && %a.pvp && %b.pvp)
			{
				return true;
			}
		}
		else
		{
			return %p;
		}
	}
};
activatePackage(dodTeams);

// #4.
// #4.1
function serverCmdMessageSent(%client, %message)
{
	messageAll('', "\c7" @ %client.clanPrefix @ $dod::TeamColor[%client.dodteam] @ %client.name @ "\c7" @ %client.clanPrefix @ "<color:ffffff>: " @ %message);
}

// #4.2
function serverCmdTeamMessageSent(%client, %message)
{
	if(%client.dodTeam == -1)
	{
		messageClient(%client, '', "\c0You can't use team chat while you're on the White team!");
		return;
	}
	for(%i = 0; %i < clientGroup.getCount(); %i++)
	{
		%c = clientGroup.getObject(%i);
		if(%c.dodTeam == %client.dodTeam)
		{
			messageClient(%client, '', $dod::TeamColor[%client.dodTeam] @ "[TEAM]\c7" @ %client.clanPrefix @ $dod::TeamColor[%client.dodTeam] @ %client.getPlayerName() @ "\c7" @ %client.clanSuffix @ "<color:FFFFFF>:" @ %message);
		}
	}
}