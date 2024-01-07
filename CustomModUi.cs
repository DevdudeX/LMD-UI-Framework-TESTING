// Mod
using MelonLoader;
using ModUiFramework;

// Unity
using UnityEngine;
using Il2CppTMPro;

// Megagon
using Il2CppMegagon.Downhill.UI;
using Il2CppMegagon.Downhill.UI.Animations;
using Il2CppMegagon.Downhill.UI.Screens;
using Il2CppMegagon.Downhill.UI.Screens.Helper;
using CustomUiFramework;

[assembly: MelonInfo(typeof(CustomModUi), "Mod Ui", "0.0.1", "DevdudeX")]
[assembly: MelonGame()]
namespace ModUiFramework
{
    public class CustomModUi : MelonMod
    {
        // Keep this updated!
        private const string MOD_VERSION = "0.0.1";
        public static CustomModUi instance;

        private GameObject _mainMenuScreen;
        private GameObject _settingsScreen;
        private GameObject _modMenuScreen;

        private bool _modButtonCreated = false;

        public override void OnEarlyInitializeMelon()
        {
            instance = this;
        }

        public override void OnUpdate()
        {
            //load automatically
            if (!_modButtonCreated)
            {
                GameObject _settingsScreen = GameObject.Find("UI(Clone)/Canvas3D/Wrapper/SettingsScreen(Clone)");
                _mainMenuScreen = GameObject.Find("UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)");
                if (_settingsScreen != null && _mainMenuScreen != null)
                {
                    GenerateMainMenuButton();
                    _modButtonCreated = true;
                }
            }

            if(Input.GetKeyDown(KeyCode.H))
            {
                OpenModMenuScreen();
            }

        }

        void GenerateMainMenuButton()
        {

            _mainMenuScreen = GameObject.Find("UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)");
            // If already present destroy
            if (GameObject.Find("UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)/MainMenuScreen/Layout_ButtonList/ListButton_Mods") != null)
            {
                GameObject.Destroy(GameObject.Find("UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)/MainMenuScreen/Layout_ButtonList/ListButton_Mods"));
            }
            if (GameObject.Find("UI(Clone)/Canvas3D/Wrapper/ModMenuScreen") != null)
            {
                GameObject.Destroy(GameObject.Find("UI(Clone)/Canvas3D/Wrapper/ModMenuScreen"));
            }

            MenuHelp.CreateButton("ListButton_Mods", "mods", OpenModMenuScreen);
            _modMenuScreen = MenuHelp.CreateScreen("ModMenuScreen", "ModOptionsMenu", "Mod Options", $"Version {MOD_VERSION}");
            MenuHelp.SetScreenButtons(_modMenuScreen, new string[] { "test text","test text 2" }, LeaveModMenuScreen);
            
            // Hide the mod menu screen by default
            _modMenuScreen.SetActive(false);
        }

        private void OpenModMenuScreen()
        {
            LoggerInstance.Msg($"Mod menu button clicked, opening mod menu.");

            _mainMenuScreen.SetActive(false);
            _modMenuScreen.SetActive(true);
        }
        private void LeaveModMenuScreen()
        {
            LoggerInstance.Msg($"Mod menu back button clicked, exiting mod menu.");

            _modMenuScreen.SetActive(false);
            _mainMenuScreen.SetActive(true);
        }

        /// <summary>
        /// Generic method to remove all child components of a certain type.
        /// Credit: https://onewheelstudio.com/blog/2020/12/27/c-generics-and-unity
        /// </summary>
        /// <param name="parent">The parent GameObject.</param>
        /// <typeparam name="T">The component type to remove.</typeparam>
        public void RemoveChildComponents<T>(GameObject parent) where T : Component
        {
            T[] components = parent.GetComponentsInChildren<T>();

            foreach (var c in components)
            {
                GameObject.Destroy(c);
            }
        }
    }
}