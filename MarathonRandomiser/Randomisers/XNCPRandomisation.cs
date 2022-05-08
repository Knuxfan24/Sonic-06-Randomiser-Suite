using XNCPLib.XNCP;

namespace MarathonRandomiser
{
    internal class XNCPRandomisation
    {
        /// <summary>
        /// Process and randomise elements in an XNCP file.
        /// </summary>
        /// <param name="xncpFile">The filepath to the XNCP file we're processing.</param>
        /// <param name="colours">Whether or not to randomise the colour elements in this XNCP.</param>
        /// <param name="coloursSame">Whether or not all the corner points of the element should share the same colours.</param>
        /// <param name="coloursAlpha">Whether or not to randomise the alpha value as well.</param>
        /// <param name="scale">Whether or not to randomly scale the XNCP elements.</param>
        /// <param name="scaleMin">The minimum scale value to use.</param>
        /// <param name="scaleMax">The maximum scale value to use.</param>
        /// <param name="zIndex">Whether or not to shuffle the ZIndex values.</param>
        /// <returns></returns>
        public static async Task Process(string xncpFile, bool? colours, bool? coloursSame, bool? coloursAlpha, bool? scale, double scaleMin, double scaleMax, bool? zIndex)
        {
            // Set up a couple of list of numbers for the layer depths.
            List<float> zIndexList = new();
            List<int> usedNumbers = new();

            // Load this XNCP file.
            FAPCFile? xncp = new();
            xncp.Load(xncpFile);

            // Loop through a massive nest to find the casts.
            foreach (FAPCEmbeddedRes? resource in xncp.Resources)
            {
                // Check if this CsdmProject exists (some vanilla XNCPs are missing this in a resource or two).
                if (resource.Content.CsdmProject != null)
                {
                    foreach (Scene? scene in resource.Content.CsdmProject.Root.Scenes)
                    {
                        // Get the ZIndex of this scene.
                        zIndexList.Add(scene.ZIndex);

                        foreach (CastGroup? uiCast in scene.UICastGroups)
                        {
                            foreach (Cast? cast in uiCast.Casts)
                            {
                                // Element Colour Randomisation.
                                if (colours == true)
                                {
                                    // Get random RGB values and set Color to the generated values.
                                    byte[] rgba = await Task.Run(() => RGBA(coloursAlpha, cast.CastInfoData.Color));
                                    cast.CastInfoData.Color = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);

                                    // If we're using the same colours for all the points then handle it here.
                                    if (coloursSame == true)
                                    {
                                        // Simply set the same values if we're randomising alpha.
                                        if (coloursAlpha == true)
                                        {
                                            cast.CastInfoData.GradientBottomLeft = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);
                                            cast.CastInfoData.GradientBottomRight = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);
                                            cast.CastInfoData.GradientTopLeft = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);
                                            cast.CastInfoData.GradientTopRight = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);
                                        }
                                        // If we're not, then fetch the original ones before applying them.
                                        else
                                        {
                                            rgba[3] = (byte)int.Parse(cast.CastInfoData.GradientBottomLeft.ToString("X8")[^2..], System.Globalization.NumberStyles.HexNumber);
                                            cast.CastInfoData.GradientBottomLeft = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);

                                            rgba[3] = (byte)int.Parse(cast.CastInfoData.GradientBottomRight.ToString("X8")[^2..], System.Globalization.NumberStyles.HexNumber);
                                            cast.CastInfoData.GradientBottomRight = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);

                                            rgba[3] = (byte)int.Parse(cast.CastInfoData.GradientTopLeft.ToString("X8")[^2..], System.Globalization.NumberStyles.HexNumber);
                                            cast.CastInfoData.GradientTopLeft = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);

                                            rgba[3] = (byte)int.Parse(cast.CastInfoData.GradientTopRight.ToString("X8")[^2..], System.Globalization.NumberStyles.HexNumber);
                                            cast.CastInfoData.GradientTopRight = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);
                                        }
                                    }

                                    // If we're not using the same colours for all the points then generate new ones for each one.
                                    else
                                    {
                                        // Get random RGB values and set GradientBottomLeft to the generated values.
                                        rgba = await Task.Run(() => RGBA(coloursAlpha, cast.CastInfoData.GradientBottomLeft));
                                        cast.CastInfoData.GradientBottomLeft = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);

                                        // Get random RGB values and set GradientBottomRight to the generated values.
                                        rgba = await Task.Run(() => RGBA(coloursAlpha, cast.CastInfoData.GradientBottomRight));
                                        cast.CastInfoData.GradientBottomRight = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);

                                        // Get random RGB values and set GradientTopLeft to the generated values.
                                        rgba = await Task.Run(() => RGBA(coloursAlpha, cast.CastInfoData.GradientTopLeft));
                                        cast.CastInfoData.GradientTopLeft = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);

                                        // Get random RGB values and set GradientTopRight to the generated values.
                                        rgba = await Task.Run(() => RGBA(coloursAlpha, cast.CastInfoData.GradientTopRight));
                                        cast.CastInfoData.GradientTopRight = uint.Parse($"{rgba[0]:X}{rgba[1]:X}{rgba[2]:X}{rgba[3]:X}", System.Globalization.NumberStyles.HexNumber);
                                    }
                                }

                                // Element Scale Randomisation.
                                if (scale == true)
                                {
                                    // Based on this: https://www.delftstack.com/howto/csharp/generate-a-random-float-in-csharp/#generate-random-float-within-a-specific-range-with-the-random-nextdouble-function-in-c
                                    double range = scaleMax - scaleMin;
                                    cast.CastInfoData.Scale = new((float)((MainWindow.Randomiser.NextDouble() * range) + scaleMin), (float)((MainWindow.Randomiser.NextDouble() * range) + scaleMax));
                                }
                            }
                        }
                    }
                }
            }

            // Loop again if we're randomising the ZIndex value.
            if (zIndex == true)
            {
                foreach (FAPCEmbeddedRes? resource in xncp.Resources)
                {
                    if (resource.Content.CsdmProject != null)
                    {
                        foreach (Scene? scene in resource.Content.CsdmProject.Root.Scenes)
                        {
                            // Pick a random number from the amount of entires in the zIndexList.
                            int index = MainWindow.Randomiser.Next(zIndexList.Count);

                            // If the selected number is already used, pick another until it isn't.
                            if (usedNumbers.Contains(index))
                            {
                                do { index = MainWindow.Randomiser.Next(zIndexList.Count); }
                                while (usedNumbers.Contains(index));
                            }

                            // Add this number to the usedNumbers list so we can't pull the same value twice.
                            usedNumbers.Add(index);

                            // Set the ZIndex to the chosen number.
                            scene.ZIndex = zIndexList[index];
                        }
                    }
                }
            }

            // Save the modified XNCP.
            xncp.Save(xncpFile);
        }

        /// <summary>
        /// Generates a random set of RGB values, with an optional alpha value.
        /// </summary>
        /// <param name="coloursAlpha">Whether or not we need to generate an alpha value as well.</param>
        /// <param name="originalColour">If we're not generating an alpha value, this number is used to parse the original one from the XNCP element.</param>
        /// <returns></returns>
        static async Task<byte[]> RGBA(bool? coloursAlpha, uint originalColour)
        {
            // Make the RGBA byte array to store the values in and return later.
            byte[] rgba = new byte[4];

            // Generate the RGB values.
            rgba[0] = (byte)MainWindow.Randomiser.Next(0, 256);
            rgba[1] = (byte)MainWindow.Randomiser.Next(0, 256);
            rgba[2] = (byte)MainWindow.Randomiser.Next(0, 256);

            // Generate an alpha value if we need it, if not, just parse it from the original colour value.
            if (coloursAlpha == true)
                rgba[3] = (byte)MainWindow.Randomiser.Next(0, 256);
            else
                rgba[3] = (byte)int.Parse(originalColour.ToString("X8")[^2..], System.Globalization.NumberStyles.HexNumber);

            // Return the RGBA byte array.
            return rgba;
        }
    }
}
