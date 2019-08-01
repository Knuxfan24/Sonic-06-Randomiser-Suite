using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    class CharacterRandomisation
    {
        static public void MovementSpeedRandomiser(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);
            int lineNum = 0;
            double maxRunSpeed = 17f;
            double walkBorder = 0.1f;

            //Run Speed
            foreach (string line in editedLua)
            {
                if (line.StartsWith("c_run_speed_max"))
                {
                    string[] tempLine = line.Split(' ');
                    maxRunSpeed = Math.Round(rng.NextDouble() * (40 - 10) + 10, 1);
                    tempLine[2] = maxRunSpeed.ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }
            lineNum = 0;
            //Walk Speed
            foreach (string line in editedLua)
            {
                if (line.StartsWith("c_walk_speed_max"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = Math.Round(rng.NextDouble() * (maxRunSpeed - 4) + 4, 1).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }
            lineNum = 0;
            //Power Sneakers Speed
            foreach (string line in editedLua)
            {
                if (line.StartsWith("c_speedup_speed_max"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = (maxRunSpeed*2).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }

            //c_speedup_speed_max

            lineNum = 0;
            //Walk Border
            foreach (string line in editedLua)
            {
                if (line.StartsWith("c_walk_border"))
                {
                    string[] tempLine = line.Split(' ');
                    walkBorder = Math.Round(rng.NextDouble() * (0.8 - 0.1) + 0.1, 2);
                    tempLine[2] = walkBorder.ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }

            lineNum = 0;
            //Run Border
            foreach (string line in editedLua)
            {
                if (line.StartsWith("c_run_border"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = Math.Round(rng.NextDouble() * (0.9 - walkBorder) + walkBorder, 2).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
        static public void JumpRandomiser(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);
            int lineNum = 0;

            foreach (string line in editedLua)
            {
                //Jump Height
                if (line.StartsWith("c_jump_speed") && !line.Contains("c_jump_speed_sand") && !line.Contains("Sqrt"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = Math.Round(rng.NextDouble() * (20 - 8) + 8, 1).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                //Jump Momentum
                if (line.StartsWith("c_jump_run") && !line.Contains("HeightAndDistanceToSpeed"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = Math.Round(rng.NextDouble() * (20 - 5) + 5, 1).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
        static public void GrindSpeedRandomiser(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);
            int lineNum = 0;
            double maxGrindSpeed = 17f;

            //Grind Speed Max
            foreach (string line in editedLua)
            {
                if (line.StartsWith("c_grind_speed_max"))
                {
                    string[] tempLine = line.Split(' ');
                    maxGrindSpeed = Math.Round(rng.NextDouble() * (60 - 10) + 10, 1);
                    tempLine[2] = maxGrindSpeed.ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }
            lineNum = 0;
            //Grind Speed Start
            foreach (string line in editedLua)
            {
                if (line.StartsWith("c_grind_speed_org"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = Math.Round(rng.NextDouble() * (maxGrindSpeed - 10) + 10, 1).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("c_grind_acc"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = Math.Round(rng.NextDouble() * (30 - 5) + 5, 2).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
        static public void CharacterAbilityRandomiser(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);
            int lineNum = 0;
            int boundJump0Height = 4;
            int slidingSpeedMin = 10;
            int flightSpeedMin = 10;
            int flightTimer = 3;

            foreach (string line in editedLua)
            {
                //Amy Double Jump
                if (line.StartsWith("c_jump_double_count"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = rng.Next(1, 6).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }

                //Sonic Bounce Attack
                if (line.StartsWith("l_bound_jump_height0"))
                {
                    string[] tempLine = line.Split(' ');
                    boundJump0Height = rng.Next(1, 11);
                    tempLine[2] = boundJump0Height.ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("l_bound_jump_height1"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = rng.Next(boundJump0Height, 21).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                //Sonic Slide
                if (line.StartsWith("c_sliding_speed_min"))
                {
                    string[] tempLine = line.Split(' ');
                    slidingSpeedMin = rng.Next(1, 21);
                    tempLine[2] = slidingSpeedMin.ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("c_sliding_speed_max"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = rng.Next(slidingSpeedMin, 41).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                //Sonic Spindash
                if (line.StartsWith("c_spindash_spd"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = rng.Next(15, 46).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }

                //Tails Flight + Knuckles & Rouge Gliding
                if (line.StartsWith("c_flight_speed_min"))
                {
                    string[] tempLine = line.Split(' ');
                    flightSpeedMin = rng.Next(1, 21);
                    tempLine[2] = flightSpeedMin.ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("c_flight_speed_max"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = rng.Next(flightSpeedMin, 40).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("c_flight_timer"))
                {
                    string[] tempLine = line.Split(' ');
                    flightTimer = rng.Next(3, 11);
                    tempLine[2] = flightTimer.ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("c_flight_timer_b"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = (flightTimer / 2).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }

                //Knuckles & Rouge Climbing
                if (line.StartsWith("c_climb_speed"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = rng.Next(1, 21).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }

                //Silver Hover
                if (line.StartsWith("c_float_walk_speed"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = rng.Next(5, 21).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                //Silver Teleport Dash
                if (line.StartsWith("l_tele_dash"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[2] = rng.Next(2, 11).ToString();
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
        static public void ModelRandomiser(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);
            int lineNum = 0;
            int index;

            foreach (string line in editedLua)
            {
                //Jump Height
                if (line.StartsWith("c_model_package"))
                {
                    string[] tempLine = line.Split(' ');
                    index = rng.Next(CharacterRandomisationForm.validModels.Count);
                    tempLine[2] = CharacterRandomisationForm.validModels[index];
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
        static public void GemPatch(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);
            int lineNum = 0;

            //Grind Speed Max
            foreach (string line in editedLua)
            {
                if (line.StartsWith("c_gauge_green"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[0] = "c_green";
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("c_gauge_red"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[0] = "c_red";
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("c_gauge_blue"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[0] = "c_blue";
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("c_gauge_white"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[0] = "c_white";
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("c_gauge_sky"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[0] = "c_sky";
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("c_gauge_yellow"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[0] = "c_yellow";
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("c_gauge_purple"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[0] = "c_purple";
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("c_gauge_super"))
                {
                    string[] tempLine = line.Split(' ');
                    tempLine[0] = "c_super";
                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
    }
}
