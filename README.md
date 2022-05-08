<p align="center">
    <img src="https://raw.githubusercontent.com/Knuxfan24/Sonic-06-Randomiser-Suite/master/MarathonRandomiser/ExternalResources/Logo.png"
         width="256"/>
</p>

<h1 align="center">Sonic '06 Randomiser Suite</h1>

<p align="center">An all in one software suite for randomising various elements in Sonic '06 on the Xbox 360 and PlayStation 3.</p>

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_general.png">

# Building
To build the Randomiser Suite, simply clone the project and its submodules using a git based solution (such as GitHub Desktop) and open the `MarathonRandomiser.sln` file in a recent version of Visual Studio (anything supporting .NET 6 should work fine, all the development was done using versions of Visual Studio 2022) and then compile it as normal, if the build fails it may be due to missing Nuget Packages, as the project uses [Marathon](https://github.com/Big-Endian-32/Marathon) for interfacing with Sonic '06's file formats, [HandyControl](https://github.com/HandyOrg/HandyControl) for much nicer looking UI controls and [Ookii Dialogs](https://github.com/ookii-dialogs/ookii-dialogs-wpf) for better file and folder browsing; ensuring that all three of those are present and accounted for should resolve any compile errors. Also check that [XNCPLib and Amicitia.IO](https://github.com/crash5band/Shuriken) are both building as well, as they are needed for the User Interface Randomisation.

Compile warnings will be thrown regarding `Possible null reference argument`s and `Dereferences of a possibly null reference` but both of those have never caused any problem for me.

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

* Miscellaneous cosmetic details

* Types of particles

* Use of an unused jump board

* Shuffling object positions

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

* The skybox a stage map should use

### Animation Randomisation

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_animations.png">

Shuffling of both Gameplay and Event Animations, as well as Event Cameras; alongside being able to change the framerate of all Ninja Animations.

### Texture Randomisation

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_textures.png">

Shuffling of textures, with options for cross archive randomisation; alongside model vertex colour randomisation.

### Audio Randomisation

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_audio.png">

Shuffling of ingame stage music and sound effects.

### Text Randomisation

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_text.png">

Modification of various attributes relating to the game's text within the message tabe (.mst) files. These are:

* Randomisation of Button Icons

* Random text generation, with optional word length enforcement

* Random text colouration, with configurable chance

* Shuffling text between different message tables

### User Interface Randomisation

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_xncp.png">

Randomisation of various bits of data within the game's User Interface (.xncp) files. These are:

* Randomisation of the UI Element Colours.

* Randomisation of the UI Element's Scale.

* Shuffling of the UI Element's Depth.

* Randomisation of the texture coordinates used by elements on the UI.

### Miscellaneous Randomisation

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_miscellaneous.png">

Randomisation of miscellaneous elements across one off files, these are:

* The minimum and maximum health that enemies and bosses can have

* The surface type for stage collision

* Enabling random patches from the user's Mod Manager Patches Directory

* Optionally automatically unlocking Shadow and Silver's episodes from the start

### Custom Elements

<img src="https://raw.githubusercontent.com/wiki/Knuxfan24/Sonic-06-Randomiser-Suite/images/tab_custom.png">

Allows the user to add custom elements to the Randomisation, these being:

* Custom Music, supporting basically any format that [vgmstream](https://github.com/vgmstream/vgmstream) can convert.

* Textures

* Voice Packs