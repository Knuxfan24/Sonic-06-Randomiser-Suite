using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MarathonRandomiser
{
    internal class SceneRandomiser
    {
        /// <summary>
        /// Process and randomise elements in a scene lua binary.
        /// </summary>
        /// <param name="sceneLua">The filepath to the lua binary we're processing.</param>
        /// <param name="ambient">Whether or not to randomise ambient lighting.</param>
        /// <param name="main">Whether or not to randomise main lighting.</param>
        /// <param name="sub">Whether or not to randomise sub lighting.</param>
        /// <param name="minLight">The minimum value a light colour can be (used to provide a way to prevent "can't see shit" vision.</param>
        /// <param name="direction">Whether or not to randomise light directions.</param>
        /// <param name="enforceDirection">Whether or not to force the main lights to be above the player/horizon in some way.</param>
        /// <param name="fogColour">Whether or not to randomise fog colour.</param>
        /// <param name="fogDensity">Whether or not to randomise how thick the fog is.</param>
        /// <param name="env">Whether or not to randomise the cubemap.</param>
        /// <param name="SceneEnvMaps">The list of valid cubemap file paths.</param>
        /// <returns></returns>
        public static async Task Process(string sceneLua, bool? ambient, bool? main, bool? sub, double minLight, bool? direction, bool? enforceDirection, bool? fogColour, bool? fogDensity, bool? env, List<string> SceneEnvMaps)
        {
            // Decompile this lua file.
            await Task.Run(() => Helpers.LuaDecompile(sceneLua));

            // Read the decompiled lua file into a string array.
            string[] lua = File.ReadAllLines(sceneLua);

            // Loop through each line in this lua binary.
            for (int i = 0; i < lua.Length; i++)
            {
                // Lighting Colours
                if (lua[i].Contains("Ambient = {") && ambient == true)
                {
                    // Alternate Lighting Setups have another line denoting their type, factor this in.
                    if (!lua[i + 1].Contains("Type"))
                        await Task.Run(() => RGBA(lua, i + 2, 6, minLight));
                    else
                        await Task.Run(() => RGBA(lua, i + 3, 6, minLight));
                }

                if (lua[i].Contains("Main = {") && !lua[i].Contains("FarDistance") && !lua[i].Contains("ClipDistance") && main == true)
                {
                    // Check if this Lua doesn't have the final main block divided up because fuck you.
                    if (lua[i + 1].Contains("ClumpClipDistance") || lua[i + 1].Contains("FarDistance"))
                        continue;

                    // Alternate Lighting Setups have another line denoting their type, factor this in.
                    if (!lua[i + 1].Contains("Type"))
                        await Task.Run(() => RGBA(lua, i + 2, 6, minLight));
                    else
                        await Task.Run(() => RGBA(lua, i + 3, 6, minLight));
                }

                if (lua[i].Contains("Sub = {") && sub == true)
                {
                    // Alternate Lighting Setups have another line denoting their type, factor this in.
                    if (!lua[i + 1].Contains("Type"))
                        await Task.Run(() => RGBA(lua, i + 2, 6, minLight));
                    else
                        await Task.Run(() => RGBA(lua, i + 3, 6, minLight));
                }

                // Lighting Direction
                if (lua[i].Contains("Direction_3dsmax") && direction == true)
                    await Task.Run(() => Direction(lua, i + 2, enforceDirection));

                // Fog. Has special exceptions for Tropical Jungle B and C as they are the ONLY stages to handle fog differently, so they becoming pure, blinding white.
                if (!sceneLua.Contains("scene_tpj_b") && !sceneLua.Contains("scene_tpj_c"))
                {
                    // Colour
                    if (lua[i].Contains("BRay") && fogColour == true)
                        await Task.Run(() => RGBA(lua, i + 1, 4));
                    // Density
                    if (lua[i].Contains("BRay") && fogDensity == true)
                    {
                        string[] power = lua[i + 4].Split(' ');
                        power[4] = $"{MainWindow.Randomiser.NextDouble() * (0.001 - 0) + 0}";
                        lua[i + 4] = string.Join(' ', power);
                    }
                }

                // Environment Maps
                if (lua[i].Contains("EnvMap") && env == true)
                {
                    string[] envMap = lua[i + 1].Split(' ');
                    envMap[4] = $"\"{SceneEnvMaps[MainWindow.Randomiser.Next(SceneEnvMaps.Count)]}\"";
                    lua[i + 1] = string.Join(' ', envMap);
                }
            }

            // Save the updated lua binary.
            File.WriteAllLines(sceneLua, lua);
        }

        /// <summary>
        /// Generates random RGBA values above a certain value.
        /// </summary>
        /// <param name="lua">The string array we're using.</param>
        /// <param name="startPos">Where in the string array we should be.</param>
        /// <param name="splitLength">How long the split's length is (used by the fog colour).</param>
        /// <param name="minLight">The value we have to be higher than.</param>
        static async Task RGBA(string[] lua, int startPos, int splitLength, double minLight)
        {
            // Split the RGB values into string arrays.
            string[] rSplit = lua[startPos].Split(' ');
            string[] gSplit = lua[startPos + 1].Split(' ');
            string[] bSplit = lua[startPos + 2].Split(' ');
            string[] powerSplit = lua[startPos + 3].Split(' ');

            // Replace the value at the specified position with a random floating point number between 0 and 1, checking if it's above the minimum number specified.
            double value = MainWindow.Randomiser.NextDouble();
            do { value = MainWindow.Randomiser.NextDouble(); }
            while (value < minLight);
            rSplit[splitLength] = $"{value},";

            value = MainWindow.Randomiser.NextDouble();
            do { value = MainWindow.Randomiser.NextDouble(); }
            while (value < minLight);
            gSplit[splitLength] = $"{value},";

            value = MainWindow.Randomiser.NextDouble();
            do { value = MainWindow.Randomiser.NextDouble(); }
            while (value < minLight);
            bSplit[splitLength] = $"{value},";

            value = MainWindow.Randomiser.NextDouble();
            do { value = MainWindow.Randomiser.NextDouble(); }
            while (value < minLight);
            powerSplit[splitLength] = $"{value}";

            // Rejoin the splits into the main string array.
            lua[startPos] = string.Join(' ', rSplit);
            lua[startPos + 1] = string.Join(' ', gSplit);
            lua[startPos + 2] = string.Join(' ', bSplit);
            lua[startPos + 3] = string.Join(' ', powerSplit);
        }

        /// <summary>
        /// Generates random RGBA values.
        /// </summary>
        /// <param name="lua">The string array we're using.</param>
        /// <param name="startPos">Where in the string array we should be.</param>
        /// <param name="splitLength">How long the split's length is (used by the fog colour).</param>
        static async Task RGBA(string[] lua, int startPos, int splitLength)
        {
            // Split the RGB values into string arrays.
            string[] rSplit = lua[startPos].Split(' ');
            string[] gSplit = lua[startPos + 1].Split(' ');
            string[] bSplit = lua[startPos + 2].Split(' ');

            // Replace the value at the specified position with a random floating point number between 0 and 1.
            rSplit[splitLength] = $"{MainWindow.Randomiser.NextDouble()},";
            gSplit[splitLength] = $"{MainWindow.Randomiser.NextDouble()},";
            bSplit[splitLength] = $"{MainWindow.Randomiser.NextDouble()},";

            // Rejoin the splits into the main string array.
            lua[startPos] = string.Join(' ', rSplit);
            lua[startPos + 1] = string.Join(' ', gSplit);
            lua[startPos + 2] = string.Join(' ', bSplit);
        }

        /// <summary>
        /// Generates a random set of light direction values.
        /// </summary>
        /// <param name="lua">The string array we're using.</param>
        /// <param name="startPos">Where in the string array we should be.</param>
        /// <param name="enforce">Whether the direction should be enforced.</param>
        /// <returns></returns>
        static async Task Direction(string[] lua, int startPos, bool? enforce)
        {
            // Split the XYZ values into string arrays.
            string[] xSplit = lua[startPos].Split(' ');
            string[] ySplit = lua[startPos + 1].Split(' ');
            string[] zSplit = lua[startPos + 2].Split(' ');

            // Generate random floating point numbers between -1 and 1, with six decimal places.
            xSplit[8] = $"{Math.Round(MainWindow.Randomiser.NextDouble() * (1 - -1) + -1, 6)},";
            ySplit[8] = $"{Math.Round(MainWindow.Randomiser.NextDouble() * (1 - -1) + -1, 6)},";
            zSplit[8] = $"{Math.Round(MainWindow.Randomiser.NextDouble() * (1 - -1) + -1, 6)}";

            // If the light value on the Z Axis is negative, flip it.
            if (zSplit[8].Contains('-') && enforce == true)
                zSplit[8] = zSplit[8].Replace("-", string.Empty);

            // Rejoin the splits into the main string array.
            lua[startPos] = string.Join(' ', xSplit);
            lua[startPos + 1] = string.Join(' ', ySplit);
            lua[startPos + 2] = string.Join(' ', zSplit);
        }
    
        /// <summary>
        /// Randomises the skybox a stage should load.
        /// </summary>
        /// <param name="skyLua">The lua file we're processing.</param>
        /// <param name="SceneSkyboxes">The list of valid skybox paths.</param>
        public static async Task SkyboxRandomisation(string skyLua, List<string> SceneSkyboxes)
        {
            // Decompile this lua file.
            await Task.Run(() => Helpers.LuaDecompile(skyLua));

            // Read the decompiled lua file into a string array.
            string[] lua = File.ReadAllLines(skyLua);

            // Loop through each line in this lua binary. We'll do this twice, as some already use this function, so let's not add it a second time.
            bool foundSky = false;
            for (int i = 0; i < lua.Length; i++)
            {
                if(lua[i].Contains("Game.LoadSky"))
                {
                    // Set our indicator that this lua already had skybox set.
                    foundSky = true;

                    // Split the line controlling the skybox based on the quote marks around the stage folder path.
                    string[] sky = lua[i].Split('"');

                    // Replace the second value in the split array (the one containing the stage folder path) with a path from the list of valid skyboxes.
                    sky[1] = SceneSkyboxes[MainWindow.Randomiser.Next(SceneSkyboxes.Count)];

                    // Rejoin the split array into one line and add it back to the original lua array.
                    lua[i] = string.Join("\"", sky);
                }
            }

            // If we haven't found an existing Game.LoadSky() function, then loop through and create one.
            if(!foundSky)
            {
                for (int i = 0; i < lua.Length; i++)
                {
                    // Use the AddComponent call as a donor, as those always seem to be on the block that controls terrain loading.
                    if (lua[i].Contains("_ARG_0_:AddComponent({"))
                    {
                        // Add a new line under the AddComponent call with the Game.LoadSky() function.
                        lua[i] += $"\n    Game.LoadSky(\"{SceneSkyboxes[MainWindow.Randomiser.Next(SceneSkyboxes.Count)]}\"),";
                    }
                }
            }

            // Save the updated lua binary.
            File.WriteAllLines(skyLua, lua);
        }
    }
}
