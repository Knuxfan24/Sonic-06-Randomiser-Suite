using System;
using HedgeLib.Sets;
using System.Collections.Generic;
using Sonic_06_Randomiser_Suite.Serialisers;

namespace Sonic_06_Randomiser_Suite
{
    class Placement
    {
        public static void RandomiseEnemies(S06SetData set, Random rng) {
            // Enemy Parameter Lists
            string[] cBiterParams      = Resources.ParseLineBreaks(Properties.Resources.cBiterParams);
            string[] cCrawlerParams    = Resources.ParseLineBreaks(Properties.Resources.cCrawlerParams);
            string[] cGazerParams      = Resources.ParseLineBreaks(Properties.Resources.cGazerParams);
            string[] cGolemParams      = Resources.ParseLineBreaks(Properties.Resources.cGolemParams);
            string[] cStalkerParams    = Resources.ParseLineBreaks(Properties.Resources.cStalkerParams);
            string[] cTakerParams      = Resources.ParseLineBreaks(Properties.Resources.cTakerParams);
            string[] cTitanParams      = Resources.ParseLineBreaks(Properties.Resources.cTitanParams);
            string[] cTrickerParams    = Resources.ParseLineBreaks(Properties.Resources.cTrickerParams);
            string[] eArmorParams      = Resources.ParseLineBreaks(Properties.Resources.eArmorParams);
            string[] eBlusterParams    = Resources.ParseLineBreaks(Properties.Resources.eBlusterParams);
            string[] eBomberParams     = Resources.ParseLineBreaks(Properties.Resources.eBomberParams);
            string[] eBusterParams     = Resources.ParseLineBreaks(Properties.Resources.eBusterParams);
            string[] eBusterFlyParams  = Resources.ParseLineBreaks(Properties.Resources.eBusterFlyParams);
            string[] eCannonParams     = Resources.ParseLineBreaks(Properties.Resources.eCannonParams);
            string[] eCannonFlyParams  = Resources.ParseLineBreaks(Properties.Resources.eCannonFlyParams);
            string[] eChaserParams     = Resources.ParseLineBreaks(Properties.Resources.eChaserParams);
            string[] eCommanderParams  = Resources.ParseLineBreaks(Properties.Resources.eCommanderParams);
            string[] eFlyerParams      = Resources.ParseLineBreaks(Properties.Resources.eFlyerParams);
            string[] eGuardianParams   = Resources.ParseLineBreaks(Properties.Resources.eGuardianParams);
            string[] eGunnerParams     = Resources.ParseLineBreaks(Properties.Resources.eGunnerParams);
            string[] eGunnerFlyParams  = Resources.ParseLineBreaks(Properties.Resources.eGunnerFlyParams);
            string[] eHunterParams     = Resources.ParseLineBreaks(Properties.Resources.eHunterParams);
            string[] eKeeperParams     = Resources.ParseLineBreaks(Properties.Resources.eKeeperParams);
            string[] eLancerParams     = Resources.ParseLineBreaks(Properties.Resources.eLancerParams);
            string[] eLancerFlyParams  = Resources.ParseLineBreaks(Properties.Resources.eLancerFlyParams);
            string[] eLinerParams      = Resources.ParseLineBreaks(Properties.Resources.eLinerParams);
            string[] eRounderParams    = Resources.ParseLineBreaks(Properties.Resources.eRounderParams);
            string[] eSearcherParams   = Resources.ParseLineBreaks(Properties.Resources.eSearcherParams);
            string[] eStingerParams    = Resources.ParseLineBreaks(Properties.Resources.eStingerParams);
            string[] eStingerFlyParams = Resources.ParseLineBreaks(Properties.Resources.eStingerFlyParams);
            string[] eSweeperParams    = Resources.ParseLineBreaks(Properties.Resources.eSweeperParams);
            string[] eWalkerParams     = Resources.ParseLineBreaks(Properties.Resources.eWalkerParams);

            // Boss Parameter Lists
            string[] eCerberusParams     = Resources.ParseLineBreaks(Properties.Resources.eCerberusParams);
            string[] eGenesisParams      = Resources.ParseLineBreaks(Properties.Resources.eGenesisParams);
            string[] eWyvernParams       = Resources.ParseLineBreaks(Properties.Resources.eWyvernParams);
            string[] firstIblisParams    = Resources.ParseLineBreaks(Properties.Resources.firstIblisParams);
            string[] secondIblisParams   = Resources.ParseLineBreaks(Properties.Resources.secondIblisParams);
            string[] thirdIblisParams    = Resources.ParseLineBreaks(Properties.Resources.thirdIblisParams);
            string[] firstmefiressParams = Resources.ParseLineBreaks(Properties.Resources.firstmefiressParams);
            string[] solaris01Params     = Resources.ParseLineBreaks(Properties.Resources.solaris01Params);
            string[] solaris02Params     = Resources.ParseLineBreaks(Properties.Resources.solaris02Params);

            foreach (SetObject obj in set.Objects)
            {
                switch (obj.ObjectType)
                {
                    case "enemy":
                    case "enemyextra":
                        obj.Parameters[0].Data = Main.Enemies[rng.Next(Main.Enemies.Count)]; // Select an enemy type from the list

                        // Enemy Palette
                        switch(obj.Parameters[0].Data.ToString())
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
                                obj.Parameters[1].Data = 1;
                                break;

                            case "eLancer":
                            case "eLancer(Fly)":
                                obj.Parameters[1].Data = 2;
                                break;

                            case "eBuster":
                            case "eBuster(Fly)":
                                obj.Parameters[1].Data = 3;
                                break;

                            // Set it to 0 by default
                            default:
                                obj.Parameters[1].Data = 0;
                                break;
                        }

                        // Enemy Behaviour
                        switch (obj.Parameters[0].Data.ToString())
                        {
                            // Enemies
                            case "cBiter":        obj.Parameters[2].Data = cBiterParams[rng.Next(cBiterParams.Length)];               break;
                            case "cCrawler":      obj.Parameters[2].Data = cCrawlerParams[rng.Next(cCrawlerParams.Length)];           break;
                            case "cGazer":        obj.Parameters[2].Data = cGazerParams[rng.Next(cGazerParams.Length)];               break;
                            case "cGolem":        obj.Parameters[2].Data = cGolemParams[rng.Next(cGolemParams.Length)];               break;
                            case "cStalker":      obj.Parameters[2].Data = cStalkerParams[rng.Next(cStalkerParams.Length)];           break;
                            case "cTaker":        obj.Parameters[2].Data = cTakerParams[rng.Next(cTakerParams.Length)];               break;
                            case "cTitan":        obj.Parameters[2].Data = cTitanParams[rng.Next(cTitanParams.Length)];               break;
                            case "cTricker":      obj.Parameters[2].Data = cTrickerParams[rng.Next(cTrickerParams.Length)];           break;
                            case "eArmor":        obj.Parameters[2].Data = eArmorParams[rng.Next(eArmorParams.Length)];               break;
                            case "eBluster":      obj.Parameters[2].Data = eBlusterParams[rng.Next(eBlusterParams.Length)];           break;
                            case "eBomber":       obj.Parameters[2].Data = eBomberParams[rng.Next(eBomberParams.Length)];             break;
                            case "eBuster":       obj.Parameters[2].Data = eBusterParams[rng.Next(eBusterParams.Length)];             break;
                            case "eBuster(Fly)":  obj.Parameters[2].Data = eBusterFlyParams[rng.Next(eBusterFlyParams.Length)];       break;
                            case "eCannon":       obj.Parameters[2].Data = eCannonParams[rng.Next(eCannonParams.Length)];             break;
                            case "eCannon(Fly)":  obj.Parameters[2].Data = eCannonFlyParams[rng.Next(eCannonFlyParams.Length)];       break;
                            case "eChaser":       obj.Parameters[2].Data = eChaserParams[rng.Next(eChaserParams.Length)];             break;
                            case "eCommander":    obj.Parameters[2].Data = eCommanderParams[rng.Next(eCommanderParams.Length)];       break;
                            case "eFlyer":        obj.Parameters[2].Data = eFlyerParams[rng.Next(eFlyerParams.Length)];               break;
                            case "eGuardian":     obj.Parameters[2].Data = eGuardianParams[rng.Next(eGuardianParams.Length)];         break;
                            case "eGunner":       obj.Parameters[2].Data = eGunnerParams[rng.Next(eGunnerParams.Length)];             break;
                            case "eGunner(Fly)":  obj.Parameters[2].Data = eGunnerFlyParams[rng.Next(eGunnerFlyParams.Length)];       break;
                            case "eHunter":       obj.Parameters[2].Data = eHunterParams[rng.Next(eHunterParams.Length)];             break;
                            case "eKeeper":       obj.Parameters[2].Data = eKeeperParams[rng.Next(eKeeperParams.Length)];             break;
                            case "eLancer":       obj.Parameters[2].Data = eLancerParams[rng.Next(eLancerParams.Length)];             break;
                            case "eLancer(Fly)":  obj.Parameters[2].Data = eLancerFlyParams[rng.Next(eLancerFlyParams.Length)];       break;
                            case "eLiner":        obj.Parameters[2].Data = eLinerParams[rng.Next(eLinerParams.Length)];               break;
                            case "eRounder":      obj.Parameters[2].Data = eRounderParams[rng.Next(eRounderParams.Length)];           break;
                            case "eSearcher":     obj.Parameters[2].Data = eSearcherParams[rng.Next(eSearcherParams.Length)];         break;
                            case "eStinger":      obj.Parameters[2].Data = eStingerParams[rng.Next(eStingerParams.Length)];           break;
                            case "eStinger(Fly)": obj.Parameters[2].Data = eStingerFlyParams[rng.Next(eStingerFlyParams.Length)];     break;
                            case "eSweeper":      obj.Parameters[2].Data = eSweeperParams[rng.Next(eSweeperParams.Length)];           break;
                            case "eWalker":       obj.Parameters[2].Data = eWalkerParams[rng.Next(eWalkerParams.Length)];             break;

                            // Bosses
                            case "eCerberus":     obj.Parameters[2].Data = eCerberusParams[rng.Next(eCerberusParams.Length)];         break;
                            case "eGenesis":      obj.Parameters[2].Data = eGenesisParams[rng.Next(eGenesisParams.Length)];           break;
                            case "eWyvern":       obj.Parameters[2].Data = eWyvernParams[rng.Next(eWyvernParams.Length)];             break;
                            case "firstIblis":    obj.Parameters[2].Data = firstIblisParams[rng.Next(firstIblisParams.Length)];       break;
                            case "secondIblis":   obj.Parameters[2].Data = secondIblisParams[rng.Next(secondIblisParams.Length)];     break;
                            case "thirdIblis":    obj.Parameters[2].Data = thirdIblisParams[rng.Next(thirdIblisParams.Length)];       break;
                            case "firstmefiress": obj.Parameters[2].Data = firstmefiressParams[rng.Next(firstmefiressParams.Length)]; break;
                            case "solaris01":     obj.Parameters[2].Data = solaris01Params[rng.Next(solaris01Params.Length)];         break;
                            case "solaris02":     obj.Parameters[2].Data = solaris02Params[rng.Next(solaris02Params.Length)];         break;
                        }
                        break;
                }
            }
        }
    
        public static void RandomiseCharacters(S06SetData set, Random rng) {
            foreach (SetObject obj in set.Objects)
            {
                switch (obj.ObjectType)
                {
                    case "player_start2": obj.Parameters[1].Data = Main.Characters[rng.Next(Main.Characters.Count)]; break;
                    case "player_npc":    obj.Parameters[0].Data = rng.Next(1, 15);                                  break;
                }
            }
        }

        public static void RandomiseItems(S06SetData set, Random rng) {
            foreach (SetObject obj in set.Objects)
            {
                switch (obj.ObjectType)
                {
                    case "itemboxa":
                    case "itemboxg":
                    case "itembox_next":
                        obj.Parameters[0].Data = Main.Items[rng.Next(Main.Items.Count)];
                        break;
                }
            }
        }

        public static void RandomiseVoices(S06SetData set, Random rng) {
            string[] voiceLines = Resources.ParseLineBreaks(Properties.Resources.S06TextStrings);

            foreach (SetObject obj in set.Objects)
            {
                switch (obj.ObjectType)
                {
                    case "common_hint":
                    case "common_hint_collision":
                        obj.Parameters[0].Data = voiceLines[rng.Next(voiceLines.Length)];
                        break;
                }
            }
        }
        
        public static void RandomisePhysicsProps(S06SetData set, Random rng) {
            string[] propList = Resources.ParseLineBreaks(Properties.Resources.S06PhysicsObjects);

            foreach (SetObject obj in set.Objects)
            {
                switch (obj.ObjectType)
                {
                    case "objectphysics":
                        obj.Parameters[0].Data = propList[rng.Next(propList.Length)];
                        break;
                }
            }
        }
    }
}
