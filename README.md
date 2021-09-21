<p align="center">
    <img src="https://github.com/Knuxfan24/Sonic-06-Randomiser-Suite/blob/master/Sonic%20'06%20Randomiser%20Suite/ExternalResources/Logo.png"
         width="256"/>
</p>

<h1 align="center">Sonic '06 Randomiser Suite</h1>

<p align="center">An all in one software suite for randomising various elements in Sonic '06 on the Xbox 360 and PlayStation 3.</p>

# Building
To build the Randomiser Suite, simply clone the project and open the `Sonic '06 Randomiser Suite.sln` file in a recent version of Visual Studio (anything supporting .NET 5 should work fine, all the development was done on preview builds of Visual Studio 2022) and then compile it as normal, if the build fails it may be due to missing Nuget Packages, as the project uses NAudio for converting MP3s and M4As to WAV files as part of the Custom Music implementation and Newtonsoft.Json as part of Marathon, ensuring that both of those are present and accounted for should resolve any compile errors.

Compile warnings will be thrown regarding `Possible null reference argument`s and `Dereferences of a possibly null reference` but both of those have never caused any problem for me.

Manually building is the recommended way to get a build of the Randomiser, as it is very likely that the builds available on the [releases](https://github.com/Knuxfan24/Sonic-06-Randomiser-Suite/releases) page will be out of date in some way.

# Usage
For a proper run down on the features and information on configuring and using the Randomiser Suite, check out the [Wiki](https://github.com/Knuxfan24/Sonic-06-Randomiser-Suite/wiki). For the basic functionality, see below.

# Features

### Object Placement Randomisation

Randomisation of various elements within '06's Object Placement (.set) files, these are:
*Enemy types and their behaviours
*The characters that get used in stages
*The contents of item capsules
*Types of props
*Voice lines that play throughout gameplay
*Types of doors
*Object draw/activation distance
*Miscellaneous cosmetic details.

A lot of the Object Placement features can be further randomised, see the Wiki for more information.

### Event Randomisation

Randomisation of various elements within the EventPlaybook.epb file that handles various factors of the game's cutscenes, these include: