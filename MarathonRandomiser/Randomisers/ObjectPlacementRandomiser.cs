using Marathon.Formats.Package;
using Marathon.Formats.Placement;
using System.Numerics;

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
        /// <param name="particle">Whether or not particles will be randomised.</param>
        /// <param name="jumpboards">Whether or not Jump Panels have a chance to be switched for a Jump Board.</param>
        /// <param name="SetEnemies">The list of valid enemies.</param>
        /// <param name="SetCharacters">The list of valid characters.</param>
        /// <param name="SetItemCapsules">The list of valid item types.</param>
        /// <param name="SetCommonProps">The list of valid Common.bin based props.</param>
        /// <param name="SetPathProps">The list of valid PathObj.bin based props.</param>
        /// <param name="SetHints">The list of valid hint voice lines.</param>
        /// <param name="SetDoors">The list of valid door types.</param>
        /// <param name="SetParticleBanks">The list of valid particle banks to draw from.</param>
        /// <param name="minDrawDistance">The minimum allowed draw distance.</param>
        /// <param name="maxDrawDistance">The maximum allowed draw distance.</param>
        /// <param name="jumpboardChance">The chance that a Jump Panel will be switched for a Jump Board.</param>
        /// <param name="shuffleTransform">Whether or not objects will have their positions shuffled around.</param>
        /// <param name="shuffleBlacklist">The list of objects we should ignore for the position shuffling process.</param>
        public static async Task Process(string setFile, bool? enemies, bool? enemiesNoBosses, bool? behaviour, bool? behaviourNoEnforce, bool? characters, bool? itemCapsules, bool? commonProps,
                                      bool? pathProps, bool? hints, bool? doors, bool? drawDistance, bool? cosmetic, bool? particle, bool? jumpboards, List<string> SetEnemies,
                                      List<string> SetCharacters, List<string> SetItemCapsules, List<string> SetCommonProps, List<string> SetPathProps, List<string> SetHints, List<string> SetDoors,
                                      List<string> SetParticleBanks, int minDrawDistance, int maxDrawDistance, int jumpboardChance, bool? shuffleTransform, List<string> shuffleBlacklist)
        {
            List<Vector3> Positions = new();
            List<Quaternion> Rotations = new();

            // Load this set file.
            using ObjectPlacement set = new(setFile);

            // Loop through all the objects in this set file.
            foreach (SetObject setObject in set.Data.Objects)
            {
                // If we're shuffling objects and this type isn't in the blacklist, then store it's position and rotation.
                if (shuffleTransform == true && !shuffleBlacklist.Contains(setObject.Type))
                {
                    Positions.Add(new(setObject.Position.X, setObject.Position.Y, setObject.Position.Z));
                    Rotations.Add(new(setObject.Rotation.X, setObject.Rotation.Y, setObject.Rotation.Z, setObject.Rotation.W));

                    // Kill the light dash paths from Rings so an accidental X button press doesn't warp you to fuck knows where.
                    if (setObject.Type == "ring")
                        setObject.Parameters[2].Data = "";
                }

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
                            await Task.Run(() => EnemyBehaviourRandomiser(setObject, behaviourNoEnforce, enemiesNoBosses));
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

                    case "particle":
                        if (particle == true)
                            await Task.Run(() => ParticleRandomiser(setObject, SetParticleBanks));
                        break;

                    case "jumppanel":
                        if (jumpboards == true)
                            await Task.Run(() => JumpboardSwitcher(setObject, jumpboardChance));
                        break;

                }

            }

            // If we're using this cursed option, then shuffle positions and rotations around.
            if (shuffleTransform == true)
            {
                // Set up a couple of lists of numbers.
                List<int> usedNumbers = new();
                List<int> eventVolumeIndices = new();

                // Loop through all the objects in this set file again.
                for (int i = 0; i < set.Data.Objects.Count; i++)
                {
                    if (!shuffleBlacklist.Contains(set.Data.Objects[i].Type))
                    {
                        // Pick a number to use.
                        int index = MainWindow.Randomiser.Next(Positions.Count);

                        // If the selected number is already used, pick another until it isn't.
                        if (usedNumbers.Contains(index))
                        {
                            do { index = MainWindow.Randomiser.Next(Positions.Count); }
                            while (usedNumbers.Contains(index));
                        }

                        // Set the rotation and position to the selected ones.
                        set.Data.Objects[i].Position = Positions[index];
                        set.Data.Objects[i].Rotation = Rotations[index];

                        // Mark this index as already being used.
                        usedNumbers.Add(index);

                        // Log this object's index as an event volume, so we don't have to loop through them all again.
                        // Only add ones that actually do something in their onintersect function.
                        switch (set.Data.Objects[i].Type)
                        {
                            case "eventbox":
                                if ((string)set.Data.Objects[i].Parameters[3].Data != "")
                                    eventVolumeIndices.Add(i);
                                break;
                            case "eventcylinder":
                                if ((string)set.Data.Objects[i].Parameters[2].Data != "")
                                    eventVolumeIndices.Add(i);
                                break;
                            case "eventsphere":
                                if ((string)set.Data.Objects[i].Parameters[1].Data != "")
                                    eventVolumeIndices.Add(i);
                                break;
                        }
                    }
                }

                // Add extra objects to indicate the positions of event volumes.
                if (eventVolumeIndices.Count != 0)
                {
                    for (int i = 0; i < eventVolumeIndices.Count; i++)
                    {
                        // Create the basic object.
                        SetObject eventIndicator = new()
                        {
                            Name = $"eventboxindicator{i}",
                            Type = "common_path_obj",
                            Position = set.Data.Objects[eventVolumeIndices[i]].Position,
                            Rotation = set.Data.Objects[eventVolumeIndices[i]].Rotation,
                            DrawDistance = set.Data.Objects[eventVolumeIndices[i]].DrawDistance
                        };

                        // Set the parameters.
                        eventIndicator.Parameters.Add(ParameterCreate("EventIndicator", ObjectDataType.String));
                        eventIndicator.Parameters.Add(ParameterCreate("", ObjectDataType.String));
                        eventIndicator.Parameters.Add(ParameterCreate(0f, ObjectDataType.Single));
                        eventIndicator.Parameters.Add(ParameterCreate(0f, ObjectDataType.Single));

                        // Add the object to the set file.
                        set.Data.Objects.Add(eventIndicator);
                    }
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
        static async Task EnemyBehaviourRandomiser(SetObject setObject, bool? dontEnforceBehaviours, bool? enemiesNoBosses)
        {
            // If this object is a boss but the user has disallowed boss randomisation, then don't change anything.
            if (enemiesNoBosses == true && (setObject.Parameters[0].Data.ToString() == "eCerberus" || setObject.Parameters[0].Data.ToString() == "eGenesis" ||
                setObject.Parameters[0].Data.ToString() == "eWyvern" || setObject.Parameters[0].Data.ToString() == "firstiblis" || setObject.Parameters[0].Data.ToString() == "secondiblis" ||
                setObject.Parameters[0].Data.ToString() == "thirdiblis" || setObject.Parameters[0].Data.ToString() == "firstmefiress" || setObject.Parameters[0].Data.ToString() == "secondmefiress" ||
                setObject.Parameters[0].Data.ToString() == "solaris01" || setObject.Parameters[0].Data.ToString() == "solaris02"))
            {
                // Check if this boss actually has a boss's parameter, if not, then it's probably a regular enemy that's been randomised into a boss and will need its parameter changing.
                // If it does have a boss parameter, then we can just return.
                if (setObject.Parameters[2].Data is (object)"secondIblis_sonic" or (object)"secondIblis_shadow" or (object)"firstmefiress_shadow" or (object)"firstmefiress_omega"
                                                 or (object)"eCerberus_sonic" or (object)"eCerberus_shadow" or (object)"eGenesis_sonic" or (object)"eGenesis_silver" or (object)"firstIblis"
                                                 or (object)"thirdIblis" or (object)"secondmefiress_shadow" or (object)"eWyvern" or (object)"solaris01" or (object)"solaris02")
                    return;
            }

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
                    case "firstiblis":     setObject.Parameters[2].Data = "firstIblis";                                                                       break;
                    case "secondiblis":    setObject.Parameters[2].Data = secondIblisParameters[MainWindow.Randomiser.Next(secondIblisParameters.Count)];     break;
                    case "thirdiblis":     setObject.Parameters[2].Data = "thirdIblis";                                                                       break;
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
        /// Randomises what particles are used in a particle emitter object.
        /// </summary>
        /// <param name="setObject">The object we're editing.</param>
        /// <param name="SetParticleBanks">The list of valid particle banks.</param>
        static async Task ParticleRandomiser(SetObject setObject, List<string> SetParticleBanks)
        {
            // Set up list of particles by bank.
            List<string> ecannonParticles = new() { "lasershot", "laserhit" };
            List<string> ecerberusParticles = new() { "sonic_sd_g", "sonic_sd", "sonic_sw_g", "sonic_sw", "sonic_ld_g", "sonic_ld", "sonic_lw_g", "sonic_lw", "sonic_lld_g", "sonic_lld", "sonic_llw_g", "sonic_llw", "run_d", "run_w", "rapid_d", "rapid_w", "hit_d", "hit_w", "dead_d", "dead_w", "domb_d", "domb_w" };
            List<string> egenesisParticles = new() { "laser_sign", "laser_g", "laser", "exelaser_sg", "exelaser_s", "exelaser_mg", "exelaser_m", "hatchlight", "shot", "rocket", "exp_s_k000", "headguard", "hatch_smoke_g", "hatch_smoke", "electric", "spouting_smoke_g", "wingdamage", "exe_wing", "break_wing", "kamikaze", "break_core" };
            List<string> egunnerParticles = new() { "boost", "heat" };
            List<string> enemy_commonParticles = new() { "hit_stop", "hit_smash", "stun_machine_s", "stun_machine", "stun_chaos_s", "stun_chaos", "appear_machine_small", "appear_machine_medium", "appear_machine_large", "appear_chaos_small", "appear_chaos_medium", "appear_chaos_large", "appear_flame_small", "appear_flame_medium", "appear_flame_large", "die_machine_small", "die_machine_medium", "die_machine_large", "die_chaos_small", "die_chaos_medium", "die_chaos_large", "die_flame_small", "die_flame_medium", "die_flame_large", "explosion_small", "explosion_medium", "explosion_large", "mshot", "smoke", "gunshot", "gunhit", "hit_nodamage", "fireball", "fireball_hit", "firebreath", "darkbreath", "sonic_small_g", "sonic_small", "sonic_medium_g", "sonic_medium", "enemysmoke_small", "enemysmoke_medium", "alarm", "lasershot", "laserhit", "darksph_sge", "darksph_s", "darksph_lge", "darksph_l", "darkbomb_sga", "darkbomb_s", "darkbomb_lga", "darkbomb_l", "laser_g", "laser", "laserbomb", "elec_sg", "elec_s", "elec_lg", "elec_l", "break_core" };
            List<string> ewyvernParticles = new() { "laser_g", "laser", "laserhit", "exelaser_mg", "exelaser_m", "exelaser_lg", "exelaser_l", "shot", "needle_ab", "needle_stick", "needle_sign", "needle_bomb", "wing_g", "wing", "damage", "smk_m_a300", "smk_l_a300", "electric", "smk_l_a302" };
            List<string> iblis01Particles = new() { "damagemark", "discharge", "discharge_000", "vbombbody", "vbombaffix", "explosion", "vbombtimer", "impact", "impactadd", "simpactadd", "simpact", "breathstart", "breath", "lparts", "sparts1", "sparts2", "sparts3", "parts1", "parts2", "parts3", "conparts1", "conparts2", "conparts3", "shape01pre", "shape01", "shape02pre", "shape02", "shape03pre", "shape03", "death00" };
            List<string> iblis02Particles = new() { "damagemark", "vbombbody", "vbombaffix", "explosion", "parts1", "parts2", "parts3", "conparts1", "conparts2", "conparts3", "sparts1", "sparts2", "sparts3", "lparts", "damage_s", "damage_l", "hp_half", "attack", "dive", "dive00_a", "appearance", "appearance00_a", "lava_sign", "lava_sign_a", "divetostick", "rush_ss", "rush_s", "rush_l", "clash", "clash_a", "faintend", "rush", "dive01_a", "appearance01_a", "sink", "stickend", "sticking", "surfacing", "waittodive", "death00" };
            List<string> iblis03Particles = new() { "damagemark", "vbombaffix", "explosion", "lparts", "sparts1", "sparts2", "sparts3", "des_head", "breathstart", "breath", "fwave_sign", "fwave_g", "fwave", "fwave_foot", "bombdischarge_g", "bombdischarge", "iblis03_core", "bombcore_g", "bombcore", "bombcore_sonic", "ground_sonic_g", "ground_sonic", "step_splash", "step_wave_l", "step_wave_s", "walk", "retreat" };
            List<string> kdv_scaffold01Particles = new() { "smoke01", "smoke02", "scaffold_smoke" };
            List<string> map_aqaParticles = new() { "fire_01", "fire_02", "bubble_01", "dorsmoke_01", "magnetic_01", "magnetic_02", "light_01" };
            List<string> map_commonParticles = new() { "woodbox", "ironbox", "savepoint", "ring", "bombbox", "wind_6m", "wind_12m", "wsmoke_6m", "wsmoke_12m", "hint_stay", "hint_hit", "itembox_get", "cdv_stay_g", "cdv_stay", "cdv_get", "lcore_stay_g", "lcore_stay", "lcore_stay_li_g", "lcore_stay_li", "lcore_get", "lcore_get_li", "flashbox", "goalring_stay", "goalring_hit", "emerald_w", "emerald_w_ap", "emerald_s", "emerald_s_ap", "emerald_y", "emerald_y_ap", "emerald_p", "emerald_p_ap", "emerald_g", "emerald_g_ap", "emerald_b", "emerald_b_ap", "emerald_r", "emerald_r_ap", "flashboxdamage", "d_smoke2m", "d_smoke4m", "d_smoke6m", "key_stay", "key_get", "warp_stay", "warp_in", "switch_on", "c_smoke1m", "c_smoke3m", "c_smoke6m", "c_smoke_m_1m", "c_smoke_m_3m", "c_smoke_m_6m", "c_smoke_h_1m", "c_smoke_h_3m", "c_smoke_h_6m", "c_smoke_w_1m", "c_smoke_w_3m", "c_smoke_w_6m", "c_splash1m", "c_splash3m", "c_splash6m", "c_splash_m_1m", "c_splash_m_3m", "c_splash_m_6m", "c_spark1m", "c_spark3m", "c_spark6m", "disk_01", "smk_ss_a300", "smk_s_a300", "smk_m_a300" };
            List<string> map_cscParticles = new() { "stream", "stream2", "stome", "heathaze1", "heathaze2", "heathaze3", "heathaze4", "smoke1", "smoke2", "smoke3", "smoke4", "smoke5", "smoke6", "smoke7", "bldg_exp4", "bldg_exp5", "car_exp1", "car_exp2", "exp_heat", "bldg_exp6", "bldg_flame1", "bldg_flame2", "bldg_flame5", "bldg_flame6", "bldg_flame7", "bldg_flame8", "bldg_flame9", "bldg_flame3", "bldg_flame4", "flame1", "flame2", "fir_l_k000", "fir_m_k000", "fir_s_k000", "firespark", "firespark2", "firespark5", "firespark3", "firespark4", "tornado_b_fire", "tornado_b_f_spawn_s_red", "tornado_b_f_spawn_m_org", "tornado_b_f_spawn_l_brwn", "tornado_b_smoke", "tornado_a_fire", "tornado_a_smoke", "exp_m_k200", "exp_s_k200", "smk_l_y100", "smk_m_y100", "smk_m_y200", "smk_m_y300", "smk_s_y200", "smk_s_y300", "spa_m_t000", "spa_m_v000", "spa_s_t000", "car_crash", "glassbreak" };
            List<string> map_dtdParticles = new() { "quicksand_01", "quicksand_03", "quicksand_04", "quicksand_05", "whlsmoke_01", "whlsmoke_02", "whlsmoke_03", "sndsmoke_01", "sndsmoke_02", "torchlight_01", "torchlight_02", "light_01", "sandwave_01", "sandwave_02", "shimmer", "exp_m_a200", "smk_l_a300", "smk_l_y100", "smk_l_y200", "smk_l_y202", "smk_l_y300", "smk_m_a300", "smk_m_y100", "smk_m_y200", "smk_m_y300", "smk_s_a300", "smk_s_y200", "smk_s_y300", "sand_whole00", "sand_canal00", "sand_canal01", "pillar_appear", "wall_collapse", "sand_wave00", "sand_wave01", "havok_stature", "havok_broken_stature", "broken_stature", "moving_foothold", "sand_distant", "whirl_sand_l", "whirl_sand_m", "whirl_sand_s", "sand_downward_s", "sand_downward_m", "torchlight1", "torchlight3", "billiards_explosion", "door_smoke" };
            List<string> map_endParticles = new() { "thunder_01", "warp_01", "warp_02", "warp_03", "inhall_g0", "outhall_g0", "warp_04", "switch_01", "switch_02" };
            List<string> map_flcParticles = new() { "distortion_01", "spark_01", "prominence_01", "lavsplash_01", "lavsmoke_01", "lavspark_01", "torchlight_01", "torchlight_02", "light_01", "steam_01", "colflame_01", "colflame_02", "smk_l_y100", "smk_l_y201", "smk_l_y300", "smk_m_y100", "smk_m_y200", "smk_m_y300", "smk_s_a300", "smk_s_y100", "smk_s_y200", "smk_s_y201", "smk_s_y300", "fir_s_a004", "fir_s_a003", "lit_l_b001", "fir_s_a001", "exp_s_k100", "lit_s_b001", "lit_s_b002", "flm_particle", "fireball", "exp_s_a200" };
            List<string> map_iblis01Particles = new() { "sparks_01", "sparks_02" };
            List<string> map_iblis02Particles = new() { "sparks_01", "sparks_02", "smk_l_y300", "spl_l_m001" };
            List<string> map_iblis03Particles = new() { "sparks_01", "sparks_02", "lavsmoke_01", "lavspark_01", "smk_l_y100", "smk_m_y100", "smk_m_y200", "smk_m_y300", "smk_s_y100", "smk_s_y200", "smk_s_y300", "smk_l_y301" };
            List<string> map_kdvParticles = new() { "w_eagle", "br_smoke1", "br_smoke2", "br_smoke3", "br_smoke4", "br_smoke5", "br_smoke6", "br_smoke7", "br_smoke8", "do_smoke", "waterfall3", "waterfall6", "st_light", "st_light2", "st_light3", "current", "current2", "leaf", "leaf3", "w_ripples", "w_ripples2", "w_ripples3", "w_ripples4", "torchlight1", "torchlight3", "torchlight5", "smoke01_fast", "smoke02_slow", "smoke_frag_small", "smoke_frag_middle", "smoke_frag_large", "spread_smoke_middle", "spread_smoke_small", "spread_smoke_large", "smoke_common_middle", "smoke_common_small", "smoke_middle", "smoke_small", "smoke_frag", "smk_s_y300", "smk_s_y200", "smk_s_a300", "smk_m_y300", "smk_m_y200", "smk_m_y100", "smk_m_h100", "smk_m_a300", "smk_ll_h100", "smk_l_y300", "smk_l_y200", "smk_l_y100", "smk_l_h101", "smk_l_h100", "smk_l_a300", "smk_3l_h100", "st_glass", "scaffold_smoke", "esp_smoke", "smoke01", "smoke02", "tower_smoke01", "tower_smoke02", "door_smoke", "common_smoke01", "common_smoke02", "dust" };
            List<string> map_rctParticles = new() { "mist_01", "smk_m_y300", "spa_s_t001", "smk_s_y300", "smk_ss_y300", "smk_m_y100", "train_02", "train_03", "container_01", "eggtrain_01", "eggtrain_02", "eggtrain_03", "eggtrain_04", "eggtrain_05" };
            List<string> map_tpjParticles = new() { "waterfall_01", "waterfall_02", "waterfall_03", "ripples_01", "ripples_02", "spore_01", "spore_02", "leaf_01", "lotus_01", "turtle_01", "turtle_02", "turtle_03", "turtle_04", "smk_l_y300", "smk_l_a300", "smk_l_y100", "smk_m_y300", "swing_01", "swing_02" };
            List<string> map_twnParticles = new() { "fountain_01", "smoke_01", "torchlight_01", "w_eagle", "smk_m_y300", "smk_s_y300", "smk_ss_y300", "spa_m_v000", "spa_s_v000", "spa_s_t000", "exp_s_k000", "darkness_03", "warpgate_01", "warpgate_02", "darkness_01", "darkness_02", "exp_m_a200", "lightning_01", "gondola_01", "row_01", "confetti_01", "run_low", "damage_fire", "damage_smoke", "r_accele" };
            List<string> map_wapParticles = new() { "snowhole01", "snowhole_02", "crystal_00", "diadust_00", "blizard_01", "blizard_02", "searchlight_brk", "snowball_stay00", "snowball_stay01", "snowball_stay02", "snowball_stay03", "snowball_stay04", "ice_m_i000", "smk_ss_w200", "smk_s_w200", "smk_s_a300", "smk_l_w101", "smk_s_w101", "smk_m_w101", "smk_l_w102", "snow_whole00", "snow_whole01", "snowcrystal", "diamonddust", "blizard00", "blizard01", "ice_brk", "door_smoke01", "door_smoke02", "watchtower_smk00", "watchtower_smk01", "watchtower_snow" };
            List<string> map_wvoParticles = new() { "smk_l_a300", "smk_l_y100", "smk_l_y200", "smk_l_y300", "smk_m_a300", "smk_m_y100", "smk_m_y200", "smk_m_y300", "smk_s_a300", "smk_s_w201", "smk_s_y100", "smk_s_y300", "smk_ss_y300", "spa_s_t101", "spa_s_t102", "ship_01" };
            List<string> mefiressParticles = new() { "flame", "darksph_sge", "darksph_s", "darksph_lge", "darksph_l", "darksph_llge", "darksph_ll", "darkbomb_sga", "darkbomb_s", "darkbomb_lga", "darkbomb_l", "darkbomb_llga", "darkbomb_ll", "darkcircle", "aura_hand", "aura_toe", "traces", "warp", "ripple00_g", "ripple00", "ripple01_g", "ripple01", "shadow_sign", "smef_splash", "zmef_body", "zmef_hold", "zmef_suicide", "zmef_dead", "lasershot", "laserexp00", "laserexp01", "laser_root_g", "laser_root", "kmef_appears_g", "kmef_appears", "kmef_root", "ripple02_g", "ripple02", "kmef_splash", "division" };
            List<string> player_amyParticles = new() { "hammer_attack0", "hammer_attack1_g", "hammer_attack1_a", "trans_start0", "trans_start1_g", "trans_end_g", "trans_end_gg", "air_jump_g", "air_jump", "jump_smoke", "landing_smoke", "walk_smoke", "brake_smoke", "jump_snow", "landing_snow", "walk_snow", "brake_snow", "snow_lump", "brake_slump_g", "brake_slump", "jump_grass", "landing_grass", "grass", "grass_00", "brake_grass", "woods", "jump_water", "splash00", "splash01", "splash02", "splash000", "splash001", "splash002", "landing_water", "rapidstop_water", "deep_water", "footprint", "grind", "wood_waste", "grind_smoke", "windload", "snowload", "poll", "speed_up_g", "speed_up", "boot_g", "barrier_g", "barrier_a", "matchles_g", "matchles", "gage_up_g", "gage_up", "faint", "numbness_g", "numbness" };
            List<string> player_blazeParticles = new() { "flame_attack", "acceltornado", "homingcrow", "crow", "friction", "spin_flame", "spinningcrow", "exp", "jump_smoke", "landing_smoke", "walk_smoke", "brake_smoke", "jump_snow", "landing_snow", "walk_snow", "brake_snow", "snow_lump", "brake_slump_g", "brake_slump", "jump_grass", "landing_grass", "grass", "grass_00", "brake_grass", "woods", "jump_water", "splash00", "splash01", "splash02", "splash000", "splash001", "splash002", "landing_water", "rapidstop_water", "deep_water", "footprint", "grind", "wood_waste", "grind_smoke", "windload", "snowload", "poll", "speed_up_g", "speed_up", "boot_g", "barrier_g", "barrier_a", "matchles_g", "matchles", "gage_up_g", "gage_up", "faint", "numbness_g", "numbness" };
            List<string> player_himesonicParticles = new() { "homing_g", "homing", "sliding_g", "sliding", "hs_boot_g", "boot_flare", "hs_barrier_g", "barrier", "barr_sandrun_g", "barr_sandrun", "barr_waterrun_g", "barr_waterrun", "jump_smoke", "landing_smoke", "walk_smoke", "brake_smoke", "jump_snow", "landing_snow", "walk_snow", "brake_snow", "snow_lump", "brake_slump_g", "brake_slump", "jump_grass", "landing_grass", "grass", "grass_00", "brake_grass", "woods", "jump_water", "splash00", "splash01", "splash02", "splash000", "splash001", "splash002", "landing_water", "rapidstop_water", "deep_water", "footprint", "grind", "wood_waste", "grind_smoke", "windload", "snowload", "poll", "speed_up_g", "speed_up", "boot_g", "barrier_g", "barrier_a", "matchles_g", "matchles", "gage_up_g", "gage_up" };
            List<string> player_knucklesParticles = new() { "charge00_g", "charge00", "homing_hand", "screwdriver", "earthquake", "screwdriver02", "charge01_g", "charge01", "particle_s", "particle_l", "punch", "backbrow", "crush", "crush_ground", "jump_smoke", "landing_smoke", "walk_smoke", "brake_smoke", "jump_snow", "landing_snow", "walk_snow", "brake_snow", "snow_lump", "brake_slump_g", "brake_slump", "jump_grass", "landing_grass", "grass", "grass_00", "brake_grass", "woods", "jump_water", "splash00", "splash01", "splash02", "splash000", "splash001", "splash002", "landing_water", "rapidstop_water", "deep_water", "footprint", "grind", "wood_waste", "grind_smoke", "windload", "snowload", "poll", "speed_up_g", "speed_up", "boot_g", "barrier_g", "barrier_a", "matchles_g", "matchles", "gage_up_g", "gage_up", "faint", "numbness_g", "numbness" };
            List<string> player_omegaParticles = new() { "vernier00_g", "vernier00", "vernier01", "shot_g", "shot", "muzzle_g", "muzzle_l", "muzzle_r", "launcher", "launcharge_g", "launcharge", "exp_l_a000", "laser_charge_g", "laser_charge", "laser_g", "laser", "laserbomb", "short_g", "short", "jump_smoke", "landing_smoke", "walk_smoke", "brake_smoke", "jump_snow", "landing_snow", "walk_snow", "brake_snow", "snow_lump", "brake_slump_g", "brake_slump", "jump_grass", "landing_grass", "grass", "grass_00", "brake_grass", "woods", "jump_water", "splash00", "splash01", "splash02", "splash000", "splash001", "splash002", "landing_water", "rapidstop_water", "deep_water", "footprint", "grind", "wood_waste", "grind_smoke", "windload", "snowload", "poll", "speed_up_g", "speed_up", "boot_g", "barrier_g", "barrier_a", "matchles_g", "matchles", "gage_up_g", "gage_up", "faint", "numbness_g", "numbness" };
            List<string> player_rougeParticles = new() { "heart_set", "heart_sign", "pink_bomb_s", "pink_bomb_l", "jump_smoke", "landing_smoke", "walk_smoke", "brake_smoke", "jump_snow", "landing_snow", "walk_snow", "brake_snow", "snow_lump", "brake_slump_g", "brake_slump", "jump_grass", "landing_grass", "grass", "grass_00", "brake_grass", "woods", "jump_water", "splash00", "splash01", "splash02", "splash000", "splash001", "splash002", "landing_water", "rapidstop_water", "deep_water", "footprint", "grind", "wood_waste", "grind_smoke", "windload", "snowload", "poll", "speed_up_g", "speed_up", "boot_g", "barrier_g", "barrier_a", "matchles_g", "matchles", "gage_up_g", "gage_up", "faint", "numbness_g", "numbness" };
            List<string> player_shadowParticles = new() { "bootsjet_g", "bootsjet", "homing_00", "homing_01", "warp", "warp_g", "chaos_shield_a", "c_attack00", "c_attack01", "c_attack02", "c_attack03", "c_attack04", "attack", "c_attack01_g", "c_attack02_g", "c_attack03_g", "c_attack04_g", "c_attack05_g", "c_attack06_g", "s_kick00", "s_kick01", "s_kick02", "s_kick02_g", "c_spear00", "c_spear01", "c_spear01_g", "c_lance00", "c_lance01", "c_lance02_a", "c_lance02_g", "c_lance01_g", "boostsign_a", "boostsign_g", "aurasign", "aurasign_g", "boostaura00_g", "boostaura0", "boostaura01_g", "boostaura1", "boostaura02_g", "boostaura2", "c_blast", "c_blast_g", "s_chaoslance_g", "s_chaoslance", "s_c_lance_h_g", "s_c_lance_h", "s_chaosblast_g", "s_chaosblast", "c_blastbomb", "accumula_g", "accumula", "flight_g", "flight", "flightaccel_g", "flyaccel_up", "flyaccel_down", "flyaccel_left", "flyaccel_right", "jump_smoke", "landing_smoke", "walk_smoke", "brake_smoke", "jump_snow", "landing_snow", "walk_snow", "brake_snow", "snow_lump", "brake_slump_g", "brake_slump", "jump_grass", "landing_grass", "grass", "grass_00", "brake_grass", "woods", "jump_water", "splash00", "splash01", "splash02", "splash000", "splash001", "splash002", "landing_water", "rapidstop_water", "deep_water", "footprint", "grind", "wood_waste", "grind_smoke", "windload", "snowload", "poll", "speed_up_g", "speed_up", "boot_g", "barrier_g", "barrier_a", "matchles_g", "matchles", "gage_up_g", "gage_up", "faint", "numbness_g", "numbness" };
            List<string> player_silverParticles = new() { "repitation00", "psy_smash00", "psy_smash000", "psy_smash01", "psy_smash001", "psy_hit000", "psy_smash02", "psy_hit001", "esp_aura01", "esp_aura000", "esp_aura001", "hold_smash00", "holdsmash01_g", "hold_smash01", "psy_shock", "psy_shock000", "esp_touch", "psy_kine00", "psy_kine000", "psy_kine01", "psy_kine001", "psy_kine02", "psy_kine002", "psy_kine03", "psy_kine003", "esp_field", "flight_g", "flight", "flightaccel_g", "flyaccel_up", "flyaccel_down", "flyaccel_left", "flyaccel_right", "rapidstop_", "landing_smoke", "walk_smoke", "brake_smoke", "jump_snow", "landing_snow", "walk_snow", "brake_snow", "snow_lump", "brake_slump_g", "brake_slump", "jump_grass", "landing_grass", "grass", "grass_00", "brake_grass", "woods", "jump_water", "splash00", "splash01", "splash02", "splash001", "splash002", "landing_water", "rapidstop_water", "deep_water", "footprint", "grind", "wood_waste", "grind_smoke", "windload", "snowload", "poll", "speed_up_g", "speed_up", "boot_g", "barrier_g", "barrier_a", "matchles_g", "matchles", "gage_up_g", "gage_up", "faint", "numbness_g", "numbness" };
            List<string> player_sonicParticles = new() { "homing_00", "homing_01", "homing_smash", "attack", "attack_000", "sliding_00", "sliding_01", "spincharge_g", "spincharge", "spin_g", "spindash_01", "spindash_00", "lightdash_00", "bound", "bound_landing", "bound_landing_g", "mach_speed_g", "mach_speed", "tornado_g", "tornado", "emerald_s", "emerald_track", "emerald_warp_g", "emerald_warp", "reductio_g", "reductio", "magnificatio_g", "magnificatio", "boot_y", "t_guard", "superchange_g", "superchange", "lightatc_g", "lightatc", "lightatc_max", "accumula_g", "accumula", "flight_g", "flight", "flightaccel_g", "flyaccel_up", "flyaccel_down", "flyaccel_left", "flyaccel_right", "jump_smoke", "landing_smoke", "walk_smoke", "brake_smoke", "jump_snow", "landing_snow", "walk_snow", "brake_snow", "snow_lump", "brake_slump_g", "brake_slump", "jump_grass", "landing_grass", "grass", "grass_00", "brake_grass", "woods", "jump_water", "splash00", "splash01", "splash02", "splash000", "splash001", "splash002", "landing_water", "brake_water_g", "brake_water", "deep_water", "footprint", "grind", "wood_waste", "grind_smoke", "windload", "snowload", "poll", "speed_up_g", "speed_up", "boot_g", "boot_flare", "barrier_g", "barrier_a", "matchles_g", "matchles", "gage_up_g", "gage_up", "faint", "numbness_g", "numbness", "board_stone_g", "snowboard_stone", "board_snow_g", "snowboard_snow", "b_brake_stone_g", "b_brake_stone", "b_brake_snow_g", "b_brake_snow", "b_jump_stone_g", "b_jump_stone", "b_land_stone_g", "diamond_dust", "b_land_stone", "snowboard_iron" };
            List<string> player_tailsParticles = new() { "rolling_g", "rolling_a", "flying01", "dammyring00_g", "dammyring01_g", "dammyring03_g", "dammyring00m_g", "dammyring02", "dammysnaip00_gg", "dammysnaip00_g", "dammysnaip01", "jump_smoke", "landing_smoke", "walk_smoke", "brake_smoke", "jump_snow", "landing_snow", "walk_snow", "brake_snow", "snow_lump", "brake_slump_g", "brake_slump", "jump_grass", "landing_grass", "grass", "grass_00", "brake_grass", "woods", "jump_water", "splash00", "splash01", "splash02", "splash000", "splash001", "splash002", "landing_water", "rapidstop_water", "deep_water", "footprint", "grind", "wood_waste", "grind_smoke", "windload", "snowload", "poll", "speed_up_g", "speed_up", "boot_g", "barrier_g", "barrier_a", "matchles_g", "matchles", "gage_up_g", "gage_up", "faint", "numbness_g", "numbness" };
            List<string> solarisParticles = new() { "laser_sign", "laser_root", "laser_point_g", "laser_point", "outhall_g0", "outhall", "outstrain_g", "outstrain", "inhall_g0", "inhall_g1", "inhall", "instrain_g", "instrain", "hit_g", "hit", "break", "budd_g", "budd01", "budd02", "corehit_g", "corehit", "wall", "metamor_g", "metamor" };
            List<string> vehicle_bikeParticles = new() { "run_low", "run_mid", "run_high", "accele", "r_accele", "brake", "clash", "damage_fire", "damage_smoke", "land", "glind", "reload", "broken" };
            List<string> vehicle_hoverParticles = new() { "run_low", "run_mid", "run_high", "accele", "brake", "clash", "damage_fire", "damage_smoke", "land", "h_jump", "hover", "reload", "broken" };
            List<string> vehicle_jeepParticles = new() { "run_low", "run_mid", "run_high", "accele", "r_accele", "brake", "clash", "damage_fire", "damage_smoke", "land", "reload", "broken" };
            List<string> vehicle_jetgriderParticles = new() { "run_low", "accele", "contrail", "r_accele", "clash", "damage_fire", "damage_smoke", "reload", "broken" };

            // Select the particle bank to use.
            setObject.Parameters[0].Data = SetParticleBanks[MainWindow.Randomiser.Next(SetParticleBanks.Count)];

            // Select the particle to use depending on the bank.
            switch (setObject.Parameters[0].Data.ToString())
            {
                case "ecannon": setObject.Parameters[1].Data = ecannonParticles[MainWindow.Randomiser.Next(ecannonParticles.Count)]; break;
                case "ecerberus": setObject.Parameters[1].Data = ecerberusParticles[MainWindow.Randomiser.Next(ecerberusParticles.Count)]; break;
                case "egenesis": setObject.Parameters[1].Data = egenesisParticles[MainWindow.Randomiser.Next(egenesisParticles.Count)]; break;
                case "egunner": setObject.Parameters[1].Data = egunnerParticles[MainWindow.Randomiser.Next(egunnerParticles.Count)]; break;
                case "enemy_common": setObject.Parameters[1].Data = enemy_commonParticles[MainWindow.Randomiser.Next(enemy_commonParticles.Count)]; break;
                case "ewyvern": setObject.Parameters[1].Data = ewyvernParticles[MainWindow.Randomiser.Next(ewyvernParticles.Count)]; break;
                case "iblis01": setObject.Parameters[1].Data = iblis01Particles[MainWindow.Randomiser.Next(iblis01Particles.Count)]; break;
                case "iblis02": setObject.Parameters[1].Data = iblis02Particles[MainWindow.Randomiser.Next(iblis02Particles.Count)]; break;
                case "iblis03": setObject.Parameters[1].Data = iblis03Particles[MainWindow.Randomiser.Next(iblis03Particles.Count)]; break;
                case "kdv_scaffold01": setObject.Parameters[1].Data = kdv_scaffold01Particles[MainWindow.Randomiser.Next(kdv_scaffold01Particles.Count)]; break;
                case "map_aqa": setObject.Parameters[1].Data = map_aqaParticles[MainWindow.Randomiser.Next(map_aqaParticles.Count)]; break;
                case "map_common": setObject.Parameters[1].Data = map_commonParticles[MainWindow.Randomiser.Next(map_commonParticles.Count)]; break;
                case "map_csc": setObject.Parameters[1].Data = map_cscParticles[MainWindow.Randomiser.Next(map_cscParticles.Count)]; break;
                case "map_dtd": setObject.Parameters[1].Data = map_dtdParticles[MainWindow.Randomiser.Next(map_dtdParticles.Count)]; break;
                case "map_end": setObject.Parameters[1].Data = map_endParticles[MainWindow.Randomiser.Next(map_endParticles.Count)]; break;
                case "map_flc": setObject.Parameters[1].Data = map_flcParticles[MainWindow.Randomiser.Next(map_flcParticles.Count)]; break;
                case "map_iblis01": setObject.Parameters[1].Data = map_iblis01Particles[MainWindow.Randomiser.Next(map_iblis01Particles.Count)]; break;
                case "map_iblis02": setObject.Parameters[1].Data = map_iblis02Particles[MainWindow.Randomiser.Next(map_iblis02Particles.Count)]; break;
                case "map_iblis03": setObject.Parameters[1].Data = map_iblis03Particles[MainWindow.Randomiser.Next(map_iblis03Particles.Count)]; break;
                case "map_kdv": setObject.Parameters[1].Data = map_kdvParticles[MainWindow.Randomiser.Next(map_kdvParticles.Count)]; break;
                case "map_rct": setObject.Parameters[1].Data = map_rctParticles[MainWindow.Randomiser.Next(map_rctParticles.Count)]; break;
                case "map_tpj": setObject.Parameters[1].Data = map_tpjParticles[MainWindow.Randomiser.Next(map_tpjParticles.Count)]; break;
                case "map_twn": setObject.Parameters[1].Data = map_twnParticles[MainWindow.Randomiser.Next(map_twnParticles.Count)]; break;
                case "map_wap": setObject.Parameters[1].Data = map_wapParticles[MainWindow.Randomiser.Next(map_wapParticles.Count)]; break;
                case "map_wvo": setObject.Parameters[1].Data = map_wvoParticles[MainWindow.Randomiser.Next(map_wvoParticles.Count)]; break;
                case "mefiress": setObject.Parameters[1].Data = mefiressParticles[MainWindow.Randomiser.Next(mefiressParticles.Count)]; break;
                case "player_amy": setObject.Parameters[1].Data = player_amyParticles[MainWindow.Randomiser.Next(player_amyParticles.Count)]; break;
                case "player_blaze": setObject.Parameters[1].Data = player_blazeParticles[MainWindow.Randomiser.Next(player_blazeParticles.Count)]; break;
                case "player_himesonic": setObject.Parameters[1].Data = player_himesonicParticles[MainWindow.Randomiser.Next(player_himesonicParticles.Count)]; break;
                case "player_knuckles": setObject.Parameters[1].Data = player_knucklesParticles[MainWindow.Randomiser.Next(player_knucklesParticles.Count)]; break;
                case "player_omega": setObject.Parameters[1].Data = player_omegaParticles[MainWindow.Randomiser.Next(player_omegaParticles.Count)]; break;
                case "player_rouge": setObject.Parameters[1].Data = player_rougeParticles[MainWindow.Randomiser.Next(player_rougeParticles.Count)]; break;
                case "player_shadow": setObject.Parameters[1].Data = player_shadowParticles[MainWindow.Randomiser.Next(player_shadowParticles.Count)]; break;
                case "player_silver": setObject.Parameters[1].Data = player_silverParticles[MainWindow.Randomiser.Next(player_silverParticles.Count)]; break;
                case "player_sonic": setObject.Parameters[1].Data = player_sonicParticles[MainWindow.Randomiser.Next(player_sonicParticles.Count)]; break;
                case "player_tails": setObject.Parameters[1].Data = player_tailsParticles[MainWindow.Randomiser.Next(player_tailsParticles.Count)]; break;
                case "solaris": setObject.Parameters[1].Data = solarisParticles[MainWindow.Randomiser.Next(solarisParticles.Count)]; break;
                case "vehicle_bike": setObject.Parameters[1].Data = vehicle_bikeParticles[MainWindow.Randomiser.Next(vehicle_bikeParticles.Count)]; break;
                case "vehicle_hover": setObject.Parameters[1].Data = vehicle_hoverParticles[MainWindow.Randomiser.Next(vehicle_hoverParticles.Count)]; break;
                case "vehicle_jeep": setObject.Parameters[1].Data = vehicle_jeepParticles[MainWindow.Randomiser.Next(vehicle_jeepParticles.Count)]; break;
                case "vehicle_jetgrider": setObject.Parameters[1].Data = vehicle_jetgriderParticles[MainWindow.Randomiser.Next(vehicle_jetgriderParticles.Count)]; break;
            }
        }

        /// <summary>
        /// Switches out Jump Panels with the unused (and infinitely less useful) Jump Boards.
        /// </summary>
        /// <param name="setObject">The object we're editing.</param>
        /// <param name="jumpboardChance">The chance for a Jump Panel to be switched.</param>
        static async Task JumpboardSwitcher(SetObject setObject, int jumpboardChance)
        {
            if (MainWindow.Randomiser.Next(0, 101) <= jumpboardChance)
            {
                // Change the Jump Panel's object type.
                setObject.Type = "common_jumpboard";

                // Remove the Jump Panel's target parameter.
                setObject.Parameters.RemoveAt(3);

                // Add the speed_max and speed_mid values.
                SetParameter param = new()
                {
                    Data = 1000f,
                    Type = ObjectDataType.Single
                };
                setObject.Parameters.Add(param);

                param = new()
                {
                    Data = 250f,
                    Type = ObjectDataType.Single
                };
                setObject.Parameters.Add(param);
            }
        }

        /// <summary>
        /// Patch boss scripts (also includes enemy scripts because hmm) so they don't screw with the camera if randomised into regular enemies.
        /// Also used to change the voice lines used during the fights.
        /// </summary>
        /// <param name="luaFile">The lua binary we're processing.</param>
        /// <param name="enemies">Whether enemy randomisation is on.</param>
        /// <param name="hints">Whether hint randomisation is on.</param>
        /// <param name="SetHints">The list of valid hint voice lines.</param>
        /// <param name="SetEnemies">The list of valid enemy types (used to prevent unneeded commenting on enemies that aren't part of the randomisation).</param>
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
                // Iblis and Mephiles are stupid and have their Luas named different to the enemy type, so I have to check for those specifically *grumble grumble*.
                if ((lua[i].Contains("CallSetCamera") || lua[i].Contains("CallMoveTargetPos") || lua[i].Contains("FirstMefiress_RandomWarp")) && enemies == true &&
                     (SetEnemies.Contains(Path.GetFileNameWithoutExtension(luaFile)) || (Path.GetFileNameWithoutExtension(luaFile) == "Iblis01" && SetEnemies.Contains("firstiblis")) ||
                     (Path.GetFileNameWithoutExtension(luaFile) == "Iblis02" && SetEnemies.Contains("secondiblis")) ||
                     (Path.GetFileNameWithoutExtension(luaFile) == "Iblis03" && SetEnemies.Contains("thirdiblis")) ||
                     (Path.GetFileNameWithoutExtension(luaFile) == "firstmefiress_omega" && SetEnemies.Contains("firstmefiress")) ||
                     (Path.GetFileNameWithoutExtension(luaFile) == "firstmefiress_shadow" && SetEnemies.Contains("firstmefiress"))))
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

        /// <summary>
        /// Patches the stage lua binaries to load all the particle containers at once.
        /// </summary>
        /// <param name="luaFile">The lua file we're processing.</param>
        public static async Task ParticlePatch(string luaFile)
        {
            // Decompile this lua file.
            await Task.Run(() => Helpers.LuaDecompile(luaFile));

            // Read the decompiled lua file into a string array.
            string[] lua = File.ReadAllLines(luaFile);

            // Loop through each line in this lua binary. We'll do this twice, as some already use this function, so let's not add it a second time.
            bool foundParticle = false;
            for (int i = 0; i < lua.Length; i++)
            {
                if (lua[i].Contains("Game.LoadParticle"))
                {
                    // Set our indicator that this lua already had a particle bank loaded.
                    foundParticle = true;

                    // Replace the particle bank load with a long set of lines to load all the PLC files.
                    lua[i] = "    Game.LoadParticle(\"particle/ecannon.plc\"),\n    Game.LoadParticle(\"particle/ecerberus.plc\"),\n    Game.LoadParticle(\"particle/egenesis.plc\"),\n    Game.LoadParticle(\"particle/egunner.plc\"),\n    Game.LoadParticle(\"particle/enemy_common.plc\"),\n    Game.LoadParticle(\"particle/ewyvern.plc\"),\n    Game.LoadParticle(\"particle/iblis01.plc\"),\n    Game.LoadParticle(\"particle/iblis02.plc\"),\n    Game.LoadParticle(\"particle/iblis03.plc\"),\n    Game.LoadParticle(\"particle/kdv_scaffold01.plc\"),\n    Game.LoadParticle(\"particle/map_aqa.plc\"),\n    Game.LoadParticle(\"particle/map_common.plc\"),\n    Game.LoadParticle(\"particle/map_csc.plc\"),\n    Game.LoadParticle(\"particle/map_dtd.plc\"),\n    Game.LoadParticle(\"particle/map_end.plc\"),\n    Game.LoadParticle(\"particle/map_flc.plc\"),\n    Game.LoadParticle(\"particle/map_iblis01.plc\"),\n    Game.LoadParticle(\"particle/map_iblis02.plc\"),\n    Game.LoadParticle(\"particle/map_iblis03.plc\"),\n    Game.LoadParticle(\"particle/map_kdv.plc\"),\n    Game.LoadParticle(\"particle/map_rct.plc\"),\n    Game.LoadParticle(\"particle/map_tpj.plc\"),\n    Game.LoadParticle(\"particle/map_twn.plc\"),\n    Game.LoadParticle(\"particle/map_wap.plc\"),\n    Game.LoadParticle(\"particle/map_wvo.plc\"),\n    Game.LoadParticle(\"particle/mefiress.plc\"),\n    Game.LoadParticle(\"particle/player_amy.plc\"),\n    Game.LoadParticle(\"particle/player_blaze.plc\"),\n    Game.LoadParticle(\"particle/player_himesonic.plc\"),\n    Game.LoadParticle(\"particle/player_knuckles.plc\"),\n    Game.LoadParticle(\"particle/player_omega.plc\"),\n    Game.LoadParticle(\"particle/player_rouge.plc\"),\n    Game.LoadParticle(\"particle/player_shadow.plc\"),\n    Game.LoadParticle(\"particle/player_silver.plc\"),\n    Game.LoadParticle(\"particle/player_sonic.plc\"),\n    Game.LoadParticle(\"particle/player_tails.plc\"),\n    Game.LoadParticle(\"particle/solaris.plc\"),\n    Game.LoadParticle(\"particle/vehicle_bike.plc\"),\n    Game.LoadParticle(\"particle/vehicle_hover.plc\"),\n    Game.LoadParticle(\"particle/vehicle_jeep.plc\"),\n    Game.LoadParticle(\"particle/vehicle_jetgrider.plc\"),";
                }
            }

            // If we haven't found an existing Game.LoadSky() function, then loop through and create one.
            if (!foundParticle)
            {
                for (int i = 0; i < lua.Length; i++)
                {
                    // Use the AddComponent call as a donor, as those always seem to be on the block that controls terrain loading.
                    if (lua[i].Contains("_ARG_0_:AddComponent({"))
                    {
                        // Add a laod of new lines under the AddComponent call with the Game.LoadParticle() function and each PLC file.
                        lua[i] += $"\n    Game.LoadParticle(\"particle/ecannon.plc\"),\n    Game.LoadParticle(\"particle/ecerberus.plc\"),\n    Game.LoadParticle(\"particle/egenesis.plc\"),\n    Game.LoadParticle(\"particle/egunner.plc\"),\n    Game.LoadParticle(\"particle/enemy_common.plc\"),\n    Game.LoadParticle(\"particle/ewyvern.plc\"),\n    Game.LoadParticle(\"particle/iblis01.plc\"),\n    Game.LoadParticle(\"particle/iblis02.plc\"),\n    Game.LoadParticle(\"particle/iblis03.plc\"),\n    Game.LoadParticle(\"particle/kdv_scaffold01.plc\"),\n    Game.LoadParticle(\"particle/map_aqa.plc\"),\n    Game.LoadParticle(\"particle/map_common.plc\"),\n    Game.LoadParticle(\"particle/map_csc.plc\"),\n    Game.LoadParticle(\"particle/map_dtd.plc\"),\n    Game.LoadParticle(\"particle/map_end.plc\"),\n    Game.LoadParticle(\"particle/map_flc.plc\"),\n    Game.LoadParticle(\"particle/map_iblis01.plc\"),\n    Game.LoadParticle(\"particle/map_iblis02.plc\"),\n    Game.LoadParticle(\"particle/map_iblis03.plc\"),\n    Game.LoadParticle(\"particle/map_kdv.plc\"),\n    Game.LoadParticle(\"particle/map_rct.plc\"),\n    Game.LoadParticle(\"particle/map_tpj.plc\"),\n    Game.LoadParticle(\"particle/map_twn.plc\"),\n    Game.LoadParticle(\"particle/map_wap.plc\"),\n    Game.LoadParticle(\"particle/map_wvo.plc\"),\n    Game.LoadParticle(\"particle/mefiress.plc\"),\n    Game.LoadParticle(\"particle/player_amy.plc\"),\n    Game.LoadParticle(\"particle/player_blaze.plc\"),\n    Game.LoadParticle(\"particle/player_himesonic.plc\"),\n    Game.LoadParticle(\"particle/player_knuckles.plc\"),\n    Game.LoadParticle(\"particle/player_omega.plc\"),\n    Game.LoadParticle(\"particle/player_rouge.plc\"),\n    Game.LoadParticle(\"particle/player_shadow.plc\"),\n    Game.LoadParticle(\"particle/player_silver.plc\"),\n    Game.LoadParticle(\"particle/player_sonic.plc\"),\n    Game.LoadParticle(\"particle/player_tails.plc\"),\n    Game.LoadParticle(\"particle/solaris.plc\"),\n    Game.LoadParticle(\"particle/vehicle_bike.plc\"),\n    Game.LoadParticle(\"particle/vehicle_hover.plc\"),\n    Game.LoadParticle(\"particle/vehicle_jeep.plc\"),\n    Game.LoadParticle(\"particle/vehicle_jetgrider.plc\"),";
                    }
                }
            }

            // Save the updated lua binary.
            File.WriteAllLines(luaFile, lua);
        }

        /// <summary>
        /// Creates and returns a '06 SET Object Parameter.
        /// </summary>
        /// <param name="value">The value of this parameter.</param>
        /// <param name="type">The type of this parameter.</param>
        private static SetParameter ParameterCreate(object value, ObjectDataType type)
        {
            SetParameter parameter = new SetParameter()
            {
                Data = value,
                Type = type
            };
            return parameter;
        }
    
        /// <summary>
        /// Patches the PathObj.bin file to add the stuff needed for the event volume position model.
        /// Also copies the resources for the model to their positions in object.arc.
        /// </summary>
        /// <param name="archivePath">The path to the already extracted object.arc.</param>
        public static async Task PathObjPatcher(string archivePath)
        {
            // Load PathObj.bin.
            PathPackage PathObj = new($@"{archivePath}\xenon\object\PathObj.bin");

            // Create our new PathObj.bin entry.
            PathObject obj = new()
            {
                Model = "object/rando/eventindicator/rando_obj_indicator.xno",
                Name = "EventIndicator",
                Text = "meshes/object/rando/sonicindicator.TXT"
            };

            // Add our new PathObj.bin entry to the file.
            PathObj.PathObjects.Add(obj);

            // Save the updates PathObj.bin.
            PathObj.Save();

            // Create the directory for the model's assets.
            Directory.CreateDirectory($@"{archivePath}\win32\object\rando\eventindicator");

            // Copy the assets to the created directroy.
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\crate_iron_normal.dds", $@"{archivePath}\win32\object\rando\eventindicator\crate_iron_normal.dds", true);
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\crate_iron_switch.dds", $@"{archivePath}\win32\object\rando\eventindicator\crate_iron_switch.dds", true);
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\rando_obj_indicator.xno", $@"{archivePath}\win32\object\rando\eventindicator\rando_obj_indicator.xno", true);
        }
    }
}
