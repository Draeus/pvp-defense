# PvpDefense
 Allows the ratio of defense to damage reduced to be controlled seperately for pvp damage.
 This plugin is dependant on .net 6.0 and tshock. While it's unlikely you don't have .net 6.0, make sure you have both before installing.

 ***

 ### Setup
 * Install a tshock server if you haven't already 
 * Locate the installation
 * Download the PvpDefense.dll file from the link below
 * Place PvpDefense.dll into the ServerPlugins folder in your tshock installation

 It should run at this point. The defense to damage ratio is set to 0.5 by default, but you can edit this number in the config.
 * Run the tshock server once with the plugin so that it'll generate the config file
 * The config is located in your tshock installation/tshock/pvpdefense.json
 * Edit this value however you like and save the file
 * run reload in the console or /reload ingame to update the config changes ingame. This will also be done automatically whenever the server launches.
 
 ***

 ### Known issues
 This is in early alpha and is lacking a lot of polish. Expect bugs. 
 * This plugin will likely interfere with other plugins that modify pvp damage directly. This is unlikely to be fixed.
 * Enable crit in the config is nonfunctional. This is intended for a later release, but is a stub for now.
 * Vanilla damage numbers haven't been able to be removed. It's a potential improvement for later.

***

### Download me!

[Download PvpDefense.dll](https://github.com/Draeus/pvp-defense/raw/main/bin/Debug/net6.0/PvpDefense.dll)