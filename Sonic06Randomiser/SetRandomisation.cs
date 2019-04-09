using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace Sonic06Randomiser
{
    class SetRandomisation
    {
        static public void SetupRandomiser(bool randomEnemies, List<string> validEnemies, bool randomCharacters, List<string> validCharacters, bool randomItems, List<string> validItems, bool randomVoices, bool spoilerLog, bool keepXML, bool randomiseFolder, int outputFolderType, string filepath, string output, string rndSeed)
        {
            string xmlName = ""; //Set up xmlName because variables suck at being defined in if statements
            Random rnd = new Random();
            string logSeed = rndSeed;
            if (outputFolderType == 0 && output == "") { outputFolderType = 1; } //Default to saving in the program directory if the user doesn't specify a path with Output Folder Type set to custom.


            if (!randomiseFolder)
            {
                #region Load the XML if we're only randomising one
                if (filepath.Contains(".set"))
                {
                    SetClass.Extract(randomiseFolder, outputFolderType, output, filepath);
                    filepath = (filepath.Remove(filepath.Length - 4) + ".xml");
                }
                string file = filepath;
                xmlName = filepath.Remove(0, Path.GetDirectoryName(filepath).Length);
                xmlName = xmlName.Remove(xmlName.Length - 4);
                xmlName = xmlName.Replace("\\", "");
                
                XElement doc = XElement.Load(file);
                #endregion

                #region Set up the Random Number Generation
                if (rndSeed != "")
                {
                    rndSeed = rndSeed + xmlName;
                    rnd = new Random(rndSeed.GetHashCode()); //Override RND with seed if the box wasn't blank
                }
                #endregion

                StreamWriter spoiler;
                #region Bodge Spoiler Log
                switch (outputFolderType)
                {
                    case 0: //Custom
                        spoiler = new StreamWriter(output + "\\" + xmlName + "_log.txt");
                        break;
                    case 1: //Source
                        spoiler = new StreamWriter(filepath.Remove(filepath.Length - 4) + "_log.txt");
                        break;
                    case 2: //Program
                        spoiler = new StreamWriter(xmlName + "_log.txt");
                        break;
                    default: //Need this otherwise spoiler becomes invalid for some god forsaken reason
                        spoiler = new StreamWriter(filepath.Remove(filepath.Length - 4) + "_log.txt");
                        break;
                }

                spoiler.WriteLine("Seed: " + logSeed);
                spoiler.WriteLine("Randomise Enemies: " + randomEnemies);
                if (randomEnemies)
                {
                    spoiler.WriteLine("Valid Enemies:");
                    validEnemies.ForEach(i => spoiler.Write("{0}, ", i));
                    spoiler.Write(Environment.NewLine);
                }
                spoiler.WriteLine("Randomise Character Spawns: " + randomCharacters);
                if (randomCharacters)
                {
                    spoiler.WriteLine("Valid Characters:");
                    validCharacters.ForEach(i => spoiler.Write("{0}, ", i));
                    spoiler.Write(Environment.NewLine);
                }
                spoiler.WriteLine("Randomise Item Capsules: " + randomItems);
                if (randomItems)
                {
                    spoiler.WriteLine("Valid Item Capsules:");
                    validItems.ForEach(i => spoiler.Write("{0}, ", i));
                    spoiler.Write(Environment.NewLine);
                }
                spoiler.WriteLine("Randomise Voice Line Triggers: " + randomVoices);
                spoiler.Write(Environment.NewLine);
                #endregion

                //Do the randomisation functions here
                if (randomEnemies) { EnemyRandomiser(doc, rnd, spoiler, validEnemies); }
                if (randomCharacters) { CharacterRandomiser(doc, rnd, spoiler, validCharacters); }
                if (randomItems) { ItemRandomiser(doc, rnd, spoiler, validItems); }
                if (randomVoices) { VoiceRandomiser(doc, rnd, spoiler); }
                spoiler.Close();
                string xml = "Not Used";
                SetSave(randomiseFolder, outputFolderType, doc, output, xmlName, filepath, keepXML, spoilerLog, xml);
            }
            else
            {
                #region Load the Folder of XMLs
                SetClass.Extract(randomiseFolder, outputFolderType, output, filepath);
                string[] xmls = Directory.GetFiles(filepath, "*.xml", SearchOption.AllDirectories);
                Console.WriteLine("Found " + xmls.Length + " xml files");
                foreach (string xml in xmls)
                {
                    Console.WriteLine(xml);
                    xmlName = xml.Remove(0, Path.GetDirectoryName(xml).Length);
                    xmlName = xmlName.Remove(xmlName.Length - 4);
                    xmlName = xmlName.Replace("\\", "");
                    XElement doc = XElement.Load(xml);
                #endregion

                    #region Set up the Random Number Generation
                    if (rndSeed != "")
                    {
                        rndSeed = rndSeed + xmlName;
                        rnd = new Random(rndSeed.GetHashCode()); //Override RND with seed if the box wasn't blank
                    }
                    #endregion

                    StreamWriter spoiler;
                    #region Bodge Spoiler Log
                    switch (outputFolderType)
                    {
                        case 0: //Custom
                            spoiler = new StreamWriter(output + "\\" + xmlName + "_log.txt");
                            break;
                        case 1: //Source
                            spoiler = new StreamWriter(filepath + "\\" + xmlName + "_log.txt");
                            break;
                        case 2: //Program
                            spoiler = new StreamWriter(xmlName + "_log.txt");
                            break;
                        default: //Need this otherwise spoiler becomes invalid for some god forsaken reason
                            spoiler = new StreamWriter(filepath.Remove(filepath.Length - 4) + "_log.txt");
                            break;
                    }

                    spoiler.WriteLine("Seed: " + logSeed);
                    spoiler.WriteLine("Randomise Enemies: " + randomEnemies);
                    spoiler.WriteLine("Randomise Character Spawns: " + randomCharacters);
                    spoiler.WriteLine("Randomise Item Capsules: " + randomItems);
                    spoiler.WriteLine("Randomise Voice Line Triggers: " + randomVoices);
                    spoiler.Write(Environment.NewLine);
                    #endregion

                    //Do the randomisation functions here
                    if (randomEnemies) { EnemyRandomiser(doc, rnd, spoiler, validEnemies); }
                    if (randomCharacters) { CharacterRandomiser(doc, rnd, spoiler, validCharacters); }
                    if (randomItems) { ItemRandomiser(doc, rnd, spoiler, validItems); }
                    if (randomVoices) { VoiceRandomiser(doc, rnd, spoiler); }
                    spoiler.Close();
                    SetSave(randomiseFolder, outputFolderType, doc, output, xmlName, filepath, keepXML, spoilerLog, xml);
                }
            }
        }

        static public void EnemyRandomiser(XElement doc, Random rnd, StreamWriter spoiler, List<String> validEnemies)
        {
            //Set up the Variables relating to enemy randomisation
            string lastEnemyName = "";
            int index = 0; //Int to use as a host for the random number generation

            String rawBlacklist = Properties.Resources.s06EnemyBlacklist;
            string[] blacklist = rawBlacklist.Split
            (
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            #region Enemy Paramters
            var cBiterParams = new List<string> { "cBiter_Normal", "cBiter_Fix", "cBiter_Freeze", "cBiter_Wall_Fix", "cBiter_Wall_Normal" }; //Valid Parameters for the cBiter enemy
            var cCrawlerParams = new List<string> { "cCrawler_Wall_Fix", "cCrawler_Fix" }; //Valid Parameters for the cCrawler enemy
            var cGazerParams = new List<string> { "cGazer_Normal", "cGazer_Fix", "cGazer_Freeze", "cGazer_Alarm", "cGazer_Wall_Fix" }; //Valid Parameters for the cGazer enemy
            var cGolemParams = new List<string> { "cGolem_Normal", "cGolem_Alarm", "cGolem_Fix" }; //Valid Parameters for the cGolem enemy
            var cStalkerParams = new List<string> { "cStalker_Normal", "cStalker_Fix", "cStalker_Wall_Fix", "cStalker_Freeze" }; //Valid Parameters for the cTaker enemy
            var cTakerParams = new List<string> { "cTaker_Fix", "cTaker_Homing", "cTaker_Chase_Bomb" }; //Valid Parameters for the cTaker enemy
            var cTitanParams = new List<string> { "cTitan_Normal", "cTitan_Fix", "cTitan_Freeze", "cTitan_Alarm" }; //Valid Parameters for the cTitan enemy
            var cTrickerParams = new List<string> { "cTricker_Fix", "cTricker_Normal", "cTricker_Slave", "cTricker_Master",
                "cTricker_Homing", "cTricker_Alarm" }; //Valid Parameters for the cTricker enemy
            var eArmorParams = new List<string> { "eArmor_Normal", "eArmor_Fix" }; //Valid Parameters for the eArmor enemy
            var eBomberParams = new List<string> { "eBomber_Normal", "eBomber_Fix", "eBomber_Wall_Fix", "eBomber_Wall_Normal" }; //Valid Parameters for the eBomber enemy
            var eBlusterParams = new List<string> { "eBluster_Homing", "eBluster_Fix", "eBluster_Normal", }; //Valid Parameters for the eBluster(?) enemy
            var eBusterParams = new List<string> { "eBuster_Fix", "eBuster_Normal", }; //Valid Parameters for the eBuster enemy
            var eCannonParams = new List<string> { "eCannon_Normal", "eCannon_Fix", "eCannon_Fix_Laser", "eCannon_Trans" }; //Valid Parameters for the eCannon enemy
            var eCommanderParams = new List<string> { "eCommander_Master", "eCommander_Fix", "eCommander_Alarm", "eCommander_Normal" }; //Valid Parameters for the eCommander enemy
            var eFlyerParams = new List<string> { "eFlyer_Homing", "eFlyer_Fix", "eFlyer_Fix_Rocket", "eFlyer_Fix_Vulcan", "eFlyer_Normal" }; //Valid Parameters for the eFlyer enemy
            var eGuardianParams = new List<string> { "eGuardian_Normal", "eGuardian_Fix", "eGuardian_Ball_Normal", "eGuardian_Ball_Fix",
                "eGuardian_Freeze" }; //Valid Parameters for the eGuardian enemy
            var eGunnerParams = new List<string> { "eGunner_Normal", "eGunner_Fix_Vulcan", "eGunner_Fix", "eGunner_Trans",
                "eGunner_Fix_Rocket" }; //Valid Parameters for the eGunner enemy
            var eKeeperParams = new List<string> { "eKeeper_Normal", "eKeeper_Ball_Fix", "eKeeper_Fix", "eKeeper_Ball_Fix",
                "eKeeper_Freeze" }; //Valid Parameters for the eKeeper enemy
            var eLinerParams = new List<string> { "eLiner_Slave", "eLiner_Normal" }; //Valid Parameters for the eLiner enemy
            var eLancerParams = new List<string> { "eLancer_Normal", "eLancer_Fix" }; //Valid Parameters for the eLancer enemy
            var eRounderParams = new List<string> { "eRounder_Normal", "eRounder_Slave", "eRounder_Twn_Escape", "eRounder_Twn_Chase",
                "eRounder_Fix" }; //Valid Parameters for the eRounder enemy
            var eSearcherParams = new List<string> { "eSearcher_Fix", "eSearcher_Normal", "eSearcher_Alarm", "eSearcher_Fix_Rocket",
                "eSearcher_Fix_Bomb" }; //Valid Parameters for the eSearcher enemy
            var eStingerParams = new List<string> { "eStinger_Fix", "eStinger_Fix_Missile", "eStinger_Normal" }; //Valid Parameters for the eStinger enemy
            var eSweeperParams = new List<string> { "eSweeper_Fix", "eSweeper_Wall_Fix" }; //Valid Parameters for the eSweeper enemy
            var eHunterParams = new List<string> { "eHunter_Normal", "eHunter_Fix", "eHunter_Hide_Fix" }; //Valid Parameters for the eHunter enemy
            var eWalkerParams = new List<string> { "eWalker_Normal", "eWalker_Fix" }; //Valid Parameters for the eWalker enemy
            var eChaserParams = new List<string> { "eChaser_Master", "eChaser_Normal", "eChaser_Alarm" }; //Valid Parameters for the eChaser enemy

            var eCerberusParams = new List<string> { "eCerberus_sonic", "eCerberus_shadow" }; //Valid Parameters for the Egg Cerberus
            var eGenesisParams = new List<string> { "eGenesis_sonic", "eGenesis_silver" }; //Valid Parameters for the Egg Cerberus
            var secondiblisParams = new List<string> { "secondIblis_sonic", "secondIblis_shadow" }; //Valid Parameters for Iblis Phase 2
            var firstmefiressParams = new List<string> { "firstmefiress_shadow", "firstmefiress_omega" }; //Valid Parameters for Iblis Phase 2
            #endregion

            spoiler.WriteLine("Enemy Randomisation Outcomes:");

            //Locate the enemy/enemyextra parts of the set data & home in on the string attributes.
            IEnumerable<XElement> enemy =
                from el in doc.Elements("Object")
                where (string)el.Attribute("type") == "enemy" || (string)el.Attribute("type") == "enemyextra"
                select el;
            foreach (XElement el in enemy)
            {
                IEnumerable<XElement> enemy2 =
                    from el2 in el.Elements("Parameters")
                    select el2;
                foreach (XElement el2 in enemy2)
                {
                    IEnumerable<XElement> enemy3 =
                        from el3 in el2.Elements("Parameter")
                        where (string)el3.Attribute("type") == "String"
                        select el3;
                    foreach (XElement el3 in enemy3)
                    {
                        //Check the Value we're looking at is blacklisted, if so, skip it.
                        if (blacklist.Contains(el3.Value))
                        {
                            if (el3.Value != "") { spoiler.Write("'" + el3.Value + "' skipped due to being a blacklisted value." + Environment.NewLine); Console.WriteLine("'" + el3.Value + "' skipped due to being a blacklisted value."); }
                            continue;
                        }

                        if (lastEnemyName == "")
                        {
                            if (!validEnemies.Contains(el3.Value) && !blacklist.Contains(el3.Value)) { Console.WriteLine("Encountered unknown enemy " + el3.Value + "!"); } //Debug stuff to alert us if we've randomised an enemy that isn't in the list.
                            spoiler.Write("'" + el3.Value + "' became: ");
                            index = rnd.Next(validEnemies.Count);
                            el3.Value = validEnemies[index];
                            spoiler.Write("'" + el3.Value + "'. Previous parameter of: ");
                            lastEnemyName = el3.Value;
                        }
                        else
                        {
                            spoiler.Write("'" + el3.Value + "' became: ");
                            switch (lastEnemyName)
                            {
                                case "cBiter":
                                    index = rnd.Next(cBiterParams.Count);
                                    el3.Value = cBiterParams[index];
                                    break;
                                case "cCrawler":
                                    index = rnd.Next(cCrawlerParams.Count);
                                    el3.Value = cCrawlerParams[index];
                                    break;
                                case "cGazer":
                                    index = rnd.Next(cGazerParams.Count);
                                    el3.Value = cGazerParams[index];
                                    break;
                                case "cGolem":
                                    index = rnd.Next(cGolemParams.Count);
                                    el3.Value = cGolemParams[index];
                                    break;
                                case "cStalker":
                                    index = rnd.Next(cStalkerParams.Count);
                                    el3.Value = cStalkerParams[index];
                                    break;
                                case "cTaker":
                                    index = rnd.Next(cTakerParams.Count);
                                    el3.Value = cTakerParams[index];
                                    break;
                                case "cTitan":
                                    index = rnd.Next(cTitanParams.Count);
                                    el3.Value = cTitanParams[index];
                                    break;
                                case "cTricker":
                                    index = rnd.Next(cTrickerParams.Count);
                                    el3.Value = cTrickerParams[index];
                                    break;
                                case "eArmor":
                                    index = rnd.Next(eArmorParams.Count);
                                    el3.Value = eArmorParams[index];
                                    break;
                                case "eBomber":
                                    index = rnd.Next(eBomberParams.Count);
                                    el3.Value = eBomberParams[index];
                                    break;
                                case "eBluster":
                                    index = rnd.Next(eBlusterParams.Count);
                                    el3.Value = eBlusterParams[index];
                                    break;
                                case "eBuster":
                                    index = rnd.Next(eBusterParams.Count);
                                    el3.Value = eBusterParams[index];
                                    break;
                                case "eCannon":
                                    index = rnd.Next(eCannonParams.Count);
                                    el3.Value = eCannonParams[index];
                                    break;
                                case "eCommander":
                                    index = rnd.Next(eCommanderParams.Count);
                                    el3.Value = eCommanderParams[index];
                                    break;
                                case "eFlyer":
                                    index = rnd.Next(eFlyerParams.Count);
                                    el3.Value = eFlyerParams[index];
                                    break;
                                case "eGuardian":
                                    index = rnd.Next(eGuardianParams.Count);
                                    el3.Value = eGuardianParams[index];
                                    break;
                                case "eGunner":
                                    index = rnd.Next(eGunnerParams.Count);
                                    el3.Value = eGunnerParams[index];
                                    break;
                                case "eKeeper":
                                    index = rnd.Next(eKeeperParams.Count);
                                    el3.Value = eKeeperParams[index];
                                    break;
                                case "eLiner":
                                    index = rnd.Next(eLinerParams.Count);
                                    el3.Value = eLinerParams[index];
                                    break;
                                case "eLancer":
                                    index = rnd.Next(eLancerParams.Count);
                                    el3.Value = eLancerParams[index];
                                    break;
                                case "eRounder":
                                    index = rnd.Next(eRounderParams.Count);
                                    el3.Value = eRounderParams[index];
                                    break;
                                case "eSearcher":
                                    index = rnd.Next(eSearcherParams.Count);
                                    el3.Value = eSearcherParams[index];
                                    break;
                                case "eStinger":
                                    index = rnd.Next(eStingerParams.Count);
                                    el3.Value = eStingerParams[index];
                                    break;
                                case "eSweeper":
                                    index = rnd.Next(eSweeperParams.Count);
                                    el3.Value = eSweeperParams[index];
                                    break;
                                case "eHunter":
                                    index = rnd.Next(eHunterParams.Count);
                                    el3.Value = eHunterParams[index];
                                    break;
                                case "eWalker":
                                    index = rnd.Next(eWalkerParams.Count);
                                    el3.Value = eWalkerParams[index];
                                    break;
                                case "eChaser":
                                    index = rnd.Next(eChaserParams.Count);
                                    el3.Value = eChaserParams[index];
                                    break;
                                case "eCerberus":
                                    index = rnd.Next(eCerberusParams.Count);
                                    el3.Value = eCerberusParams[index];
                                    break;
                                case "eGenesis":
                                    index = rnd.Next(eGenesisParams.Count);
                                    el3.Value = eGenesisParams[index];
                                    break;
                                case "eWyvern":
                                    el3.Value = "eWyvern";
                                    break;
                                case "firstiblis":
                                    el3.Value = "firstIblis";
                                    break;
                                case "secondiblis":
                                    index = rnd.Next(secondiblisParams.Count);
                                    el3.Value = secondiblisParams[index];
                                    break;
                                case "thirdiblis":
                                    el3.Value = "thirdIblis";
                                    break;
                                case "firstmefiress":
                                    index = rnd.Next(firstmefiressParams.Count);
                                    el3.Value = firstmefiressParams[index];
                                    break;
                                case "solaris01":
                                    el3.Value = "solaris01";
                                    break;
                                default:
                                    break;
                            }
                            lastEnemyName = "";
                            spoiler.Write("'" + el3.Value + "'" + Environment.NewLine);
                        }
                    }
                }
            }
            spoiler.Write(Environment.NewLine);
        }

        static public void CharacterRandomiser(XElement doc, Random rnd, StreamWriter spoiler, List<String> validCharacters)
        {
            int index = 0; //Int to use as a host for the random number generation
            spoiler.WriteLine("Character Randomisation Outcomes:");

            var elements = doc.Descendants().Where(e => (e.Value == "sonic_new") || (e.Value == "tails") || (e.Value == "knuckles") || (e.Value == "shadow") || (e.Value == "shadow_none") || (e.Value == "rouge") || (e.Value == "omega") || (e.Value == "silver") || (e.Value == "blaze") || (e.Value == "amy") || (e.Value == "princess") || (e.Value == "snow_board_wap") || (e.Value == "snow_board") || (e.Value == "sonic_fast") || (e.Value == "shadow_jeep") || (e.Value == "shadow_glider") || (e.Value == "shadow_hover") || (e.Value == "shadow_bike"));
            foreach (var ele3 in elements)
            {
                index = rnd.Next(validCharacters.Count);
                spoiler.Write(ele3.Value + " spawn point became a ");
                ele3.Value = validCharacters[index];
                spoiler.Write(ele3.Value + " spawn point." + Environment.NewLine);
            }

            spoiler.Write(Environment.NewLine);
        }

        static public void ItemRandomiser(XElement doc, Random rnd, StreamWriter spoiler, List<String> validItems)
        {
            int index = 0; //Int to use as a host for the random number generation
            spoiler.WriteLine("Item Capsule Randomisation Outcomes:");

            IEnumerable<XElement> itembox =
                from el in doc.Elements("Object")
                where (string)el.Attribute("type") == "itemboxg" || (string)el.Attribute("type") == "itemboxa"
                select el;
            foreach (XElement el in itembox)
            {
                IEnumerable<XElement> itembox2 =
                    from el2 in el.Elements("Parameters")
                    select el2;
                foreach (XElement el2 in itembox2)
                {
                    IEnumerable<XElement> itembox3 =
                        from el3 in el2.Elements("Parameter")
                        where (string)el3.Attribute("type") == "Int32"
                        select el3;
                    foreach (XElement el3 in itembox3)
                    {
                        switch (el3.Value)
                        {
                            case "0":
                                spoiler.Write("Empty capsule became ");
                                break;
                            case "1":
                                spoiler.Write("5 Ring capsule became ");
                                break;
                            case "2":
                                spoiler.Write("10 Ring capsule became ");
                                break;
                            case "3":
                                spoiler.Write("20 Ring capsule became ");
                                break;
                            case "4":
                                spoiler.Write("1UP capsule became ");
                                break;
                            case "5":
                                spoiler.Write("Power Sneakers capsule became ");
                                break;
                            case "6":
                                spoiler.Write("Power Gauge Refill capsule became ");
                                break;
                            case "7":
                                spoiler.Write("Invincibility capsule became ");
                                break;
                            case "8":
                                spoiler.Write("Shield capsule became ");
                                break;
                        }
                        index = rnd.Next(validItems.Count);
                        el3.Value = validItems[index];
                        switch (el3.Value)
                        {
                            case "0":
                                spoiler.Write("an Empty capsule." + Environment.NewLine);
                                break;
                            case "1":
                                spoiler.Write("a 5 Ring capsule." + Environment.NewLine);
                                break;
                            case "2":
                                spoiler.Write("a 10 Ring capsule." + Environment.NewLine);
                                break;
                            case "3":
                                spoiler.Write("a 20 Ring capsule." + Environment.NewLine);
                                break;
                            case "4":
                                spoiler.Write("a 1UP capsule." + Environment.NewLine);
                                break;
                            case "5":
                                spoiler.Write("a Power Sneakers capsule." + Environment.NewLine);
                                break;
                            case "6":
                                spoiler.Write("a Power Gauge Refill capsule." + Environment.NewLine);
                                break;
                            case "7":
                                spoiler.Write("an Invincibility capsule." + Environment.NewLine);
                                break;
                            case "8":
                                spoiler.Write("a Shield capsule." + Environment.NewLine);
                                break;
                        }
                    }
                }
            }
            spoiler.Write(Environment.NewLine);
        }

        static public void VoiceRandomiser(XElement doc, Random rnd, StreamWriter spoiler)
        {
            int index = 0; //Int to use as a host for the random number generation
            spoiler.WriteLine("Voice Trigger Randomisation Outcomes:");

            String rawLines = Properties.Resources.s06TextStrings;
            string[] lines = rawLines.Split
            (
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );
            
            //Locate the voice/voiceextra parts of the set data & home in on the string attributes.
            IEnumerable<XElement> voice =
                from el in doc.Elements("Object")
                where (string)el.Attribute("type") == "common_hint_collision" || (string)el.Attribute("type") == "common_hint"
                select el;
            foreach (XElement el in voice)
            {
                IEnumerable<XElement> voice2 =
                    from el2 in el.Elements("Parameters")
                    select el2;
                foreach (XElement el2 in voice2)
                {
                    IEnumerable<XElement> voice3 =
                        from el3 in el2.Elements("Parameter")
                        where (string)el3.Attribute("type") == "String"
                        select el3;
                    foreach (XElement el3 in voice3)
                    {
                        index = rnd.Next(lines.Length);
                        spoiler.Write(el3.Value + " voice trigger became a ");
                        el3.Value = lines[index];
                        spoiler.Write(el3.Value + " voice trigger." + Environment.NewLine);
                    }
                }
            }
        }

        static public void SetSave(bool randomiseFolder, int outputFolderType, XElement doc, string output, string xmlName, string filepath, bool keepXML, bool spoilerLog, string xml)
        {
            var setData = new HedgeLib.Sets.S06SetData();
            switch (outputFolderType)
            {
                case 0: //Custom
                    doc.Save(output + "\\" + xmlName + "_edit.xml");
                    File.Delete(output + "\\" + xmlName + ".set");
                    setData.ImportXML(output + "\\" + xmlName + "_edit.xml");
                    setData.Save(output + "\\" + xmlName + ".set");
                    if (!keepXML) { File.Delete(output + "\\" + xmlName + "_edit.xml"); }
                    if (!spoilerLog) { File.Delete(output + "\\" + xmlName + "_log.txt"); }
                    break;
                case 1: //Source
                    if (!randomiseFolder)
                    {
                        doc.Save(filepath.Remove(filepath.Length - 4) + "_edit.xml");
                        File.Delete(filepath.Remove(filepath.Length - 4) + ".set");
                        setData.ImportXML(filepath.Remove(filepath.Length - 4) + "_edit.xml");
                        setData.Save(filepath.Remove(filepath.Length - 4) + ".set");
                        if (!keepXML) { File.Delete(filepath.Remove(filepath.Length - 4) + "_edit.xml"); }
                        if (!spoilerLog) { File.Delete(filepath.Remove(filepath.Length - 4) + "_log.txt"); }
                    }
                    if (randomiseFolder)
                    {
                        doc.Save(Path.GetDirectoryName(xml) + "\\" + xmlName + "_edit.xml");
                        File.Delete(Path.GetDirectoryName(xml) + "\\" + xmlName + ".set");
                        setData.ImportXML(Path.GetDirectoryName(xml) + "\\" + xmlName + "_edit.xml");
                        setData.Save(Path.GetDirectoryName(xml) + "\\" + xmlName + ".set");
                        if (!keepXML) { File.Delete(Path.GetDirectoryName(xml) + "\\" + xmlName + "_edit.xml"); }
                        if (!spoilerLog) { File.Delete(filepath + "\\" + xmlName + "_log.txt"); }
                    }
                    break;
                case 2: //Program
                    doc.Save(xmlName + "_edit.xml");
                    File.Delete(xmlName + ".set");
                    setData.ImportXML(xmlName + "_edit.xml");
                    setData.Save(xmlName + ".set");
                    if (!keepXML) { File.Delete(xmlName + "_edit.xml"); }
                    if (!spoilerLog) { File.Delete(xmlName + "_log.txt"); }
                    break;
            }
        }
    }
}
