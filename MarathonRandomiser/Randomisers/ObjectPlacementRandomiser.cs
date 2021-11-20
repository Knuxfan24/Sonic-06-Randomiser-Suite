using Marathon.Formats.Placement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MarathonRandomiser
{
    internal class ObjectPlacementRandomiser
    {
        /// <summary>
        /// Process and randomise elements in a set file.
        /// </summary>
        /// <param name="setFile">The filepath to the set file we're processing.</param>
        /// <param name="enemies">Whether or not enemy types should be randomised.</param>
        /// <param name="enemiesNoBosses">Whether or not bosses should be excluded from enemy randomisation/</param>
        /// <param name="behaviour">Whether or not enemy behaviours should be randomised.</param>
        /// <param name="behaviourNoEnforce">Whether or not enemy behaviours can be from another enemy type.</param>
        /// <param name="characters">Whether or not player_start2 objects have their characters randomised.</param>
        /// <param name="itemCapsules">Whether or not the contents of Item Capsules are randomised.</param>
        /// <param name="commonProps">Whether or not props that pull from Common.bin should be randomised.</param>
        /// <param name="pathProps">Whether or not props that pull from PathObj.bin should be randomised.</param>
        /// <param name="hints">Whether or not voice lines should be randomised.</param>
        /// <param name="doors">Whether or not door types should be randomised.</param>
        /// <param name="drawDistance">Whether or not object draw distances should be randomised.</param>
        /// <param name="cosmetic">Whether or not various small cosmetic elements should be randomised.</param>
        /// <param name="SetEnemies">The list of valid enemies.</param>
        /// <param name="SetCharacters">The list of valid characters.</param>
        /// <param name="SetItemCapsules">The list of valid item types.</param>
        /// <param name="SetCommonProps">The list of valid Common.bin based props.</param>
        /// <param name="SetPathProps">The list of valid PathObj.bin based props.</param>
        /// <param name="SetHints">The list of valid hint voice lines.</param>
        /// <param name="SetDoors">The list of valid door types.</param>
        /// <param name="minDrawDistance">The minimum allowed draw distance.</param>
        /// <param name="maxDrawDistance">The maximum allowed draw distance.</param>
        /// <returns></returns>
        public static async Task Process(string setFile, bool? enemies, bool? enemiesNoBosses, bool? behaviour, bool? behaviourNoEnforce, bool? characters, bool? itemCapsules, bool? commonProps,
                                      bool? pathProps, bool? hints, bool? doors, bool? drawDistance, bool? cosmetic, List<string> SetEnemies, List<string> SetCharacters, List<string> SetItemCapsules,
                                      List<string> SetCommonProps, List<string> SetPathProps, List<string> SetHints, List<string> SetDoors, int minDrawDistance, int maxDrawDistance)
        {
            // Load this set file.
            using ObjectPlacement set = new(setFile);

            // Loop through all the objects in this set file.
            foreach (SetObject setObject in set.Data.Objects)
            {
                // If we're randomising the object's draw distance, then pick a number for it between the specified values
                if (drawDistance == true)
                    setObject.DrawDistance = MainWindow.Randomiser.Next(minDrawDistance, maxDrawDistance + 1);

                // If we're randomising the cosmetic stuff, then pass this object to that function to see if we have to do anything to it.
                if (cosmetic == true)
                    await Task.Run(() => CosmeticRandomiser(setObject));

                // Check this object's type to see if we need to do anything with it.
                switch (setObject.Type)
                {
                    // Randomise enemy types and/or their behaviours if we need to.
                    case "enemy":
                    case "enemyextra":
                        if (enemies == true)
                            await Task.Run(() => EnemyTypeRandomiser(setObject, SetEnemies, enemiesNoBosses));
                        if (behaviour == true)
                            await Task.Run(() => EnemyBehaviourRandomiser(setObject, behaviourNoEnforce));
                        break;

                    // Randomise character types if we need to.
                    case "player_start2":
                        if (characters == true)
                            setObject.Parameters[1].Data = SetCharacters[MainWindow.Randomiser.Next(SetCharacters.Count)];
                        break;

                    // Randomise item capsule contents if we need to.
                    case "itemboxg":
                    case "itemboxa":
                        if (itemCapsules == true)
                            setObject.Parameters[0].Data = int.Parse(SetItemCapsules[MainWindow.Randomiser.Next(SetItemCapsules.Count)]);
                        break;

                    // Randomise prop elements if we need to.
                    case "objectphysics":
                    case "physicspath":
                    case "objectphysics_item":
                    case "wap_conifer":
                    case "end_outputwarp":
                        if (commonProps == true)
                            await Task.Run(() => CommonPropRandomiser(setObject, SetCommonProps));
                        break;
                    case "common_path_obj":
                        if (pathProps == true)
                            setObject.Parameters[0].Data = SetPathProps[MainWindow.Randomiser.Next(SetPathProps.Count)];
                        break;

                    // Randomise voice line triggers if we need to.
                    case "common_hint":
                    case "common_hint_collision":
                        if (hints == true)
                            setObject.Parameters[0].Data = SetHints[MainWindow.Randomiser.Next(SetHints.Count)];
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
                        if (doors == true)
                            setObject.Type = SetDoors[MainWindow.Randomiser.Next(SetDoors.Count)];
                        break;
                }

            }

            // Save the updated set file.
            set.Save();
        }

        /// <summary>
        /// Randomises the type of enemy to spawn in this object.
        /// </summary>
        /// <param name="setObject">The object we're editing.</param>
        /// <param name="enemyTypes">The list of valid enemy types.</param>
        static async Task EnemyTypeRandomiser(SetObject setObject, List<string> enemyTypes, bool? enemiesNoBosses)
        {
            // If this object is a boss but the user has disallowed boss randomisation, then don't change anything.
            if (enemiesNoBosses == true && (setObject.Parameters[0].Data.ToString() == "eCerberus" || setObject.Parameters[0].Data.ToString() == "eGenesis" ||
                setObject.Parameters[0].Data.ToString() == "eWyvern" || setObject.Parameters[0].Data.ToString() == "firstiblis" || setObject.Parameters[0].Data.ToString() == "secondiblis" ||
                setObject.Parameters[0].Data.ToString() == "thirdiblis" || setObject.Parameters[0].Data.ToString() == "firstmefiress" || setObject.Parameters[0].Data.ToString() == "secondmefiress" ||
                setObject.Parameters[0].Data.ToString() == "solaris01" || setObject.Parameters[0].Data.ToString() == "solaris02"))
                return;

            // Set the type of enemy from the list of types.
            setObject.Parameters[0].Data = enemyTypes[MainWindow.Randomiser.Next(enemyTypes.Count)];

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
            if (enemiesNoBosses == false && setObject.Parameters[0].Data.ToString() == "eCerberus")
                setObject.Parameters[1].Data = MainWindow.Randomiser.Next(0, 2);
        }

        /// <summary>
        /// Randomises the behaviour of the enemy in this object.
        /// </summary>
        /// <param name="setObject">The object we're editing.</param>
        /// <param name="dontEnforceBehaviours">Whether we should ensure that the chosen behaviour belongs to this enemy type.</param>
        static async Task EnemyBehaviourRandomiser(SetObject setObject, bool? dontEnforceBehaviours)
        {
            // Setup for if we are enforcing the behaviour type.
            if (dontEnforceBehaviours == false)
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
                    case "eGunner":        setObject.Parameters[2].Data = eGunnerParameters[MainWindow.Randomiser.Next(eGunnerParameters.Count)];             break;
                    case "eGunner(Fly)":   setObject.Parameters[2].Data = eGunnerFlyParameters[MainWindow.Randomiser.Next(eGunnerFlyParameters.Count)];       break;
                    case "eStinger":       setObject.Parameters[2].Data = eStingerParameters[MainWindow.Randomiser.Next(eStingerParameters.Count)];           break;
                    case "eStinger(Fly)":  setObject.Parameters[2].Data = eStingerFlyParameters[MainWindow.Randomiser.Next(eStingerFlyParameters.Count)];     break;
                    case "eLancer":        setObject.Parameters[2].Data = eLancerParameters[MainWindow.Randomiser.Next(eLancerParameters.Count)];             break;
                    case "eLancer(Fly)":   setObject.Parameters[2].Data = eLancerFlyParameters[MainWindow.Randomiser.Next(eLancerFlyParameters.Count)];       break;
                    case "eBuster":        setObject.Parameters[2].Data = eBusterParameters[MainWindow.Randomiser.Next(eBusterParameters.Count)];             break;
                    case "eBuster(Fly)":   setObject.Parameters[2].Data = eBusterFlyParameters[MainWindow.Randomiser.Next(eBusterFlyParameters.Count)];       break;
                    case "eFlyer":         setObject.Parameters[2].Data = eFlyerParameters[MainWindow.Randomiser.Next(eFlyerParameters.Count)];               break;
                    case "eBluster":       setObject.Parameters[2].Data = eBlusterParameters[MainWindow.Randomiser.Next(eBlusterParameters.Count)];           break;
                    case "eSearcher":      setObject.Parameters[2].Data = eSearcherParameters[MainWindow.Randomiser.Next(eSearcherParameters.Count)];         break;
                    case "eHunter":        setObject.Parameters[2].Data = eHunterParameters[MainWindow.Randomiser.Next(eHunterParameters.Count)];             break;
                    case "eRounder":       setObject.Parameters[2].Data = eRounderParameters[MainWindow.Randomiser.Next(eRounderParameters.Count)];           break;
                    case "eCommander":     setObject.Parameters[2].Data = eCommanderParameters[MainWindow.Randomiser.Next(eCommanderParameters.Count)];       break;
                    case "eLiner":         setObject.Parameters[2].Data = eLinerParameters[MainWindow.Randomiser.Next(eLinerParameters.Count)];               break;
                    case "eChaser":        setObject.Parameters[2].Data = eChaserParameters[MainWindow.Randomiser.Next(eChaserParameters.Count)];             break;
                    case "eBomber":        setObject.Parameters[2].Data = eBomberParameters[MainWindow.Randomiser.Next(eBomberParameters.Count)];             break;
                    case "eArmor":         setObject.Parameters[2].Data = eArmorParameters[MainWindow.Randomiser.Next(eArmorParameters.Count)];               break;
                    case "eSweeper":       setObject.Parameters[2].Data = eSweeperParameters[MainWindow.Randomiser.Next(eSweeperParameters.Count)];           break;
                    case "eCannon":        setObject.Parameters[2].Data = eCannonParameters[MainWindow.Randomiser.Next(eCannonParameters.Count)];             break;
                    case "eWalker":        setObject.Parameters[2].Data = eWalkerParameters[MainWindow.Randomiser.Next(eWalkerParameters.Count)];             break;
                    case "eCannon(Fly)":   setObject.Parameters[2].Data = eCannonFlyParameters[MainWindow.Randomiser.Next(eCannonFlyParameters.Count)];       break;
                    case "eGuardian":      setObject.Parameters[2].Data = eGuardianParameters[MainWindow.Randomiser.Next(eGuardianParameters.Count)];         break;
                    case "eKeeper":        setObject.Parameters[2].Data = eKeeperParameters[MainWindow.Randomiser.Next(eKeeperParameters.Count)];             break;
                    case "cBiter":         setObject.Parameters[2].Data = cBiterParameters[MainWindow.Randomiser.Next(cBiterParameters.Count)];               break;
                    case "cStalker":       setObject.Parameters[2].Data = cStalkerParameters[MainWindow.Randomiser.Next(cStalkerParameters.Count)];           break;
                    case "cTaker":         setObject.Parameters[2].Data = cTakerParameters[MainWindow.Randomiser.Next(cTakerParameters.Count)];               break;
                    case "cTricker":       setObject.Parameters[2].Data = cTrickerParameters[MainWindow.Randomiser.Next(cTrickerParameters.Count)];           break;
                    case "cCrawler":       setObject.Parameters[2].Data = cCrawlerParameters[MainWindow.Randomiser.Next(cCrawlerParameters.Count)];           break;
                    case "cGazer":         setObject.Parameters[2].Data = cGazerParameters[MainWindow.Randomiser.Next(cGazerParameters.Count)];               break;
                    case "cGolem":         setObject.Parameters[2].Data = cGolemParameters[MainWindow.Randomiser.Next(cGolemParameters.Count)];               break;
                    case "cTitan":         setObject.Parameters[2].Data = cTitanParameters[MainWindow.Randomiser.Next(cTitanParameters.Count)];               break;
                    case "firstIblis":     setObject.Parameters[2].Data = "firstIblis";                                                                       break;
                    case "secondIblis":    setObject.Parameters[2].Data = secondIblisParameters[MainWindow.Randomiser.Next(secondIblisParameters.Count)];     break;
                    case "thirdIblis":     setObject.Parameters[2].Data = "thirdIblis";                                                                       break;
                    case "firstmefiress":  setObject.Parameters[2].Data = firstmefiressParameters[MainWindow.Randomiser.Next(firstmefiressParameters.Count)]; break;
                    case "secondmefiress": setObject.Parameters[2].Data = "secondmefiress_shadow";                                                            break;
                    case "kyozoress":      setObject.Parameters[2].Data = "kyozoress";                                                                        break;
                    case "eCerberus":      setObject.Parameters[2].Data = eCerberusParameters[MainWindow.Randomiser.Next(eCerberusParameters.Count)];         break;
                    case "eGenesis":       setObject.Parameters[2].Data = eGenesisParameters[MainWindow.Randomiser.Next(eGenesisParameters.Count)];           break;
                    case "eWyvern":        setObject.Parameters[2].Data = "eWyvern";                                                                          break;
                    case "solaris01":      setObject.Parameters[2].Data = "solaris01";                                                                        break;
                    case "solaris02":      setObject.Parameters[2].Data = "solaris02";                                                                        break;
                    default:               throw new Exception($"Couldn't find anything for enemy type {setObject.Parameters[0].Data}.");
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
                setObject.Parameters[2].Data = Parameters[MainWindow.Randomiser.Next(Parameters.Count)];
            }
        }

        /// <summary>
        /// Randomises props that use values in Common.bin.
        /// </summary>
        /// <param name="setObject">The object we're editing.</param>
        /// <param name="commonPropTypes">The list of valid prop types.</param>
        static async Task CommonPropRandomiser(SetObject setObject, List<string> commonPropTypes)
        {
            // Standard physics props (physicspath seemed to crash the game a lot, so it's disabled for now).
            if (setObject.Type == "objectphysics" /*|| SetObject.Type == "physicspath"*/ || setObject.Type == "objectphysics_item")
            {
                // Set a random prop type from the list.
                setObject.Parameters[0].Data = commonPropTypes[MainWindow.Randomiser.Next(commonPropTypes.Count)];

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
                // Roll the prop type here, as there's no point changing it, only to find we made it a wap_confier and have to undo the change.
                string newProp = commonPropTypes[MainWindow.Randomiser.Next(commonPropTypes.Count)];
                
                // If we wouldn't just be making it back into a wap_confier, then proceed.
                if (newProp != "wap_confier")
                {
                    // Change the object type.
                    setObject.Type = "objectphysics";

                    // Create the parameters that objectphysics needs, the prop type from the list and the restart boolean.
                    SetParameter setparam = new()
                    {
                        Data = newProp,
                        Type = ObjectDataType.String
                    };
                    setObject.Parameters.Add(setparam);
                    setparam = new()
                    {
                        Data = false,
                        Type = ObjectDataType.Boolean
                    };
                    setObject.Parameters.Add(setparam);
                }
            }

            // Red Solaris Eyes in End of the World
            if (setObject.Type == "end_outputwarp")
            {
                // Loop through 10 times, as this object has 10 different slots for prop types it can spawn, just make sure it can't be wap_conifer.
                for (int i = 0; i <= 9; i++)
                {
                    setObject.Parameters[i].Data = commonPropTypes[MainWindow.Randomiser.Next(commonPropTypes.Count)];
                    if (setObject.Parameters[i].Data.ToString() == "wap_conifer")
                    {
                        do { setObject.Parameters[i].Data = commonPropTypes[MainWindow.Randomiser.Next(commonPropTypes.Count)]; }
                        while (setObject.Parameters[i].Data.ToString() == "wap_conifer");
                    }
                }

            }
        }

        /// <summary>
        /// Randomises various tiny cosmetic elements of the SETs
        /// </summary>
        /// <param name="setObject">The object we're editing.</param>
        static async Task CosmeticRandomiser(SetObject setObject)
        {
            switch (setObject.Type)
            {
                // Toss a coin and change the Aquatic Base glass doors to the other type.
                case "aqa_glass_blue":
                case "aqa_glass_red":
                    if (MainWindow.Randomiser.Next(0, 2) == 0)
                        setObject.Type = "aqa_glass_blue";
                    else
                        setObject.Type = "aqa_glass_red";
                    break;

                // Choose a different colour for the Chaos Emeralds.
                case "common_chaosemerald":
                    setObject.Parameters[0].Data = MainWindow.Randomiser.Next(1, 8);
                    break;

                // Choose a different character for player_npc points.
                case "player_npc":
                    setObject.Parameters[0].Data = MainWindow.Randomiser.Next(1, 15);
                    break;

                // Choose different appearances for town NPCs in Soleanna.
                // TODO: Maybe make this a seperate option?
                case "townsman":
                    // Change mantype, exempt value 17 (the shopkeeper, as changing them seems to break the shop)
                    if ((int)setObject.Parameters[1].Data != 17)
                        setObject.Parameters[1].Data = MainWindow.Randomiser.Next(0, 48);

                    // Change bodycolour.
                    setObject.Parameters[2].Data = MainWindow.Randomiser.Next(0, 3);

                    // Change haircolour.
                    setObject.Parameters[3].Data = MainWindow.Randomiser.Next(0, 3);

                    // Change manvariation
                    setObject.Parameters[10].Data = MainWindow.Randomiser.Next(0, 15);
                    break;

                // Choose a different colour for the posts used to indicate the Trials of Soleanna.
                case "trial_post":
                    setObject.Parameters[2].Data = MainWindow.Randomiser.Next(1, 4);
                    break;

                // Choose a different stage graphic for the Mirrors of Soleanna.
                case "warpgate":
                    setObject.Parameters[1].Data = MainWindow.Randomiser.Next(1, 11);

                    // Wave Ocean's mirror graphic is on slots 4, 10 AND 12. If we roll a 10, set it to 11 so we use End of the World's mirror instead.
                    if (setObject.Parameters[1].Data.ToString() == "10")
                        setObject.Parameters[1].Data = 11;
                    break;
            }
        }

        /// <summary>
        /// Patch boss scripts (also includes enemy scripts because hmm) so they don't screw with the camera if randomised into regular enemies.
        /// Also used to change the voice lines used during the fights.
        /// </summary>
        /// <param name="luaFile">The lua binary we're processing.</param>
        /// <param name="enemies">Whether enemy randomisation is on.</param>
        /// <param name="hints">Whether hint randomisation is on.</param>
        /// <param name="SetHints">The list of valid hint voice lines</param>
        public static async Task BossPatch(string luaFile, bool? enemies, bool? hints, List<string> SetHints, List<string> SetEnemies)
        {
            // Decompile this luaFile.
            await Task.Run(() => Helpers.LuaDecompile(luaFile));

            // Read the decompiled lua file into a string array.
            string[] lua = File.ReadAllLines(luaFile);

            // Loop through each line in this lua binary.
            for (int i = 0; i < lua.Length; i++)
            {
                // If enemy randomisation is on, then comment out the lines that control the camera forcing, player movement forcing and Mephiles' random teleportations.
                if ((lua[i].Contains("CallSetCamera") || lua[i].Contains("CallMoveTargetPos") || lua[i].Contains("FirstMefiress_RandomWarp")) && enemies == true && SetEnemies.Contains(Path.GetFileNameWithoutExtension(luaFile)))
                    lua[i] = $"--{lua[i]}";

                // If voice randomisation is on, then randomise the lua hint messages.
                if (lua[i].Contains("CallHintMessage") && hints == true)
                {
                    // Split the line controlling the hint up based on the quote marks around the hint name.
                    string[] split = lua[i].Split('\"');

                    // Replace the second value in the split array (the one containing the hint name) with a song from the list of valid hint voice lines.
                    split[1] = SetHints[MainWindow.Randomiser.Next(SetHints.Count)];

                    // Rejoin the split array into one line and add it back to the original lua array.
                    lua[i] = string.Join('\"', split);
                }
            }

            // Save the updated lua binary.
            File.WriteAllLines(luaFile, lua);
        }

        /// <summary>
        /// Randomises player_start2 values set in Lua, used for a few stages and character battles.
        /// </summary>
        /// <param name="luaFile">The lua binary we're processing.</param>
        /// <param name="characters">Whether character randomisation is on.</param>
        /// <param name="SetCharacters">The list of valid characters.</param>
        /// <param name="enemies">Whether enemy randomisation is on.</param>
        /// <param name="enemiesNoBosses">Whether we're randomising bosses or not.</param>
        /// <returns></returns>
        public static async Task LuaPlayerStartRandomiser(string luaFile, bool? characters, List<string> SetCharacters, bool? enemies, bool? enemiesNoBosses)
        {
            // Make a list of the three boss player luas to pick from.
            List<string> BossCharacters = new() { "boss_sonic.lua", "boss_shadow.lua", "boss_silver.lua" };

            // Decompile this lua binary.
            await Task.Run(() => Helpers.LuaDecompile(luaFile));

            // Read the decompiled lua file into a string array.
            string[] lua = File.ReadAllLines(luaFile);

            // Loop through each line in this lua binary.
            for (int i = 0; i < lua.Length; i++)
            {
                // Search for the line that controls player spawns.
                if (lua[i].Contains("Game.SetPlayer"))
                {
                    // Check if we need to use a regular character for this line.
                    if (!lua[i].Contains("boss_") && characters == true)
                    {
                        // Split the line controlling the character type based on the quote marks around the lua name.
                        string[] character = lua[i].Split('"');

                        // Replace the second value in the split array (the one containing the player lua name) with a selection from the characters list.
                        character[1] = $"{SetCharacters[MainWindow.Randomiser.Next(SetCharacters.Count)]}.lua";

                        // Rejoin the split array into one line and add it back to the original lua array.
                        lua[i] = string.Join("\"", character);
                    }

                    // Check if we need to use a boss for this line.
                    if (lua[i].Contains("boss_") && enemies == true && enemiesNoBosses == false)
                    {
                        // Split the line controlling the character type based on the quote marks around the lua name.
                        string[] character = lua[i].Split('"');

                        // Replace the second value in the split array (the one containing the player lua name) with a selection from the three valid boss luas.
                        character[1] = BossCharacters[MainWindow.Randomiser.Next(BossCharacters.Count)];

                        // Rejoin the split array into one line and add it back to the original lua array.
                        lua[i] = string.Join("\"", character);
                    }
                }

                // Save the updated lua binary.
                File.WriteAllLines(luaFile, lua);
            }
        }
    }
}
