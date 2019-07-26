# SONIC THE HEDGEHOG (2006) Randomiser Suite
A small software package designed to randomise various elements in SONIC THE HEDGEHOG (2006) on the Xbox 360 and Playstation 3 consoles, ranging from random enemy types in levels to random speed values for the characters.

# Building
To manually build the Mod Manager, simply clone this repository to a location on your computer and open the `SONIC THE HEDGEHOG (2006) Randomiser Suite.sln` solution file in Visual Studio (VS2019 recommended, although other versions SHOULD work fine). Right-click on the project in the Solution Explorer and choose Rebuild Solution to build an executable in `..\SONIC THE HEDGEHOG (2006) Randomiser Suite\bin\Debug`.

# Usage (SET Randomiser)
<p align="center">
	<img src="https://github.com/Knuxfan24/SONIC-THE-HEDGEHOG-2006-Randomiser-Suite/blob/Rewrite/README%20Graphics/SET%20Randomiser.png" />
</p>
The SET Randomiser allows the user to modify various elements within the game's SET files, these files can be found scattered around scripts.arc and are responsible for the object layout of every stage. The SET Randomiser can (currently) randomise:

Enemies - This will change what enemies appear in the stage and how they behave. The different enemy types can be toggled on or off in the Enemy Configuration menu, accessible from the top bar of the SET Randomiser window.

Characters - This will change what playable characters appear in the stage. The different avaliable characters can be toggled on or off in the Character Configuration menu, accessible from the top bar of the SET Randomiser window.

Item Capsules - This will change what the various Item Capsules within the stage contain. The different avaliable items that can appear in the capsules can be toggled on or off in the Character Configuration menu, accessible from the top bar of the SET Randomiser window.

Voice Triggers - This will change what hint dialog lines (including events such as character banter) should be displayed throughout the stage. Most voice files won't play due to a limitation of the game itself, but the text box will always display correctly.

The SET Randomiser also has an option to remove any Doors and Cages within the act, this is to workaround an issue caused by [HedgeLib's](https://github.com/Radfordhound/HedgeLib/) incomplete Sonic '06 SET Data implementation, causing the game to lose various bits of information that is needed to allow these objects to work properly. This workaround removes these objects, making more stages playable when randomised.

# Usage (Light Randomiser)


# Usage (Music Randomiser)


# Usage (Mission Randomiser)


# Usage (Character Attributes Randomiser Randomiser)



# Notes
Things such as bosses can also wreck havoc on the game's already low stability, therefore, use caution when enabling them. Avoid using them on real hardware, as they cause major glitches (as seen here: https://youtu.be/_g_AUWhp6Ls?t=12630).
Only certain characters can do certain actions, for example, only Sonic can do the water surfing in Wave Ocean and only Sonic holding Elise and Silver can use the Swing Vines in Tropical Jungle.