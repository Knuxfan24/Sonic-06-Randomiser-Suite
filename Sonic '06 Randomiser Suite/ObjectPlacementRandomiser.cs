using Marathon.Formats.Placement;
using System.Collections.Generic;
using System.IO;

namespace Sonic_06_Randomiser_Suite
{
    class ObjectPlacementRandomiser
    {
        /// <summary>
        /// The main entry block for the SET Randomisation, all the other functions (minus BossPatch) in this file are accessed through here.
        /// If a function to randomise something about an object would have only been one line, then it is also handled in this function instead of a seperate one.
        /// </summary>
        /// <param name="archivePath">The path to the already unpacked scripts.arc containing all the object placement set files.</param>
        /// <param name="enemies">Whether the enemies should be randomised.</param>
        /// <param name="enemyTypes">The valid enemy types to use.</param>
        /// <param name="behaviours">Whether the enemy behvaiours should be randomised.</param>
        /// <param name="dontEnforceBehaviours">Whether the enemy behaviours are foreced to come from the same enemy.</param>
        /// <param name="characters">Whether the characters should be randomised.</param>
        /// <param name="characterTypes">The valid character types to use.</param>
        /// <param name="items">Whether the item capsule contents should be randomised.</param>
        /// <param name="itemTypes">The valid item types to use.</param>
        /// <param name="commonProps">Whether the props using entries in Common.bin should be randomised.</param>
        /// <param name="pathProps">Whether the props using entries in PathObj.bin should be randomised.</param>
        /// <param name="commonPropTypes">The valid Common.bin prop entries to use.</param>
        /// <param name="pathPropTypes">The valid PathObj.bin prop entries to use.</param>
        /// <param name="voices">Whether the hint voice lines should be randomised.</param>
        /// <param name="voiceLines">The valid hint voice lines to use.</param>
        /// <param name="doors">Whether the door types should be randomised.</param>
        /// <param name="doorTypes">The valid door types to use.</param>
        /// <param name="drawDistance">Whether objects should have their draw distance randomly set between 0 and 10000.</param>
        /// <param name="minDrawDistance">The minimum value to use for Draw Distance randomisation.</param>
        /// <param name="maxDrawDistance">The maximum value to use for Draw Distance randomisation.</param>
        /// <param name="cosmetics">Whether we should change entirely cosmetic stuff (like the Soleanna mirrors and Chaos Emerald colours).</param>
        public static void Load(string archivePath, bool enemies, List<string> enemyTypes, bool behaviours, bool dontEnforceBehaviours, bool characters, List<string> characterTypes, bool items,
                                List<int> itemTypes, bool commonProps, bool pathProps, List<string> commonPropTypes, List<string> pathPropTypes, bool voices, List<string> voiceLines, bool doors,
                                List<string> doorTypes, bool drawDistance, int minDrawDistance, int maxDrawDistance, bool cosmetics)
        {
            // Get a list of all the set files in scripts.arc.
            string[] setFiles = Directory.GetFiles(archivePath, "*.set", SearchOption.AllDirectories);

            // Loop through all the set files.
            foreach(string setFile in setFiles)
            {
                System.Console.WriteLine($@"Randomising '{setFile}'.");

                // Load this set file.
                ObjectPlacement set = new(setFile);

                // Loop through all the objects in this set file.
                foreach (SetObject setObject in set.Data.Objects)
                {
                    // If we're randomising the object's draw distance, then pick a number for it between the specified values
                    if(drawDistance)
                        setObject.DrawDistance = Form_Main.Randomiser.Next(minDrawDistance, maxDrawDistance + 1);

                    // If we're randomising the cosmetic stuff, then pass this object to that function to see if we have to do anything to it.
                    if (cosmetics)
                        CosmeticRandomiser(setObject);

                    // Check this object's type to see if we need to do anything with it.
                    switch (setObject.Type)
                    {
                        // Randomise enemy types and/or their behaviours if we need to.
                        case "enemy":
                        case "enemyextra":
                            if (enemies)
                                EnemyTypeRandomiser(setObject, enemyTypes);
                            if (behaviours)
                                EnemyBehaviourRandomiser(setObject, dontEnforceBehaviours);
                            break;

                        // Randomise character types if we need to.
                        case "player_start2":
                            if(characters)
                                setObject.Parameters[1].Data = characterTypes[Form_Main.Randomiser.Next(characterTypes.Count)];
                            break;

                        // Randomise item capsule contents if we need to.
                        case "itemboxg":
                        case "itemboxa":
                            if (items)
                                setObject.Parameters[0].Data = itemTypes[Form_Main.Randomiser.Next(itemTypes.Count)];
                            break;

                        // Randomise prop elements if we need to.
                        case "objectphysics":
                        case "physicspath":
                        case "objectphysics_item":
                        case "wap_conifer":
                        case "end_outputwarp":
                            if (commonProps)
                                CommonPropRandomiser(setObject, commonPropTypes);
                            break;
                        case "common_path_obj":
                            if (pathProps)
                                setObject.Parameters[0].Data = pathPropTypes[Form_Main.Randomiser.Next(pathPropTypes.Count)];
                            break;

                        // Randomise voice line triggers if we need to.
                        case "common_hint":
                        case "common_hint_collision":
                            if (voices)
                                setObject.Parameters[0].Data = voiceLines[Form_Main.Randomiser.Next(voiceLines.Count)];
                            break;

                        // Randomise door types if we need to.
                        case "aqa_door":
                        case "dtd_door":
                        case "flc_door":
                        case "kdv_door":
                        case "rct_door":
                        case "wap_door":
                        case "wvo_doorA":
                        case "wvo_doorB":
                            if (doors)
                                setObject.Type = doorTypes[Form_Main.Randomiser.Next(doorTypes.Count)];
                            break;
                    }

                }

                // Save the updated set file.
                set.Save();
            }
        }

        /// <summary>
        /// Randomises the type of enemy to spawn in this object.
        /// </summary>
        /// <param name="setObject">The object we're editing.</param>
        /// <param name="enemyTypes">The list of valid enemy types.</param>
        static void EnemyTypeRandomiser(SetObject setObject, List<string> enemyTypes)
        {
            // Set the type of enemy from the list of types.
            setObject.Parameters[0].Data = enemyTypes[Form_Main.Randomiser.Next(enemyTypes.Count)];

            // Set this enemy's palette value depending on what enemy it is.
            switch (setObject.Parameters[0].Data.ToString())
            {
                case "cGazer":
                case "cStalker":
                case "cTitan":
                case "cTricker":
                case "eArmor":
                case "eBluster":
                case "eChaser":
                case "eCommander":
                case "eHunter":
                case "eKeeper":
                case "eStinger":
                case "eStinger(Fly)":
                case "eWalker":
                case "solaris02":
                    setObject.Parameters[1].Data = 1;
                    break;

                case "eLancer":
                case "eLancer(Fly)":
                case "eSweeper":
                    setObject.Parameters[1].Data = 2;
                    break;

                case "eBuster":
                case "eBuster(Fly)":
                    setObject.Parameters[1].Data = 3;
                    break;

                // Set it to 0 by default
                default:
                    setObject.Parameters[1].Data = 0;
                    break;
            }

            // If this enemy is an Egg Cerberus, then set the palette to either 0 (Sonic) or 1 (Shadow).
            if (setObject.Parameters[0].Data.ToString() == "eCerberus")
                setObject.Parameters[1].Data = Form_Main.Randomiser.Next(0, 2);
        }

        /// <summary>
        /// Randomises the behaviour of the enemy in this object.
        /// </summary>
        /// <param name="setObject">The object we're editing.</param>
        /// <param name="dontEnforceBehaviours">Whether we should ensure that the chosen behaviour belongs to this enemy type.</param>
        static void EnemyBehaviourRandomiser(SetObject setObject, bool dontEnforceBehaviours)
        {
            // Setup for if we are enforcing the behaviour type.
            if (!dontEnforceBehaviours)
            {
                // Create lists of the valid parameters for each enemy (based on data previously gathered from examination of ScriptParameter.bin).
                List<string> eGunnerParameters       = new() { "eGunner_Normal", "eGunner_Fix", "eGunner_Fix_Vulcan", "eGunner_Fix_Rocket", "eGunner_Wall_Fix", "eGunner_Chase", "eGunner_Trans" };
                List<string> eGunnerFlyParameters    = new() { "eGunnerFly_Normal", "eGunnerFly_Fix", "eGunnerFly_Fix_Vulcan", "eGunnerFly_Fix_Rocket", "eGunnerFly_Chase", "eGunnerFly_Homing",
                                                               "eGunnerFly_Trans", "eGunnerFly_Boss_Vulcan" };
                List<string> eStingerParameters      = new() { "eStinger_Normal", "eStinger_Fix", "eStinger_Fix_Missile", "eStinger_Chase", "eStinger_Wall_Fix", "eStinger_Trans" };
                List<string> eStingerFlyParameters   = new() { "eStingerFly_Normal", "eStingerFly_Fix", "eStingerFly_Fix_Missile", "eStingerFly_Chase", "eStingerFly_Homing", "eStingerFly_Trans" };
                List<string> eLancerParameters       = new() { "eLancer_Normal", "eLancer_Fix", "eLancer_Chase", "eLancer_Wall_Fix", "eLancer_Trans" };
                List<string> eLancerFlyParameters    = new() { "eLancerFly_Normal", "eLancerFly_Fix", "eLancerFly_Fix_Laser", "eLancerFly_Chase", "eLancerFly_Trans" };
                List<string> eBusterParameters       = new() { "eBuster_Normal", "eBuster_Fix", "eBuster_Wall_Fix", "eBuster_Trans" };
                List<string> eBusterFlyParameters    = new() { "eBusterFly_Normal", "eBusterFly_Fix", "eBusterFly_Trans" };
                List<string> eFlyerParameters        = new() { "eFlyer_Normal", "eFlyer_Fix", "eFlyer_Fix_Vulcan", "eFlyer_Fix_Rocket", "eFlyer_Homing", "eFlyer_Boss_Homing", "eFlyer_Boss_Vulcan" };
                List<string> eBlusterParameters      = new() { "eBluster_Normal", "eBluster_Fix", "eBluster_Homing" };
                List<string> eSearcherParameters     = new() { "eSearcher_Normal", "eSearcher_Fix", "eSearcher_Fix_Rocket", "eSearcher_Fix_Bomb", "eSearcher_Alarm" };
                List<string> eHunterParameters       = new() { "eHunter_Normal", "eHunter_Fix", "eHunter_Hide_Fix" };
                List<string> eRounderParameters      = new() { "eRounder_Normal", "eRounder_Fix", "eRounder_Slave", "eRounder_Twn_Chase", "eRounder_Twn_Escape", "eRounder_Boss" };
                List<string> eCommanderParameters    = new() { "eCommander_Normal", "eCommander_Fix", "eCommander_Master", "eCommander_Alarm" };
                List<string> eLinerParameters        = new() { "eLiner_Normal", "eLiner_Chase", "eLiner_Slave" };
                List<string> eChaserParameters       = new() { "eChaser_Normal", "eChaser_Chase", "eChaser_Master", "eChaser_Alarm" };
                List<string> eBomberParameters       = new() { "eBomber_Normal", "eBomber_Fix", "eBomber_Wall_Normal", "eBomber_Wall_Fix", "eBomber_Allaround", "eBomber_Boss" };
                List<string> eArmorParameters        = new() { "eArmor_Normal", "eArmor_Fix" };
                List<string> eSweeperParameters      = new() { "eSweeper_Fix", "eSweeper_Wall_Fix" };
                List<string> eCannonParameters       = new() { "eCannon_Normal", "eCannon_Fix", "eCannon_Fix_Laser", "eCannon_Fix_Launcher", "eCannon_Wall_Fix", "eCannon_Trans" };
                List<string> eWalkerParameters       = new() { "eWalker_Normal", "eWalker_Fix", "eWalker_Wall_Fix" };
                List<string> eCannonFlyParameters    = new() { "eCannonFly_Normal", "eCannonFly_Fix", "eCannonFly_Trans", "eCannonFly_Carrier" };
                List<string> eGuardianParameters     = new() { "eGuardian_Normal", "eGuardian_Fix", "eGuardian_Ball_Normal", "eGuardian_Ball_Fix", "eGuardian_Freeze" };
                List<string> eKeeperParameters       = new() { "eKeeper_Normal", "eKeeper_Fix", "eKeeper_Ball_Normal", "eKeeper_Ball_Fix", "eKeeper_Freeze" };
                List<string> cBiterParameters        = new() { "cBiter_Normal", "cBiter_Fix", "cBiter_Wall_Normal", "cBiter_Wall_Fix", "cBiter_Freeze" };
                List<string> cStalkerParameters      = new() { "cStalker_Normal", "cStalker_Fix", "cStalker_Wall_Normal", "cStalker_Wall_Fix", "cStalker_Freeze" };
                List<string> cTakerParameters        = new() { "cTaker_Normal", "cTaker_Fix", "cTaker_Normal_Bomb", "cTaker_Fix_Bomb", "cTaker_Chase_Bomb", "cTaker_Homing" };
                List<string> cTrickerParameters      = new() { "cTricker_Normal", "cTricker_Fix", "cTricker_Homing", "cTricker_Master", "cTricker_Slave", "cTricker_Alarm" };
                List<string> cCrawlerParameters      = new() { "cCrawler_Normal", "cCrawler_Fix", "cCrawler_Wall_Fix", "cCrawler_Homing", "cCrawler_Wall_Homing", "cCrawler_Freeze", "cCrawler_Alarm" };
                List<string> cGazerParameters        = new() { "cGazer_Normal", "cGazer_Fix", "cGazer_Wall_Fix", "cGazer_Freeze", "cGazer_Alarm" };
                List<string> cGolemParameters        = new() { "cGolem_Normal", "cGolem_Fix", "cGolem_Freeze", "cGolem_Alarm" };
                List<string> cTitanParameters        = new() { "cTitan_Normal", "cTitan_Fix", "cTitan_Freeze", "cTitan_Alarm" };
                List<string> secondIblisParameters   = new() { "secondIblis_sonic", "secondIblis_shadow" };
                List<string> firstmefiressParameters = new() { "firstmefiress_shadow", "firstmefiress_omega" };
                List<string> eCerberusParameters     = new() { "eCerberus_sonic", "eCerberus_shadow" };
                List<string> eGenesisParameters      = new() { "eGenesis_sonic", "eGenesis_silver" };

                // Get the enemy type and pick a behaviour from the approriate parameter list.
                switch (setObject.Parameters[0].Data.ToString())
                {
                    case "eGunner":        setObject.Parameters[2].Data = eGunnerParameters[Form_Main.Randomiser.Next(eGunnerParameters.Count)];             break;
                    case "eGunner(Fly)":   setObject.Parameters[2].Data = eGunnerFlyParameters[Form_Main.Randomiser.Next(eGunnerFlyParameters.Count)];       break;
                    case "eStinger":       setObject.Parameters[2].Data = eStingerParameters[Form_Main.Randomiser.Next(eStingerParameters.Count)];           break;
                    case "eStinger(Fly)":  setObject.Parameters[2].Data = eStingerFlyParameters[Form_Main.Randomiser.Next(eStingerFlyParameters.Count)];     break;
                    case "eLancer":        setObject.Parameters[2].Data = eLancerParameters[Form_Main.Randomiser.Next(eLancerParameters.Count)];             break;
                    case "eLancer(Fly)":   setObject.Parameters[2].Data = eLancerFlyParameters[Form_Main.Randomiser.Next(eLancerFlyParameters.Count)];       break;
                    case "eBuster":        setObject.Parameters[2].Data = eBusterParameters[Form_Main.Randomiser.Next(eBusterParameters.Count)];             break;
                    case "eBuster(Fly)":   setObject.Parameters[2].Data = eBusterFlyParameters[Form_Main.Randomiser.Next(eBusterFlyParameters.Count)];       break;
                    case "eFlyer":         setObject.Parameters[2].Data = eFlyerParameters[Form_Main.Randomiser.Next(eFlyerParameters.Count)];               break;
                    case "eBluster":       setObject.Parameters[2].Data = eBlusterParameters[Form_Main.Randomiser.Next(eBlusterParameters.Count)];           break;
                    case "eSearcher":      setObject.Parameters[2].Data = eSearcherParameters[Form_Main.Randomiser.Next(eSearcherParameters.Count)];         break;
                    case "eHunter":        setObject.Parameters[2].Data = eHunterParameters[Form_Main.Randomiser.Next(eHunterParameters.Count)];             break;
                    case "eRounder":       setObject.Parameters[2].Data = eRounderParameters[Form_Main.Randomiser.Next(eRounderParameters.Count)];           break;
                    case "eCommander":     setObject.Parameters[2].Data = eCommanderParameters[Form_Main.Randomiser.Next(eCommanderParameters.Count)];       break;
                    case "eLiner":         setObject.Parameters[2].Data = eLinerParameters[Form_Main.Randomiser.Next(eLinerParameters.Count)];               break;
                    case "eChaser":        setObject.Parameters[2].Data = eChaserParameters[Form_Main.Randomiser.Next(eChaserParameters.Count)];             break;
                    case "eBomber":        setObject.Parameters[2].Data = eBomberParameters[Form_Main.Randomiser.Next(eBomberParameters.Count)];             break;
                    case "eArmor":         setObject.Parameters[2].Data = eArmorParameters[Form_Main.Randomiser.Next(eArmorParameters.Count)];               break;
                    case "eSweeper":       setObject.Parameters[2].Data = eSweeperParameters[Form_Main.Randomiser.Next(eSweeperParameters.Count)];           break;
                    case "eCannon":        setObject.Parameters[2].Data = eCannonParameters[Form_Main.Randomiser.Next(eCannonParameters.Count)];             break;
                    case "eWalker":        setObject.Parameters[2].Data = eWalkerParameters[Form_Main.Randomiser.Next(eWalkerParameters.Count)];             break;
                    case "eCannon(Fly)":   setObject.Parameters[2].Data = eCannonFlyParameters[Form_Main.Randomiser.Next(eCannonFlyParameters.Count)];       break;
                    case "eGuardian":      setObject.Parameters[2].Data = eGuardianParameters[Form_Main.Randomiser.Next(eGuardianParameters.Count)];         break;
                    case "eKeeper":        setObject.Parameters[2].Data = eKeeperParameters[Form_Main.Randomiser.Next(eKeeperParameters.Count)];             break;
                    case "cBiter":         setObject.Parameters[2].Data = cBiterParameters[Form_Main.Randomiser.Next(cBiterParameters.Count)];               break;
                    case "cStalker":       setObject.Parameters[2].Data = cStalkerParameters[Form_Main.Randomiser.Next(cStalkerParameters.Count)];           break;
                    case "cTaker":         setObject.Parameters[2].Data = cTakerParameters[Form_Main.Randomiser.Next(cTakerParameters.Count)];               break;
                    case "cTriker":        setObject.Parameters[2].Data = cTrickerParameters[Form_Main.Randomiser.Next(cTrickerParameters.Count)];           break;
                    case "cCrawler":       setObject.Parameters[2].Data = cCrawlerParameters[Form_Main.Randomiser.Next(cCrawlerParameters.Count)];           break;
                    case "cGazer":         setObject.Parameters[2].Data = cGazerParameters[Form_Main.Randomiser.Next(cGazerParameters.Count)];               break;
                    case "cGolem":         setObject.Parameters[2].Data = cGolemParameters[Form_Main.Randomiser.Next(cGolemParameters.Count)];               break;
                    case "cTitan":         setObject.Parameters[2].Data = cTitanParameters[Form_Main.Randomiser.Next(cTitanParameters.Count)];               break;
                    case "firstIblis":     setObject.Parameters[2].Data = "firstIblis";                                                                      break;
                    case "secondIblis":    setObject.Parameters[2].Data = secondIblisParameters[Form_Main.Randomiser.Next(secondIblisParameters.Count)];     break;
                    case "thirdIblis":     setObject.Parameters[2].Data = "thirdIblis";                                                                      break;
                    case "firstmefiress":  setObject.Parameters[2].Data = firstmefiressParameters[Form_Main.Randomiser.Next(firstmefiressParameters.Count)]; break;
                    case "secondmefiress": setObject.Parameters[2].Data = "secondmefiress_shadow";                                                           break;
                    case "kyozoress":      setObject.Parameters[2].Data = "kyozoress";                                                                       break;
                    case "eCerberus":      setObject.Parameters[2].Data = eCerberusParameters[Form_Main.Randomiser.Next(eCerberusParameters.Count)];         break;
                    case "eGenesis":       setObject.Parameters[2].Data = eGenesisParameters[Form_Main.Randomiser.Next(eGenesisParameters.Count)];           break;
                    case "eWyvern":        setObject.Parameters[2].Data = "eWyvern";                                                                         break;
                    case "solaris01":      setObject.Parameters[2].Data = "solaris01";                                                                       break;
                    case "solaris02":      setObject.Parameters[2].Data = "solaris02";                                                                       break;
                }
            }

            // Setup for if we're NOT enforcing the behaviour type.
            else
            {
                // Create a list of practically every parameter listed in ScriptParameter.bin
                List<string> Parameters = new() { "eGunner_Normal", "eGunner_Fix", "eGunner_Fix_Vulcan", "eGunner_Fix_Rocket", "eGunner_Wall_Fix", "eGunner_Chase", "eGunner_Trans", "eGunnerFly_Normal",
                                                  "eGunnerFly_Fix", "eGunnerFly_Fix_Vulcan", "eGunnerFly_Fix_Rocket", "eGunnerFly_Chase", "eGunnerFly_Homing", "eGunnerFly_Trans", 
                                                  "eGunnerFly_Boss_Vulcan", "eStinger_Normal", "eStinger_Fix", "eStinger_Fix_Missile", "eStinger_Chase", "eStinger_Wall_Fix", "eStinger_Trans",
                                                  "eStingerFly_Normal", "eStingerFly_Fix", "eStingerFly_Fix_Missile", "eStingerFly_Chase", "eStingerFly_Homing", "eStingerFly_Trans", "eLancer_Normal",
                                                  "eLancer_Fix", "eLancer_Chase", "eLancer_Wall_Fix", "eLancer_Trans", "eLancerFly_Normal", "eLancerFly_Fix", "eLancerFly_Fix_Laser", "eLancerFly_Chase",
                                                  "eLancerFly_Trans", "eBuster_Normal", "eBuster_Fix", "eBuster_Wall_Fix", "eBuster_Trans", "eBusterFly_Normal", "eBusterFly_Fix", "eBusterFly_Trans",
                                                  "eFlyer_Normal", "eFlyer_Fix", "eFlyer_Fix_Vulcan", "eFlyer_Fix_Rocket", "eFlyer_Homing", "eFlyer_Boss_Homing", "eFlyer_Boss_Vulcan", "eBluster_Normal",
                                                  "eBluster_Fix", "eBluster_Homing", "eSearcher_Normal", "eSearcher_Fix", "eSearcher_Fix_Rocket", "eSearcher_Fix_Bomb", "eSearcher_Alarm",
                                                  "eSearcher_Option", "eHunter_Normal", "eHunter_Fix", "eHunter_Hide_Fix", "eHunter_Option", "eRounder_Normal", "eRounder_Fix", "eRounder_Slave",
                                                  "eRounder_Twn_Chase", "eRounder_Twn_Escape", "eRounder_Boss", "eCommander_Normal", "eCommander_Fix", "eCommander_Master", "eCommander_Alarm",
                                                  "eLiner_Normal", "eLiner_Chase", "eLiner_Slave", "eChaser_Normal", "eChaser_Chase", "eChaser_Master", "eChaser_Alarm", "eBomber_Normal", "eBomber_Fix",
                                                  "eBomber_Wall_Normal", "eBomber_Wall_Fix", "eBomber_Allaround", "eBomber_Boss", "eArmor_Normal", "eArmor_Fix", "eSweeper_Fix", "eSweeper_Wall_Fix",
                                                  "eCannon_Normal", "eCannon_Fix", "eCannon_Fix_Laser", "eCannon_Fix_Launcher", "eCannon_Wall_Fix", "eCannon_Trans", "eWalker_Normal", "eWalker_Fix",
                                                  "eWalker_Wall_Fix", "eCannonFly_Normal", "eCannonFly_Fix", "eCannonFly_Trans", "eCannonFly_Carrier", "eGuardian_Normal", "eGuardian_Fix",
                                                  "eGuardian_Ball_Normal", "eGuardian_Ball_Fix", "eGuardian_Freeze", "eKeeper_Normal", "eKeeper_Fix", "eKeeper_Ball_Normal", "eKeeper_Ball_Fix",
                                                  "eKeeper_Freeze", "cBiter_Normal", "cBiter_Fix", "cBiter_Wall_Normal", "cBiter_Wall_Fix", "cBiter_Freeze", "cStalker_Normal", "cStalker_Fix",
                                                  "cStalker_Wall_Normal", "cStalker_Wall_Fix", "cStalker_Freeze", "cTaker_Normal", "cTaker_Fix", "cTaker_Normal_Bomb", "cTaker_Fix_Bomb",
                                                  "cTaker_Chase_Bomb", "cTaker_Homing", "cTricker_Normal", "cTricker_Fix", "cTricker_Homing", "cTricker_Master", "cTricker_Slave", "cTricker_Alarm",
                                                  "cCrawler_Normal", "cCrawler_Fix", "cCrawler_Wall_Fix", "cCrawler_Homing", "cCrawler_Wall_Homing", "cCrawler_Freeze", "cCrawler_Alarm", "cGazer_Normal",
                                                  "cGazer_Fix", "cGazer_Wall_Fix", "cGazer_Freeze", "cGazer_Alarm", "cGolem_Normal", "cGolem_Fix", "cGolem_Freeze", "cGolem_Alarm", "cTitan_Normal",
                                                  "cTitan_Fix", "cTitan_Freeze", "cTitan_Alarm", "firstIblis", "secondIblis_sonic", "secondIblis_shadow", "thirdIblis", "firstmefiress_shadow",
                                                  "firstmefiress_omega", "secondmefiress_shadow", "kyozoress", "eCerberus_sonic", "eCerberus_shadow", "eGenesis_sonic", "eGenesis_silver", "eGenesis_wing",
                                                  "eGenesisSpotLight", "eWyvern", "eWyvernOption", "eWyvernEggman", "solaris01", "solaris02" };
                
                // Pick a random parameter from the list to use on this enemy.
                setObject.Parameters[2].Data = Parameters[Form_Main.Randomiser.Next(Parameters.Count)];
            }
        }

        /// <summary>
        /// Randomises props that use values in Common.bin.
        /// </summary>
        /// <param name="setObject">The object we're editing.</param>
        /// <param name="commonPropTypes">The list of valid prop types.</param>
        static void CommonPropRandomiser(SetObject setObject, List<string> commonPropTypes)
        {
            // Standard physics props (physicspath seemed to crash the game a lot, so it's disabled for now).
            if (setObject.Type == "objectphysics" /*|| SetObject.Type == "physicspath" */|| setObject.Type == "objectphysics_item")
            {
                // Set a random prop type from the list.
                setObject.Parameters[0].Data = commonPropTypes[Form_Main.Randomiser.Next(commonPropTypes.Count)];

                // If we picked wap_confier (which is a standalone object), then change the object's type to it and remove its parameters.
                // Return here so we don't accidentaly undo the wap_confifer change.
                if (setObject.Parameters[0].Data.ToString() == "wap_conifer")
                {
                    setObject.Type = "wap_conifer";
                    setObject.Parameters.Clear();
                    return;
                }
            }

            // wap_confifer hack.
            // If this object is a wap_conifer object, then we change it into an objectphyiscs object instead.
            if (setObject.Type == "wap_conifer")
            {
                // Change the object type.
                setObject.Type = "objectphysics";

                // Create the parameters that objectphysics needs, the prop type from the list and the restart boolean.
                SetParameter setparam = new()
                {
                    Data = commonPropTypes[Form_Main.Randomiser.Next(commonPropTypes.Count)],
                    DataType = typeof(string)
                };
                setObject.Parameters.Add(setparam);
                setparam = new()
                {
                    Data = false,
                    DataType = typeof(bool)
                };
                setObject.Parameters.Add(setparam);

                // If we SOMEHOW picked wap_confier for this one, then just undo what we did.
                if (setObject.Parameters[0].Data.ToString() == "wap_confier")
                {
                    setObject.Type = "wap_conifer";
                    setObject.Parameters.Clear();
                }

                return;
            }

            // Red Solaris Eyes in End of the World
            if (setObject.Type == "end_outputwarp")
            {
                // Loop through 10 times, as this object has 10 different slots for prop types it can spawn, just make sure it can't be wap_conifer.
                for (int i = 0; i <= 9; i++)
                {
                    setObject.Parameters[i].Data = commonPropTypes[Form_Main.Randomiser.Next(commonPropTypes.Count)];
                    if (setObject.Parameters[i].Data.ToString() == "wap_conifer")
                    {
                        do { setObject.Parameters[i].Data = commonPropTypes[Form_Main.Randomiser.Next(commonPropTypes.Count)]; }
                        while (setObject.Parameters[i].Data.ToString() == "wap_conifer");
                    }
                }

            }
        }

        /// <summary>
        /// Randomises various tiny cosmetic elements of the SETs
        /// </summary>
        /// <param name="setObject">The object we're editing.</param>
        public static void CosmeticRandomiser(SetObject setObject)
        {
            switch (setObject.Type)
            {
                // Toss a coin and change the Aquatic Base glass doors to the other type.
                case "aqa_glass_blue":
                case "aqa_glass_red":
                    if (Form_Main.Randomiser.Next(0, 2) == 0)
                        setObject.Type = "aqa_glass_blue";
                    else
                        setObject.Type = "aqa_glass_red";
                    break;

                // Choose a different colour for the Chaos Emeralds.
                case "common_chaosemerald":
                    setObject.Parameters[0].Data = Form_Main.Randomiser.Next(1, 8);
                    break;

                // Choose a different character for player_npc points.
                case "player_npc":
                    setObject.Parameters[0].Data = Form_Main.Randomiser.Next(1, 15);
                    break;

                // Choose different appearances for town NPCs in Soleanna.
                // TODO: Maybe make this a seperate option?
                case "townsman":
                    // Change mantype, exempt value 17 (the shopkeeper, as changing them seems to break the shop)
                    if ((int)setObject.Parameters[1].Data != 17)
                        setObject.Parameters[1].Data = Form_Main.Randomiser.Next(0, 48);

                    // Change bodycolour.
                    setObject.Parameters[2].Data = Form_Main.Randomiser.Next(0, 3);

                    // Change haircolour.
                    setObject.Parameters[3].Data = Form_Main.Randomiser.Next(0, 3);

                    // Change manvariation
                    setObject.Parameters[10].Data = Form_Main.Randomiser.Next(0, 15);
                    break;

                // Choose a different colour for the posts used to indicate the Trials of Soleanna.
                case "trial_post":
                    setObject.Parameters[2].Data = Form_Main.Randomiser.Next(1, 4);
                    break;

                // Choose a different stage graphic for the Mirrors of Soleanna.
                case "warpgate":
                    setObject.Parameters[1].Data = Form_Main.Randomiser.Next(0, 10);

                    // Wave Ocean's mirror graphic is on slots 3, 9 AND 11. If we roll a 9, set it to 10 so we use End of the World's mirror instead.
                    if (setObject.Parameters[1].Data.ToString() == "9")
                        setObject.Parameters[1].Data = 10;
                    break;
            }
        }

        /// <summary>
        /// Patches various parts of the boss lua files for better usage in the randomiser.
        /// </summary>
        /// <param name="archivePath">The path to the already unpacked scripts.arc containing the boss lua binaries.</param>
        /// <param name="enemies">Whether or not enemy randomisation is enabled.</param>
        /// <param name="voices">Whether or not voice line randomisaiton is enabled.</param>
        /// <param name="voiceLines">The valid hint voice lines to use.</param>
        public static void BossPatch(string archivePath, bool enemies, bool voices, List<string> voiceLines)
        {
            // Get a list of all the lua binaries in the enemy folder of scripts.arc.
            string[] luaFiles = Directory.GetFiles($"{archivePath}\\xenon\\scripts\\enemy", "*.lub", SearchOption.TopDirectoryOnly);

            // Loop through all the lua binaries.
            foreach (string luaFile in luaFiles)
            {
                // Decompile this lua binary.
                System.Console.WriteLine($@"Patching '{luaFile}'.");
                LuaHandler.Decompile(luaFile);

                // Read the decompiled lua file into a string array.
                string[] lua = File.ReadAllLines(luaFile);

                // Loop through each line in this lua binary.
                for (int i = 0; i < lua.Length; i++)
                {
                    // If enemy randomisation is on, then comment out the lines that control the camera forcing, player movement forcing and Mephiles' random teleportations.
                    if((lua[i].Contains("CallSetCamera") || lua[i].Contains("CallMoveTargetPos") || lua[i].Contains("FirstMefiress_RandomWarp")) && enemies)
                        lua[i] = $"--{lua[i]}";

                    // If voice randomisation is on, then randomise the lua hint messages.
                    if(lua[i].Contains("CallHintMessage") && voices)
                    {
                        // Split the line controlling the hint up based on the quote marks around the hint name.
                        string[] split = lua[i].Split('\"');

                        // Replace the second value in the split array (the one containing the hint name) with a song from the list of valid hint voice lines.
                        split[1] = voiceLines[Form_Main.Randomiser.Next(voiceLines.Count)];

                        // Rejoin the split array into one line and add it back to the original lua array.
                        lua[i] = string.Join('\"', split);
                    }
                }

                // Save the updated lua binary.
                File.WriteAllLines(luaFile, lua);
            }
        }

        /// <summary>
        /// Replaces the player_start2s found in stage luas, while most stages overwrite these in SET, some (like bosses) do not.
        /// </summary>
        /// <param name="archivePath">The path to the already unpacked scripts.arc containing the stage lua binaries.</param>
        /// <param name="characterTypes">The valid character types to use.</param>
        /// <param name="characters">Whether to randomise player characters.</param>
        /// <param name="bosses">Whether to randomise boss characters.</param>
        public static void LuaPlayerStartRandomiser(string archivePath, List<string> characterTypes, bool characters, bool bosses)
        {
            // Make a list of the three boss player luas to pick from.
            List<string> BossCharacters = new() { "boss_sonic.lua", "boss_shadow.lua", "boss_silver.lua" };

            // Get a list of all the lua binaries in scripts.arc
            string[] luaFiles = Directory.GetFiles(archivePath, "*.lub", SearchOption.AllDirectories);

            // Loop through the lua binaries.
            foreach (string luaFile in luaFiles)
            {
                // Ignore any lua binaries that are not in a stage folder or don't contain a mission name.
                if (!luaFile.Contains("\\stage\\") && !luaFile.Contains("mission"))
                    continue;

                // Minimise the number of unnecessary lua decompilations.
                if (luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("a_")               || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("b_")               ||
                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("c_")               || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("d_")               ||
                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("e_")               || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("f_")               ||
                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("f1_")              || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("f2_")              ||
                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("g_")               || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("eCerberus")        ||
                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("eGenesis")         || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("eWyvern")          ||
                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("firstmefiress")    || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("iblis01")          ||
                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("secondiblis")      || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("secondmefiress")   ||
                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("shadow_vs_silver") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("silver_vs_shadow") ||
                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("solaris_super3")   || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("sonic_vs_silver")  ||
                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("thirdiblis"))
                {
                    // Decompile this lua binary.
                    System.Console.WriteLine($@"Randomising player spawns in in '{luaFile}'.");
                    LuaHandler.Decompile(luaFile);

                    // Read the decompiled lua file into a string array.
                    string[] lua = File.ReadAllLines(luaFile);

                    // Loop through each line in this lua binary.
                    for (int i = 0; i < lua.Length; i++)
                    {
                        // Search for the line that controls player spawns.
                        if (lua[i].Contains("Game.SetPlayer"))
                        {
                            // Check if we need to use a regular character for this line.
                            if (!lua[i].Contains("boss_") && characters)
                            {
                                // Split the line controlling the music playback up based on the quote marks around the song name.
                                string[] character = lua[i].Split('"');

                                // Replace the second value in the split array (the one containing the player lua name) with a selection from the characters list.
                                character[1] = $"{characterTypes[Form_Main.Randomiser.Next(characterTypes.Count)]}.lua";

                                // Rejoin the split array into one line and add it back to the original lua array.
                                lua[i] = string.Join("\"", character);
                            }
                            // Check if we need to use a boss for this line.
                            if (lua[i].Contains("boss_") && bosses)
                            {
                                // Split the line controlling the music playback up based on the quote marks around the song name.
                                string[] character = lua[i].Split('"');

                                // Replace the second value in the split array (the one containing the player lua name) with a selection from the three valid boss luas.
                                character[1] = BossCharacters[Form_Main.Randomiser.Next(BossCharacters.Count)];

                                // Rejoin the split array into one line and add it back to the original lua array.
                                lua[i] = string.Join("\"", character);
                            }
                        }
                    }

                    // Save the updated lua binary.
                    File.WriteAllLines(luaFile, lua);
                }
            }
        }
    }
}
