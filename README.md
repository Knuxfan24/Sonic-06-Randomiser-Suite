# SONIC THE HEDGEHOG (2006) Randomiser
A tool designed to randomise various elements of the Set Object Layout Data in SONIC THE HEDGEHOG (2006) on the Xbox 360 and Playstation 3.

# Building
1: Clone this repository and obtain a copy of the HedgeLib DLL from [Radfordhound's](https://github.com/Radfordhound) [HedgeLib](https://github.com/Radfordhound/HedgeLib) repo. A prebuilt copy of the DLL can be found on [AppVeyor](https://ci.appveyor.com/project/Radfordhound/hedgelib/builds/21427946/artifacts) or the DLL can be compiled manually.

2: Open up the Sonic06Randomiser.sln file in Visual Studio. Right click on the References tab in the Solution Explorer and select Add Reference. Choose browse on the left then on the bottom and navigate to the HedgeLib.dll, choose Add then click OK in the Reference Manager (ensure the HedgeLib.dll actually has a checkmark).

3: Build the program (selecting Start should suffice).
