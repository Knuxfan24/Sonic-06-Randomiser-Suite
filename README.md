# SONIC THE HEDGEHOG (2006) Randomiser
A tool designed to randomise various elements of the Set Object Layout Data in SONIC THE HEDGEHOG (2006) on the Xbox 360 and Playstation 3.

# Building
The build process for this is currently a tad clunky both due to me not knowing how to propely make HedgeLib a dependancy and due to the S06SetData class in HedgeLib being broken. Therefore, when building the randomiser, you need to manually fix the HedgeLib DLL.

1) Clone this repository to a folder on your computer & clone the DirectX version of [Radfordhound's](https://github.com/Radfordhound) [HedgeLib](https://github.com/Radfordhound/HedgeLib/tree/directX) repository to a different folder.
2) Open the HedgeLib.sln file in Visual Studio and open HedgeLib/Sets/SetData.cs. Scroll down to Line 91 & replace 

var template = (objectTemplates.ContainsKey(obj.ObjectType)) ?
with:
var template = (objectTemplates != null && objectTemplates.ContainsKey(obj.ObjectType)) ?

Then compile the HedgeLib project to obtain a DLL in HedgeLib/bin/Debug.

3) Open the Sonic06Randomiser.sln file in Visual Studio; right click on References and choose Add Reference. Click browse then navigate to and select the HedgeLib.dll, then click OK.
4) Build the program like you would any other C# application.
