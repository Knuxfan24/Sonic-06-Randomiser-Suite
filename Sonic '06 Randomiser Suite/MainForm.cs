using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Sonic_06_Randomiser_Suite
{
    public partial class Form_Main : Form
    {
        // Set up the Random function.
        public static Random Randomiser = new();

        /// <summary>
        /// Standard WinForms setup.
        /// </summary>
        public Form_Main()
        {
            InitializeComponent();
            SetDefaults();
        }

        /// <summary>
        /// Sets some default values, mostly in regards to the CheckedListBoxes. 
        /// </summary>
        private void SetDefaults()
        {
            // Load the mods directory and game executable paths from the settings.
            TextBox_General_ModsDirectory.Text  = Properties.Settings.Default.modsDirectory;
            TextBox_General_GameExecutable.Text = Properties.Settings.Default.gameExecutable;

            // Generate a random seed.
            TextBox_General_Seed.Text = Randomiser.Next().ToString();

            // Enemy Types.
            for(int i = 0; i < 32; i++)
                CheckedList_SET_Enemies.SetItemChecked(i, true);

            // Character Types.
            CheckedList_SET_Characters.SetItemChecked(0, true);
            // Skip Sonic's alternate states.
            for (int i = 5; i < 13; i++)
                CheckedList_SET_Characters.SetItemChecked(i, true);

            // Item Types.
            for (int i = 0; i < CheckedList_SET_Items.Items.Count; i++)
                CheckedList_SET_Items.SetItemChecked(i, true);

            // Prop Types.
            for (int i = 0; i < CheckedList_SET_CommonProps.Items.Count; i++)
            {
                // Don't include the brk props.
                if(!CheckedList_SET_CommonProps.Items[i].ToString().ToLower().Contains("brk"))
                    CheckedList_SET_CommonProps.SetItemChecked(i, true);
            }
            for (int i = 0; i < CheckedList_SET_PathProps.Items.Count; i++)
                CheckedList_SET_PathProps.SetItemChecked(i, true);

            // Door Types.
            for (int i = 0; i < CheckedList_SET_Doors.Items.Count; i++)
                CheckedList_SET_Doors.SetItemChecked(i, true);

            // Hint Lines.
            for (int i = 0; i < CheckedList_SET_Voices.Items.Count; i++)
                CheckedList_SET_Voices.SetItemChecked(i, true);

            // Lighting.
            for (int i = 0; i < CheckedList_Event_Lighting.Items.Count; i++)
                CheckedList_Event_Lighting.SetItemChecked(i, true);

            // Terrain.
            for (int i = 0; i < CheckedList_Event_Terrain.Items.Count; i++)
                CheckedList_Event_Terrain.SetItemChecked(i, true);

            // Environment Maps.
            for (int i = 0; i < CheckedList_Scene_EnvMaps.Items.Count; i++)
                CheckedList_Scene_EnvMaps.SetItemChecked(i, true);

            // Music.
            for (int i = 0; i < CheckedList_Misc_Songs.Items.Count; i++)
                CheckedList_Misc_Songs.SetItemChecked(i, true);

            // Languages.
            CheckedList_Misc_Languages.SetItemChecked(0, true);
        }

        #region Configuration
        /// <summary>
        /// Saves a configuration file.
        /// </summary>
        /// <param name="location">Where to save the config to.</param>
        private void SaveConfig(string location)
        {
            using (StreamWriter configInfo = new(File.Open(location, FileMode.Create)))
            {
                // Basic Header, used to identify in the loading process.
                configInfo.WriteLine($"[Sonic '06 Randomiser Suite Configuration File]");
                configInfo.WriteLine("");

                // General Block.
                configInfo.WriteLine($"[General]");
                configInfo.WriteLine($"generalModsDir={TextBox_General_ModsDirectory.Text}");
                configInfo.WriteLine($"generalGameExe={TextBox_General_GameExecutable.Text}");
                configInfo.WriteLine($"generalSeed={TextBox_General_Seed.Text}");
                configInfo.WriteLine("");

                // Object Placement Block.
                configInfo.WriteLine($"[Object Placement]");
                configInfo.WriteLine($"setEnemies={CheckBox_SET_Enemies.Checked}");
                configInfo.WriteLine(CheckListEnumerators.Config("setEnemyTypes", CheckedList_SET_Enemies));
                configInfo.WriteLine($"setEnemyBehaviour={CheckBox_SET_Behaviours.Checked}");
                configInfo.WriteLine($"setNoBehaviourEnforce={CheckBox_SET_AllBehaviours.Checked}");
                configInfo.WriteLine($"setCharacters={CheckBox_SET_Characters.Checked}");
                configInfo.WriteLine(CheckListEnumerators.Config("setCharacterTypes", CheckedList_SET_Characters));
                configInfo.WriteLine($"setItems={CheckBox_SET_Items.Checked}");
                configInfo.WriteLine(CheckListEnumerators.Config("setItemTypes", CheckedList_SET_Items));
                configInfo.WriteLine($"setCommonProps={CheckBox_SET_CommonProps.Checked}");
                configInfo.WriteLine($"setPathProps={CheckBox_SET_PathProps.Checked}");
                configInfo.WriteLine(CheckListEnumerators.Config("setCommonPropTypes", CheckedList_SET_CommonProps));
                configInfo.WriteLine(CheckListEnumerators.Config("setPathPropTypes", CheckedList_SET_PathProps));
                configInfo.WriteLine($"setVoice={CheckBox_SET_Voices.Checked}");
                configInfo.WriteLine(CheckListEnumerators.Config("setVoiceTypes", CheckedList_SET_Voices));
                configInfo.WriteLine($"setDoors={CheckBox_SET_Doors.Checked}");
                configInfo.WriteLine(CheckListEnumerators.Config("setDoorTypes", CheckedList_SET_Doors));
                configInfo.WriteLine($"setDrawDistance={CheckBox_SET_DrawDistance.Checked}");
                configInfo.WriteLine($"setDrawDistanceMin={Numeric_SET_DrawDistanceMin.Value}");
                configInfo.WriteLine($"setDrawDistanceMax={Numeric_SET_DrawDistanceMax.Value}");
                configInfo.WriteLine($"setCosmetic={CheckBox_SET_Cosmetic.Checked}");
                configInfo.WriteLine("");

                // Event Block.
                configInfo.WriteLine($"[Event]");
                configInfo.WriteLine($"eventLighting={CheckBox_Event_Scene.Checked}");
                configInfo.WriteLine(CheckListEnumerators.Config("eventLightingLuas", CheckedList_Event_Lighting));
                configInfo.WriteLine($"eventRotX={CheckBox_Event_RotationX.Checked}");
                configInfo.WriteLine($"eventRotY={CheckBox_Event_RotationY.Checked}");
                configInfo.WriteLine($"eventRotZ={CheckBox_Event_RotationZ.Checked}");
                configInfo.WriteLine($"eventPosX={CheckBox_Event_PositionX.Checked}");
                configInfo.WriteLine($"eventPosY={CheckBox_Event_PositionY.Checked}");
                configInfo.WriteLine($"eventPosZ={CheckBox_Event_PositionZ.Checked}");
                configInfo.WriteLine($"eventVoice={CheckBox_Event_XMAs.Checked}");
                configInfo.WriteLine($"eventVoiceJpn={CheckBox_Event_XMAJapanese.Checked}");
                configInfo.WriteLine($"eventVoiceGameplay={CheckBox_Event_XMAGameplay.Checked}");
                configInfo.WriteLine($"eventTerrain={CheckBox_Event_Terrain.Checked}");
                configInfo.WriteLine(CheckListEnumerators.Config("eventTerrainList", CheckedList_Event_Terrain));
                configInfo.WriteLine($"eventOrder={CheckBox_Event_Order.Checked}");
                configInfo.WriteLine("");

                // Scene Block.
                configInfo.WriteLine($"[Scene]");
                configInfo.WriteLine($"sceneAmbient={CheckBox_Scene_Ambient.Checked}");
                configInfo.WriteLine($"sceneMain={CheckBox_Scene_Main.Checked}");
                configInfo.WriteLine($"sceneSub={CheckBox_Scene_Sub.Checked}");
                configInfo.WriteLine($"sceneLightDir={CheckBox_Scene_Direction.Checked}");
                configInfo.WriteLine($"sceneLightDirEnforce={CheckBox_Scene_DirectionEnforce.Checked}");
                configInfo.WriteLine($"sceneFogColour={CheckBox_Scene_FogColour.Checked}");
                configInfo.WriteLine($"sceneFogDensity={CheckBox_Scene_FogDensity.Checked}");
                configInfo.WriteLine($"sceneEnvMaps={CheckBox_Scene_EnvMaps.Checked}");
                configInfo.WriteLine(CheckListEnumerators.Config("sceneEnvMapTypes", CheckedList_Scene_EnvMaps));
                configInfo.WriteLine("");

                // Misc Block.
                configInfo.WriteLine($"[Misc]");
                configInfo.WriteLine($"miscMusic={CheckBox_Misc_Songs.Checked}");
                configInfo.WriteLine(CheckListEnumerators.Config("miscSongs", CheckedList_Misc_Songs));
                configInfo.WriteLine($"miscEnemyHealth={CheckBox_Misc_EnemyHealth.Checked}");
                configInfo.WriteLine($"miscEnemyHealthMin={Numeric_Misc_HealthMin.Value}");
                configInfo.WriteLine($"miscEnemyHealthMax={Numeric_Misc_HealthMax.Value}");
                configInfo.WriteLine($"miscBossHealth={CheckBox_Misc_BossHealth.Checked}");
                configInfo.WriteLine($"miscCollision={CheckBox_Misc_Surfaces.Checked}");
                configInfo.WriteLine($"miscCollisionFaces={CheckBox_Misc_SurfacesFaces.Checked}");
                configInfo.WriteLine($"miscText={Checkbox_Misc_Text.Checked}");
                configInfo.WriteLine(CheckListEnumerators.Config("miscLanguages", CheckedList_Misc_Languages));
                configInfo.WriteLine($"miscPatches={Checkbox_Misc_Patches.Checked}");
                configInfo.WriteLine("");

                // Custom Block.
                configInfo.WriteLine($"[Custom]");
                configInfo.WriteLine($"customMusic={TextBox_Custom_Music.Text}");
                configInfo.WriteLine($"customMusicCache={Checkbox_Custom_XMACache.Checked}");
                configInfo.WriteLine($"customVoxPacks={TextBox_Custom_Vox.Text}");

                // Finish.
                configInfo.Close();
            }
        }

        /// <summary>
        /// Loads a configuration file.
        /// </summary>
        /// <param name="location">The config file to load.</param>
        private void LoadConfig(string location)
        {
            // Read the config file into a string array.
            string[] config = File.ReadAllLines(location);

            // Check that the first thing in the config file is our header. If not, abort.
            if (config[0] != "[Sonic '06 Randomiser Suite Configuration File]")
                return;

            // Loop through each line in the string array.
            foreach (string setting in config)
            {
                // Ignore comment values (currently don't write any, but could do).
                if (setting.StartsWith(';'))
                    continue;

                // Split this line so we can get the key and the value(s).
                var split = setting.Split('=');

                // Determine what this line is.
                switch (split[0])
                {
                    // General Block
                    case "generalModsDir": TextBox_General_ModsDirectory.Text  = split[1]; break;
                    case "generalGameExe": TextBox_General_GameExecutable.Text = split[1]; break;
                    case "generalSeed":    TextBox_General_Seed.Text           = split[1]; break;

                    // Object Placement Block
                    case "setEnemies":            CheckBox_SET_Enemies.Checked = bool.Parse(split[1]);               break;
                    case "setEnemyTypes":         ConfigChecklist(CheckedList_SET_Enemies, split[1].Split(','));     break;
                    case "setEnemyBehaviour":     CheckBox_SET_Behaviours.Checked = bool.Parse(split[1]);            break;
                    case "setNoBehaviourEnforce": CheckBox_SET_AllBehaviours.Checked = bool.Parse(split[1]);         break;
                    case "setCharacters":         CheckBox_SET_Characters.Checked = bool.Parse(split[1]);            break;
                    case "setCharacterTypes":     ConfigChecklist(CheckedList_SET_Characters, split[1].Split(','));  break;
                    case "setItems":              CheckBox_SET_Items.Checked = bool.Parse(split[1]);                 break;
                    case "setItemTypes":          ConfigChecklist(CheckedList_SET_Items, split[1].Split(','));       break;
                    case "setCommonProps":        CheckBox_SET_CommonProps.Checked = bool.Parse(split[1]);           break;
                    case "setPathProps":          CheckBox_SET_PathProps.Checked = bool.Parse(split[1]);             break;
                    case "setCommonPropTypes":    ConfigChecklist(CheckedList_SET_CommonProps, split[1].Split(',')); break;
                    case "setPathPropTypes":      ConfigChecklist(CheckedList_SET_PathProps, split[1].Split(','));   break;
                    case "setVoice":              CheckBox_SET_Voices.Checked = bool.Parse(split[1]);                break;
                    case "setVoiceTypes":         ConfigChecklist(CheckedList_SET_Voices, split[1].Split(','));      break;
                    case "setDoors":              CheckBox_SET_Doors.Checked = bool.Parse(split[1]);                 break;
                    case "setDoorTypes":          ConfigChecklist(CheckedList_SET_Doors, split[1].Split(','));       break;
                    case "setDrawDistance":       CheckBox_SET_DrawDistance.Checked = bool.Parse(split[1]);          break;
                    case "setDrawDistanceMin":    Numeric_SET_DrawDistanceMin.Value = int.Parse(split[1]);           break;
                    case "setDrawDistanceMax":    Numeric_SET_DrawDistanceMax.Value = int.Parse(split[1]);           break;
                    case "setCosmetic":           CheckBox_SET_Cosmetic.Checked = bool.Parse(split[1]);              break;

                    // Event Block
                    case "eventLighting":      CheckBox_Event_Scene.Checked = bool.Parse(split[1]);              break;
                    case "eventLightingLuas":  ConfigChecklist(CheckedList_Event_Lighting, split[1].Split(',')); break;
                    case "eventRotX":          CheckBox_Event_RotationX.Checked = bool.Parse(split[1]);          break;
                    case "eventRotY":          CheckBox_Event_RotationY.Checked = bool.Parse(split[1]);          break;
                    case "eventRotZ":          CheckBox_Event_RotationZ.Checked = bool.Parse(split[1]);          break;
                    case "eventPosX":          CheckBox_Event_PositionX.Checked = bool.Parse(split[1]);          break;
                    case "eventPosY":          CheckBox_Event_PositionY.Checked = bool.Parse(split[1]);          break;
                    case "eventPosZ":          CheckBox_Event_PositionZ.Checked = bool.Parse(split[1]);          break;
                    case "eventVoice":         CheckBox_Event_XMAs.Checked = bool.Parse(split[1]);               break;
                    case "eventVoiceJpn":      CheckBox_Event_XMAJapanese.Checked = bool.Parse(split[1]);        break;
                    case "eventVoiceGameplay": CheckBox_Event_XMAGameplay.Checked = bool.Parse(split[1]);        break;
                    case "eventTerrain":       CheckBox_Event_Terrain.Checked = bool.Parse(split[1]);            break;
                    case "eventTerrainList":   ConfigChecklist(CheckedList_Event_Terrain, split[1].Split(','));  break;
                    case "eventOrder":         CheckBox_Event_Order.Checked = bool.Parse(split[1]);              break;

                    // Scene Block
                    case "sceneAmbient":         CheckBox_Scene_Ambient.Checked = bool.Parse(split[1]);           break;
                    case "sceneMain":            CheckBox_Scene_Main.Checked = bool.Parse(split[1]);              break;
                    case "sceneSub":             CheckBox_Scene_Sub.Checked = bool.Parse(split[1]);               break;
                    case "sceneLightDir":        CheckBox_Scene_Direction.Checked = bool.Parse(split[1]);         break;
                    case "sceneLightDirEnforce": CheckBox_Scene_DirectionEnforce.Checked = bool.Parse(split[1]);  break;
                    case "sceneFogColour":       CheckBox_Scene_FogColour.Checked = bool.Parse(split[1]);         break;
                    case "sceneFogDensity":      CheckBox_Scene_FogDensity.Checked = bool.Parse(split[1]);        break;
                    case "sceneEnvMaps":         CheckBox_Scene_EnvMaps.Checked = bool.Parse(split[1]);           break;
                    case "sceneEnvMapTypes":     ConfigChecklist(CheckedList_Scene_EnvMaps, split[1].Split(',')); break;

                    // Misc Block
                    case "miscMusic":          CheckBox_Misc_Songs.Checked = bool.Parse(split[1]);               break;
                    case "miscSongs":          ConfigChecklist(CheckedList_Misc_Songs, split[1].Split(','));     break;
                    case "miscEnemyHealth":    CheckBox_Misc_EnemyHealth.Checked = bool.Parse(split[1]);         break;
                    case "miscEnemyHealthMin": Numeric_Misc_HealthMin.Value = int.Parse(split[1]);               break;
                    case "miscEnemyHealthMax": Numeric_Misc_HealthMax.Value = int.Parse(split[1]);               break;
                    case "miscBossHealth":     CheckBox_Misc_EnemyHealth.Checked = bool.Parse(split[1]);         break;
                    case "miscCollision":      CheckBox_Misc_Surfaces.Checked = bool.Parse(split[1]);            break;
                    case "miscCollisionFaces": CheckBox_Misc_SurfacesFaces.Checked = bool.Parse(split[1]);       break;
                    case "miscText":           Checkbox_Misc_Text.Checked = bool.Parse(split[1]);                break;
                    case "miscLanguages":      ConfigChecklist(CheckedList_Misc_Languages, split[1].Split(',')); break;
                    case "miscPatches":        Checkbox_Misc_Patches.Checked = bool.Parse(split[1]);             break;

                    // Custom
                    case "customMusic":      TextBox_Custom_Music.Text = split[1];                    break;
                    case "customMusicCache": Checkbox_Custom_XMACache.Checked = bool.Parse(split[1]); break;
                    case "customVoxPacks":   TextBox_Custom_Vox.Text = split[1];                      break;
                }
            }
        }

        /// <summary>
        /// Parse the numbers in a config key for a CheckListBox.
        /// </summary>
        /// <param name="list">The CheckedListBox to edit.</param>
        /// <param name="values">The list of values passed in from the load function.</param>
        private static void ConfigChecklist(CheckedListBox list, string[] values)
        {
            // Clear the CheckedListBox first.
            for (int i = 0; i < list.Items.Count; i++)
                list.SetItemChecked(i, false);

            // If the list of values is completely empty, don't try to check anything.
            if (values[0] == "")
                return;

            // Check the values present in the ini.
            for (int i = 0; i < values.Length; i++)
                list.SetItemChecked(int.Parse(values[i]), true);
        }
        #endregion

        #region Buttons and Checkboxes
        /// <summary>
        /// Open the Randomiser's Wiki in the user's browser of choice.
        /// </summary>
        private void Button_Documentation_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/Knuxfan24/Sonic-06-Randomiser-Suite/wiki",
                UseShellExecute = true
            });
        }

        /// <summary>
        /// Display a message box of credits.
        /// </summary>
        private void Button_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Credits:\n\n" +
                            "Knuxfan24: Development.\n" +
                            "HyperBE32: Marathon.\n" +
                            "ShadowLAG: Lua Decompilation.\n" +
                            "Mark Heath: NAudio.",
                            "Sonic '06 Randomiser Suite",
                            MessageBoxButtons.OK);
        }

        /// <summary>
        /// Saves the mods directory into the settings.
        /// </summary>
        private void TextBox_General_ModsDirectory_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.modsDirectory = TextBox_General_ModsDirectory.Text;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Browses for the user's Mods Directory.
        /// </summary>
        private void Button_General_ModsDirectory_Click(object sender, EventArgs e)
        {
            // Configure the Tool_FolderBrowser control with the settings for the mods directory.
            Tool_FolderBrowser.Description = "Select Mods Directory";

            // Set the mods directory textbox to the specified path if it is valid.
            if (Tool_FolderBrowser.ShowDialog() == DialogResult.OK)
                TextBox_General_ModsDirectory.Text = Tool_FolderBrowser.SelectedPath;
        }

        /// <summary>
        /// Saves the game executable path into the settings.
        /// </summary>
        private void TextBox_General_GameExecutable_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.gameExecutable = TextBox_General_GameExecutable.Text;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Browses for the user's game executable.
        /// </summary>
        private void Button_General_GameExecutable_Click(object sender, EventArgs e)
        {
            // Configure the Tool_FileDialog control with the settings for the game executable.
            Tool_FileDialog.Title = "Select Game Executable";
            Tool_FileDialog.Multiselect = false;
            Tool_FileDialog.Filter = "Xbox 360 Executable|default.xex|PlayStation 3 Executable|EBOOT.BIN";

            // Set the game executable textbox to the specified file if it is valid.
            if (Tool_FileDialog.ShowDialog() == DialogResult.OK)
                TextBox_General_GameExecutable.Text = Tool_FileDialog.FileName;
        }

        /// <summary>
        /// Generates a new random seed.
        /// </summary>
        private void Button_General_Seed_Click(object sender, EventArgs e)
        {
            // Set the seed textbox to a random number.
            TextBox_General_Seed.Text = Randomiser.Next().ToString();
        }

        /// <summary>
        /// Chooses where to save a configuration ini.
        /// </summary>
        private void Button_General_SaveConfig_Click(object sender, EventArgs e)
        {
            // Setup a SaveFileDialog with the settings for the randomiser config.
            SaveFileDialog configSaveBrowser = new()
            {
                Filter = "Randomiser Config (*.ini)|*.ini",
                RestoreDirectory = true
            };

            // Run the SaveConfig funciton if the file location specified is valid.
            if (configSaveBrowser.ShowDialog() == DialogResult.OK)
                SaveConfig(configSaveBrowser.FileName);
        }

        /// <summary>
        /// Chooses a configuration ini or Wildcard log file to be loaded.
        /// </summary>
        private void Button_General_LoadConfig_Click(object sender, EventArgs e)
        {
            // Configure the Tool_FileDialog control with the settings for the randomiser config.
            Tool_FileDialog.Title = "Select Configuration File";
            Tool_FileDialog.Multiselect = false;
            Tool_FileDialog.Filter = "Randomiser Config (*.ini)|*.ini|Wildcard Log (*.log)|*.log";

            // Run the LoadConfig funciton if the file location specified is valid.
            if (Tool_FileDialog.ShowDialog() == DialogResult.OK)
                LoadConfig(Tool_FileDialog.FileName);
        }

        /// <summary>
        /// Enable or disable the `Don't Enforce Behaviour Types` checkbox depending on the state of the `Randomise Enemy Behaviour` checkbox.
        /// </summary>
        private void CheckBox_SET_Behaviours_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_SET_Behaviours.Checked)
                CheckBox_SET_AllBehaviours.Enabled = true;

            if (!CheckBox_SET_Behaviours.Checked)
                CheckBox_SET_AllBehaviours.Enabled = false;
        }

        /// <summary>
        /// Selects all the enemy types in the Object Placement tab.
        /// </summary>
        private void Button_SET_EnemiesSelectAll_Click(object sender, EventArgs e)
        {
            // Loop through and check all the elements in the enemy list.
            for (int i = 0; i < CheckedList_SET_Enemies.Items.Count; i++)
                CheckedList_SET_Enemies.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselects all the enemy types in the Object Placement tab.
        /// </summary>
        private void Button_SET_EnemiesDeselectAll_Click(object sender, EventArgs e)
        {
            // Loop through and uncheck all the elements in the enemy list.
            for (int i = 0; i < CheckedList_SET_Enemies.Items.Count; i++)
                CheckedList_SET_Enemies.SetItemChecked(i, false);
        }

        /// <summary>
        /// Selects all the character types in the Object Placement tab.
        /// </summary>
        private void Button_SET_CharactersSelectAll_Click(object sender, EventArgs e)
        {
            // Loop through and check all the elements in the character list.
            for (int i = 0; i < CheckedList_SET_Characters.Items.Count; i++)
                CheckedList_SET_Characters.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselects all the character types in the Object Placement tab.
        /// </summary>
        private void Button_SET_CharactersDeselectAll_Click(object sender, EventArgs e)
        {
            // Loop through and uncheck all the elements in the character list.
            for (int i = 0; i < CheckedList_SET_Characters.Items.Count; i++)
                CheckedList_SET_Characters.SetItemChecked(i, false);
        }

        /// <summary>
        /// Selects all the item capsule types in the Object Placement tab.
        /// </summary>
        private void Button_SET_ItemsSelectAll_Click(object sender, EventArgs e)
        {
            // Loop through and check all the elements in the items list.
            for (int i = 0; i < CheckedList_SET_Items.Items.Count; i++)
                CheckedList_SET_Items.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselects all the item capsule types in the Object Placement tab.
        /// </summary>
        private void Button_SET_ItemsDeselectAll_Click(object sender, EventArgs e)
        {
            // Loop through and uncheck all the elements in the items list.
            for (int i = 0; i < CheckedList_SET_Items.Items.Count; i++)
                CheckedList_SET_Items.SetItemChecked(i, false);
        }

        /// <summary>
        /// Selects all the Common.bin based prop types in the Object Placement tab.
        /// </summary>
        private void Button_SET_CommonPropsSelectAll_Click(object sender, EventArgs e)
        {
            // Loop through and check all the elements in the common props list.
            for (int i = 0; i < CheckedList_SET_CommonProps.Items.Count; i++)
                CheckedList_SET_CommonProps.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselects all the Common.bin based prop types in the Object Placement tab.
        /// </summary>
        private void Button_SET_CommonPropsDeselectAll_Click(object sender, EventArgs e)
        {
            // Loop through and uncheck all the elements in the common props list.
            for (int i = 0; i < CheckedList_SET_CommonProps.Items.Count; i++)
                CheckedList_SET_CommonProps.SetItemChecked(i, false);
        }

        /// <summary>
        /// Selects all the PathObj.bin based prop types in the Object Placement tab.
        /// </summary>
        private void Button_SET_PathPropsSelectAll_Click(object sender, EventArgs e)
        {
            // Loop through and check all the elements in the path props list.
            for (int i = 0; i < CheckedList_SET_PathProps.Items.Count; i++)
                CheckedList_SET_PathProps.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselects all the PathObj.bin based prop types in the Object Placement tab.
        /// </summary>
        private void Button_SET_PathPropsDeselectAll_Click(object sender, EventArgs e)
        {
            // Loop through and uncheck all the elements in the path props list.
            for (int i = 0; i < CheckedList_SET_PathProps.Items.Count; i++)
                CheckedList_SET_PathProps.SetItemChecked(i, false);
        }

        /// <summary>
        /// Selects all the hint lines in the Object Placement tab.
        /// </summary>
        private void Button_SET_VoicesSelectAll_Click(object sender, EventArgs e)
        {
            // Loop through and check all the elements in the hints list.
            for (int i = 0; i < CheckedList_SET_Voices.Items.Count; i++)
                CheckedList_SET_Voices.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselects all the hint lines in the Object Placement tab.
        /// </summary>
        private void Button_SET_VoicesDeselectAll_Click(object sender, EventArgs e)
        {
            // Loop through and uncheck all the elements in the hints list.
            for (int i = 0; i < CheckedList_SET_Voices.Items.Count; i++)
                CheckedList_SET_Voices.SetItemChecked(i, false);
        }

        /// <summary>
        /// Selects all the door types in the Object Placement tab.
        /// </summary>
        private void Button_SET_DoorsSelectAll_Click(object sender, EventArgs e)
        {
            // Loop through and check all the elements in the doors list.
            for (int i = 0; i < CheckedList_SET_Doors.Items.Count; i++)
                CheckedList_SET_Doors.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselects all the door types in the Object Placement tab.
        /// </summary>
        private void Button_SET_DoorsDeselectAll_Click(object sender, EventArgs e)
        {
            // Loop through and uncheck all the elements in the doors list.
            for (int i = 0; i < CheckedList_SET_Doors.Items.Count; i++)
                CheckedList_SET_Doors.SetItemChecked(i, false);
        }

        /// <summary>
        /// Enable or disable the `Include Japanese Lines` and `Include Gameplay Lines` checkboxes depending on the state of the `Shuffle Event Voice Lines` checkbox.
        /// </summary>
        private void CheckBox_Event_XMAs_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_Event_XMAs.Checked)
            {
                CheckBox_Event_XMAJapanese.Enabled = true;
                CheckBox_Event_XMAGameplay.Enabled = true;
            }

            if (!CheckBox_Event_XMAs.Checked)
            {
                CheckBox_Event_XMAJapanese.Enabled = false;
                CheckBox_Event_XMAGameplay.Enabled = false;
            }
        }

        /// <summary>
        /// Selects all the scene luas in the Event tab.
        /// </summary>
        private void Button_Event_LightingSelectAll_Click(object sender, EventArgs e)
        {
            // Loop through and check all the elements in the scene luas list.
            for (int i = 0; i < CheckedList_Event_Lighting.Items.Count; i++)
                CheckedList_Event_Lighting.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselects all the scene luas in the Event tab.
        /// </summary>
        private void Button_Event_LightingDeselectAll_Click(object sender, EventArgs e)
        {
            // Loop through and uncheck all the elements in the scene luas list.
            for (int i = 0; i < CheckedList_Event_Lighting.Items.Count; i++)
                CheckedList_Event_Lighting.SetItemChecked(i, false);
        }

        /// <summary>
        /// Selects all the terrain folders in the Event tab.
        /// </summary>
        private void Button_Event_TerrainSelectAll_Click(object sender, EventArgs e)
        {
            // Loop through and check all the elements in the terrain list.
            for (int i = 0; i < CheckedList_Event_Terrain.Items.Count; i++)
                CheckedList_Event_Terrain.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselects all the terrain folders in the Event tab.
        /// </summary>
        private void Button_Event_TerrainDeselectAll_Click(object sender, EventArgs e)
        {
            // Loop through and uncheck all the elements in the terrain list.
            for (int i = 0; i < CheckedList_Event_Terrain.Items.Count; i++)
                CheckedList_Event_Terrain.SetItemChecked(i, false);
        }

        /// <summary>
        /// Enable or disable the `Enforce Light Above` checkbox depending on the state of the `Randomise Light Direction` checkbox.
        /// </summary>
        private void CheckBox_Scene_Direction_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_Scene_Direction.Checked)
                CheckBox_Scene_DirectionEnforce.Enabled = true;

            if (!CheckBox_Scene_Direction.Checked)
                CheckBox_Scene_DirectionEnforce.Enabled = false;
        }

        /// <summary>
        /// Selects all the environment map types in the Scene tab.
        /// </summary>
        private void Button_Scene_EnvMapsSelectAll_Click(object sender, EventArgs e)
        {
            // Loop through and check all the elements in the environment maps list.
            for (int i = 0; i < CheckedList_Scene_EnvMaps.Items.Count; i++)
                CheckedList_Scene_EnvMaps.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselects all the environment map types in the Scene tab.
        /// </summary>
        private void Button_Scene_EnvMapsDeselectAll_Click(object sender, EventArgs e)
        {
            // Loop through and uncheck all the elements in the environment maps list.
            for (int i = 0; i < CheckedList_Scene_EnvMaps.Items.Count; i++)
                CheckedList_Scene_EnvMaps.SetItemChecked(i, false);
        }

        /// <summary>
        /// Enable or disable the `Include Bosses` checkbox depending on the state of the `Randomise Enemy Health` checkbox.
        /// </summary>
        private void CheckBox_Misc_EnemyHealth_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_Misc_EnemyHealth.Checked)
                CheckBox_Misc_BossHealth.Enabled = true;

            if (!CheckBox_Misc_EnemyHealth.Checked)
                CheckBox_Misc_BossHealth.Enabled = false;
        }

        /// <summary>
        /// Enable or disable the `Randomise Per Face` checkbox depending on the state of the `Randomise Surface Types` checkbox.
        /// </summary>
        private void CheckBox_Misc_Surfaces_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_Misc_Surfaces.Checked)
                CheckBox_Misc_SurfacesFaces.Enabled = true;

            if (!CheckBox_Misc_Surfaces.Checked)
                CheckBox_Misc_SurfacesFaces.Enabled = false;
        }

        /// <summary>
        /// Selects all the songs in the Miscellaneous tab.
        /// </summary>
        private void Button_Misc_SongsSelectAll_Click(object sender, EventArgs e)
        {
            // Loop through and check all the elements in the songs list.
            for (int i = 0; i < CheckedList_Misc_Songs.Items.Count; i++)
                CheckedList_Misc_Songs.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselects all the songs in the Miscellaneous tab.
        /// </summary>
        private void Button_Misc_SongsDeselectAll_Click(object sender, EventArgs e)
        {
            // Loop through and uncheck all the elements in the songs list.
            for (int i = 0; i < CheckedList_Misc_Songs.Items.Count; i++)
                CheckedList_Misc_Songs.SetItemChecked(i, false);
        }

        /// <summary>
        /// Selects all the languages in the Miscellaneous tab.
        /// </summary>
        private void Button_Misc_LanguagesSelectAll_Click(object sender, EventArgs e)
        {
            // Loop through and check all the elements in the languages list.
            for (int i = 0; i < CheckedList_Misc_Languages.Items.Count; i++)
                CheckedList_Misc_Languages.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselects all the languages in the Miscellaneous tab.
        /// </summary>
        private void Button_Misc_LanguagesDeselectAll_Click(object sender, EventArgs e)
        {
            // Loop through and uncheck all the elements in the languages list.
            for (int i = 0; i < CheckedList_Misc_Languages.Items.Count; i++)
                CheckedList_Misc_Languages.SetItemChecked(i, false);
        }

        /// <summary>
        /// Browses for custom music files to add to the randomisation process.
        /// </summary>
        private void Button_Custom_Music_Click(object sender, EventArgs e)
        {
            // Configure the Tool_FileDialog control with the settings for the custom music.
            Tool_FileDialog.Title = "Select Songs";
            Tool_FileDialog.Multiselect = true;
            Tool_FileDialog.Filter = "All Supported Types|*.wav;*.mp3;*.m4a;*.xma|Waveform Audio File|*.wav|MP3 Audio File|*.mp3|MPEG4 Audio File|*.m4a|Xbox Media Audio File|*.xma";

            // If the selections are valid, add them to the list of text in the custom music textbox.
            if (Tool_FileDialog.ShowDialog() == DialogResult.OK)
            {
                // Don't erase the box, just add a seperator.
                if (TextBox_Custom_Music.Text.Length != 0)
                    TextBox_Custom_Music.Text += "|";

                // Add selected files to the text box.
                for (int i = 0; i < Tool_FileDialog.FileNames.Length; i++)
                    TextBox_Custom_Music.Text += $"{Tool_FileDialog.FileNames[i]}|";

                // Remove the extra comma added at the end.
                TextBox_Custom_Music.Text = TextBox_Custom_Music.Text.Remove(TextBox_Custom_Music.Text.LastIndexOf('|'));
            }
        }

        /// <summary>
        /// Browses for voice pack zip files to add to the randomisation process.
        /// </summary>
        private void Button_Custom_Vox_Click(object sender, EventArgs e)
        {
            // Configure the Tool_FileDialog control with the settings for the voice packs.
            Tool_FileDialog.Title = "Select Voice Packs";
            Tool_FileDialog.Multiselect = true;
            Tool_FileDialog.Filter = "Voice Pack ZIP Archive|*.zip";

            // If the selections are valid, add them to the list of text in the voice packs textbox.
            if (Tool_FileDialog.ShowDialog() == DialogResult.OK)
            {
                // Don't erase the box, just add a seperator.
                if (TextBox_Custom_Vox.Text.Length != 0)
                    TextBox_Custom_Vox.Text += "|";

                // Add selected files to the text box.
                for (int i = 0; i < Tool_FileDialog.FileNames.Length; i++)
                    TextBox_Custom_Vox.Text += $"{Tool_FileDialog.FileNames[i]}|";

                // Remove the extra comma added at the end.
                TextBox_Custom_Vox.Text = TextBox_Custom_Vox.Text.Remove(TextBox_Custom_Vox.Text.LastIndexOf('|'));
            }
        }
        #endregion

        #region Wildcard Parsing
        /// <summary>
        /// Checks or unchecks a CheckBox element based on Wildcard Weight.
        /// </summary>
        /// <param name="checkBox">The CheckBox element to affect.</param>
        private void WildcardCheckbox(CheckBox checkBox)
        {
            // Generate a random number from 0, 100. If it's smaller than the Wildcard's Weight value, then check the CheckBox element, if not, uncheck it.
            if (Randomiser.Next(0, 101) < Numerical_General_WildcardWeight.Value)
                checkBox.Checked = true;
            else
                checkBox.Checked = false;
        }

        /// <summary>
        /// Checks and unchecks various elements in a CheckedListBox element based on Wildcard Weight.
        /// </summary>
        /// <param name="checkedList">The CheckedListBox element to affect.</param>
        private void WildcardListBox(CheckedListBox checkedList)
        {
            // Loop through all the items in the CheckedListBox element.
            for (int i = 0; i < checkedList.Items.Count; i++)
            {
                // Generate a random number from 0, 100. If it's smaller than the Wildcard's Weight value, then check the item, if not, uncheck it.
                if (Randomiser.Next(0, 101) < Numerical_General_WildcardWeight.Value)
                    checkedList.SetItemChecked(i, true);
                else
                    checkedList.SetItemChecked(i, false);
            }
        }
        #endregion

        /// <summary>
        /// Actually do the Randomisation.
        /// </summary>
        private void Button_Randomise_Click(object sender, EventArgs e)
        {
            // Check that our mods directory and game executable actually exist.
            if (!Directory.Exists(TextBox_General_ModsDirectory.Text) || !File.Exists(TextBox_General_GameExecutable.Text))
            {
                MessageBox.Show("Either your Game Executable or Mods Directory don't exist, please check your general settings.",
                                "Sonic '06 Randomiser Suite",
                                MessageBoxButtons.OK);
                return;
            }

            // Set up a new Randomiser variable with the new seed.
            Randomiser = new Random(TextBox_General_Seed.Text.GetHashCode());

            // Get a list of all the archives based on the location of the game executable.
            string[] archives = Directory.GetFiles($@"{Path.GetDirectoryName(TextBox_General_GameExecutable.Text)}", "*.arc", SearchOption.AllDirectories);

            // Create Mod Directory (prompting the user if they want to delete it first or cancel if it already exists.)
            if (Directory.Exists($@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text})"))
            {
                DialogResult check = MessageBox.Show($"A mod with the seed {TextBox_General_Seed.Text} already exists.\nDo you want to replace it?",
                                             "Sonic '06 Randomiser Suite",
                                             MessageBoxButtons.YesNoCancel);

                if (check == DialogResult.Yes)
                    Directory.Delete($@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text})", true);

                if (check == DialogResult.Cancel)
                    return;
            }

            Directory.CreateDirectory($@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text})");

            // Create console window to show progress.
            NativeMethods.AllocConsole();

            // Write mod configuration ini.
            using (Stream configCreate = File.Open(Path.Combine($@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text})", "mod.ini"), FileMode.Create))
            using (StreamWriter configInfo = new(configCreate))
            {
                configInfo.WriteLine("[Details]");
                configInfo.WriteLine($"Title=\"Sonic '06 Randomised ({TextBox_General_Seed.Text})\"");
                configInfo.WriteLine($"Version=\"{TextBox_General_Seed.Text}\"");
                configInfo.WriteLine($"Date=\"{DateTime.Now:dd/MM/yyyy}\"");
                configInfo.WriteLine($"Author=\"Sonic '06 Randomiser Suite\"");

                if (TextBox_General_GameExecutable.Text.ToLower().EndsWith(".xex"))
                    configInfo.WriteLine($"Platform=\"Xbox 360\"");

                if (TextBox_General_GameExecutable.Text.ToLower().EndsWith(".bin"))
                    configInfo.WriteLine($"Platform=\"PlayStation 3\"");

                configInfo.WriteLine("\n[Filesystem]");
                configInfo.WriteLine($"Merge=\"False\"");
                configInfo.WriteLine($"CustomFilesystem=\"False\"");

                configInfo.Close();
            }

            // Wildcard Setup
            if (Checkbox_General_Wildcard.Checked)
            {
                // Backup configuration while we fuck with it.
                SaveConfig(Path.Combine($@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text})", "wildcard.ini"));

                // SET
                WildcardCheckbox(CheckBox_SET_Enemies);
                WildcardCheckbox(CheckBox_SET_Behaviours);
                WildcardCheckbox(CheckBox_SET_Characters);
                WildcardCheckbox(CheckBox_SET_Items);
                WildcardCheckbox(CheckBox_SET_CommonProps);
                WildcardCheckbox(CheckBox_SET_PathProps);

                // Force music randomisation on if custom songs are provided.
                if (TextBox_Custom_Vox.Text != "")
                    WildcardCheckbox(CheckBox_SET_Voices);
                else
                    CheckBox_SET_Voices.Checked = true;

                WildcardCheckbox(CheckBox_SET_Doors);
                WildcardCheckbox(CheckBox_SET_DrawDistance);
                WildcardCheckbox(CheckBox_SET_Cosmetic);

                WildcardListBox(CheckedList_SET_Enemies);
                WildcardListBox(CheckedList_SET_Characters);
                WildcardListBox(CheckedList_SET_Items);
                WildcardListBox(CheckedList_SET_CommonProps);
                WildcardListBox(CheckedList_SET_PathProps);
                WildcardListBox(CheckedList_SET_Voices);
                WildcardListBox(CheckedList_SET_Doors);

                // Event
                WildcardCheckbox(CheckBox_Event_Scene);
                WildcardCheckbox(CheckBox_Event_RotationX);
                WildcardCheckbox(CheckBox_Event_RotationY);
                WildcardCheckbox(CheckBox_Event_RotationZ);
                WildcardCheckbox(CheckBox_Event_PositionX);
                WildcardCheckbox(CheckBox_Event_PositionY);
                WildcardCheckbox(CheckBox_Event_PositionZ);
                WildcardCheckbox(CheckBox_Event_XMAs);
                WildcardCheckbox(CheckBox_Event_XMAJapanese);
                WildcardCheckbox(CheckBox_Event_XMAGameplay);
                WildcardCheckbox(CheckBox_Event_Terrain);
                WildcardCheckbox(CheckBox_Event_Order);

                WildcardListBox(CheckedList_Event_Lighting);
                WildcardListBox(CheckedList_Event_Terrain);

                // Scene
                WildcardCheckbox(CheckBox_Scene_Ambient);
                WildcardCheckbox(CheckBox_Scene_Main);
                WildcardCheckbox(CheckBox_Scene_Sub);
                WildcardCheckbox(CheckBox_Scene_Direction);
                WildcardCheckbox(CheckBox_Scene_DirectionEnforce);
                WildcardCheckbox(CheckBox_Scene_FogColour);
                WildcardCheckbox(CheckBox_Scene_FogDensity);
                WildcardCheckbox(CheckBox_Scene_EnvMaps);

                WildcardListBox(CheckedList_Scene_EnvMaps);

                // Misc
                // Force music randomisation on if custom songs are provided.
                if (TextBox_Custom_Music.Text != "")
                    WildcardCheckbox(CheckBox_Misc_Songs);
                else
                    CheckBox_Misc_Songs.Checked = true;

                WildcardCheckbox(CheckBox_Misc_EnemyHealth);
                WildcardCheckbox(CheckBox_Misc_BossHealth);
                WildcardCheckbox(CheckBox_Misc_Surfaces);
                WildcardCheckbox(CheckBox_Misc_SurfacesFaces);
                WildcardCheckbox(Checkbox_Misc_Text);
                WildcardCheckbox(Checkbox_Misc_Patches);

                WildcardListBox(CheckedList_Misc_Songs);
                WildcardListBox(CheckedList_Misc_Languages);
            }

            // Enumerate the Checked List Boxes for the user's settings on lists.
            List<string> SetEnemies     = CheckListEnumerators.SET_EnumerateEnemiesList(CheckedList_SET_Enemies);
            List<string> SetCharacters  = CheckListEnumerators.SET_EnumerateCharactersList(CheckedList_SET_Characters);
            List<int>    SetItems       = CheckListEnumerators.SET_EnumerateItemsList(CheckedList_SET_Items);
            List<string> SetCommonProps = CheckListEnumerators.SET_EnumeratePropsList(CheckedList_SET_CommonProps);
            List<string> SetPathProps   = CheckListEnumerators.SET_EnumeratePropsList(CheckedList_SET_PathProps);
            List<string> SetVoices      = CheckListEnumerators.SET_EnumerateHintsList(CheckedList_SET_Voices);
            List<string> SetDoors       = CheckListEnumerators.SET_EnumerateDoorsList(CheckedList_SET_Doors);
            List<string> EventLighting  = CheckListEnumerators.Event_EnumerateLightingList(CheckedList_Event_Lighting);
            List<string> EventTerrain   = CheckListEnumerators.Event_EnumerateLightingList(CheckedList_Event_Terrain);
            List<string> SceneEnvMaps   = CheckListEnumerators.Scene_EnumerateEnvList(CheckedList_Scene_EnvMaps);
            List<string> MiscMusic      = CheckListEnumerators.Misc_EnumerateMusicList(CheckedList_Misc_Songs);
            List<string> MiscLanguages  = CheckListEnumerators.Misc_EnumerateLanguagesList(CheckedList_Misc_Languages);

            // Custom Stuff
            // Custom Music
            if (TextBox_Custom_Music.Text.Length != 0)
                MiscMusic = Custom.CustomMusic(TextBox_Custom_Music.Text, TextBox_General_ModsDirectory.Text, TextBox_General_Seed.Text, Checkbox_Custom_XMACache.Checked, MiscMusic, archives, TextBox_General_GameExecutable.Text);

            // Voice Packs.
            if (TextBox_Custom_Vox.Text.Length != 0)
            {
                // Insert the patched voice_all_e.sbk file into sound.arc first.
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "sound.arc")
                    {
                        string unpackedArchive = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(TextBox_General_GameExecutable.Text));
                        System.Console.WriteLine($@"Patching 'voice_all_e.sbk'.");
                        File.Copy($@"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\ExternalResources\voice_all_e.sbk", $@"{unpackedArchive}\xenon\sound\voice_all_e.sbk", true);
                    }
                }

                // Handle the voice packs.
                Custom.VoicePacks(TextBox_Custom_Vox.Text, TextBox_General_ModsDirectory.Text, TextBox_General_Seed.Text, SetVoices, archives, TextBox_General_GameExecutable.Text);
            }

            // Disable options if they have nothing to pick from.
            if (SetEnemies.Count == 0)
                CheckBox_SET_Enemies.Checked = false;
            if (SetCharacters.Count == 0)
                CheckBox_SET_Characters.Checked = false;
            if (SetItems.Count == 0)
                CheckBox_SET_Items.Checked = false;
            if (SetCommonProps.Count == 0)
                CheckBox_SET_CommonProps.Checked = false;
            if (SetPathProps.Count == 0)
                CheckBox_SET_PathProps.Checked = false;
            if (SetVoices.Count == 0)
                CheckBox_SET_Voices.Checked = false;
            if (EventLighting.Count == 0)
                CheckBox_Event_Scene.Checked = false;
            if (EventTerrain.Count == 0)
                CheckBox_Event_Terrain.Checked = false;
            if (SceneEnvMaps.Count == 0)
                CheckBox_Scene_EnvMaps.Checked = false;
            if (MiscMusic.Count == 0)
                CheckBox_Misc_Songs.Checked = false;
            if (MiscLanguages.Count == 0)
                Checkbox_Misc_Text.Checked = false;

            // Object Placement.
            if (CheckBox_SET_Enemies.Checked || CheckBox_SET_Behaviours.Checked || CheckBox_SET_Characters.Checked || CheckBox_SET_Items.Checked || CheckBox_SET_CommonProps.Checked ||
                CheckBox_SET_Voices.Checked || CheckBox_SET_Doors.Checked || CheckBox_SET_DrawDistance.Checked || CheckBox_SET_Cosmetic.Checked)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                    {
                        // Do the standard SET Randomisation.
                        string unpackedArchive = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(TextBox_General_GameExecutable.Text));
                        ObjectPlacementRandomiser.Load(unpackedArchive, CheckBox_SET_Enemies.Checked, SetEnemies, CheckBox_SET_Behaviours.Checked, CheckBox_SET_AllBehaviours.Checked,
                                                       CheckBox_SET_Characters.Checked, SetCharacters, CheckBox_SET_Items.Checked, SetItems, CheckBox_SET_CommonProps.Checked,
                                                       CheckBox_SET_PathProps.Checked, SetCommonProps, SetPathProps, CheckBox_SET_Voices.Checked, SetVoices, CheckBox_SET_Doors.Checked, SetDoors,
                                                       CheckBox_SET_DrawDistance.Checked, (int)Numeric_SET_DrawDistanceMin.Value, (int)Numeric_SET_DrawDistanceMax.Value, CheckBox_SET_Cosmetic.Checked);

                        // If we have voices enabled or the enemy list contains a boss, then patch them.
                        if (CheckBox_SET_Voices.Checked || SetEnemies.Contains("eCerberus") || SetEnemies.Contains("eGenesis") || SetEnemies.Contains("eWyvern") || SetEnemies.Contains("firstIblis") ||
                            SetEnemies.Contains("secondIblis") || SetEnemies.Contains("thirdIblis") || SetEnemies.Contains("firstmefiress") || SetEnemies.Contains("secondmefiress") ||
                            SetEnemies.Contains("solaris01") || SetEnemies.Contains("solaris02"))
                            ObjectPlacementRandomiser.BossPatch(unpackedArchive, CheckBox_SET_Enemies.Checked, CheckBox_SET_Voices.Checked, SetVoices);
                    }
                }
                // Patch voice_all_e.sbk to include every in game voice.
                // Only do this if we have no voice packs installed, as the voice pack installation process already handles this.
                if (TextBox_Custom_Vox.Text.Length == 0 && CheckBox_SET_Voices.Checked)
                {
                    foreach (string archive in archives)
                    {
                        if (Path.GetFileName(archive).ToLower() == "sound.arc")
                        {
                            string unpackedArchive = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(TextBox_General_GameExecutable.Text));
                            System.Console.WriteLine($@"Patching 'voice_all_e.sbk'.");
                            File.Copy($@"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\ExternalResources\voice_all_e.sbk", $@"{unpackedArchive}\xenon\sound\voice_all_e.sbk", true);
                        }
                    }
                }
            }

            // Event Randomisation.
            if (CheckBox_Event_Scene.Checked || CheckBox_Event_RotationX.Checked || CheckBox_Event_RotationY.Checked || CheckBox_Event_RotationZ.Checked || CheckBox_Event_PositionX.Checked ||
                CheckBox_Event_PositionY.Checked || CheckBox_Event_PositionZ.Checked || CheckBox_Event_Terrain.Checked || CheckBox_Event_Order.Checked)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "cache.arc")
                    {
                        string unpackedArchive = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(TextBox_General_GameExecutable.Text));
                        EventPlaybookRandomiser.Load(unpackedArchive, CheckBox_Event_Scene.Checked, EventLighting, CheckBox_Event_RotationX.Checked, CheckBox_Event_RotationY.Checked,
                                                              CheckBox_Event_RotationZ.Checked, CheckBox_Event_PositionX.Checked, CheckBox_Event_PositionY.Checked, CheckBox_Event_PositionZ.Checked,
                                                              CheckBox_Event_Terrain.Checked, EventTerrain, CheckBox_Event_Order.Checked, TextBox_General_ModsDirectory.Text,
                                                              TextBox_General_GameExecutable.Text, TextBox_General_Seed.Text);
                    }
                }
            }

            // Scene Randomisation.
            if (CheckBox_Scene_Ambient.Checked || CheckBox_Scene_Main.Checked || CheckBox_Scene_Sub.Checked || CheckBox_Scene_Direction.Checked || CheckBox_Scene_FogColour.Checked ||
                CheckBox_Scene_FogDensity.Checked || CheckBox_Scene_EnvMaps.Checked)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                    {
                        string unpackedArchive = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(TextBox_General_GameExecutable.Text));
                        SceneRandomiser.Load(unpackedArchive, CheckBox_Scene_Ambient.Checked, CheckBox_Scene_Main.Checked, CheckBox_Scene_Sub.Checked, CheckBox_Scene_Direction.Checked,
                                             CheckBox_Scene_DirectionEnforce.Checked, CheckBox_Scene_FogColour.Checked, CheckBox_Scene_FogDensity.Checked, CheckBox_Scene_EnvMaps.Checked, SceneEnvMaps);
                    }
                }
            }

            // Miscellaneous Randomisation.
            if (CheckBox_Misc_Songs.Checked)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                    {
                        string unpackedArchive = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(TextBox_General_GameExecutable.Text));
                        MiscellaneousRandomiser.MusicRandomiser(unpackedArchive, MiscMusic);
                    }
                }
            }

            if (CheckBox_Misc_EnemyHealth.Checked)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "enemy.arc")
                    {
                        string unpackedArchive = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(TextBox_General_GameExecutable.Text));
                        MiscellaneousRandomiser.EnemyHealthRandomiser(unpackedArchive, (int)Numeric_Misc_HealthMin.Value, (int)Numeric_Misc_HealthMax.Value, CheckBox_Misc_BossHealth.Checked);
                    }
                }
            }

            if (CheckBox_Misc_Surfaces.Checked)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "stage.arc")
                    {
                        string unpackedArchive = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(TextBox_General_GameExecutable.Text));
                        MiscellaneousRandomiser.SurfaceRandomiser(unpackedArchive, CheckBox_Misc_SurfacesFaces.Checked);
                    }
                }
            }

            if(Checkbox_Misc_Text.Checked)
            {
                string eventArc = "";
                string textArc = "";
                // Get event.arc and text.arc, as we need both for Text Randomisation.
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "event.arc")
                        eventArc = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(TextBox_General_GameExecutable.Text));

                    if (Path.GetFileName(archive).ToLower() == "text.arc")
                        textArc = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(TextBox_General_GameExecutable.Text));
                }
                MiscellaneousRandomiser.TextRandomiser(eventArc, textArc, MiscLanguages);
            }

            if(Checkbox_Misc_Patches.Checked)
                MiscellaneousRandomiser.PatchRandomiser(TextBox_General_ModsDirectory.Text, TextBox_General_Seed.Text);

            //Repack Archives
            foreach (string archive in archives)
                if (Directory.Exists($@"{Program.TemporaryDirectory}{archive.Substring(0, archive.Length - 4).Replace(Path.GetDirectoryName(TextBox_General_GameExecutable.Text), "")}"))
                    ArchiveHandler.RepackArchive($@"{Program.TemporaryDirectory}{archive.Substring(0, archive.Length - 4).Replace(Path.GetDirectoryName(TextBox_General_GameExecutable.Text), "")}", $@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text}){archive.Substring(0, archive.Length - 4).Replace(Path.GetDirectoryName(TextBox_General_GameExecutable.Text), "")}");

            // XMA Shuffle for Event Randomisation
            if (CheckBox_Event_XMAs.Checked)
            {
                string neededFolder = "xenon";
                if (TextBox_General_GameExecutable.Text.ToLower().EndsWith(".bin"))
                    neededFolder = "ps3";

                string[] eventXMAs = Directory.GetFiles($@"{Path.GetDirectoryName(TextBox_General_GameExecutable.Text)}\{neededFolder}\event", "E*.xma", SearchOption.AllDirectories);
                if(CheckBox_Event_XMAJapanese.Checked)
                    eventXMAs = Directory.GetFiles($@"{Path.GetDirectoryName(TextBox_General_GameExecutable.Text)}\{neededFolder}\event", "*.xma", SearchOption.AllDirectories);

                // Do this so we don't accidentally shuffle EVERY voice line.
                string[] shuffleArray = eventXMAs;

                if(CheckBox_Event_XMAGameplay.Checked)
                {
                    string[] gameplayXMAs = Directory.GetFiles($@"{Path.GetDirectoryName(TextBox_General_GameExecutable.Text)}\{neededFolder}\sound\voice\e", "*.xma", SearchOption.AllDirectories);
                    if (CheckBox_Event_XMAJapanese.Checked)
                    {
                        gameplayXMAs = gameplayXMAs.Concat(Directory.GetFiles($@"{Path.GetDirectoryName(TextBox_General_GameExecutable.Text)}\{neededFolder}\sound\voice\j", "*.xma", SearchOption.AllDirectories)).ToArray();
                        eventXMAs = eventXMAs.Concat(gameplayXMAs).ToArray();
                    }
                    if (TextBox_Custom_Vox.Text != "")
                    {
                        gameplayXMAs = gameplayXMAs.Concat(Directory.GetFiles($@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text})\xenon\sound\voice\e\", "*.xma", SearchOption.AllDirectories)).ToArray();
                        eventXMAs = eventXMAs.Concat(gameplayXMAs).ToArray();
                    }
                }

                List<int> usedNumbers = new();

                for(int i = 0; i < shuffleArray.Length; i++)
                {
                    int index = Randomiser.Next(eventXMAs.Length);
                    if (usedNumbers.Contains(index))
                    {
                        do { index = Randomiser.Next(eventXMAs.Length); }
                        while (usedNumbers.Contains(index));
                    }
                    usedNumbers.Add(index);

                    if (!Directory.Exists($@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text}){Path.GetDirectoryName(shuffleArray[i].Substring(0, shuffleArray[i].Length).Replace(Path.GetDirectoryName(TextBox_General_GameExecutable.Text), ""))}"))
                        Directory.CreateDirectory($@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text}){Path.GetDirectoryName(shuffleArray[i].Substring(0, shuffleArray[i].Length).Replace(Path.GetDirectoryName(TextBox_General_GameExecutable.Text), ""))}");

                    System.Console.WriteLine($@"Replacing '{eventXMAs[index]}' with '{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text}){shuffleArray[i].Substring(0, shuffleArray[i].Length).Replace(Path.GetDirectoryName(TextBox_General_GameExecutable.Text), "")}'.");
                    File.Copy(eventXMAs[index], $@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text}){shuffleArray[i].Substring(0, shuffleArray[i].Length).Replace(Path.GetDirectoryName(TextBox_General_GameExecutable.Text), "")}");
}
            }

            // Undo the Wildcard's settings
            if (Checkbox_General_Wildcard.Checked)
            {
                SaveConfig(Path.Combine($@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text})", "wildcard.log"));
                LoadConfig(Path.Combine($@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text})", "wildcard.ini"));
                File.Delete(Path.Combine($@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({TextBox_General_Seed.Text})", "wildcard.ini"));
            }

            // Delete the temp directory, as they quickly get large, esepecially when custom XMAs are involved.
            if(Directory.Exists(Program.TemporaryDirectory))
                Directory.Delete(Program.TemporaryDirectory, true);

            // Close the console window and show the complete message box.
            NativeMethods.FreeConsole();
            MessageBox.Show("Randomisation Complete!",
                            "Sonic '06 Randomiser Suite",
                            MessageBoxButtons.OK);
        }
    }
}
