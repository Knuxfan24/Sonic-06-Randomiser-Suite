# SONIC THE HEDGEHOG (2006) Randomiser
A tool designed to randomise various elements of the Set Object Layout Data in SONIC THE HEDGEHOG (2006) on the Xbox 360 and Playstation 3.

# Building
1) Clone this repository into a folder of your choice using the Git CMD tool with this command: `git clone --recurse-submodules https://github.com/Knuxfan24/SONIC-THE-HEDGEHOG-2006-Randomiser`
2) Open Sonic06Randomiser.sln in Visual Studio and build the HedgeLib project to create the HedgeLib DLL. Then run the program to build and run a binary, located in `..\Sonic06Randomiser\bin\Debug`

# Usage
1) In order to use the randomiser, you need to extract the scripts.arc file that contains all of the Sonic 06 set data files. I recommend using Hyper's [Sonic '06 Toolkit](https://gamebanana.com/tools/6576) application for this purpose.
2) Once you have extracted a set file from the scripts.arc file, go to Misc Tools in the randomiser and select Set Extractor to open the Set Extractor window. Choose your set file in the first textbox and click extract. If the HedgeLib DLL fix worked, then an XML file should be created in your output directory of choice. Now we can randomise it.
3) Close the Set Extractor window and choose the XML in the first textbox of the main randomiser window. Now configure your desired options on what should be randomised and what objects are valid to be swapped in. Once you're happy, click Randomise and a new set file should be created in your output directory of choice. If you enable the spoiler log or choose to keep the edited XML, then they both appear here too.
4) Replace the original set data in scripts.arc using the Sonic '06 Toolkit and then repack the arc. Once that is done, you can load the game up with the modified scripts.arc and test the stage to see if the randomisation worked.

# Notes
Either due to my code or due to HedgeLib's unfinished S06SetData class, a lot of things don't work. Most notably, the lack of grouping support in HedgeLib breaks anything that relys on it (such as Cages or Doors that are opened by defeating or certain enemies).

Things such as bosses can also wreck havoc on the game's already low stability, therefore, use caution when enabling them. Avoid using them on real hardware, as they cause major glitches (as seen here: https://youtu.be/_g_AUWhp6Ls?t=12630).

Only certain characters can do certain actions, for example, only Sonic can do the water surfing in Wave Ocean and only Sonic holding Elise and Silver can use the Swing Vines in Tropical Jungle.

Some stages will crash the game consistently when various elements are randomised, if these still happen once HedgeLib is fixed, then I will be diagnosing them to see if I can find where the fault lies.
