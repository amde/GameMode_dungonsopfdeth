////////////////////////////////////////////////////
// GameMode_dungonsopfdeth/scripts/persistence.cs //
////////////////////////////////////////////////////

// table of contents
// #1. new commands
//   #1.1 /createAccount
//   #1.2 /login
//   #1.3 /account
//   #1.4 /logout
//   #1.5 /deleteAccount
// #2. package
//   #2.1

// #1.

// #1.1
function serverCmdCreateAccount(%client, %username, %password, %extra)
{
	if(strLen(%username) == 0 || strLen(%password) == 0 || strLen(%extra) != 0)
	{
		messageClient(%client, '', "\c0Please register with a one-word username and a one-word password (\c3/createAccount username password\c0).");
		return;
	}
	if(isFile("config/server/dungonsopfdeth/" @ %username @ ".cs"))
	{
		messageClient(%client, '', "\c0The username " @ %username @ " is taken. Please choose a different username.");
		return;
	}
	%acc = new ScriptObject("dodAccount_" @ %username)
	{
		class = "dodAccount";
		username = %username;
		password = %password;
		lastUser = %client.getBLID();
		dodTeam = %client.dodTeam;
	};
	%acc.save("config/server/dungonsopfdeth/" @ %username @ ".cs");
	messageClient(%client, '', "\c2Account successfully made with the username \c3" @ %username @ "\c2 and password \c3" @ %password @ "\c2.");
}

// #1.2
function serverCmdLogin(%client, %username, %password, %extra)
{
	if(strLen(%username) == 0 || strLen(%password) == 0 || strLen(%extra) != 0)
	{
		messageClient(%client, '', "\c0Please login with a one-word username and a one-word password (\c3/login username password\c0).");
		return;
	}
	%file = "config/server/dungonsopfdeth/" @ %username @ ".cs";
	if(!isFile(%file))
	{
		messageClient(%client, '', "\c0There does not seem to be an account with the username " @ %username @ ".");
		return;
	}
	//load the file to check the password
	exec(%file);
	%acc = "dodAccount_" @ %username;
	if(!isObject(%acc))
	{
		error("dod: Account " @ %username @ " has file, but load failed.");
		messageClient(%client, '', "\c0Whoops... something very bad happened.");
		return;
	}
	if(%acc.password !$= %password)
	{
		messageClient(%client, '', "\c0Incorrect password.");
		echo(%client.getPlayerName() @ "(ID " @ %client.getBLID() @ ") failed to log in to the account " @ %username @ " (wrong password).");
		%acc.delete();
		return;
	}
	else
	{
		if(isObject("dodAccount_" @ %username))
		{
			messageClient(%client, '', "\c0You cannot login to an account that someone is currently using.");
			return;
		}
		//save old account
		%client.dodAccount.save("config/server/dungonsopfdeth/" @ %client.dodAccount.username @ ".cs");
		//move to new account
		%client.dodAccount = %acc;
		%acc.lastUser = %client.getBLID();
		if(%acc.dodTeam != %client.dodTeam)
		{
			serverCmdJoinTeam(%client, $dod::Team[%acc.dodTeam]);
		}
		messageClient(%client, '', "\c2Successfully logged into account \c3" @ %username @ "\c2.");
	}
}

// #1.3
function serverCmdAccount(%client)
{
	messageClient(%client, '', "\c3You are currently logged into the account \c3" @ %client.dodAccount.username @ "\c2.");
}

// #1.4
function serverCmdLogout(%client)
{
	messageClient(%client, '', "To log out, simply leave the server. If you want to switch accounts, just log in to the other account.");
}

// #1.5
function serverCmdChangePassword(%client, %newpass)
{
	%client.dodAccount.password = %newpass;
	messageClient(%client, '', "\c2Your account's password has been changed to \c3" @ %newpass @ "\c2.");
}

// #1.6
function serverCmdDeleteAccount(%client, %username, %password)
{
	if(strLen(%username) == 0 || strLen(%password) == 0)
	{
		messageClient(%client, '', "\c0You must specify the username and password of the account (\c3/deleteAccount username password\c0).");
		return;
	}
	if(%client.dodAccount.username $= %username)
	{
		messageClient(%client, '', "\c0You must be logged in to another account to delete an account.");
		return;
	}
	if(isObject("dodAccount_" @ %username))
	{
		messageClient(%client, '', "\c0You cannot delete an account that someone is currently using.");
		return;
	}
	//load the file to check the password
	%file = "config/server/dungonsopfdeth/" @ %username @ ".cs";
	exec(%file);
	%acc = "dodAccount_" @ %username;
	if(!isObject(%acc))
	{
		error("dod: Account " @ %username @ " has file, but load failed.");
		messageClient(%client, '', "\c0Whoops... something very bad happened.");
		return;
	}
	if(%acc.password !$= %password)
	{
		messageClient(%client, '', "\c0Incorrect password.");
		echo(%client.getPlayerName() @ "(ID " @ %client.getBLID() @ ") failed to delete the account " @ %username @ " (wrong password).");
		%acc.delete();
		return;
	}
	else
	{
		%acc.delete();
		fileDelete(%file);
	}
}

// #2.
// #2.1
package dodPeristence
{
	function GameConnection::AutoAdminCheck(%client)
	{
		%p = parent::AutoAdminChecl(%client);
		%name = %client.getPlayerName();
		%file = "config/server/dungonsopfdeth/" @ %name @ ".cs";
		if(isFile(%file))
		{
			exec(%file);
			%acc = "dodAccount_" @ %name;
			%password = %acc.password;
			%acc.delete();
			schedule(0, serverCmdLogin, %client, %client.getPlayerName(), %password);
		}
		else
		{
			schedule(0, serverCmdCreateAccount, %name, %client.getBLID());
		}
		return %p;
	}
};

activatePackage(dodPersistence);