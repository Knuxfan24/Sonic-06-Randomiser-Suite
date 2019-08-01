using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HedgeLib.Sets;

namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    class SetRandomisation
    {
        static public void EnemyRandomiser(string filepath, Random rng)
        {
            S06SetData setTarget = new S06SetData();

            List<string> cBiterParams = new List<string> { "cBiter_Fix", "cBiter_Freeze", "cBiter_Normal", "cBiter_Wall_Fix", "cBiter_Wall_Normal" };
            List<string> cCrawlerParams = new List<string> { "cCrawler_Alarm", "cCrawler_Fix", "cCrawler_Freeze", "cCrawler_Normal", "cCrawler_Wall_Fix", "cCrawler_Wall_Homing" };
            List<string> cGazerParams = new List<string> { "cGazer_Alarm", "cGazer_Fix", "cGazer_Freeze", "cGazer_Normal", "cGazer_Wall_Fix" };
            List<string> cGolemParams = new List<string> { "cGolem_Alarm", "cGolem_Fix", "cGolem_Freeze", "cGolem_Normal" };
            List<string> cStalkerParams = new List<string> { "cStalker_Fix", "cStalker_Freeze", "cStalker_Normal", "cStalker_Wall_Fix", "cStalker_Wall_Normal" };
            List<string> cTakerParams = new List<string> { "cTaker_Chase_Bomb", "cTaker_Fix", "cTaker_Fix_Bomb", "cTaker_Homing", "cTaker_Normal", "cTaker_Normal_Bomb" };
            List<string> cTitanParams = new List<string> { "cTitan_Alarm", "cTitan_Fix", "cTitan_Freeze", "cTitan_Normal" };
            List<string> cTrickerParams = new List<string> { "cTricker_Alarm", "cTricker_Fix", "cTricker_Homing", "cTricker_Master", "cTricker_Normal", "cTricker_Slave" };
            List<string> eArmorParams = new List<string> { "eArmor_Fix", "eArmor_Normal" };
            List<string> eBlusterParams = new List<string> { "eBluster_Fix", "eBluster_Homing", "eBluster_Normal"};
            List<string> eBomberParams = new List<string> { "eBomber_Allaround", "eBomber_Boss", "eBomber_Fix", "eBomber_Normal", "eBomber_Wall_Fix", "eBomber_Wall_Normal" };
            List<string> eBusterParams = new List<string> { "eBuster_Fix", "eBuster_Normal", "eBuster_Trans" };
            List<string> eBusterFlyParams = new List<string> { "eBusterFly_Fix", "eBusterFly_Normal", "eBusterFly_Trans" };
            List<string> eCannonParams = new List<string> { "eCannon_Fix", "eCannon_Fix_Laser", "eCannon_Fix_Launcher", "eCannon_Normal", "eCannon_Trans", "eCannon_Wall_Fix" };
            List<string> eCannonFlyParams = new List<string> { "eCannonFly_Carrier", "eCannonFly_Fix", "eCannonFly_Normal", "eCannonFly_Trans" };
            List<string> eChaserParams = new List<string> { "eChaser_Alarm", "eChaser_Chase", "eChaser_Master", "eChaser_Normal" };
            List<string> eCommanderParams = new List<string> { "eCommander_Alarm", "eCommander_Fix", "eCommander_Master", "eCommander_Normal" };
            List<string> eFlyerParams = new List<string> { "eFlyer_Boss_Homing", "eFlyer_Boss_Vulcan", "eFlyer_Fix", "eFlyer_Fix_Rocket", "eFlyer_Fix_Vulcan", "eFlyer_Homing", "eFlyer_Normal" };
            List<string> eGuardianParams = new List<string> { "eGuardian_Ball_Fix", "eGuardian_Ball_Normal", "eGuardian_Fix", "eGuardian_Freeze", "eGuardian_Normal" };
            List<string> eGunnerParams = new List<string> { "eGunner_Chase", "eGunner_Fix", "eGunner_Fix_Rocket", "eGunner_Fix_Vulcan", "eGunner_Normal", "eGunner_Trans" };
            List<string> eGunnerFlyParams = new List<string> { "eGunnerFly_Chase", "eGunnerFly_Fix", "eGunnerFly_Fix_Rocket", "eGunnerFly_Fix_Vulcan", "eGunnerFly_Homing", "eGunnerFly_Normal", "eGunnerFly_Trans" };
            List<string> eHunterParams = new List<string> { "eHunter_Fix", "eHunter_Hide_Fix", "eHunter_Normal" };
            List<string> eKeeperParams = new List<string> { "eKeeper_Ball_Fix", "eKeeper_Fix", "eKeeper_Freeze", "eKeeper_Normal" };
            List<string> eLancerParams = new List<string> { "eLancer_Fix", "eLancer_Normal", "eLancer_Trans" };
            List<string> eLancerFlyParams = new List<string> { "eLancerFly_Chase", "eLancerFly_Fix", "eLancerFly_Fix_Laser", "eLancerFly_Normal", "eLancerFly_Trans" };
            List<string> eLinerParams = new List<string> { "eLiner_Chase", "eLiner_Normal", "eLiner_Slave" };
            List<string> eRounderParams = new List<string> { "eRounder_Fix", "eRounder_Normal", "eRounder_Slave", "eRounder_Twn_Chase", "eRounder_Twn_Escape" };
            List<string> eSearcherParams = new List<string> { "eSearcher_Alarm", "eSearcher_Fix", "eSearcher_Fix_Bomb", "eSearcher_Fix_Rocket", "eSearcher_Normal" };
            List<string> eStingerParams = new List<string> { "eStinger_Chase", "eStinger_Fix", "eStinger_Fix_Missile", "eStinger_Normal", "eStinger_Trans" };
            List<string> eStingerFlyParams = new List<string> { "eStingerFly_Chase", "eStingerFly_Fix", "eStingerFly_Fix_Missile", "eStingerFly_Homing", "eStingerFly_Normal", "eStingerFly_Trans" };
            List<string> eSweeperParams = new List<string> { "eSweeper_Fix", "eSweeper_Wall_Fix" };
            List<string> eWalkerParams = new List<string> { "eWalker_Fix", "eWalker_Normal" };

            List<string> eCerberusParams = new List<string> { "eCerberus_shadow", "eCerberus_sonic" };
            List<string> eGenesisParams = new List<string> { "eGenesis_silver", "eGenesis_sonic" };
            List<string> eWyvernParams = new List<string> { "eWyvern" };
            List<string> firstIblisParams = new List<string> { "firstIblis" };
            List<string> secondIblisParams = new List<string> { "secondIblis_shadow", "secondIblis_sonic" };
            List<string> thirdIblisParams = new List<string> { "thirdIblis" };
            List<string> firstmefiressParams = new List<string> { "firstmefiress_omega", "firstmefiress_shadow" };
            List<string> solaris01Params = new List<string> { "solaris01" };
            List<string> solaris02Params = new List<string> { "solaris02" };

            int index;
            
            setTarget.Load(filepath);
            foreach (SetObject s06Object in setTarget.Objects)
            {
                if (s06Object.ObjectType == "enemy" || s06Object.ObjectType == "enemyextra")
                {
                    //Enemy Type
                    index = rng.Next(SetRandomisationForm.validEnemies.Count);
                    s06Object.Parameters[0].Data = SetRandomisationForm.validEnemies[index];

                    //Enemy Pallete
                    s06Object.Parameters[1].Data = 0;

                    if (s06Object.Parameters[0].Data.ToString() == "cGazer" || s06Object.Parameters[0].Data.ToString() == "cStalker" || s06Object.Parameters[0].Data.ToString() == "cTitan"
                    || s06Object.Parameters[0].Data.ToString() == "cTricker" || s06Object.Parameters[0].Data.ToString() == "eArmor" || s06Object.Parameters[0].Data.ToString() == "eBluster"
                    || s06Object.Parameters[0].Data.ToString() == "eChaser" || s06Object.Parameters[0].Data.ToString() == "eCommander" || s06Object.Parameters[0].Data.ToString() == "eHunter"
                    || s06Object.Parameters[0].Data.ToString() == "eKeeper" || s06Object.Parameters[0].Data.ToString() == "eStinger" || s06Object.Parameters[0].Data.ToString() == "eStinger(Fly)"
                    || s06Object.Parameters[0].Data.ToString() == "eWalker" || s06Object.Parameters[0].Data.ToString() == "solaris02") { s06Object.Parameters[1].Data = 1; }

                    if (s06Object.Parameters[0].Data.ToString() == "eLancer" || s06Object.Parameters[0].Data.ToString() == "eLancer(Fly)") { s06Object.Parameters[1].Data = 2; }

                    if (s06Object.Parameters[0].Data.ToString() == "eBuster" || s06Object.Parameters[0].Data.ToString() == "eBuster(Fly)") { s06Object.Parameters[1].Data = 3; }

                    //Enemy Parameter
                    switch (SetRandomisationForm.validEnemies[index])
                    {
                        case "cBiter":
                            index = rng.Next(cBiterParams.Count);
                            s06Object.Parameters[2].Data = cBiterParams[index];
                            break;
                        case "cCrawler":
                            index = rng.Next(cCrawlerParams.Count);
                            s06Object.Parameters[2].Data = cCrawlerParams[index];
                            break;
                        case "cGazer":
                            index = rng.Next(cGazerParams.Count);
                            s06Object.Parameters[2].Data = cGazerParams[index];
                            break;
                        case "cGolem":
                            index = rng.Next(cGolemParams.Count);
                            s06Object.Parameters[2].Data = cGolemParams[index];
                            break;
                        case "cStalker":
                            index = rng.Next(cStalkerParams.Count);
                            s06Object.Parameters[2].Data = cStalkerParams[index];
                            break;
                        case "cTaker":
                            index = rng.Next(cTakerParams.Count);
                            s06Object.Parameters[2].Data = cTakerParams[index];
                            break;
                        case "cTitan":
                            index = rng.Next(cTitanParams.Count);
                            s06Object.Parameters[2].Data = cTitanParams[index];
                            break;
                        case "cTricker":
                            index = rng.Next(cTrickerParams.Count);
                            s06Object.Parameters[2].Data = cTrickerParams[index];
                            break;
                        case "eArmor":
                            index = rng.Next(eArmorParams.Count);
                            s06Object.Parameters[2].Data = eArmorParams[index];
                            break;
                        case "eBluster":
                            index = rng.Next(eBlusterParams.Count);
                            s06Object.Parameters[2].Data = eBlusterParams[index];
                            break;
                        case "eBomber":
                            index = rng.Next(eBomberParams.Count);
                            s06Object.Parameters[2].Data = eBomberParams[index];
                            break;
                        case "eBuster":
                            index = rng.Next(eBusterParams.Count);
                            s06Object.Parameters[2].Data = eBusterParams[index];
                            break;
                        case "eBuster(Fly)":
                            index = rng.Next(eBusterFlyParams.Count);
                            s06Object.Parameters[2].Data = eBusterFlyParams[index];
                            break;
                        case "eCannon":
                            index = rng.Next(eCannonParams.Count);
                            s06Object.Parameters[2].Data = eCannonParams[index];
                            break;
                        case "eCannon(Fly)":
                            index = rng.Next(eCannonFlyParams.Count);
                            s06Object.Parameters[2].Data = eCannonFlyParams[index];
                            break;
                        case "eChaser":
                            index = rng.Next(eChaserParams.Count);
                            s06Object.Parameters[2].Data = eChaserParams[index];
                            break;
                        case "eCommander":
                            index = rng.Next(eCommanderParams.Count);
                            s06Object.Parameters[2].Data = eCommanderParams[index];
                            break;
                        case "eFlyer":
                            index = rng.Next(eFlyerParams.Count);
                            s06Object.Parameters[2].Data = eFlyerParams[index];
                            break;
                        case "eGuardian":
                            index = rng.Next(eGuardianParams.Count);
                            s06Object.Parameters[2].Data = eGuardianParams[index];
                            break;
                        case "eGunner":
                            index = rng.Next(eGunnerParams.Count);
                            s06Object.Parameters[2].Data = eGunnerParams[index];
                            break;
                        case "eGunner(Fly)":
                            index = rng.Next(eGunnerFlyParams.Count);
                            s06Object.Parameters[2].Data = eGunnerFlyParams[index];
                            break;
                        case "eHunter":
                            index = rng.Next(eHunterParams.Count);
                            s06Object.Parameters[2].Data = eHunterParams[index];
                            break;
                        case "eKeeper":
                            index = rng.Next(eKeeperParams.Count);
                            s06Object.Parameters[2].Data = eKeeperParams[index];
                            break;
                        case "eLancer":
                            index = rng.Next(eLancerParams.Count);
                            s06Object.Parameters[2].Data = eLancerParams[index];
                            break;
                        case "eLancer(Fly)":
                            index = rng.Next(eLancerFlyParams.Count);
                            s06Object.Parameters[2].Data = eLancerFlyParams[index];
                            break;
                        case "eLiner":
                            index = rng.Next(eLinerParams.Count);
                            s06Object.Parameters[2].Data = eLinerParams[index];
                            break;
                        case "eRounder":
                            index = rng.Next(eRounderParams.Count);
                            s06Object.Parameters[2].Data = eRounderParams[index];
                            break;
                        case "eSearcher":
                            index = rng.Next(eSearcherParams.Count);
                            s06Object.Parameters[2].Data = eSearcherParams[index];
                            break;
                        case "eStinger":
                            index = rng.Next(eStingerParams.Count);
                            s06Object.Parameters[2].Data = eStingerParams[index];
                            break;
                        case "eStinger(Fly)":
                            index = rng.Next(eStingerFlyParams.Count);
                            s06Object.Parameters[2].Data = eStingerFlyParams[index];
                            break;
                        case "eSweeper":
                            index = rng.Next(eSweeperParams.Count);
                            s06Object.Parameters[2].Data = eSweeperParams[index];
                            break;
                        case "eWalker":
                            index = rng.Next(eWalkerParams.Count);
                            s06Object.Parameters[2].Data = eWalkerParams[index];
                            break;

                        case "eCerberus":
                            index = rng.Next(eCerberusParams.Count);
                            s06Object.Parameters[2].Data = eCerberusParams[index];
                            s06Object.Parameters[1].Data = index;
                            break;
                        case "eGenesis":
                            index = rng.Next(eGenesisParams.Count);
                            s06Object.Parameters[2].Data = eGenesisParams[index];
                            break;
                        case "eWyvern":
                            index = rng.Next(eWyvernParams.Count);
                            s06Object.Parameters[2].Data = eWyvernParams[index];
                            break;

                        case "firstIblis":
                            index = rng.Next(firstIblisParams.Count);
                            s06Object.Parameters[2].Data = firstIblisParams[index];
                            break;
                        case "secondIblis":
                            index = rng.Next(secondIblisParams.Count);
                            s06Object.Parameters[2].Data = secondIblisParams[index];
                            break;
                        case "thirdIblis":
                            index = rng.Next(thirdIblisParams.Count);
                            s06Object.Parameters[2].Data = thirdIblisParams[index];
                            break;

                        case "firstmefiress":
                            index = rng.Next(firstmefiressParams.Count);
                            s06Object.Parameters[2].Data = firstmefiressParams[index];
                            break;

                        case "solaris01":
                            index = rng.Next(solaris01Params.Count);
                            s06Object.Parameters[2].Data = solaris01Params[index];
                            break;
                        case "solaris02":
                            index = rng.Next(solaris02Params.Count);
                            s06Object.Parameters[2].Data = solaris02Params[index];
                            break;

                        default:
                            Console.WriteLine("Ended up with an enemy name that doesn't exist. Replacing it with a cBiter.");
                            s06Object.Parameters[0].Data = "cBiter";
                            s06Object.Parameters[2].Data = "cBiter_Fix";
                            break;
                    }
                }
            }

            setTarget.Save(filepath, true);
        }

        static public void CharacterRandomiser(string filepath, Random rng)
        {
            S06SetData setTarget = new S06SetData();
            int index;

            setTarget.Load(filepath);
            foreach (SetObject s06Object in setTarget.Objects)
            {
                if (s06Object.ObjectType == "player_start2")
                {
                    index = rng.Next(SetRandomisationForm.validCharacters.Count);
                    s06Object.Parameters[1].Data = SetRandomisationForm.validCharacters[index];
                }
            }

            setTarget.Save(filepath, true);
        }

        static public void ItemCapsuleRandomiser(string filepath, Random rng)
        {
            S06SetData setTarget = new S06SetData();
            int index;

            setTarget.Load(filepath);
            foreach (SetObject s06Object in setTarget.Objects)
            {
                if (s06Object.ObjectType == "itemboxg" || s06Object.ObjectType == "itemboxa")
                {
                    index = rng.Next(SetRandomisationForm.validItems.Count);
                    s06Object.Parameters[0].Data = SetRandomisationForm.validItems[index];
                }
            }

            setTarget.Save(filepath, true);
        }

        static public void VoiceRandomiser(string filepath, Random rng)
        {
            S06SetData setTarget = new S06SetData();
            //Setup the list of avaliable lines
            String rawLines = Properties.Resources.s06TextStrings;
            string[] lines = rawLines.Split
            (
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );
            int index;

            setTarget.Load(filepath);
            foreach (SetObject s06Object in setTarget.Objects)
            {
                if (s06Object.ObjectType == "common_hint" || s06Object.ObjectType == "common_hint_collision")
                {
                    index = rng.Next(lines.Length);
                    s06Object.Parameters[0].Data = lines[index];
                }
            }

            setTarget.Save(filepath, true);
        }

        static public void PhysicsPropsRandomiser(string filepath, Random rng)
        {
            S06SetData setTarget = new S06SetData();
            //Setup the list of avaliable lines
            String rawLines = Properties.Resources.s06PhysicsObjects;
            string[] lines = rawLines.Split
            (
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );
            int index;

            setTarget.Load(filepath);
            foreach (SetObject s06Object in setTarget.Objects)
            {
                if (s06Object.ObjectType == "objectphysics" || s06Object.ObjectType == "objectphysics_item")
                {
                    if (s06Object.Parameters[0].Data.ToString() != "HedgeLib Workaround" && s06Object.Parameters[0].Data.ToString() != "Randomised Indicator")
                    {
                        index = rng.Next(lines.Length);
                        s06Object.Parameters[0].Data = lines[index];
                    }
                }
            }

            setTarget.Save(filepath, true);
        }

        static public void DoorHack(string filepath)
        {
            S06SetData setTarget = new S06SetData();

            setTarget.Load(filepath);
            foreach (SetObject s06Object in setTarget.Objects)
            {
                if (s06Object.ObjectType == "aqa_door" || s06Object.ObjectType == "common_cage" || s06Object.ObjectType == "common_stopplayercollision" || s06Object.ObjectType == "dtd_door" || s06Object.ObjectType == "flc_door" || s06Object.ObjectType == "kdv_door" || s06Object.ObjectType == "rct_door" || s06Object.ObjectType == "wap_door")
                {
                    s06Object.Transform.Position.Y = 9999999;
                }
            }

            setTarget.Save(filepath, true);
        }
    }
}
