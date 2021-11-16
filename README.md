<p align="center">
    <img src="https://raw.githubusercontent.com/Knuxfan24/Sonic-06-Randomiser-Suite/master/MarathonRandomiser/ExternalResources/Logo.png"
         width="256"/>
</p>

<h1 align="center">Sonic '06 Randomiser Suite</h1>

<p align="center">An all in one software suite for randomising various elements in Sonic '06 on the Xbox 360 and PlayStation 3.</p>

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_general.png">

# Building
To build the Randomiser Suite, simply clone the project and open the `MarathonRandomiser.sln` file in a recent version of Visual Studio (anything supporting .NET 5 should work fine, all the development was done on preview builds of Visual Studio 2022) and then compile it as normal, if the build fails it may be due to missing Nuget Packages, as the project uses [Marathon](https://github.com/Big-Endian-32/Marathon) for interfacing with Sonic '06's file formats and [HandyControl](https://github.com/HandyOrg/HandyControl) for much nicer looking UI controls., ensuring that both of those are present and accounted for should resolve any compile errors.

Compile warnings will be thrown regarding `Possible null reference argument`s and `Dereferences of a possibly null reference` but both of those have never caused any problem for me.

Manually building is the recommended way to get a build of the Randomiser, as it is very likely that the builds available on the [releases](https://github.com/Knuxfan24/Sonic-06-Randomiser-Suite/releases) page will be out of date in some way.

# Usage
For a proper run down on the features and information on configuring and using the Randomiser Suite, check out the [Wiki](https://github.com/Knuxfan24/Sonic-06-Randomiser-Suite/wiki). For the basic functionality, see below.

# Features

### Object Placement Randomisation

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_object_placement.png">

Randomisation of various elements within '06's Object Placement (.set) files, these are:

* Enemy types and their behaviours

* The characters that get used in stages

* The contents of item capsules

* Types of props

* Voice lines that play throughout gameplay

* Types of doors

* Object draw/activation distance

* Miscellaneous cosmetic details.

A lot of the Object Placement features can be further randomised, see the Wiki for more information.

### Event Randomisation

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_event.png">

Randomisation of various elements within the EventPlaybook.epb file that handles various factors of the game's cutscenes, these are:

* The scene parameter file used in cutscenes

* The rotation of the scene

* The position in the map to base the scene around

* The voice lines used in cutscenes

* The map to play the scene in

* The order that cutscenes play in the game's story progression

### Scene Parameter Randomisation

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_scene.png">

Randomisation of various values within the game's scene lua binaries (.lub) files, these are:

* The colour and strength of the three light types (including secondary lights)

* The direction of a scene's light source(s)

* The colour and density of the scene's fog

* The environment map the scene should use

### Miscellaneous Randomisation

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_miscellaneous.png">

Randomisation of miscellaneous elements across one off files, these are:

* Change the music used in stages and missions

* The minimum and maximum health that enemies and bosses can have

* The surface type for stage collision

* Shuffling all of the game's text around

* Enabling random patches from the user's Mod Manager Patches Directory

### Custom Elements

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_custom.png">

Allows the user to add custom elements to the Randomisation, these being:

* Custom Music, supporting WAV (which is always used as part of the process), MP3 and M4A

* Voice Packs