using HedgeLib;
using System.IO;
using HedgeLib.Sets;

namespace Sonic_06_Randomiser_Suite
{
    class Beatable
    {
        public static void DetermineStage(string setPath, S06SetData set)
        {
            switch(Path.GetFileNameWithoutExtension(setPath))
            {
                case "set_mission_0001_01": set_mission_0001_01(set); break; // Before Sonic's Wave Ocean
                case "set_wvoA_sonic": set_wvoA_sonic(set); break; //Wave Ocean A (Sonic)
            }
        }

        public static void set_mission_0001_01(S06SetData set)
        {
            if(set.Objects[99].Parameters[1].Data.ToString() == "sonic_new") { return; }

            uint objectID = 0;
            foreach(SetObject existingObj in set.Objects)
            {
                if (existingObj.ObjectID != 0) { objectID = existingObj.ObjectID; }
            }

            SetObject spring_twn1 = Objects.spring(new Vector3(-5007.04f, 280.006f, 1304.52f), new Quaternion(-0.23457f, -0.408218f, -0.109382f, 0.875426f), 2000f, 1f, 293u, 6000, objectID, true);
            set.Objects.Add(spring_twn1);
            objectID++;

            SetObject pointsample1 = Objects.pointsample(new Vector3(-9655.91f, 210.306f, 5491.07f), new Quaternion(0f, 0f, 0f, 1f), 0, objectID);
            set.Objects.Add(pointsample1);
            objectID++;
        }

        public static void set_wvoA_sonic(S06SetData set)
        {
            uint objectID = 0;
            foreach (SetObject existingObj in set.Objects)
            {
                if (existingObj.ObjectID != 0) { objectID = existingObj.ObjectID; }
            }

            // First Loop
            SetObject jumppanel1 = Objects.jumppanel(new Vector3(2009.92f, 18.018f, -6491.59f), new Quaternion(0f, 0.91103f, 0f, 0.412344f), 45f, 4000f, 1f, 0u, objectID);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { jumppanel1.UnknownBytes[3] = 1; }
            set.Objects.Add(jumppanel1);
            objectID++;

            // Second Loop
            SetObject jumppanel2 = Objects.jumppanel(new Vector3(20772.4f, 36.8728f, -30022.3f), new Quaternion(0f, 1f, 0f, -0.000000162921f), 45f, 4000f, 1f, 196u, objectID);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { jumppanel2.UnknownBytes[3] = 1; }
            set.Objects.Add(jumppanel2);
            objectID++;

            // Large Double Loop
            SetObject jumppanel3 = Objects.jumppanel(new Vector3(-1548.01f, 24.5935008f, -46324.3f), new Quaternion(0f, 0.814631f, 0f, 0.579979f), 26f, 2300f, 1.8f, 696u, objectID);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { jumppanel3.UnknownBytes[3] = 1; }
            set.Objects.Add(jumppanel3);
            objectID++;

            SetObject jumppanel4 = Objects.jumppanel(new Vector3(-1448.23f, 24.5935008f, -46038.3f), new Quaternion(0f, 0.814631f, 0f, 0.579979f), 26f, 2300f, 1.8f, 696u, objectID);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { jumppanel4.UnknownBytes[3] = 1; }
            set.Objects.Add(jumppanel4);
            objectID++;

            SetObject spring1 = Objects.spring(new Vector3(573.580027f, 488.078f, -47987.1979f), new Quaternion(0f, 0.507095f, 0f, 0.86189f), 2000f, 0.1f, 697u, 0, objectID, false);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { spring1.UnknownBytes[3] = 1; }
            set.Objects.Add(spring1);
            objectID++;

            SetObject spring2 = Objects.spring(new Vector3(2268.15987f, 66.4865f, -47811f), new Quaternion(0f, 0.93664f, 0f, -0.350294f), 2000f, 0.1f, 698u, 0, objectID, false);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { spring2.UnknownBytes[3] = 1; }
            set.Objects.Add(spring2);
            objectID++;

            SetObject spring3 = Objects.spring(new Vector3(1665.64f, 341.854f, -49514.1022f), new Quaternion(0f, 0.914834f, 0f, -0.40383f), 2000f, 0.1f, 699u, 0, objectID, false);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { spring3.UnknownBytes[3] = 1; }
            set.Objects.Add(spring3);
            objectID++;

            SetObject spring4 = Objects.spring(new Vector3(2353.03f, 319.71f, -50343.6981f), new Quaternion(0f, 0.886519f, 0f, -0.462692f), 1500f, 0.1f, 700u, 0, objectID, false);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { spring4.UnknownBytes[3] = 1; }
            set.Objects.Add(spring4);
            objectID++;

            SetObject pointsample1 = Objects.pointsample(new Vector3(1120.04f, 119.67f, -52929.2f), new Quaternion(0f, 0f, 0f, 1f), 0, objectID);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { pointsample1.UnknownBytes[3] = 1; }
            set.Objects.Add(pointsample1);
            objectID++;

            // Before Whale Chase
            SetObject jumppanel5 = Objects.jumppanel(new Vector3(-32387.4f, 186.813f, -70859f), new Quaternion(0f, -0.840452f, 0f, 0.541886f), 26f, 2300f, 1.8f, 703u, objectID);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { jumppanel5.UnknownBytes[3] = 1; }
            set.Objects.Add(jumppanel5);
            objectID++;

            SetObject jumppanel6 = Objects.jumppanel(new Vector3(-32505.9f, 186.813f, -70582.8f), new Quaternion(0f, -0.840452f, 0f, 0.541886f), 26f, 2300f, 1.8f, 703u, objectID);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { jumppanel6.UnknownBytes[3] = 1; }
            set.Objects.Add(jumppanel6);
            objectID++;

            SetObject spring5 = Objects.spring(new Vector3(-37352.9f, 1080.27f, -71987.6f), new Quaternion(0f, -0.993854f, 0f, 0.110732f), 3000, 0.5f, 704u, 0, objectID, false);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { spring5.UnknownBytes[3] = 1; }
            set.Objects.Add(spring5);
            objectID++;

            SetObject spring6 = Objects.spring(new Vector3(-37851.6f, 1220.91f, -74300.4f), new Quaternion(-0.0604923f, -0.750435f, -0.0693582f, 0.654509f), 3000, 0.5f, 705u, 0, objectID, false);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { spring6.UnknownBytes[3] = 1; }
            set.Objects.Add(spring6);
            objectID++;

            SetObject spring7 = Objects.spring(new Vector3(-41289.2f, 1208.37f, -74961.5f), new Quaternion(0.00417437f, 0.994243f, -0.0235483f, 0.104464f), 3000, 0.5f, 335u, 0, objectID, false);
            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { spring7.UnknownBytes[3] = 1; }
            set.Objects.Add(spring7);
            objectID++;

            // After Whale Chase
            SetObject jumppanel7 = Objects.jumppanel(new Vector3(-21400.8f, 539.166f, -10038.7f), new Quaternion(0f, -0.53315f, 0f, -0.846023f), 26f, 2300f, 1.8f, 707u, objectID);
            if (set.Objects[243].Parameters[1].Data.ToString() == "tails" || set.Objects[243].Parameters[1].Data.ToString() == "knuckles" || set.Objects[243].Parameters[1].Data.ToString() == "rouge" || set.Objects[243].Parameters[1].Data.ToString() == "omega") { jumppanel7.UnknownBytes[3] = 1; }
            set.Objects.Add(jumppanel7);
            objectID++;

            SetObject pointsample2 = Objects.pointsample(new Vector3(-17283.8f, 452.273f, -98491.9f), new Quaternion(0f, 0f, 0f, 1f), 0, objectID);
            set.Objects.Add(pointsample2);
            objectID++;

            SetObject jumppanel8 = Objects.jumppanel(new Vector3(-13907.4f, 450f, -98218.2f), new Quaternion(0f, 0.95076f, 0f, 0.309934f), 26f, 2300f, 1.8f, 29u, objectID);
            if (set.Objects[243].Parameters[1].Data.ToString() == "tails") { jumppanel7.UnknownBytes[3] = 1; }
            set.Objects.Add(jumppanel8);
            objectID++;

            SetObject spring8 = Objects.spring(new Vector3(-11962.7f, 459.769f, -105925f), new Quaternion(-0.0416072f, 0.902359f, 0.419544f, 0.0894892f), 1000, 2f, 710u, 0, objectID, false);
            if (set.Objects[243].Parameters[1].Data.ToString() == "tails" || set.Objects[243].Parameters[1].Data.ToString() == "knuckles" || set.Objects[243].Parameters[1].Data.ToString() == "shadow" || set.Objects[243].Parameters[1].Data.ToString() == "rouge" || set.Objects[243].Parameters[1].Data.ToString() == "silver" || set.Objects[243].Parameters[1].Data.ToString() == "blaze") { spring8.UnknownBytes[3] = 1; }
            set.Objects.Add(spring8);
            objectID++;

            SetObject pointsample3 = Objects.pointsample(new Vector3(-11492f, 450f, -108296f), new Quaternion(0f, 0f, 0f, 1f), 0, objectID);
            set.Objects.Add(pointsample3);
            objectID++;

            SetObject jumppanel9 = Objects.jumppanel(new Vector3(-11843.5f, 450f, -109361f), new Quaternion(0f, -0.886974f, 0f, 0.461821f), 26f, 2300f, 1.8f, 164u, objectID);
            if (set.Objects[243].Parameters[1].Data.ToString() == "tails" || set.Objects[243].Parameters[1].Data.ToString() == "knuckles" || set.Objects[243].Parameters[1].Data.ToString() == "shadow" || set.Objects[243].Parameters[1].Data.ToString() == "rouge" || set.Objects[243].Parameters[1].Data.ToString() == "silver" || set.Objects[243].Parameters[1].Data.ToString() == "blaze") { jumppanel9.UnknownBytes[3] = 1; }
            set.Objects.Add(jumppanel8);
            objectID++;

            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { set.Objects[33].UnknownBytes[3] = 1; }

            SetObject spring9 = Objects.spring(new Vector3(-26496.6f, 942.65f, -118787f), new Quaternion(-0.000000042167f, 0.965926f, 0.258819f, -0.000000157369f), 1000, 2f, 711u, 0, objectID, false);
            set.Objects.Add(spring9);
            objectID++;

            SetObject spring10 = Objects.spring(new Vector3(-26355.1f, 2046.61f, -121464f), new Quaternion(0f, -0.823214f, 0f, 0.567734f), 1000, 2f, 324u, 0, objectID, false);
            set.Objects.Add(spring10);
            objectID++;

        }
    }
}
