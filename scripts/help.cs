/////////////////////////////////////////////
// GameMode_dungonsopfdeth/scripts/help.cs //
/////////////////////////////////////////////

//table of contents
// #1. help functionality
// #2. help topics

// #1.
function serverCmdHelp(%client, %a1, %a2, %a3, %a4, %a5)
{
	%a = %a1 SPC %a2 SPC %a3 SPC %a4 SPC %a5;
	if($dod::Help[%a] $= "")
	{
		messageClient(%client, '', "\c0Could not find help topic \c3" @ %a @ "\c0. Try \c3/help Topics\c0 for a list of help topics.");
	}
	else
	{
		for(%i = 0; %i < getRecordCount($dod::Help[%a]); %i++)
		{
			schedule(%i * 3000, 0, messageClient, %client, '', getRecord($dod::Help[%a], %i));
		}
	}
}

// #2.
$dod::Help["Topics"] = "\c2Help topics: About, Credits";
$dod::Help[""] = $dod::Help["Topics"];

$dod::Help["About"] = "dungons... opf deth... is a multiplayer role-playing-game. We set our sights high on this one, and we sincerely hope you have fun. - The Craftsmen";
$dod::Help["Credits"] = "\c2Build: Sock, Wizzeh, LegoPepper (Thanks to Clockturn, _Sovereign, Amade)\n\c2Scripts: Amade (Thanks to Clockturn)";