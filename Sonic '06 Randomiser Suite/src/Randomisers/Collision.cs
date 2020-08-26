using System;
using System.Text;
using Marathon.IO.Formats.Meshes;

namespace Sonic_06_Randomiser_Suite
{
    class Collision_Randomiser
    {
        public static void PropertyRandomiser(string filePath, Random rng, bool respectWalls, bool respectWater, bool respectDeath)
        {
            Collision collision = new Collision();
            collision.Load(filePath);

            for (int i = 0; i < collision.Faces.Count; i++)
            {
                StringBuilder surfaceType = new StringBuilder("00000000", 8);
                string collisionFlags = collision.Faces[i].Flags.ToString("X8");

                if (respectWalls)
                {
                    // Wall
                    if (collisionFlags[3].ToString() == "1") {
                        surfaceType.Remove(3, 1);
                        surfaceType.Insert(3, "1");
                    }

                    // No Stand
                    if (collisionFlags[3].ToString() == "4") {
                        surfaceType.Remove(3, 1);
                        surfaceType.Insert(3, "4");
                    }

                    // Fall if player stops moving
                    if (collisionFlags[0].ToString() == "1") {
                        surfaceType.Remove(0, 1);
                        surfaceType.Insert(0, "1");
                    }

                    // Climbable Wall
                    if (collisionFlags[0].ToString() == "8") {
                        surfaceType.Remove(0, 1);
                        surfaceType.Insert(0, "8");
                    }

                    // Unknown
                    if (collisionFlags[1].ToString() == "8") {
                        surfaceType.Remove(1, 1);
                        surfaceType.Insert(1, "8");
                    }

                    // Player Only Collision
                    if (collisionFlags[2].ToString() == "2") {
                        surfaceType.Remove(2, 1);
                        surfaceType.Insert(2, "2");
                    }

                    // Unknown
                    if (collisionFlags[2].ToString() == "8") {
                        surfaceType.Remove(2, 1);
                        surfaceType.Insert(2, "8");
                    }
                }

                if (respectWater)
                {
                    // Water
                    if (collisionFlags[3].ToString() == "8") {
                        surfaceType.Remove(3, 1);
                        surfaceType.Insert(3, "8");
                    }

                    // Water
                    if (collisionFlags[0].ToString() == "4") {
                        surfaceType.Remove(0, 1);
                        surfaceType.Insert(0, "4");
                    }
                }

                if (respectDeath)
                {
                    // Corner Damage
                    if (collisionFlags[0].ToString() == "2") {
                        surfaceType.Remove(0, 1);
                        surfaceType.Insert(0, "2");
                    }

                    // Deadly Water
                    if (collisionFlags[0].ToString() == "6") {
                        surfaceType.Remove(0, 1);
                        surfaceType.Insert(0, "6");
                    }

                    // Damage
                    if (collisionFlags[1].ToString() == "8") {
                        surfaceType.Remove(1, 1);
                        surfaceType.Insert(1, "8");
                    }

                    // Death
                    if (collisionFlags[2].ToString() == "1") {
                        surfaceType.Remove(2, 1);
                        surfaceType.Insert(2, "1");
                    }
                }

                surfaceType.Remove(7, 1);
                surfaceType.Insert(7, Main.Collision_Surfaces[rng.Next(Main.Collision_Surfaces.Count)]);

                collision.Faces[i].Flags = Convert.ToUInt32(surfaceType.ToString(), 16);
            }

            collision.Save(filePath, true);
        }
    }
}
