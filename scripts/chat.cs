/////////////////////////////////////////////
// GameMode_dungonsopfdeth/scripts/chat.cs //
/////////////////////////////////////////////

// table of contents
// #1.0 definitions
//   #1.1 serverCmdMessageSent

// #1.0

// #1.1
function serverCmdMessageSent(%client, %message)
{
	messageAll('', $dod::TeamColor[%client.team] @ %client.name @ "<color:ffffff>: " @ %message);
}