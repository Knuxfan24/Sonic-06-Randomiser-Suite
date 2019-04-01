# SONIC THE HEDGEHOG (2006) Randomiser
A tool designed to randomise various elements of the Set Object Layout Data in SONIC THE HEDGEHOG (2006) on the Xbox 360 and Playstation 3.

# Building
The build process for this is currently a tad clunky both due to me not knowing how to propely make HedgeLib a dependancy and due to the S06SetData class in HedgeLib being broken. Therefore, when building the randomiser, you need to manually fix the HedgeLib DLL.

1) Clone this repository to a folder on your computer & clone the DirectX version of [Radfordhound's](https://github.com/Radfordhound) [HedgeLib](https://github.com/Radfordhound/HedgeLib/tree/directX) repository to a different folder.
2) Open the HedgeLib.sln file in Visual Studio and open HedgeLib/Sets/SetData.cs. Scroll down to Line 91 & replace 
# var template = (objectTemplates.ContainsKey(obj.ObjectType)) ?
with:
# var template = (objectTemplates != null && objectTemplates.ContainsKey(obj.ObjectType)) ?
Then compile the HedgeLib project to obtain a DLL in HedgeLib/bin/Debug.
3) Open the Sonic06Randomiser.sln file in Visual Studio; right click on References and choose Add Reference. Click browse then navigate to and select the HedgeLib.dll, then click OK.
4) Build the program like you would any other C# application.

# Usage
1) In order to use the randomiser, you need to extract the scripts.arc file that contains all of the Sonic 06 set data files. I recommend using Hyper's [Sonic '06 Toolkit](https://gamebanana.com/tools/6576) application for this purpose.
2) Once you have extracted a set file from the scripts.arc file, go to Misc Tools in the randomiser and select Set Extractor to open the Set Extractor window. Choose your set file in the first textbox and click extract. If the HedgeLib DLL fix worked, then an XML file should be created in your output directory of choice. Now we can randomise it.
3) Close the Set Extractor window and choose the XML in the first textbox of the main randomiser window. Now configure your desired options on what should be randomised and what objects are valid to be swapped in. Once you're happy, click Randomise and a new set file should be created in your output directory of choice. If you enable the spoiler log or choose to keep the edited XML, then they both appear here too.
4) Replace the original set data in scripts.arc using the Sonic '06 Toolkit and then repack the arc. Once that is done, you can load the game up with the modified scripts.arc and test the stage to see if the randomisation worked.

# Notes
Either due to my code or due to HedgeLib's unfinished S06SetData class, a lot of things don't work. Most notably, the lack of grouping support in HedgeLib breaks anything that relys on it (such as Cages or Doors that are opened by defeating or certain enemies).
Things such as bosses can also wreck havoc on the game's already low stability, therefore, use caution when enabling them.
Only certain characters can do certain actions, for example, only Sonic can do the water surfing in Wave Ocean and only Sonic holding Elise and Silver can use the Swing Vines in Tropical Jungle.
Some stages will crash the game consistently when various elements are randomised, if these still happen once HedgeLib is fixed, then I will be diagnosing them to see if I can find where the fault lies.
