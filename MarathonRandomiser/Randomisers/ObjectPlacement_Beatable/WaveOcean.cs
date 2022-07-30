using Marathon.Formats.Placement;
using System.Numerics;

namespace MarathonRandomiser.Randomisers.ObjectPlacement_Beatable
{
    internal class WaveOcean
    {
        // Only affects the Tails section, as the end whale launch only works with Sonic to my knowledge, so he HAS to be the player at object 244.
        public static readonly List<string> set_wvoA_sonic_Characters = new()
        {
            "sonic_new", "tails", "knuckles", "shadow", "rouge", "omega", "silver", "blaze", "amy"
        };
        public static readonly List<int> set_wvoA_sonic_NoBoss = new()
        {
            87 // Spawns a Light Dash trail.
        };
        public static readonly Dictionary<int, string> set_wvoA_sonic_RequiredProps = new()
        {
            { 120, "wvo_bridgeA" },
            { 121, "wvo_bridgeA" },
            { 116, "wvo_bridgeA" },
            { 117, "wvo_bridgeA" },
            { 119, "wvo_bridgeA" },
            { 118, "wvo_bridgeB" },
            { 122, "wvo_bridgeA" },
            { 123, "wvo_bridgeA" },
            { 125, "wvo_bridgeA" },
            { 124, "wvo_bridgeA" },
            { 134, "wvo_bridgeA" },
            { 135, "wvo_bridgeA" },
            { 133, "wvo_bridgeA" },
            { 132, "wvo_bridgeA" },
            { 128, "wvo_bridgeB" },
            { 129, "wvo_bridgeA" },
            { 127, "wvo_bridgeA" },
            { 126, "wvo_bridgeA" },
            { 131, "wvo_bridgeA" },
            { 130, "wvo_bridgeA" },
            { 137, "wvo_bridgeA" },
            { 136, "wvo_bridgeA" },
            { 164, "wvo_bridgeA" },
            { 163, "wvo_bridgeA" },
            { 162, "wvo_bridgeA" },
            { 161, "wvo_bridgeA" },
            { 160, "wvo_bridgeA" },
            { 165, "wvo_bridgeA" },
            { 166, "wvo_bridgeA" },
            { 170, "wvo_bridgeA" },
            { 169, "wvo_bridgeA" },
            { 167, "wvo_bridgeA" },
            { 168, "wvo_bridgeA" }
        };

        /// <summary>
        /// Adds new objects to set_wvoA_sonic.set depending on the player character.
        /// </summary>
        public static async Task set_wvoA_sonic_NewObjects(ObjectPlacement set)
        {
            SetObject obj; // Initialise a dummy object.

            // Figure out what character has replaced Tails.
            string player = set.Data.Objects[243].Parameters[1].ToString();

            // Tails doesn't need new objects for this section, as it is normally his.
            // Knuckles also doesn't need new objects, as very careful usage of the Heat Knuckle and climbing/gliding up sloped surfaces can carry him through it.
            // Rouge doesn't need new objects because her glide is just so much better.
            // Omega is just lol.
            if (player is "tails" or "knuckles" or "rouge" or "omega")
                return;

            // Spring that connects the first two islands, with a point sample to target.
            if (player is "sonic_new" or "shadow" or "blaze" or "amy")
            {
                obj = Helpers.ObjectCreate("HackSpring01", "spring", false, new Vector3(-21270f, 524.664f, -100405f), new Quaternion(-0.146453f, 0.529137f, 0.0933011f, 0.830579f));
                obj.Parameters.Add(Helpers.ParameterCreate(3000f, ObjectDataType.Single));
                obj.Parameters.Add(Helpers.ParameterCreate(0.5f, ObjectDataType.Single));
                obj.Parameters.Add(Helpers.ParameterCreate(set.Data.Objects.Count + 1, ObjectDataType.UInt32));
                set.Data.Objects.Add(obj);

                obj = Helpers.ObjectCreate("HackSpring01Target", "pointsample", false, new Vector3(-17256.9f, 663.092f, -98923.2f), new Quaternion(0f, 0f, 0f, 1f));
                obj.Parameters.Add(Helpers.ParameterCreate(0, ObjectDataType.Int32));
                set.Data.Objects.Add(obj);
            }

            // Spring after the second island, targeting the Dash Ring.
            if (player is "sonic_new" or "shadow" or "silver" or "amy")
            {
                obj = Helpers.ObjectCreate("HackSpring02", "spring", false, new Vector3(-13894.7f, 450f, -98230.4f), new Quaternion(-0.0650847f, 0.93111f, 0.206422f, 0.293578f));
                obj.Parameters.Add(Helpers.ParameterCreate(3000f, ObjectDataType.Single));
                obj.Parameters.Add(Helpers.ParameterCreate(0.5f, ObjectDataType.Single));
                obj.Parameters.Add(Helpers.ParameterCreate(29, ObjectDataType.UInt32));
                set.Data.Objects.Add(obj);
            }

            // Spring for Amy to get her from the third island to the little isolated boardwalk then to the physics prop boardwalk.
            if (player is "amy")
            {
                obj = Helpers.ObjectCreate("HackSpring03", "spring", false, new Vector3(-12847.3f, 1321.05f, -106354f), new Quaternion(-0.0778285f, 0.92122f, 0.24684f, 0.29046f));
                obj.Parameters.Add(Helpers.ParameterCreate(3000f, ObjectDataType.Single));
                obj.Parameters.Add(Helpers.ParameterCreate(0.5f, ObjectDataType.Single));
                obj.Parameters.Add(Helpers.ParameterCreate(set.Data.Objects.Count + 1, ObjectDataType.UInt32));
                set.Data.Objects.Add(obj);

                obj = Helpers.ObjectCreate("HackSpring03Target", "pointsample", false, new Vector3(-11287.6f, 1537.28f, -108600f), new Quaternion(0f, 0f, 0f, 1f));
                obj.Parameters.Add(Helpers.ParameterCreate(0, ObjectDataType.Int32));
                set.Data.Objects.Add(obj);

                obj = Helpers.ObjectCreate("HackSpring04", "spring", false, new Vector3(-11883f, 451f, -109357f), new Quaternion(0.0801818f, 0.873535f, 0.154028f, -0.454733f));
                obj.Parameters.Add(Helpers.ParameterCreate(1500f, ObjectDataType.Single));
                obj.Parameters.Add(Helpers.ParameterCreate(1f, ObjectDataType.Single));
                obj.Parameters.Add(Helpers.ParameterCreate(164, ObjectDataType.UInt32));
                set.Data.Objects.Add(obj);
            }

            // Spring that connects the last two rocks.
            if (player is "sonic_new" or "shadow" or "amy")
            {
                obj = Helpers.ObjectCreate("HackSpring05", "spring", false, new Vector3(-26256f, 955.263f, -117802f), new Quaternion(-0.433013f, 0.433013f, 0.25f, 0.75f));
                obj.Parameters.Add(Helpers.ParameterCreate(1000f, ObjectDataType.Single));
                obj.Parameters.Add(Helpers.ParameterCreate(1f, ObjectDataType.Single));
                obj.Parameters.Add(Helpers.ParameterCreate(set.Data.Objects.Count + 1, ObjectDataType.UInt32));
                set.Data.Objects.Add(obj);

                obj = Helpers.ObjectCreate("HackSpring05Target", "pointsample", false, new Vector3(-24276.9f, 1810.72f, -116800f), new Quaternion(0f, 0f, 0f, 1f));
                obj.Parameters.Add(Helpers.ParameterCreate(0, ObjectDataType.Int32));
                set.Data.Objects.Add(obj);
            }
        }
    }
}
