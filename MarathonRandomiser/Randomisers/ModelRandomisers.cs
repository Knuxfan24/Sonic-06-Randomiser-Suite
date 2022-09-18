using Marathon.Formats.Mesh.Ninja;

namespace MarathonRandomiser
{
    internal class ModelRandomisers
    {
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
        /// <param name="diffuse">Whether or not to randomise the material's diffuse colour.</param>
        /// <param name="ambient">Whether or not to randomise the material's ambient colour.</param>
        /// <param name="specular">Whether or not to randomise the material's specular colour.</param>
        /// <param name="emissive">Whether or not to randomise the material's emissive colour.</param>
        public static async Task RandomiseMaterialColours(string xnoFile, bool? diffuse, bool? ambient, bool? specular, bool? emissive)
        {
            // This try catch is needed due to a couple of XNOs using a chunk that isn't supported in Marathon as of yet.
            try
            {
                // Load the XNO.
                NinjaNext xno = new(xnoFile);

                // Loop through each material colour in the XNO.
                foreach (NinjaMaterialColours? materialColour in xno.Data.Object.MaterialColours)
                {
                    // TODO: Should I really be changing the alpha value? That sounds like a poor choice.
                    if (diffuse == true)
                        materialColour.Diffuse = new((float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble());
                    if (ambient == true)
                        materialColour.Ambient = new((float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble());
                    if (specular == true)
                        materialColour.Specular = new((float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble());
                    if (emissive == true)
                        materialColour.Emissive = new((float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble(), (float)MainWindow.Randomiser.NextDouble());
                }

                // Save the updated XNO.
                xno.Save();
            }
            catch { }
        }
    }
}
