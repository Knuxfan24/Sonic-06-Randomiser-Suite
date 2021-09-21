using System.Collections.Generic;
using System.Windows.Forms;

namespace Sonic_06_Randomiser_Suite
{
    class CheckListEnumerators
    {
        /// <summary>
        /// Saves a list of checked values to the configuration ini.
        /// </summary>
        /// <param name="iniKey">The key to write the resulting list to.</param>
        /// <param name="items">The CheckedListBox element to parse.</param>
        /// <returns></returns>
        public static string Config(string iniKey, CheckedListBox items)
        {
            // Set up the key.
            string typeList = $"{iniKey}=";

            // Loop through the CheckedListBox element and add the ids to the key.
            foreach (int value in items.CheckedIndices)
                typeList += $"{value},";

            // Remove the last comma.
            if (typeList.Contains(','))
                typeList = typeList.Remove(typeList.LastIndexOf(','));

            // Return the string to write to the configuration ini.
            return typeList;
        }

        /// <summary>
        /// Create the list of enemies selected by the user for use in the enemy randomiser.
        /// </summary>
        /// <param name="items">The CheckedListBox element to parse.</param>
        /// <returns></returns>
        public static List<string> SET_EnumerateEnemiesList(CheckedListBox items)
        {
            // Create the list.
            List<string> Enemies = new();

            // Loop through the enemy list and add the enemy names.
            foreach (int item in items.CheckedIndices)
            {
                switch (item)
                {
                    // Iblis and Mephiles Monsters.
                    case 0:  Enemies.Add("cBiter");         break;
                    case 1:  Enemies.Add("cGolem");         break;
                    case 2:  Enemies.Add("cTaker");         break;
                    case 3:  Enemies.Add("cCrawler");       break;
                    case 4:  Enemies.Add("cGazer");         break;
                    case 5:  Enemies.Add("cStalker");       break;
                    case 6:  Enemies.Add("cTitan");         break;
                    case 7:  Enemies.Add("cTricker");       break;

                    // Eggman Robots.
                    case 8:  Enemies.Add("eArmor");         break;
                    case 9:  Enemies.Add("eBluster");       break;
                    case 10: Enemies.Add("eBuster");        break;
                    case 11: Enemies.Add("eBuster(Fly)");   break;
                    case 12: Enemies.Add("eBomber");        break;
                    case 13: Enemies.Add("eCannon");        break;
                    case 14: Enemies.Add("eCannon(Fly)");   break;
                    case 15: Enemies.Add("eChaser");        break;
                    case 16: Enemies.Add("eCommander");     break;
                    case 17: Enemies.Add("eFlyer");         break;
                    case 18: Enemies.Add("eGuardian");      break;
                    case 19: Enemies.Add("eGunner");        break;
                    case 20: Enemies.Add("eGunner(Fly)");   break;
                    case 21: Enemies.Add("eHunter");        break;
                    case 22: Enemies.Add("eKeeper");        break;
                    case 23: Enemies.Add("eLancer");        break;
                    case 24: Enemies.Add("eLancer(Fly)");   break;
                    case 25: Enemies.Add("eLiner");         break;
                    case 26: Enemies.Add("eRounder");       break;
                    case 27: Enemies.Add("eSearcher");      break;
                    case 28: Enemies.Add("eStinger");       break;
                    case 29: Enemies.Add("eStinger(Fly)");  break;
                    case 30: Enemies.Add("eSweeper");       break;
                    case 31: Enemies.Add("eWalker");        break;

                    // Bosses.
                    case 32: Enemies.Add("eCerberus");      break;
                    case 33: Enemies.Add("eGenesis");       break;
                    case 34: Enemies.Add("eWyvern");        break;
                    case 35: Enemies.Add("firstIblis");     break;
                    case 36: Enemies.Add("secondIblis");    break;
                    case 37: Enemies.Add("thirdIblis");     break;
                    case 38: Enemies.Add("firstmefiress");  break;
                    case 39: Enemies.Add("secondmefiress"); break;
                    case 40: Enemies.Add("kyozoress");      break;
                    case 41: Enemies.Add("solaris01");      break;
                    case 42: Enemies.Add("solaris02");      break;
                }
            }

            // Return the list to be used by the enemy randomiser.
            return Enemies;
        }

        /// <summary>
        /// Create the list of characters selected by the user for use in the character randomiser.
        /// </summary>
        /// <param name="items">The CheckedListBox element to parse.</param>
        /// <returns></returns>
        public static List<string> SET_EnumerateCharactersList(CheckedListBox items)
        {
            // Create the list.
            List<string> Characters = new();

            // Loop through the character list and add the character names.
            foreach (int item in items.CheckedIndices)
            {
                switch (item)
                {
                    case 0:  Characters.Add("sonic_new");      break;
                    case 1:  Characters.Add("sonic_fast");     break;
                    case 2:  Characters.Add("princess");       break;
                    case 3:  Characters.Add("snow_board_wap"); break;
                    case 4:  Characters.Add("snow_board");     break;
                    case 5:  Characters.Add("shadow");         break;
                    case 6:  Characters.Add("silver");         break;
                    case 7:  Characters.Add("tails");          break;
                    case 8:  Characters.Add("knuckles");       break;
                    case 9:  Characters.Add("rouge");          break;
                    case 10: Characters.Add("omega");          break;
                    case 11: Characters.Add("blaze");          break;
                    case 12: Characters.Add("amy");            break;
                }
            }

            // Return the list to be used by the character randomiser.
            return Characters;
        }

        /// <summary>
        /// Create the list of item capsule types selected by the user for use in the item randomiser.
        /// </summary>
        /// <param name="items">The CheckedListBox element to parse.</param>
        /// <returns></returns>
        public static List<int> SET_EnumerateItemsList(CheckedListBox items)
        {
            // Create the list.
            List<int> Items = new();

            // Loop through the item capsule list and add their ids, incremented by 1 to match the values used in the game.
            foreach (int item in items.CheckedIndices)
                Items.Add(item + 1);

            // Return the list to be used by the item randomiser.
            return Items;
        }

        /// <summary>
        /// Create the list of door types selected by the user for use in the door randomiser.
        /// </summary>
        /// <param name="items">The CheckedListBox element to parse.</param>
        /// <returns></returns>
        public static List<string> SET_EnumerateDoorsList(CheckedListBox items)
        {
            // Create the list.
            List<string> Doors = new();

            // Loop through the door list and add the door object names.
            foreach (int item in items.CheckedIndices)
            {
                switch (item)
                {
                    case 0: Doors.Add("aqa_door");  break;
                    case 1: Doors.Add("dtd_door");  break;
                    case 2: Doors.Add("flc_door");  break;
                    case 3: Doors.Add("kdv_door");  break;
                    case 4: Doors.Add("rct_door");  break;
                    case 5: Doors.Add("wap_door");  break;
                    case 6: Doors.Add("wvo_doorA"); break;
                    case 7: Doors.Add("wvo_doorB"); break;
                }
            }

            // Return the list to be used by the door randomiser.
            return Doors;
        }

        /// <summary>
        /// Create the list of props selected by the user for use in the prop randomiser.
        /// This function is used for both Common and Path Props, as both lists are written using the internal names due to the sheer amount of objects.
        /// </summary>
        /// <param name="items">The CheckedListBox element to parse.</param>
        /// <returns></returns>
        public static List<string> SET_EnumeratePropsList(CheckedListBox items)
        {
            // Create the list.
            List<string> Props = new();

            // Loop through the prop list and add the prop name if it is checked.
            for (int i = 0; i < items.Items.Count; i++)
            {
                if (items.GetItemChecked(i))
                    Props.Add(items.Items[i].ToString());
            }

            // Return the list to be used by the prop randomiser.
            return Props;
        }

        /// <summary>
        /// Create the list of '06 hints selected by the user for use in the voice line randomiser.
        /// Custom lines provided by Voice Packs are added to this list afterwards rather than handled here.
        /// </summary>
        /// <param name="items">The CheckedListBox element to parse.</param>
        /// <returns></returns>
        public static List<string> SET_EnumerateHintsList(CheckedListBox items)
        {
            // Create the list.
            List<string> Hints = new();

            // Loop through the hint list and add the hint name if it is checked.
            for (int i = 0; i < items.Items.Count; i++)
            {
                if (items.GetItemChecked(i))
                    Hints.Add(items.Items[i].ToString());
            }

            // Return the list to be used by the voice line randomiser.
            return Hints;
        }

        /// <summary>
        /// Create the list of scene luas selected by the user for use in the event lighting shuffler.
        /// </summary>
        /// <param name="items">The CheckedListBox element to parse.</param>
        /// <returns></returns>
        public static List<string> Event_EnumerateLightingList(CheckedListBox items)
        {
            // Create the list.
            List<string> SceneLuas = new();

            // Loop through the scene lua list and add the file paths for each lua binary.
            foreach (int item in items.CheckedIndices)
            {
                switch (item)
                {
                    // Event Scene Luas
                    case 0:  SceneLuas.Add("scripts/stage/event/scene_e0031.lub"); break;
                    case 1:  SceneLuas.Add("scripts/stage/event/scene_e0001.lub"); break;
                    case 2:  SceneLuas.Add("scripts/stage/event/scene_e0002.lub"); break;
                    case 3:  SceneLuas.Add("scripts/stage/event/scene_e0003.lub"); break;
                    case 4:  SceneLuas.Add("scripts/stage/event/scene_e0004.lub"); break;
                    case 5:  SceneLuas.Add("scripts/stage/event/scene_e0006.lub"); break;
                    case 6:  SceneLuas.Add("scripts/stage/event/scene_e0007.lub"); break;
                    case 7:  SceneLuas.Add("scripts/stage/event/scene_e0009.lub"); break;
                    case 8:  SceneLuas.Add("scripts/stage/event/scene_e0010.lub"); break;
                    case 9:  SceneLuas.Add("scripts/stage/event/scene_e0011.lub"); break;
                    case 10: SceneLuas.Add("scripts/stage/event/scene_e0012.lub"); break;
                    case 11: SceneLuas.Add("scripts/stage/event/scene_e0013.lub"); break;
                    case 12: SceneLuas.Add("scripts/stage/event/scene_e0014.lub"); break;
                    case 13: SceneLuas.Add("scripts/stage/event/scene_e0016.lub"); break;
                    case 14: SceneLuas.Add("scripts/stage/event/scene_e0017.lub"); break;
                    case 15: SceneLuas.Add("scripts/stage/event/scene_e0018.lub"); break;
                    case 16: SceneLuas.Add("scripts/stage/event/scene_e0019.lub"); break;
                    case 17: SceneLuas.Add("scripts/stage/event/scene_e0021.lub"); break;
                    case 18: SceneLuas.Add("scripts/stage/event/scene_e0022.lub"); break;
                    case 19: SceneLuas.Add("scripts/stage/event/scene_e0023.lub"); break;
                    case 20: SceneLuas.Add("scripts/stage/event/scene_e0026.lub"); break;
                    case 21: SceneLuas.Add("scripts/stage/event/scene_e0027.lub"); break;
                    case 22: SceneLuas.Add("scripts/stage/event/scene_e0028.lub"); break;
                    case 23: SceneLuas.Add("scripts/stage/event/scene_e0102.lub"); break;
                    case 24: SceneLuas.Add("scripts/stage/event/scene_e0103.lub"); break;
                    case 25: SceneLuas.Add("scripts/stage/event/scene_e0104.lub"); break;
                    case 26: SceneLuas.Add("scripts/stage/event/scene_e0105.lub"); break;
                    case 27: SceneLuas.Add("scripts/stage/event/scene_e0106.lub"); break;
                    case 28: SceneLuas.Add("scripts/stage/event/scene_e0109.lub"); break;
                    case 29: SceneLuas.Add("scripts/stage/event/scene_e0113.lub"); break;
                    case 30: SceneLuas.Add("scripts/stage/event/scene_e0114.lub"); break;
                    case 31: SceneLuas.Add("scripts/stage/event/scene_e0115.lub"); break;
                    case 32: SceneLuas.Add("scripts/stage/event/scene_e0118.lub"); break;
                    case 33: SceneLuas.Add("scripts/stage/event/scene_e0119.lub"); break;
                    case 34: SceneLuas.Add("scripts/stage/event/scene_e0120.lub"); break;
                    case 35: SceneLuas.Add("scripts/stage/event/scene_e0125.lub"); break;
                    case 36: SceneLuas.Add("scripts/stage/event/scene_e0126.lub"); break;
                    case 37: SceneLuas.Add("scripts/stage/event/scene_e0127.lub"); break;
                    case 38: SceneLuas.Add("scripts/stage/event/scene_e0128.lub"); break;
                    case 39: SceneLuas.Add("scripts/stage/event/scene_e0129.lub"); break;
                    case 40: SceneLuas.Add("scripts/stage/event/scene_e0201.lub"); break;
                    case 41: SceneLuas.Add("scripts/stage/event/scene_e0204.lub"); break;
                    case 42: SceneLuas.Add("scripts/stage/event/scene_e0205.lub"); break;
                    case 43: SceneLuas.Add("scripts/stage/event/scene_e0210.lub"); break;
                    case 44: SceneLuas.Add("scripts/stage/event/scene_e0211.lub"); break;
                    case 45: SceneLuas.Add("scripts/stage/event/scene_e0214.lub"); break;
                    case 46: SceneLuas.Add("scripts/stage/event/scene_e0215.lub"); break;
                    case 47: SceneLuas.Add("scripts/stage/event/scene_e0216.lub"); break;
                    case 48: SceneLuas.Add("scripts/stage/event/scene_e0217.lub"); break;
                    case 49: SceneLuas.Add("scripts/stage/event/scene_e0221.lub"); break;
                    case 50: SceneLuas.Add("scripts/stage/event/scene_e0223.lub"); break;
                    case 51: SceneLuas.Add("scripts/stage/event/scene_e0226.lub"); break;
                    case 52: SceneLuas.Add("scripts/stage/event/scene_e0227.lub"); break;
                    case 53: SceneLuas.Add("scripts/stage/event/scene_e0300.lub"); break;
                    case 54: SceneLuas.Add("scripts/stage/event/scene_e0301.lub"); break;
                    case 55: SceneLuas.Add("scripts/stage/event/scene_e0304.lub"); break;
                    case 57: SceneLuas.Add("scripts/stage/event/scene_event.lub"); break;

                    // Other Scene Luas.
                    // Aquatic Base
                    case 58: SceneLuas.Add("scripts/stage/aqa/scene_aqa_a.lub");          break;
                    case 59: SceneLuas.Add("scripts/stage/aqa/scene_aqa_b.lub");          break;
                    case 60: SceneLuas.Add("scripts/stage/other/scene_test_aqa.lub");     break;
                    case 61: SceneLuas.Add("scripts/stage/boss/scene_eWyvern_sonic.lub"); break;

                    // Crisis City.
                    case 62: SceneLuas.Add("scripts/stage/csc/scene_csc_a.lub");       break;
                    case 63: SceneLuas.Add("scripts/stage/csc/scene_csc_b.lub");       break;
                    case 64: SceneLuas.Add("scripts/stage/csc/scene_csc_c.lub");       break;
                    case 65: SceneLuas.Add("scripts/stage/csc/scene_csc_e.lub");       break;
                    case 66: SceneLuas.Add("scripts/stage/csc/scene_csc_f.lub");       break;
                    case 67: SceneLuas.Add("scripts/stage/end/scene_end_a.lub");       break;
                    case 68: SceneLuas.Add("scripts/stage/boss/scene_firstiblis.lub"); break;

                    // Dusty Desrt.
                    case 69: SceneLuas.Add("scripts/stage/boss/scene_eCerberus_sonic.lub"); break;
                    case 70: SceneLuas.Add("scripts/stage/dtd/scene_dtd_a_sonic.lub");      break;
                    case 71: SceneLuas.Add("scripts/stage/end/scene_end_d.lub");            break;
                    case 72: SceneLuas.Add("scripts/stage/dtd/scene_dtd_b_sonic.lub");      break;
                    case 73: SceneLuas.Add("scripts/stage/boss/scene_secondmefiress.lub");  break;
                    case 74: SceneLuas.Add("scripts/stage/dtd/scene_dtd_c_shadow.lub");     break;

                    // End of the World Standalone
                    case 75: SceneLuas.Add("scripts/stage/boss/scene_solaris_super3.lub"); break;

                    // Flame Core.
                    case 76: SceneLuas.Add("scripts/stage/flc/scene_flc_a.lub");          break;
                    case 77: SceneLuas.Add("scripts/stage/end/scene_end_b.lub");          break;
                    case 78: SceneLuas.Add("scripts/stage/boss/scene_thirdiblis.lub");    break;
                    case 79: SceneLuas.Add("scripts/stage/flc/scene_flc_b.lub.lub");      break;
                    case 80: SceneLuas.Add("scripts/stage/boss/scene_secondiblis.lub");   break;
                    case 81: SceneLuas.Add("scripts/stage/boss/scene_firstmefiress.lub"); break;

                    // Gold Medal
                    case 82: SceneLuas.Add("scripts/stage/other/scene_getgoldmedal.lub"); break;

                    // Kingdom Valley
                    case 83: SceneLuas.Add("scripts/stage/kdv/scene_kdv_a.lub"); break;
                    case 84: SceneLuas.Add("scripts/stage/end/scene_end_g.lub"); break;
                    case 85: SceneLuas.Add("scripts/stage/kdv/scene_kdv_b.lub"); break;
                    case 86: SceneLuas.Add("scripts/stage/kdv/scene_kdv_c.lub"); break;
                    case 87: SceneLuas.Add("scripts/stage/kdv/scene_kdv_d.lub"); break;

                    // Radical Train
                    case 88: SceneLuas.Add("scripts/stage/rct/scene_rct_a.lub");             break;
                    case 89: SceneLuas.Add("scripts/stage/rct/scene_rct_b.lub");             break;
                    case 90: SceneLuas.Add("scripts/stage/boss/scene_shadow_vs_silver.lub"); break;

                    // Soleanna
                    case 91: SceneLuas.Add("scripts/stage/twn/scene_twn_a.lub");           break;
                    case 92: SceneLuas.Add("scripts/stage/twn/scene_twn_b.lub");           break;
                    case 93: SceneLuas.Add("scripts/stage/twn/scene_twn_c.lub");           break;
                    case 94: SceneLuas.Add("scripts/stage/boss/scene_eGenesis_sonic.lub"); break;
                    case 95: SceneLuas.Add("scripts/stage/twn/scene_twn_d.lub");           break;

                    // Tropical Jungle
                    case 96: SceneLuas.Add("scripts/stage/tpj/scene_tpj_a.lub"); break;
                    case 97: SceneLuas.Add("scripts/stage/tpj/scene_tpj_b.lub"); break;
                    case 98: SceneLuas.Add("scripts/stage/tpj/scene_tpj_c.lub"); break;
                    case 99: SceneLuas.Add("scripts/stage/end/scene_end_c.lub"); break;

                    // Wave Ocean
                    case 100: SceneLuas.Add("scripts/stage/wvo/scene_wvo_a.lub"); break;
                    case 101: SceneLuas.Add("scripts/stage/end/scene_end_e.lub"); break;
                    case 102: SceneLuas.Add("scripts/stage/wvo/scene_wvo_b.lub"); break;

                    // White Acropolis
                    case 103: SceneLuas.Add("scripts/stage/wap/scene_wap_a.lub");             break;
                    case 104: SceneLuas.Add("scripts/stage/wap/scene_wap_b.lub");             break;
                    case 105: SceneLuas.Add("scripts/stage/end/scene_end_f.lub");             break;
                    case 106: SceneLuas.Add("scripts/stage/boss/scene_eGenesis_silver.lub");  break;
                    case 107: SceneLuas.Add("scripts/stage/boss/scene_eCerberus_shadow.lub"); break;
                }
            }

            // Return the list to be used by the event lighting shuffler.
            return SceneLuas;
        }

        /// <summary>
        /// Create the list of terrain maps selected by the user for use in the terrain randomiser.
        /// </summary>
        /// <param name="items">The CheckedListBox element to parse.</param>
        /// <returns></returns>
        public static List<string> Event_EnumerateTerrainList(CheckedListBox items)
        {
            // Create the list.
            List<string> EventTerrain = new();

            // Loop through the terrain list and add the folder paths.
            foreach (int item in items.CheckedIndices)
            {
                switch (item)
                {
                    // Aquatic Base
                    case 0: EventTerrain.Add("stage/aqa/a");        break;
                    case 1: EventTerrain.Add("stage/aqa/b");        break;
                    case 2: EventTerrain.Add("stage/event/ev0028"); break;
                    case 3: EventTerrain.Add("stage/event/ev0221"); break;

                    // Crisis City
                    case 4:  EventTerrain.Add("stage/csc/a");        break;
                    case 5:  EventTerrain.Add("stage/csc/b");        break;
                    case 6:  EventTerrain.Add("stage/csc/c");        break;
                    case 7:  EventTerrain.Add("stage/csc/e");        break;
                    case 8:  EventTerrain.Add("stage/csc/f");        break;
                    case 9:  EventTerrain.Add("stage/csc/iblis01");  break;
                    case 10: EventTerrain.Add("stage/event/ev0106"); break;
                    case 11: EventTerrain.Add("stage/event/ev0012"); break;
                    case 12: EventTerrain.Add("stage/event/ev0105"); break;

                    // Dusty Desert
                    case 13: EventTerrain.Add("stage/event/ev0003");            break;
                    case 14: EventTerrain.Add("stage/boss/dr1_dtd");            break;
                    case 15: EventTerrain.Add("stage/dtd/a");                   break;
                    case 16: EventTerrain.Add("stage/dtd/b");                   break;
                    case 17: EventTerrain.Add("stage/boss/secondmefiress_dtd"); break;

                    // Egg Carrier
                    case 18: EventTerrain.Add("stage/boss/dr3_eggcarrier");        break;
                    case 19: EventTerrain.Add("stage/boss/dr3_eggcarrier/action"); break;
                    case 20: EventTerrain.Add("stage/event/ev0022");               break;

                    // End of the World Standalone
                    case 21: EventTerrain.Add("stage/boss/solaris_last"); break;

                    // Flame Core
                    case 22: EventTerrain.Add("stage/flc/a");                  break;
                    case 23: EventTerrain.Add("stage/boss/thirdiblis_flc");    break;
                    case 24: EventTerrain.Add("stage/flc/b");                  break;
                    case 25: EventTerrain.Add("stage/boss/secondiblis_flc");   break;
                    case 26: EventTerrain.Add("stage/boss/firstmefiress_flc"); break;

                    // Kingdom Valley
                    case 27: EventTerrain.Add("stage/event/ev0023"); break;
                    case 28: EventTerrain.Add("stage/kdv/a");        break;
                    case 29: EventTerrain.Add("stage/kdv/b");        break;
                    case 30: EventTerrain.Add("stage/kdv/c");        break;
                    case 31: EventTerrain.Add("stage/kdv/d");        break;
                    case 32: EventTerrain.Add("stage/event/ev0104"); break;
                    case 33: EventTerrain.Add("stage/event/ev0026"); break;
                    case 34: EventTerrain.Add("stage/event/ev0125"); break;

                    // Radical Train
                    case 35: EventTerrain.Add("stage/rct/a");               break;
                    case 36: EventTerrain.Add("stage/rct/b");               break;
                    case 37: EventTerrain.Add("stage/event/ev0120");        break;
                    case 38: EventTerrain.Add("stage/boss/charaboss2_rct"); break;

                    // Soleanna
                    case 39: EventTerrain.Add("stage/twn/a");           break;
                    case 40: EventTerrain.Add("stage/event/ev0031");    break;
                    case 41: EventTerrain.Add("stage/event/ev0206");    break;
                    case 42: EventTerrain.Add("stage/event/ev0304");    break;
                    case 43: EventTerrain.Add("stage/event/ev0216");    break;
                    case 44: EventTerrain.Add("stage/event/ev0021");    break;
                    case 45: EventTerrain.Add("stage/twn/b");           break;
                    case 46: EventTerrain.Add("stage/twn/c");           break;
                    case 47: EventTerrain.Add("stage/boss/dr2_forest"); break;
                    case 48: EventTerrain.Add("stage/twn/circuit");     break;

                    // Tropical Jungle
                    case 49: EventTerrain.Add("stage/tpj/a"); break;
                    case 50: EventTerrain.Add("stage/tpj/b"); break;
                    case 51: EventTerrain.Add("stage/tpj/c"); break;

                    // Wave Ocean
                    case 52: EventTerrain.Add("stage/wvo/a"); break;
                    case 53: EventTerrain.Add("stage/wvo/b"); break;

                    // White Acropolis
                    case 54: EventTerrain.Add("stage/wap/a");        break;
                    case 55: EventTerrain.Add("stage/wap/b");        break;
                    case 56: EventTerrain.Add("stage/boss/dr1_wap"); break;
                    case 57: EventTerrain.Add("stage/event/ev0009"); break;
                    case 58: EventTerrain.Add("stage/event/ev0010"); break;
                    case 59: EventTerrain.Add("stage/event/ev0214"); break;
                }
            }

            // Return the list to be used by the terrain randomiser.
            return EventTerrain;
        }

        /// <summary>
        /// Create the list of environment map selected by the user for use in the environment map randomiser.
        /// </summary>
        /// <param name="items">The CheckedListBox element to parse.</param>
        /// <returns></returns>
        public static List<string> Scene_EnumerateEnvList(CheckedListBox items)
        {
            // Create the list.
            List<string> EnvMaps = new();

            // Loop through the environment map list and add the file paths for each environment map.
            foreach (int item in items.CheckedIndices)
            {
                switch (item)
                {
                    case 0:  EnvMaps.Add("stage/aqa/a/aqa_envmap.dds");                  break;
                    case 1:  EnvMaps.Add("stage/csc/a/csc_envmap.dds");                  break;
                    case 2:  EnvMaps.Add("stage/csc/b/csc_envmap.dds");                  break;
                    case 3:  EnvMaps.Add("stage/end/a/endA_envmap.dds");                 break;
                    case 4:  EnvMaps.Add("stage/dtd/a/suna_cube.dds");                   break;
                    case 5:  EnvMaps.Add("stage/dtd/b/suna_cube.dds");                   break;
                    case 6:  EnvMaps.Add("stage/boss/secondmefiress_dtd/suna_cube.dds"); break;
                    case 7:  EnvMaps.Add("stage/end/d/endD_envmap.dds");                 break;
                    case 8:  EnvMaps.Add("stage/boss/solaris_last/solaris_envmap.dds");  break;
                    case 9:  EnvMaps.Add("stage/flc/a/flcA_envmap.dds");                 break;
                    case 10: EnvMaps.Add("stage/flc/b/flcB_envmap.dds");                 break;
                    case 11: EnvMaps.Add("stage/end/b/endB_envmap.dds");                 break;
                    case 12: EnvMaps.Add("object/twn/goldmdl/item_envmap.dds");          break;
                    case 13: EnvMaps.Add("stage/kdv/a/kdv_envmap.dds");                  break;
                    case 14: EnvMaps.Add("stage/kdv/b/kdv_envmap.dds");                  break;
                    case 15: EnvMaps.Add("stage/kdv/d/kdv_envmap.dds");                  break;
                    case 16: EnvMaps.Add("stage/kdv/c/kdv_envmap.dds");                  break;
                    case 17: EnvMaps.Add("stage/end/g/endG_envmap.dds");                 break;
                    case 18: EnvMaps.Add("stage/boss/charaboss2_rct/rct_envmap.dds");    break;
                    case 19: EnvMaps.Add("stage/twn/a/twn_a_cubemap_df_e.dds");          break;
                    case 20: EnvMaps.Add("stage/event/ev0031/ev0031_cubemap.dds");       break;
                    case 21: EnvMaps.Add("stage/event/ev0304/e0304_envmap.dds");         break;
                    case 22: EnvMaps.Add("stage/twn/b/twn_n_cubemap_df_e.dds");          break;
                    case 23: EnvMaps.Add("stage/twn/c/twn_c_cubemap_df_e.dds");          break;
                    case 24: EnvMaps.Add("stage/twn/d/twn_r_cubemap_df_e.dds");          break;
                    case 25: EnvMaps.Add("stage/tpj/a/tpj_envmap.dds");                  break;
                    case 26: EnvMaps.Add("stage/tpj/c/tpjC_envmap.dds");                 break;
                    case 27: EnvMaps.Add("stage/end/c/endC_envmap.dds");                 break;
                    case 28: EnvMaps.Add("stage/wvo/a/wvo_envmap.dds");                  break;
                    case 29: EnvMaps.Add("stage/end/e/endE_envmap.dds");                 break;
                    case 30: EnvMaps.Add("stage/wap/a/wap_envmap.dds");                  break;
                    case 31: EnvMaps.Add("stage/end/f/endF_envmap.dds");                 break;
                }
            }

            // Return the list to be used by the environment map randomiser.
            return EnvMaps;
        }

        /// <summary>
        /// Create the list of '06 songs selected by the user for use in the music randomiser.
        /// Custom songs are added to this list afterwards rather than handled here.
        /// </summary>
        /// <param name="items">The CheckedListBox element to parse.</param>
        /// <returns></returns>
        public static List<string> Misc_EnumerateMusicList(CheckedListBox items)
        {
            // Create the list.
            List<string> Music = new();

            // Loop through the song list and add the song names.
            foreach (int item in items.CheckedIndices)
            {
                switch (item)
                {
                    // Stages
                    case 0:  Music.Add("stg_wvo_a");           break;
                    case 1:  Music.Add("stg_wvo_b");           break;
                    case 2:  Music.Add("stg_dtd_a");           break;
                    case 3:  Music.Add("stg_dtd_b");           break;
                    case 4:  Music.Add("stg_wap_a");           break;
                    case 5:  Music.Add("stg_wap_b");           break;
                    case 6:  Music.Add("stg_csc_a");           break;
                    case 7:  Music.Add("stg_csc_b");           break;
                    case 8:  Music.Add("stage_csc_e");         break;
                    case 9:  Music.Add("stg_csc_f");           break;
                    case 10: Music.Add("stg_flc_a");           break;
                    case 11: Music.Add("stg_flc_b");           break;
                    case 12: Music.Add("stg_rct_a");           break;
                    case 13: Music.Add("stg_rct_b");           break;
                    case 14: Music.Add("stg_tpj_a");           break;
                    case 15: Music.Add("stg_tpj_b");           break;
                    case 16: Music.Add("stg_tpj_c");           break;
                    case 17: Music.Add("stg_kdv_a");           break;
                    case 18: Music.Add("stg_kdv_b");           break;
                    case 19: Music.Add("stg_kdv_c");           break;
                    case 20: Music.Add("stg_kdv_d");           break;
                    case 21: Music.Add("stg_aqa_a");           break;
                    case 22: Music.Add("stg_aqa_b");           break;
                    case 23: Music.Add("stg_end_a");           break;
                    case 24: Music.Add("stg_end_b");           break;
                    case 25: Music.Add("stg_end_c");           break;
                    case 26: Music.Add("stg_end_d");           break;
                    case 27: Music.Add("stg_end_e");           break;
                    case 28: Music.Add("stg_end_f");           break;
                    case 29: Music.Add("stg_end_g");           break;

                    // Bosses
                    case 30: Music.Add("boss_iblis01");        break;
                    case 31: Music.Add("boss_iblis03");        break;
                    case 32: Music.Add("boss_mefiless01");     break;
                    case 33: Music.Add("boss_mefiless02");     break;
                    case 34: Music.Add("boss_character");      break;
                    case 35: Music.Add("boss_cerberus");       break;
                    case 36: Music.Add("boss_wyvern");         break;
                    case 37: Music.Add("boss_solaris1");       break;
                    case 38: Music.Add("boss_solaris2");       break;

                    // Town
                    case 39: Music.Add("stg_twn_a");           break;
                    case 40: Music.Add("stg_twn_b");           break;
                    case 41: Music.Add("stg_twn_c");           break;
                    case 42: Music.Add("stg_twn_shop");        break;
                    case 43: Music.Add("twn_mission_slow");    break;
                    case 44: Music.Add("twn_mission_comical"); break;
                    case 45: Music.Add("twn_mission_fast");    break;
                    case 46: Music.Add("twn_accordion");       break;
                    
                    // Miscellaneous
                    case 47: Music.Add("result");              break;
                    case 48: Music.Add("mainmenu");            break;
                    case 49: Music.Add("select");              break;
                    case 50: Music.Add("extra");               break;
                }
            }

            // Return the list to be used by the music randomiser.
            return Music;
        }

        /// <summary>
        /// Create the list of languages selected by the user for use in the text shuffler.
        /// </summary>
        /// <param name="items">The CheckedListBox element to parse.</param>
        /// <returns></returns>
        public static List<string> Misc_EnumerateLanguagesList(CheckedListBox items)
        {
            // Create the list.
            List<string> Languages = new();

            // Loop through the language list and add the language characters.
            foreach (int item in items.CheckedIndices)
            {
                switch (item)
                {
                    case 0: Languages.Add("e"); break;
                    case 1: Languages.Add("f"); break;
                    case 2: Languages.Add("g"); break;
                    case 3: Languages.Add("i"); break;
                    case 4: Languages.Add("j"); break;
                    case 5: Languages.Add("s"); break;
                }
            }

            // Return the list to be used by the text shuffler.
            return Languages;
        }
    }
}
