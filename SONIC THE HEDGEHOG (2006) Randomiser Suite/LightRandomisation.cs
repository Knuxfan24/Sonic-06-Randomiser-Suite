using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    class LightRandomisation
    {
        static public void AmbientRandomiser(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);

            int lineNum = 0;
            foreach (string line in editedLua)
            {
                bool isLightColourLine = line.Contains("Ambient");
                if (isLightColourLine)
                {
                    //Red
                    string[] colourRArray = editedLua[lineNum + 2].Split(' ');
                    double colourR = Math.Round(rng.NextDouble(), 2);
                    colourRArray[colourRArray.GetUpperBound(0)] = colourR.ToString() + ",";

                    //Green
                    string[] colourGArray = editedLua[lineNum + 3].Split(' ');
                    double colourG = Math.Round(rng.NextDouble(), 2);
                    colourGArray[colourGArray.GetUpperBound(0)] = colourG.ToString() + ",";

                    //Blue
                    string[] colourBArray = editedLua[lineNum + 4].Split(' ');
                    double colourB = Math.Round(rng.NextDouble(), 2);
                    colourBArray[colourBArray.GetUpperBound(0)] = colourB.ToString() + ",";

                    editedLua[lineNum + 2] = string.Join(" ", colourRArray);
                    editedLua[lineNum + 3] = string.Join(" ", colourGArray);
                    editedLua[lineNum + 4] = string.Join(" ", colourBArray);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
        static public void MainRandomiser(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);

            int lineNum = 0;
            foreach (string line in editedLua)
            {
                bool isLightColourLine = (line.Contains("Main") && !line.Contains("FarDistance") && !line.Contains("ClipDistance"));
                if (isLightColourLine)
                {
                    //Red
                    string[] colourRArray = editedLua[lineNum + 2].Split(' ');
                    double colourR = Math.Round(rng.NextDouble(), 2);
                    colourRArray[colourRArray.GetUpperBound(0)] = colourR.ToString() + ",";

                    //Green
                    string[] colourGArray = editedLua[lineNum + 3].Split(' ');
                    double colourG = Math.Round(rng.NextDouble(), 2);
                    colourGArray[colourGArray.GetUpperBound(0)] = colourG.ToString() + ",";

                    //Blue
                    string[] colourBArray = editedLua[lineNum + 4].Split(' ');
                    double colourB = Math.Round(rng.NextDouble(), 2);
                    colourBArray[colourBArray.GetUpperBound(0)] = colourB.ToString() + ",";

                    editedLua[lineNum + 2] = string.Join(" ", colourRArray);
                    editedLua[lineNum + 3] = string.Join(" ", colourGArray);
                    editedLua[lineNum + 4] = string.Join(" ", colourBArray);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
        static public void SubRandomiser(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);

            int lineNum = 0;
            foreach (string line in editedLua)
            {
                bool isLightColourLine = line.Contains("Sub");
                if (isLightColourLine)
                {
                    //Red
                    string[] colourRArray = editedLua[lineNum + 2].Split(' ');
                    double colourR = Math.Round(rng.NextDouble(), 2);
                    colourRArray[colourRArray.GetUpperBound(0)] = colourR.ToString() + ",";

                    //Green
                    string[] colourGArray = editedLua[lineNum + 3].Split(' ');
                    double colourG = Math.Round(rng.NextDouble(), 2);
                    colourGArray[colourGArray.GetUpperBound(0)] = colourG.ToString() + ",";

                    //Blue
                    string[] colourBArray = editedLua[lineNum + 4].Split(' ');
                    double colourB = Math.Round(rng.NextDouble(), 2);
                    colourBArray[colourBArray.GetUpperBound(0)] = colourB.ToString() + ",";

                    editedLua[lineNum + 2] = string.Join(" ", colourRArray);
                    editedLua[lineNum + 3] = string.Join(" ", colourGArray);
                    editedLua[lineNum + 4] = string.Join(" ", colourBArray);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
        static public void DirectionRandomiser(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);

            int lineNum = 0;
            foreach (string line in editedLua)
            {
                bool isLightColourLine = line.Contains("Direction_3dsmax");
                if (isLightColourLine)
                {
                    //X
                    string[] directionXArray = editedLua[lineNum + 2].Split(' ');
                    double directionX = Math.Round(rng.NextDouble(), 6);
                    directionXArray[directionXArray.GetUpperBound(0)] = directionX.ToString() + ",";

                    //Y
                    string[] directionYArray = editedLua[lineNum + 3].Split(' ');
                    double directionY = Math.Round(rng.NextDouble(), 6);
                    directionYArray[directionYArray.GetUpperBound(0)] = directionY.ToString() + ",";

                    //Z
                    string[] directionZArray = editedLua[lineNum + 4].Split(' ');
                    double directionZ = Math.Round(rng.NextDouble(), 6);
                    directionZArray[directionZArray.GetUpperBound(0)] = directionZ.ToString() + ",";

                    editedLua[lineNum + 2] = string.Join(" ", directionXArray);
                    editedLua[lineNum + 3] = string.Join(" ", directionYArray);
                    editedLua[lineNum + 4] = string.Join(" ", directionZArray);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
        static public void FogColourRandomiser(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);

            int lineNum = 0;
            foreach (string line in editedLua)
            {
                bool isLightColourLine = line.Contains("BRay");
                if (isLightColourLine)
                {
                    //Red
                    string[] colourRArray = editedLua[lineNum + 1].Split(' ');
                    double colourR = Math.Round(rng.NextDouble(), 2);
                    colourRArray[colourRArray.GetUpperBound(0)] = colourR.ToString() + ",";

                    //Green
                    string[] colourGArray = editedLua[lineNum + 2].Split(' ');
                    double colourG = Math.Round(rng.NextDouble(), 2);
                    colourGArray[colourGArray.GetUpperBound(0)] = colourG.ToString() + ",";

                    //Blue
                    string[] colourBArray = editedLua[lineNum + 3].Split(' ');
                    double colourB = Math.Round(rng.NextDouble(), 2);
                    colourBArray[colourBArray.GetUpperBound(0)] = colourB.ToString() + ",";

                    editedLua[lineNum + 1] = string.Join(" ", colourRArray);
                    editedLua[lineNum + 2] = string.Join(" ", colourGArray);
                    editedLua[lineNum + 3] = string.Join(" ", colourBArray);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
        static public void FogDensityRandomiser(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);

            int lineNum = 0;
            foreach (string line in editedLua)
            {
                bool isLightColourLine = line.Contains("BRay");
                if (isLightColourLine)
                {
                    //Density?
                    string[] densityArray = editedLua[lineNum + 4].Split(' ');
                    int densityNonRound = rng.Next(0, 101);
                    string density = "0.00100";
                    if (densityNonRound.ToString().Count() == 2)
                    {
                        density = "0.000" + densityNonRound.ToString();
                    }
                    if (densityNonRound.ToString().Count() == 1)
                    {
                        density = "0.0000" + densityNonRound.ToString();
                    }
                    densityArray[densityArray.GetUpperBound(0)] = density;

                    editedLua[lineNum + 4] = string.Join(" ", densityArray);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
    }
}
