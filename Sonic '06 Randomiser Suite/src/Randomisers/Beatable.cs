using System;
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

            SetObject spring_twn1 = new SetObject()
            {
                ObjectType = "spring_twn",
                ObjectID = objectID,
                ObjectName = $"spring_twn{objectID}",
                DrawDistance = 0,
                UnknownBytes = new byte[] { 64, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 }
            };

            spring_twn1.Transform.Position.X = -4747.54f;
            spring_twn1.Transform.Position.Y = 81.1743f;
            spring_twn1.Transform.Position.Z = 1681.55f;

            spring_twn1.Transform.Rotation.X = -0.23457f;
            spring_twn1.Transform.Rotation.Y = -0.408218f;
            spring_twn1.Transform.Rotation.Z = -0.109382f;
            spring_twn1.Transform.Rotation.W = 0.875426f;

            spring_twn1.Parameters.Add(new SetObjectParam(typeof(float), 2000f));
            spring_twn1.Parameters.Add(new SetObjectParam(typeof(float), 1f));
            spring_twn1.Parameters.Add(new SetObjectParam(typeof(uint), 293u));
            spring_twn1.Parameters.Add(new SetObjectParam(typeof(int), 6000));

            set.Objects.Add(spring_twn1);
            objectID++;

            SetObject pointsample1 = new SetObject()
            {
                ObjectType = "pointsample",
                ObjectID = objectID,
                ObjectName = $"pointsample{objectID}",
                DrawDistance = 0,
                UnknownBytes = new byte[] { 64, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 }
            };

            pointsample1.Transform.Position.X = -9655.91f;
            pointsample1.Transform.Position.Y = 210.306f;
            pointsample1.Transform.Position.Z = 5491.07f;

            pointsample1.Parameters.Add(new SetObjectParam(typeof(int), 0));

            set.Objects.Add(pointsample1);
        }

        public static void set_wvoA_sonic(S06SetData set)
        {
            uint objectID = 0;
            foreach (SetObject existingObj in set.Objects)
            {
                if (existingObj.ObjectID != 0) { objectID = existingObj.ObjectID; }
            }

            SetObject jumppanel1 = new SetObject()
            {
                ObjectType = "jumppanel",
                ObjectID = objectID,
                ObjectName = $"jumppanel{objectID}",
                DrawDistance = 0,
                UnknownBytes = new byte[] { 64, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 }
            };

            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { jumppanel1.UnknownBytes[3] = 1; }

            jumppanel1.Transform.Position.X = 2009.92f;
            jumppanel1.Transform.Position.Y = 18.018f;
            jumppanel1.Transform.Position.Z = -6491.59f;

            jumppanel1.Transform.Rotation.X = 0f;
            jumppanel1.Transform.Rotation.Y = 0.91103f;
            jumppanel1.Transform.Rotation.Z = 0f;
            jumppanel1.Transform.Rotation.W = 0.412344f;

            jumppanel1.Parameters.Add(new SetObjectParam(typeof(float), 20f));
            jumppanel1.Parameters.Add(new SetObjectParam(typeof(float), 2000f));
            jumppanel1.Parameters.Add(new SetObjectParam(typeof(float), 0.5f));
            jumppanel1.Parameters.Add(new SetObjectParam(typeof(uint), 0u));

            set.Objects.Add(jumppanel1);
            objectID++;

            SetObject jumppanel2 = new SetObject()
            {
                ObjectType = "jumppanel",
                ObjectID = objectID,
                ObjectName = $"jumppanel{objectID}",
                DrawDistance = 0,
                UnknownBytes = new byte[] { 64, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 }
            };

            if (set.Objects[244].Parameters[1].Data.ToString() == "sonic_new") { jumppanel2.UnknownBytes[3] = 1; }

            jumppanel2.Transform.Position.X = 20772.4f;
            jumppanel2.Transform.Position.Y = 36.8728f;
            jumppanel2.Transform.Position.Z = -30022.3f;

            jumppanel2.Transform.Rotation.X = 0f;
            jumppanel2.Transform.Rotation.Y = 1f;
            jumppanel2.Transform.Rotation.Z = 0f;
            jumppanel2.Transform.Rotation.W = -0.000000162921f;

            jumppanel2.Parameters.Add(new SetObjectParam(typeof(float), 20f));
            jumppanel2.Parameters.Add(new SetObjectParam(typeof(float), 2000f));
            jumppanel2.Parameters.Add(new SetObjectParam(typeof(float), 0.5f));
            jumppanel2.Parameters.Add(new SetObjectParam(typeof(uint), 196u));

            set.Objects.Add(jumppanel2);
            objectID++;
        }
    }
}
