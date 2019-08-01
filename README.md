# SONIC THE HEDGEHOG (2006) Randomiser Suite
A small software package designed to randomise various elements in SONIC THE HEDGEHOG (2006) on the Xbox 360 and PlayStation 3 consoles, ranging from random enemy types in levels to random speed values for the characters.

# Building
To manually build the Randomiser Suite, simply clone this repository to a location on your computer and open the `SONIC THE HEDGEHOG (2006) Randomiser Suite.sln` solution file in Visual Studio (VS2019 recommended, although other versions SHOULD work fine). Right-click on the project in the Solution Explorer and choose Rebuild Solution to build an executable in `..\SONIC THE HEDGEHOG (2006) Randomiser Suite\bin\Debug`.

# Usage
The Randomiser Suite works on various files located in the scripts.arc and player.arc archives in the xenon/scripts (or ps3/scripts/ for the PlayStation 3 version) folder. To work with these archives, use the [Sonic '06 Toolkit](https://github.com/HyperPolygon64/Sonic-06-Toolkit/releases), this will allow you to open and modify these archives. Most of the file search boxes in the suite are filtered to only take certain files, so finding the ones you want to work on should be simple. Alternatively, use the Randomise Folder option in each Randomiser to randomise multiple files at once, starting from the specified directory and looking through any subfolders.

To remove any previously randomised files, paste the folder path containing the randomised files (this will also include subfolders) and click Clean Up, this will remove any files with a .s06back version and rename the .s06back version into the normal one.

# Details (SET Randomiser)
<p align="center">
	<img src="https://github.com/Knuxfan24/SONIC-THE-HEDGEHOG-2006-Randomiser-Suite/blob/master/README%20Graphics/SET%20Randomiser.png" />
</p>
The SET Randomiser allows the user to modify various elements within the game's SET files, these files can be found scattered around scripts.arc and are responsible for the object layout of every stage. The SET Randomiser can (currently) randomise:

Enemies - This will change what enemies appear in the stage and how they behave. The different enemy types can be toggled on or off in the Enemy Configuration menu, accessible from the top bar of the SET Randomiser window.

Characters - This will change what playable characters appear in the stage. The different avaliable characters can be toggled on or off in the Character Configuration menu, accessible from the top bar of the SET Randomiser window.

Item Capsules - This will change what the various Item Capsules within the stage contain. The different avaliable items that can appear in the capsules can be toggled on or off in the Character Configuration menu, accessible from the top bar of the SET Randomiser window.

Voice Triggers - This will change what hint dialog lines (including events such as character banter) should be displayed throughout the stage. Most voice files won't play due to a limitation of the game itself, but the text box will always display correctly.

Physics Props - This will randomise what the props scattered around the level will be, some of these obey physics, others just float there.

The SET Randomiser also has an option to remove any Doors and Cages within the act, this is to workaround an issue caused by [HedgeLib's](https://github.com/Radfordhound/HedgeLib/) incomplete Sonic '06 SET Data implementation, causing the game to lose various bits of information that is needed to allow these objects to work properly. This workaround removes these objects, making more stages playable when randomised.

# Details (Light Randomiser)
<p align="center">
	<img src="https://github.com/Knuxfan24/SONIC-THE-HEDGEHOG-2006-Randomiser-Suite/blob/master/README%20Graphics/Light%20Randomiser.png" />
</p>
The Light Randomiser allows the user to modify various elements of a stage's lighting, alongside the stage's fog, by modifying various values in a stage's scene LUA file, found in the scripts/stage/ folder of scripts.arc. The Light Randomiser can (currently) randomise:

Ambient, Main and Sub Lighting Colours - This will adjust the various colours that make up a stage's lighting.

Light Direction - This will change the direction the light in the stage comes from, affecting the length and angle shadows are cast at.

Fog Colour - This will change the colour of the stage's fog.

Fog Density - This will change how thick a stage's fog is.

# Details (Music Randomiser)
<p align="center">
	<img src="https://github.com/Knuxfan24/SONIC-THE-HEDGEHOG-2006-Randomiser-Suite/blob/master/README%20Graphics/Music%20Randomiser.png" />
</p>
The Music Randomiser allows the user to change the music that is played on a stage by editing a value in a stage's area LUA, each character that plays through a stage will have one of these LUA files, with a letter to indicate the section and their name. These can be found in the scripts/stage/ folder of scripts.arc. The avaliable music choices can be configured using the check list on the bottom left of the Music Randomiser window.

# Details (Mission Randomiser)
<p align="center">
	<img src="https://github.com/Knuxfan24/SONIC-THE-HEDGEHOG-2006-Randomiser-Suite/blob/master/README%20Graphics/Mission%20Randomiser.png" />
</p>
The Mission Randomiser allows the user to modify a couple of values in a stage's mission file, found in the scripts/mission/ folder of scripts.arc, this allows the user to randomise the loading screen mission text (with almost any string across the whole game being eligable) and also allows the user to adjust a few score related values, namely the Time and Ring Bonuses given out upon completing a stage, and the score required for the different ranks the game awards you.

# Details (Character Attributes Randomiser)
<p align="center">
	<img src="https://github.com/Knuxfan24/SONIC-THE-HEDGEHOG-2006-Randomiser-Suite/blob/master/README%20Graphics/Character%20Attributes%20Randomiser.png" />
</p>
The Character Attributes Randomiser allows the user to randomise various character parameters found in the LUA files within player.arc. The Character Attributes Randomiser can (currently) randomise:

Movement Speed: This randomises how fast the different characters move when on the ground, either speeding them up dramatically or having a low chance to slow them down by a considerable margin.

Jump Height: This will change the character's jump height, along with the amount of momentum they carry into a jump when not at a standstill.

Grinding Speed: This will change how fast the character grinds on rails, being able to adjust both their minimum and maximum grinding speeds, as well as the rate at which they accellerate when on rails.

Ability Parameters: This randomises the values used for certain character abilities, such as the speed of Tails' flight and the amount of Double Jumps Amy can do before needing to touch the ground.

Model Randomiser: This randomises what model package a player will use, effectively swapping their ingame model with someone elses while retaining (most) of their own abilities. The different avaliable models can be configured using the Model Configuration menu, accessible from the top bar of the Character Attributes Randomiser window.

Gem Patch: This will edit Sonic's character LUA to fix the typo that prevents his Action Gauge from draining when using his gems.

# Details (Collision Properties Randomiser)
<p align="center">
	<img src="https://github.com/Knuxfan24/SONIC-THE-HEDGEHOG-2006-Randomiser-Suite/blob/master/README%20Graphics/Collision%20Properties%20Randomiser.png" />
</p>
The Collision Properties Randomiser allows the user to mess with the Surface Types of the stage collision files, the different types of which can be configured in the Surface Type Configuration menu, accessible from the top bar of the Collision Properties Randomiser window. This can also allow the user to remove all walls, water and death planes from the stage, allowing them to run and stand on them as if they were solid ground.

# Notes
Things such as bosses can also wreck havoc on the game's already low stability, therefore, use caution when enabling them. Avoid using them on real hardware, as they cause major glitches (as seen here: https://youtu.be/_g_AUWhp6Ls?t=12630).)

Only certain characters can do certain actions, for example, only Sonic can do the water surfing in Wave Ocean and only Sonic holding Elise, Silver and Rouge can use the Swing Vines in Tropical Jungle.

Aquatic Base and Radical Train seem to crash often when randomised.

Randomising character models can cause a lot of characters to lose their abilities due to a lack of animations; the Model Randomiser can also not affect Silver and Omega as the game would always crash upon randomising their models, so their files will be skipped when using it. Changing Sonic's model and then trying to switch to one of his gems will cause the game to crash instantly.

The Character Attributes Randomiser can often make characters difficult to control.

# Credits
[Knuxfan24](https://github.com/Knuxfan24) - Randomisation Code and UI Design

[Radfordhound](https://github.com/Radfordhound) - HedgeLib (used for SET Editing)

Shadow LAG - Sonic '06 Toolkit (used for automatic LUA Decompilation)

[Hyper](https://github.com/HyperPolygon64) - Showing how to change the model package for characters (blame them for the Model Randomiser in the Character Attributes Randomiser!)

[Ookii](http://www.ookii.org/software/dialogs/) - The much nicer Folder Browser.