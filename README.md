# SONIC THE HEDGEHOG (2006) Randomiser
A tool designed to randomise various elements of the Set Object Layout Data in SONIC THE HEDGEHOG (2006) on the Xbox 360 and Playstation 3, as well as a few stage lua elements.

# Building
1) Clone this repository into a folder of your choice using the Git CMD tool with this command: `git clone --recurse-submodules https://github.com/Knuxfan24/SONIC-THE-HEDGEHOG-2006-Randomiser`
2) Open Sonic06Randomiser.sln in Visual Studio and build the HedgeLib project to create the HedgeLib DLL. Then run the program to build and run a binary, located in `..\Sonic06Randomiser\bin\Debug`

# Usage (Set Randomisation)
1) In order to use the randomiser, you need to extract the scripts.arc file that contains all of the Sonic 06 set data files. I recommend using Hyper's [Sonic '06 Toolkit](https://gamebanana.com/tools/6576) application for this purpose.
2) Either extract a set file from scripts.arc or open the temp folder that the Sonic '06 Toolkit extracts to and work from there. Select the set file or folder you wish to randomise (baring in mind that the folder randomiser will include subfolders well), then configure the settings to what you want. While the randomiser can auto extract the set data to xml, the user can manually extract them too using the Set Extractor utility in the Misc Tools tab.
3) Click on the Randomise button once you have selected your settings to randomise the selected file. If a set file was the chosen file, then it will first be extracted to an xml, it is recommended to use the xml as the source file for any future randomisations of the same set.
4) Replace the original set data in scripts.arc using the Sonic '06 Toolkit (this will already be done if you're directly working with the temp folder that the toolkit extracts archives to) and then repack the arc. Once that is done, you can load the game up with the modified scripts.arc and test the stage to see if the randomisation worked.

# Usage (LUA Randomisation)
1) The randomiser also includes a very basic LUA Randomiser in the Misc Tools tab to randomise a stage's music, lighting colours and light direction. The files to randomise are also found in scripts.arc, but are compiled into LUA Binary files. The Sonic '06 Toolkit can decompile these, allowing us to randomise them. Find the folder for the .LUB file you wish to randomise and choose the "Decompile all LUBs in this directory" option from the SDK tab of the Sonic '06 Toolkit.
2) Select the decompile .LUB file that you wish to randomise in the LUA Randomiser tool, a stage's [x]_[character_name].lub file holds the music definition, while the stage's scene_*.lub file contains the lighting information. Configure the randomisation to your liking, then click randomise.
3) Repack scripts.arc with the Sonic '06 Toolkit then load the edited stage in game.

# Notes
Either due to my code or due to HedgeLib's unfinished S06SetData class, a lot of things don't work. Most notably, the lack of grouping support in HedgeLib breaks anything that relys on it (such as Cages or Doors that are opened by defeating or certain enemies).

Things such as bosses can also wreck havoc on the game's already low stability, therefore, use caution when enabling them. Avoid using them on real hardware, as they cause major glitches (as seen here: https://youtu.be/_g_AUWhp6Ls?t=12630).

Only certain characters can do certain actions, for example, only Sonic can do the water surfing in Wave Ocean and only Sonic holding Elise and Silver can use the Swing Vines in Tropical Jungle.

Some stages will crash the game consistently when various elements are randomised, if these still happen once HedgeLib is fixed, then I will be diagnosing them to see if I can find where the fault lies.
