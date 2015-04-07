///////////////////////////////////////
// GameMode_dungonsopfdeth/server.cs //
///////////////////////////////////////

// table of contents
// #1. executions
// #2. playertype

// #1.0

exec("./scripts/help.cs");

//support functions
exec("./scripts/geometry.cs");

//rpg
exec("./scripts/persistence.cs");

//pseudo-minigame
exec("./scripts/teams.cs");

//combat systems
exec("./scripts/melee.cs");
exec("./scripts/ranged.cs");
exec("./scripts/magic_tokens.cs");
exec("./scripts/magic_effects.cs");
exec("./scripts/magic_casting.cs");
exec("./scripts/statuseffects.cs");
exec("./scripts/playerstats.cs");

//enemies
exec("./scripts/enemies.cs");

// #2.
datablock PlayerData(dodPlayerData : PlayerStandardArmor)
{
	canJet = false;
	airControl = 0;

	maxEnergy = 200;
	rechargeRate = 0.5;
	showEnergyBar = true;

	uiName = "player... opf deth...";
};

datablock PlayerData(dodPlayerFPdata : dodPlayerData)
{
	firstPersonOnly = true;

	uiName = "";
};