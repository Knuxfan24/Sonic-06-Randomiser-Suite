using System;
using System.IO;
using HedgeLib.Text;
using System.Collections.Generic;
using Sonic_06_Randomiser_Suite.Serialisers;

namespace Sonic_06_Randomiser_Suite
{
    class Strings
    {
        /// <summary>
        /// Randomises all loading screen text
        /// </summary>
        public static void RandomiseLoadingText(string[] editedLub, Random rng) {
            string[] msgActTitle          = Resources.ParseLineBreaks(Properties.Resources.msgActTitleEntries);
            string[] msgAudioroom         = Resources.ParseLineBreaks(Properties.Resources.msgAudioroomEntries);
            string[] msgBattle            = Resources.ParseLineBreaks(Properties.Resources.msgBattleEntries);
            string[] msgFileselect        = Resources.ParseLineBreaks(Properties.Resources.msgFileselectEntries);
            string[] msgGoldResults       = Resources.ParseLineBreaks(Properties.Resources.msgGoldResultsEntries);
            string[] msgHint              = Resources.ParseLineBreaks(Properties.Resources.msgHintEntries);
            string[] msgHintXenon         = Resources.ParseLineBreaks(Properties.Resources.msgHintXenonEntries);
            string[] msgMainmenu          = Resources.ParseLineBreaks(Properties.Resources.msgMainmenuEntries);
            string[] msgMultiplayer       = Resources.ParseLineBreaks(Properties.Resources.msgMultiplayerEntries);
            string[] msgOptions           = Resources.ParseLineBreaks(Properties.Resources.msgOptionsEntries);
            string[] msgShop              = Resources.ParseLineBreaks(Properties.Resources.msgShopEntries);
            string[] msgSystem            = Resources.ParseLineBreaks(Properties.Resources.msgSystemEntries);
            string[] msgTag               = Resources.ParseLineBreaks(Properties.Resources.msgTagEntries);
            string[] msgTheaterroom       = Resources.ParseLineBreaks(Properties.Resources.msgTheaterroomEntries);
            string[] msgTitle             = Resources.ParseLineBreaks(Properties.Resources.msgTitleEntires);
            string[] msgTownMissionShadow = Resources.ParseLineBreaks(Properties.Resources.msgTownMissionShadowEntries);
            string[] msgTownMissionSilver = Resources.ParseLineBreaks(Properties.Resources.msgTownMissionSilverEntries);
            string[] msgTownMissionSonic  = Resources.ParseLineBreaks(Properties.Resources.msgTownMissionSonicEntries);
            string[] msgTwnShadow         = Resources.ParseLineBreaks(Properties.Resources.msgTwnShadowEntries);
            string[] msgTwnSilver         = Resources.ParseLineBreaks(Properties.Resources.msgTwnSilverEntries);
            string[] msgTwnSonic          = Resources.ParseLineBreaks(Properties.Resources.msgTwnSonicEntries);
            string[] staff                = Resources.ParseLineBreaks(Properties.Resources.staffEntries);

            int lineNum = 0;
            int index;
            string missionText = "";

            foreach (string line in editedLub)
            {
                if (line.Contains("mission_string"))
                {
                    string[] tempLine = line.Split('"');
                    index = rng.Next(0, 22);

                    switch (index)
                    {
                        case 0:
                            index = rng.Next(msgActTitle.Length);
                            tempLine[1] = msgActTitle[index];
                            missionText = "text/msg_act_title.mst";
                            break;
                        case 1:
                            index = rng.Next(msgAudioroom.Length);
                            tempLine[1] = msgAudioroom[index];
                            missionText = "text/msg_audioroom.mst";
                            break;
                        case 2:
                            index = rng.Next(msgBattle.Length);
                            tempLine[1] = msgBattle[index];
                            missionText = "text/msg_battle.mst";
                            break;
                        case 3:
                            index = rng.Next(msgFileselect.Length);
                            tempLine[1] = msgFileselect[index];
                            missionText = "text/msg_fileselect.mst";
                            break;
                        case 4:
                            index = rng.Next(msgGoldResults.Length);
                            tempLine[1] = msgGoldResults[index];
                            missionText = "text/msg_gold_results.mst";
                            break;
                        case 5:
                            index = rng.Next(msgHint.Length);
                            tempLine[1] = msgHint[index];
                            missionText = "text/msg_hint.mst";
                            break;
                        case 6:
                            index = rng.Next(msgHintXenon.Length);
                            tempLine[1] = msgHintXenon[index];
                            missionText = "text/msg_hint_xenon.mst";
                            break;
                        case 7:
                            index = rng.Next(msgMainmenu.Length);
                            tempLine[1] = msgMainmenu[index];
                            missionText = "text/msg_mainmenu.mst";
                            break;
                        case 8:
                            index = rng.Next(msgMultiplayer.Length);
                            tempLine[1] = msgMultiplayer[index];
                            missionText = "text/msg_multiplayer.mst";
                            break;
                        case 9:
                            index = rng.Next(msgOptions.Length);
                            tempLine[1] = msgOptions[index];
                            missionText = "text/msg_options.mst";
                            break;
                        case 10:
                            index = rng.Next(msgShop.Length);
                            tempLine[1] = msgShop[index];
                            missionText = "text/msg_shop.mst";
                            break;
                        case 11:
                            index = rng.Next(msgSystem.Length);
                            tempLine[1] = msgSystem[index];
                            missionText = "text/msg_system.mst";
                            break;
                        case 12:
                            index = rng.Next(msgTag.Length);
                            tempLine[1] = msgTag[index];
                            missionText = "text/msg_tag.mst";
                            break;
                        case 13:
                            index = rng.Next(msgTheaterroom.Length);
                            tempLine[1] = msgTheaterroom[index];
                            missionText = "text/msg_theaterroom.mst";
                            break;
                        case 14:
                            index = rng.Next(msgTitle.Length);
                            tempLine[1] = msgTitle[index];
                            missionText = "text/msg_title.mst";
                            break;
                        case 15:
                            index = rng.Next(msgTownMissionShadow.Length);
                            tempLine[1] = msgTownMissionShadow[index];
                            missionText = "text/msg_town_mission_shadow.mst";
                            break;
                        case 16:
                            index = rng.Next(msgTownMissionSilver.Length);
                            tempLine[1] = msgTownMissionSilver[index];
                            missionText = "text/msg_town_mission_silver.mst";
                            break;
                        case 17:
                            index = rng.Next(msgTownMissionSonic.Length);
                            tempLine[1] = msgTownMissionSonic[index];
                            missionText = "text/msg_town_mission_sonic.mst";
                            break;
                        case 18:
                            index = rng.Next(msgTwnShadow.Length);
                            tempLine[1] = msgTwnShadow[index];
                            missionText = "text/msg_twn_shadow.mst";
                            break;
                        case 19:
                            index = rng.Next(msgTwnSilver.Length);
                            tempLine[1] = msgTwnSilver[index];
                            missionText = "text/msg_twn_silver.mst";
                            break;
                        case 20:
                            index = rng.Next(msgTwnSonic.Length);
                            tempLine[1] = msgTwnSonic[index];
                            missionText = "text/msg_twn_sonic.mst";
                            break;
                        case 21:
                            index = rng.Next(staff.Length);
                            tempLine[1] = staff[index];
                            missionText = "text/staff.mst";
                            break;
                    }
                    editedLub[lineNum] = string.Join("\"", tempLine);
                }

                if (line.Contains("mission_text"))
                {
                    string[] tempLine = line.Split('"');
                    tempLine[1] = missionText;
                    editedLub[lineNum] = string.Join("\"", tempLine);
                }
                lineNum++;
            }
        }
    
        /// <summary>
        /// Randomises all strings in an MST
        /// </summary>
        public static void RandomiseMSTContents(string folderPath, List<string> languages, Random rng) {
            List<string> availableText = new List<string>(),
                         usedMSTs = new List<string>();
            List<int> usedNumbers = new List<int>();
            int index;

            foreach (string lang in languages) {
                // Extract strings
                foreach (string mstData in Directory.GetFiles(folderPath, $"*{lang}.mst", SearchOption.AllDirectories)) {
                    usedMSTs.Add(mstData);
                    MST mst = new MST();
                    mst.Load(mstData);
                
                    foreach (MSTEntries getString in mst.entries)
                        availableText.Add(getString.Text);
                }
            }

            // Remake MSTs
            foreach (string mstData in usedMSTs) {
                Console.WriteLine($"Randomising Text: {mstData}");
                MST mst = new MST();
                mst.Load(mstData);

                foreach (MSTEntries getString in mst.entries) {
                    index = rng.Next(availableText.Count);
                    if (usedNumbers.Contains(index)) {
                        do { index = rng.Next(availableText.Count); }
                        while (usedNumbers.Contains(index));
                    }
                    usedNumbers.Add(index);
                    getString.Text = availableText[index];
                }

                mst.Save(mstData, true);
            }
        }
    }
}
