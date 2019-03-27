using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace Sonic06Randomiser
{
    class Randomisation
    {
        static public void Randomise(string filepath, string rndSeed, string output, bool randomEnemies, bool randomItems, bool randomCharacters, bool altCharacters, bool randomVoices, bool spoilerLog, bool keepXML, bool sourceOutput)
        {
            string logSeed = rndSeed; //Store the old seed for the Spoiler Log

            //Load XML
            string file = filepath;
            string xmlName = filepath.Remove(0, Path.GetDirectoryName(filepath).Length);
            xmlName = xmlName.Remove(xmlName.Length - 4);
            xmlName = xmlName.Replace("\\", "");

            //Set RND Seed
            Random rnd = new Random(); //Set up RND
            if (rndSeed != "")
            {
                rndSeed = rndSeed + xmlName;
                rnd = new Random(rndSeed.GetHashCode()); //Override RND with seed if user inputs one
            }

            //Setup Spoiler Log
            StreamWriter sw = new StreamWriter(xmlName + "_log.txt");
            if (output != "")
            {
                sw.Close();
                File.Delete(xmlName + "_log.txt");
                File.Delete(output + "\\" + xmlName + "_log.txt");
                sw = new StreamWriter(output + "\\" + xmlName + "_log.txt");
            }
            else
            {
                if (sourceOutput)
                {
                    sw.Close();
                    File.Delete(xmlName + "_log.txt");
                    File.Delete(filepath.Remove(filepath.Length - 4) + "_log.txt");
                    sw = new StreamWriter(filepath.Remove(filepath.Length - 4) + "_log.txt");
                }
            }

            //Spoiler Log Seed
            sw.WriteLine("Seed: " + logSeed);
            sw.WriteLine("Randomise Enemies: " + randomEnemies);
            sw.WriteLine("Randomise Item Capsules: " + randomItems);
            sw.WriteLine("Randomise Character Spawns: " + randomCharacters);
            sw.WriteLine("Randomise Voice Line Triggers: " + randomVoices);
            sw.Write(Environment.NewLine);

            //Enemy Randomisation Strings
            var enemyNames = new List<string> { "cBiter", "cCrawler", "cGolem", "cTaker", "eBomber", "eCannon", "eCannon(Fly)", "eFlyer", "eGuardian", "eGunner", "eGunner(Fly)", "eLiner", "eRounder", "eSearcher" }; //Valid Enemy Names to switch in
            var cBiterParams = new List<string> { "cBiter_Normal", "cBiter_Fix", "cBiter_Freeze", "cBiter_Wall_Fix", "cBiter_Wall_Normal" }; //Valid Parameters for the cBiter enemy
            var cCrawlerParams = new List<string> { "cCrawler_Wall_Fix", "cCrawler_Fix" }; //Valid Parameters for the cCrawler enemy
            var cGolemParams = new List<string> { "cGolem_Normal", "cGolem_Alarm", "cGolem_Fix" }; //Valid Parameters for the cGolem enemy
            var cTakerParams = new List<string> { "cTaker_Fix", "cTaker_Homing" }; //Valid Parameters for the cTaker enemy
            var eBomberParams = new List<string> { "eBomber_Normal", "eBomber_Fix", "eBomber_Wall_Fix", "eBomber_Wall_Normal" }; //Valid Parameters for the eBomber enemy
            var eCannonParams = new List<string> { "eCannon_Normal", "eCannon_Fix", "eCannon_Fix_Laser", "eCannon_Trans" }; //Valid Parameters for the eCannon enemy
            var eCannonFlyParams = new List<string> { "eCannonFly_Fix", "eCannonFly_Carrier", "eCannonFly_Normal" }; //Valid Parameters for the eCannon(Fly) enemy
            var eFlyerParams = new List<string> { "eFlyer_Homing", "eFlyer_Fix", "eFlyer_Fix_Rocket", "eFlyer_Fix_Vulcan", "eFlyer_Normal" }; //Valid Parameters for the eFlyer enemy
            var eGuardianParams = new List<string> { "eGuardian_Normal", "eGuardian_Fix", "eGuardian_Ball_Normal", "eGuardian_Ball_Fix", "eGuardian_Freeze" }; //Valid Parameters for the eGuardian enemy
            var eGunnerParams = new List<string> { "eGunner_Normal", "eGunner_Fix_Vulcan", "eGunner_Fix", "eGunner_Trans", "eGunner_Fix_Rocket" }; //Valid Parameters for the eGunner enemy
            var eGunnerFlyParams = new List<string> { "eGunnerFly_Fix", "eGunnerFly_Normal", "eGunnerFly_Homing", "eGunnerFly_Chase", "eGunnerFly_Fix_Vulcan", "eGunnerFly_Fix_Rocket" }; //Valid Parameters for the eGunner(Fly) enemy
            var eLinerParams = new List<string> { "eLiner_Slave", "eLiner_Normal" }; //Valid Parameters for the eLiner enemy
            var eRounderParams = new List<string> { "eRounder_Normal", "eRounder_Slave", "eRounder_Twn_Escape", "eRounder_Twn_Chase", "eRounder_Fix" }; //Valid Parameters for the eRounder enemy
            var eSearcherParams = new List<string> { "eSearcher_Fix", "eSearcher_Normal", "eSearcher_Alarm", "eSearcher_Fix_Rocket", "eSearcher_Fix_Bomb" }; //Valid Parameters for the eSearcher enemy
            var characterNames = new List<string> { "sonic_new", "tails", "knuckles", "shadow", "rouge", "omega", "silver", "blaze", "amy", "princess" }; //Valid Character Names to switch in
            var characterNamesAlts = new List<string> { "sonic_new", "tails", "knuckles", "shadow", "rouge", "omega", "silver", "blaze", "amy", "princess", "snow_board_wap", "snow_board", "sonic_fast" }; //Valid Character Names to switch in when using vehicles

            int index = 0; //Random Number
            var randomisedNames = new List<String>();

            XElement doc = XElement.Load(file);

            if (randomEnemies)
            {
                sw.WriteLine("Enemy Randomisation Outcomes");
                //Randomise Enemy Type
                var elements = doc.Descendants().Where(e => (e.Value == "cBiter") || (e.Value == "cCrawler") || (e.Value == "cGolem") || (e.Value == "cTaker") || (e.Value == "eBomber") || (e.Value == "eCannon") || (e.Value == "eFlyer") || (e.Value == "eGuardian") || (e.Value == "eGunner") || (e.Value == "eLiner") || (e.Value == "eRounder") || (e.Value == "eSearcher"));
                foreach (var ele in elements)
                {
                    index = rnd.Next(enemyNames.Count);
                    sw.Write(ele.Value + " became: ");
                    ele.Value = enemyNames[index];
                    sw.Write(ele.Value + "." + Environment.NewLine);
                    randomisedNames.Add(ele.Value);
                }
                sw.Write(Environment.NewLine);

                //Randomise Enemy Attributes
                var elements2 = doc.Descendants().Where(e => (e.Value == "cBiter_Normal") || (e.Value == "cBiter_Fix") || (e.Value == "cBiter_Freeze") || (e.Value == "cBiter_Wall_Fix") || (e.Value == "cBiter_Wall_Normal") || (e.Value == "cCrawler_Wall_Fix") || (e.Value == "cCrawler_Fix") || (e.Value == "cGolem_Normal") || (e.Value == "cGolem_Alarm") || (e.Value == "cGolem_Fix") || (e.Value == "cTaker_Fix") || (e.Value == "cTaker_Homing") || (e.Value == "eBomber_Normal") || (e.Value == "eBomber_Fix") || (e.Value == "eBomber_Wall_Fix") || (e.Value == "eBomber_Wall_Normal") || (e.Value == "eCannon_Normal") || (e.Value == "eCannon_Fix") || (e.Value == "eCannon_Fix_Laser") || (e.Value == "eCannon_Trans") || (e.Value == "eFlyer_Homing") || (e.Value == "eFlyer_Fix") || (e.Value == "eFlyer_Fix_Rocket") || (e.Value == "eFlyer_Fix_Vulcan") || (e.Value == "eFlyer_Normal") || (e.Value == "eGuardian_Normal") || (e.Value == "eGuardian_Fix") || (e.Value == "eGuardian_Ball_Normal") || (e.Value == "eGuardian_Ball_Fix") || (e.Value == "eGuardian_Freeze") || (e.Value == "eGunner_Normal") || (e.Value == "eGunner_Fix_Vulcan") || (e.Value == "eGunner_Fix") || (e.Value == "eGunner_Trans") || (e.Value == "eGunner_Fix_Rocket") || (e.Value == "eLiner_Slave") || (e.Value == "eLiner_Normal") || (e.Value == "eRounder_Normal") || (e.Value == "eRounder_Slave") || (e.Value == "eRounder_Twn_Escape") || (e.Value == "eRounder_Twn_Chase") || (e.Value == "eRounder_Fix") || (e.Value == "eSearcher_Fix") || (e.Value == "eSearcher_Normal") || (e.Value == "eSearcher_Alarm") || (e.Value == "eSearcher_Fix_Rocket") || (e.Value == "eSearcher_Fix_Bomb"));
                int enemyNumber = 0;
                foreach (var ele2 in elements2)
                {
                    sw.Write(ele2.Value + " paramater became: ");
                    switch (randomisedNames[enemyNumber])
                    {
                        case "cBiter":
                            index = rnd.Next(cBiterParams.Count);
                            ele2.Value = cBiterParams[index];
                            break;
                        case "cCrawler":
                            index = rnd.Next(cCrawlerParams.Count);
                            ele2.Value = cCrawlerParams[index];
                            break;
                        case "cGolem":
                            index = rnd.Next(cGolemParams.Count);
                            ele2.Value = cGolemParams[index];
                            break;
                        case "cTaker":
                            index = rnd.Next(cTakerParams.Count);
                            ele2.Value = cTakerParams[index];
                            break;
                        case "eBomber":
                            index = rnd.Next(eBomberParams.Count);
                            ele2.Value = eBomberParams[index];
                            break;
                        case "eCannon":
                            index = rnd.Next(eCannonParams.Count);
                            ele2.Value = eCannonParams[index];
                            break;
                        case "eFlyer":
                            index = rnd.Next(eFlyerParams.Count);
                            ele2.Value = eFlyerParams[index];
                            break;
                        case "eGuardian":
                            index = rnd.Next(eGuardianParams.Count);
                            ele2.Value = eGuardianParams[index];
                            break;
                        case "eGunner":
                            index = rnd.Next(eGunnerParams.Count);
                            ele2.Value = eGunnerParams[index];
                            break;
                        case "eLiner":
                            index = rnd.Next(eLinerParams.Count);
                            ele2.Value = eLinerParams[index];
                            break;
                        case "eRounder":
                            index = rnd.Next(eRounderParams.Count);
                            ele2.Value = eRounderParams[index];
                            break;
                        case "eSearcher":
                            index = rnd.Next(eSearcherParams.Count);
                            ele2.Value = eSearcherParams[index];
                            break;
                        default:
                            Console.WriteLine("Unknown Object?");
                            break;
                    }
                    sw.Write(ele2.Value + "." + Environment.NewLine);
                    enemyNumber++;
                }
                sw.Write(Environment.NewLine);
            }

            if (randomItems)
            {
                sw.WriteLine("Item Capsule Randomisation Outcomes");
                //Randomise Item Boxes (oh god this is ugly & I barely even know why it works)
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
                                case "1":
                                    sw.Write("5 Ring capsule became a ");
                                    break;
                                case "2":
                                    sw.Write("10 Ring capsule became a ");
                                    break;
                                case "3":
                                    sw.Write("20 Ring capsule became a ");
                                    break;
                                case "4":
                                    sw.Write("Power Gauge Refill capsule became a ");
                                    break;
                                case "5":
                                    sw.Write("Power Sneakers capsule became a ");
                                    break;
                                case "6":
                                    sw.Write("Power Gauge Refill? capsule became a ");
                                    break;
                                case "7":
                                    sw.Write("Invincibility capsule became a ");
                                    break;
                            }
                            el3.Value = rnd.Next(1, 9).ToString(); //Randomise the Item Boxes to a value from 1 to 8 (C# why does the second value have to be 1 higher than what I want?). 0 is valid, but is an empty item box.
                            switch (el3.Value)
                            {
                                case "1":
                                    sw.Write("5 Ring capsule." + Environment.NewLine);
                                    break;
                                case "2":
                                    sw.Write("10 Ring capsule." + Environment.NewLine);
                                    break;
                                case "3":
                                    sw.Write("20 Ring capsule." + Environment.NewLine);
                                    break;
                                case "4":
                                    sw.Write("Power Gauge Refill capsule." + Environment.NewLine);
                                    break;
                                case "5":
                                    sw.Write("Power Sneakers capsule." + Environment.NewLine);
                                    break;
                                case "6":
                                    sw.Write("Power Gauge Refill? capsule." + Environment.NewLine);
                                    break;
                                case "7":
                                    sw.Write("Invincibility capsule." + Environment.NewLine);
                                    break;
                                case "8":
                                    sw.Write("Shield capsule." + Environment.NewLine);
                                    break;
                            }
                        }
                    }
                }
                sw.Write(Environment.NewLine);
            }

            //Randomise player_start2 objects
            if (randomCharacters)
            {
                sw.WriteLine("Character Randomisation Outcomes");
                if (!altCharacters)
                {
                    var elements3 = doc.Descendants().Where(e => (e.Value == "sonic_new") || (e.Value == "tails") || (e.Value == "knuckles") || (e.Value == "shadow") || (e.Value == "rouge") || (e.Value == "omega") || (e.Value == "silver") || (e.Value == "blaze") || (e.Value == "amy") || (e.Value == "princess"));
                    foreach (var ele3 in elements3)
                    {
                        index = rnd.Next(characterNames.Count);
                        sw.Write(ele3.Value + " spawn point became a ");
                        ele3.Value = characterNames[index];
                        sw.Write(ele3.Value + " spawn point." + Environment.NewLine);
                    }
                }
                else
                {
                    var elements3 = doc.Descendants().Where(e => (e.Value == "sonic_new") || (e.Value == "tails") || (e.Value == "knuckles") || (e.Value == "shadow") || (e.Value == "rouge") || (e.Value == "omega") || (e.Value == "silver") || (e.Value == "blaze") || (e.Value == "amy") || (e.Value == "princess") || (e.Value == "snow_board_wap") || (e.Value == "snow_board") || (e.Value == "sonic_fast") || (e.Value == "shadow_jeep") || (e.Value == "shadow_glider") || (e.Value == "shadow_hover") || (e.Value == "shadow_bike"));
                    foreach (var ele3 in elements3)
                    {
                        index = rnd.Next(characterNamesAlts.Count);
                        sw.Write(ele3.Value + " spawn point became a ");
                        ele3.Value = characterNamesAlts[index];
                        sw.Write(ele3.Value + " spawn point." + Environment.NewLine);
                    }
                }
                sw.Write(Environment.NewLine);
            }

            //Randomise Hint Voice Lines
            if (randomVoices)
            {
                sw.WriteLine("Voice Trigger Randomisation Outcomes");
                String rawLines = Properties.Resources.s06TextStrings;
                string[] lines = rawLines.Split
                (
                    new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.None
                );

                int arrayCount = 0;
                foreach (string x in lines)
                {
                    var elements4 = doc.Descendants().Where(e => (e.Value == lines[arrayCount]));
                    foreach (var ele4 in elements4)
                    {
                        index = rnd.Next(lines.Length);
                        sw.Write(ele4.Value + " voice trigger became a ");
                        ele4.Value = lines[index];
                        sw.Write(ele4.Value + " voice trigger." + Environment.NewLine);
                    }
                    arrayCount++;
                }
                sw.Write(Environment.NewLine);
            }

            //Save & cleanup

            if (output != "")
            {
                doc.Save(output + "\\" + xmlName + "_edit.xml");
            }
            else
            {
                if (!sourceOutput)
                {
                    doc.Save(xmlName + "_edit.xml");
                }
                else
                {
                    doc.Save(filepath.Remove(filepath.Length - 4) + "_edit.xml");
                }
            }
            
            if (output != "")
            {
                File.Delete(output + "\\" + xmlName + ".set");
            }
            else
            {
                if (!sourceOutput)
                {
                    File.Delete(xmlName + ".set");
                }
                else
                {
                    File.Delete(filepath.Remove(filepath.Length - 4) + ".set");
                }
            }

            var setData = new HedgeLib.Sets.S06SetData();
            if (output != "")
            {
                setData.ImportXML(output + "\\" + xmlName + "_edit.xml");
            }
            else
            {
                if (!sourceOutput)
                {
                    setData.ImportXML(xmlName + "_edit.xml");
                }
                else
                {
                    setData.ImportXML(filepath.Remove(filepath.Length - 4) + "_edit.xml");
                }
            }
            //setData.ImportXML(xmlName + "_edit.xml");
            if (output != "")
            {
                setData.Save(output + "\\" + xmlName + ".set");
            }
            else
            {
                if (!sourceOutput)
                {
                    setData.Save(xmlName + ".set");
                }
                else
                {
                    setData.Save(filepath.Remove(filepath.Length - 4) + ".set");
                }
            }

            if (!keepXML)
            {
                if (output != "")
                {
                    File.Delete(output + "\\" + xmlName + "_edit.xml");
                }
                else
                {
                    if (!sourceOutput)
                    {
                        File.Delete(xmlName + "_edit.xml");
                    }
                    else
                    {
                        File.Delete(filepath.Remove(filepath.Length - 4) + "_edit.xml");
                    }
                }
            }

            //Close the Spoiler Log File & delete if we don't need it.
            sw.Close();
            if (!spoilerLog)
            {
                if (output != "")
                {
                    File.Delete(output + "\\" + xmlName + "_log.txt");
                }
                else
                {
                    if (!sourceOutput)
                    {
                        File.Delete(xmlName + "_log.txt");
                    }
                    else
                    {
                        File.Delete(filepath.Remove(filepath.Length - 4) + "_log.txt");
                    }
                }
            }
        }
    }
}
