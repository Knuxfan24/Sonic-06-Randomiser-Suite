using Marathon.Formats.Mesh.Ninja;

namespace MarathonRandomiser
{
    internal class TextureRandomiser
    {
        /// <summary>
        /// Fetch all the valid textures in the specified archive and adds them to a list for later use.
        /// </summary>
        /// <param name="Textures">The list of valid textures to update.</param>
        /// <param name="archivePath">The path to the current archive.</param>
        public static async Task<List<string>> FetchTextures(List<string> Textures, string archivePath)
        {
            // Find all the DDS texture files in this archive.
            string[] ddsFiles = Directory.GetFiles(archivePath, "*.dds", SearchOption.AllDirectories);

            // Loop through each DDS.
            for (int i = 0; i < ddsFiles.Length; i++)
            {
                // If it's not a shadow, light or cubemap, or a font, then throw a .rnd extension on and add it to the list.
                if (!ddsFiles[i].Contains("_sdw_") && !ddsFiles[i].Contains("_lm_") && !ddsFiles[i].Contains("_zlm_") && !ddsFiles[i].Contains("envmap") && !ddsFiles[i].Contains("suna_cube") && !ddsFiles[i].Contains("cubemap") && !ddsFiles[i].Contains("font"))
                {
                    File.Move(ddsFiles[i], $"{ddsFiles[i]}.rnd");
                    Textures.Add($"{ddsFiles[i]}.rnd");
                }
            }

            // Return the update texture list.
            return Textures;
        }

        /// <summary>
        /// Shuffles textures between archives.
        /// </summary>
        /// <param name="usedNumbers">A list showing what textures have already being used.</param>
        /// <param name="archivePath">The path to the current archive.</param>
        /// <param name="Textures">The list of valid textures.</param>
        /// <param name="allowDupes">Whether or not the same texture can be used more than once.</param>
        public static async Task<List<int>> ShuffleTextures(List<int> usedNumbers, string archivePath, List<string> Textures, bool? allowDupes)
        {
            // Find all the previously determined as valid texture files in this archive.
            string[] ddsFiles = Directory.GetFiles(archivePath, "*.dds.rnd", SearchOption.AllDirectories);

            // Loop through the textures.
            for (int i = 0; i < ddsFiles.Length; i++)
            {
                // Pick a random number from the amount of entires in the Textures list.
                int index = MainWindow.Randomiser.Next(Textures.Count);

                if (allowDupes == false)
                {
                    // If the selected number is already used, pick another until it isn't.
                    if (usedNumbers.Contains(index))
                    {
                        do { index = MainWindow.Randomiser.Next(Textures.Count); }
                        while (usedNumbers.Contains(index));
                    }

                    // Add this number to the usedNumbers list so we can't pull the same DDS file twice.
                    usedNumbers.Add(index);
                }

                // Copy this DDS to the name of the chosen one, minus our .rnd addition.
                File.Copy(Textures[index], $"{ddsFiles[i].Remove(ddsFiles[i].LastIndexOf('.'))}");
            }

            // Return our updated list of used numbers.
            return usedNumbers;
        }

        /// <summary>
        /// Randomises textures per archive.
        /// </summary>
        /// <param name="archivePath">The path to the current archive.</param>
        /// <param name="CustomTextureFiles">Our custom textures.</param>
        /// <param name="allowDupes">Whether or not the same texture can be used more than once.</param>
        /// <param name="onlyCustom">Whether ONLY custom textures can be used.</param>
        public static async Task PerArchive(string archivePath, List<string> CustomTextureFiles, bool? allowDupes, bool? onlyCustom)
        {
            // Set up a list of numbers.
            List<int> usedNumbers = new();

            // Find all the DDS texture files in this archive.
            string[] ddsFiles = Directory.GetFiles(archivePath, "*.dds", SearchOption.AllDirectories);

            // Set up a new list to fill with valid DDS files.
            List<string> validDDSFiles = new();
            List<string> validDDSFilesReadOnly = new();

            for (int i = 0; i < ddsFiles.Length; i++)
            {
                // If it's not a shadow, light or cubemap, or a font, then throw a .rnd extension on and add it to the list.
                if (!ddsFiles[i].Contains("_sdw_") && !ddsFiles[i].Contains("_lm_") && !ddsFiles[i].Contains("_zlm_") && !ddsFiles[i].Contains("envmap") && !ddsFiles[i].Contains("suna_cube") && !ddsFiles[i].Contains("cubemap") && !ddsFiles[i].Contains("font"))
                {
                    File.Move(ddsFiles[i], $"{ddsFiles[i]}.rnd");
                    validDDSFiles.Add($"{ddsFiles[i]}.rnd");
                    validDDSFilesReadOnly.Add($"{ddsFiles[i]}.rnd");
                }
            }
            if (onlyCustom == true)
                validDDSFiles.Clear();

            foreach (string custom in CustomTextureFiles)
                validDDSFiles.Add(custom);

            // Loop through again for the actual randomisation, based on the new list..
            for (int i = 0; i < validDDSFilesReadOnly.Count; i++)
            {
                // Pick a random number from the amount of entires in the DDS list.
                int index = MainWindow.Randomiser.Next(validDDSFiles.Count);

                if (allowDupes == false)
                {
                    // If the selected number is already used, pick another until it isn't.
                    if (usedNumbers.Contains(index))
                    {
                        do { index = MainWindow.Randomiser.Next(validDDSFiles.Count); }
                        while (usedNumbers.Contains(index));
                    }

                    // Add this number to the usedNumbers list so we can't pull the same DDS file twice.
                    usedNumbers.Add(index);
                }

                // Copy this DDS to the name of the chosen one, minus our .rnd addition.
                File.Copy(validDDSFiles[index], $"{validDDSFilesReadOnly[i].Remove(validDDSFilesReadOnly[i].LastIndexOf('.'))}");
            }
        }

        /// <summary>
        /// Deletes the textures in the archive.
        /// </summary>
        /// <param name="archivePath">The archive to delete textures in.</param>
        public static async Task DeleteTextures(string archivePath)
        {
            // Find all the DDS texture files in this archive.
            string[] ddsFiles = Directory.GetFiles(archivePath, "*.dds", SearchOption.AllDirectories);

            // Freaking yeet 'em.
            foreach(string ddsFile in ddsFiles)
                File.Delete(ddsFile);
        }
    
        /// <summary>
        /// Randomises the Vertex Colours in an XNO.
        /// </summary>
        /// <param name="xnoFile">The XNO to process.</param>
        public static async Task RandomiseVertexColours(string xnoFile)
        {
            // This try catch is needed due to a couple of XNOs using a chunk that isn't supported in Marathon as of yet.
            try
            {
                // Load the XNO.
                NinjaNext xno = new(xnoFile);

                // Loop through each vertex lits in the XNO.
                foreach (NinjaVertexList? vertexList in xno.Data.Object.VertexLists)
                {
                    // Loop through each vertex in the vertex list.
                    foreach (NinjaVertex? vertex in vertexList.Vertices)
                    {
                        // If this vertex has colours (they might all do? this is to be safe), then randomly generate a value for each.
                        if (vertex.VertexColours != null)
                        {
                            vertex.VertexColours[0] = (byte)MainWindow.Randomiser.Next(0, 256);
                            vertex.VertexColours[1] = (byte)MainWindow.Randomiser.Next(0, 256);
                            vertex.VertexColours[2] = (byte)MainWindow.Randomiser.Next(0, 256);
                        }

                        // If this weird second set is present, then generate values for those too.
                        if (vertex.VertexColours2 != null)
                        {
                            vertex.VertexColours2[0] = (byte)MainWindow.Randomiser.Next(0, 256);
                            vertex.VertexColours2[1] = (byte)MainWindow.Randomiser.Next(0, 256);
                            vertex.VertexColours2[2] = (byte)MainWindow.Randomiser.Next(0, 256);
                        }
                    }
                }

                // Save the updated XNO.
                xno.Save();
            }
            catch { }
        }

        /// <summary>
        /// Randomises the Material Colours in an XNO.
        /// </summary>
        /// <param name="xnoFile">The XNO to process.</param>
        /// <param name="doWhite">Whether materials that have their values the same (so are a white colour) should be randomised as well.</param>
        public static async Task RandomiseMaterialColours(string xnoFile, bool? doWhite)
        {
            // TODO: Mess with stuff other than diffuse and see if there's an obvious difference, if so, maybe add more options.

            // This try catch is needed due to a couple of XNOs using a chunk that isn't supported in Marathon as of yet.
            try
            {
                // Load the XNO.
                NinjaNext xno = new(xnoFile);

                // Loop through each material colour in the XNO.
                foreach (NinjaMaterialColours? materialColour in xno.Data.Object.MaterialColours)
                {
                    // If we're not colouring materials that have a white value, then just skip this one.
                    if (doWhite == false && (materialColour.Diffuse.X == materialColour.Diffuse.Y) && (materialColour.Diffuse.X == materialColour.Diffuse.Z))
                        continue;

                    // TODO: Should I really be changing the alpha value? That sounds like a poor choice.
                    materialColour.Diffuse = new((float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble());
                }

                // Save the updated XNO.
                xno.Save();
            }
            catch { }
        }
    }
}
