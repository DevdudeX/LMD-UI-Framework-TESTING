using Il2CppMegagon.Downhill.UI;
using Il2CppMegagon.Downhill.UI.Screens.Helper;
using UnityEngine;
using UnityEngine.UI;
using Il2CppTMPro;
using Il2CppMegagon.Downhill.UI.Screens;
using Il2CppMegagon.Downhill.UI.Animations;

namespace CustomUiFramework
{
    static class MenuHelp
    {
        static public GameObject CreateButton(string name, string buttonText, Action callback)
        {
            GameObject _mainMenuOptionsBtn = GameObject.Find("UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)/MainMenuScreen/Layout_ButtonList/ListButton_Options");
            // Clone the existing button
            GameObject button = GameObject.Instantiate(_mainMenuOptionsBtn, _mainMenuOptionsBtn.transform.parent);
            button.name = name;
            GameObject buttonTextDisplay = button.transform.Find("Elements/TextMeshPro Text_OptionsButton").gameObject;
            TextMeshProUGUI tmpComponent = buttonTextDisplay.GetComponent<TextMeshProUGUI>();
            tmpComponent.m_text = buttonText;

            // Add onClick function to open the mod menu
            buttonTextDisplay.GetComponent<Button>().onClick.AddListener(callback);

            // Destroy all Megagon scripts as I have no way of knowing how they work (thanks il2cpp)
            // Probably aren't important ¯\_(ツ)_/¯
            RemoveChildComponents<ListButton>(button);
            RemoveChildComponents<UIAnimation_FadeContainer>(button);
            RemoveChildComponents<InteractiveUIElement>(buttonTextDisplay);

            return button;
        }

        //max buttons is 5 at the moment
        static public GameObject CreateScreen(string name, string menuText, string menuHeaderText="", string menuSubheaderText="")
        {
            GameObject _settingsScreen = GameObject.Find("UI(Clone)/Canvas3D/Wrapper/SettingsScreen(Clone)");

            // Create a new menu screen
            GameObject screen = GameObject.Instantiate(_settingsScreen, _settingsScreen.transform.parent);
            screen.name = name;
            screen.transform.Find("SettingsMenu").gameObject.name = menuText;

            // Remove useless scripts
            RemoveChildComponents<SettingsScreen>(screen);
            RemoveChildComponents<ControlsDisplayAnimations>(screen);
            RemoveChildComponents<DummyShowHideAnimation>(screen);
            RemoveChildComponents<ScreenElement>(screen);
            RemoveChildComponents<InteractiveUIElement>(screen);
            RemoveChildComponents<UIAnimation_FadeContainer>(screen);
            RemoveChildComponents<UIAnimation_MoveContainer>(screen);
            RemoveChildComponents<TextMeshProLocalized>(screen);

            // Set menu header text
            TextMeshProUGUI menuSubHeader_TMP = screen.transform.Find("ModOptionsMenu/MenuHeader/TextMeshPro Text_SubHeadline").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI menuHeader_TMP = screen.transform.Find("ModOptionsMenu/MenuHeader/TextMeshPro Text_GameMenuHeadline").GetComponent<TextMeshProUGUI>();
            menuHeader_TMP.m_text = menuHeaderText;
            menuSubHeader_TMP.m_text = menuSubheaderText;

            return screen;
        }

        //max lenght is 5 at the moment
        static public void SetScreenButtons(GameObject screen, string[] buttons, Action BackFunctionality)
        {
            // Get the TMP components for each category button
            // These existing buttons are just for debugging and would be replaced in future
            string btnNav = "ModOptionsMenu/Layout_Categories/";
            TextMeshProUGUI categoryBtn_Game_TMP = screen.transform.Find(btnNav + "CategoryButton_Game/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI categoryBtn_Display_TMP = screen.transform.Find(btnNav + "CategoryButton_Display/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI categoryBtn_Graphics_TextDisp = screen.transform.Find(btnNav + "CategoryButton_Graphics/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI categoryBtn_Controls_TextDisp = screen.transform.Find(btnNav + "CategoryButton_Controls/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI categoryBtn_Audio_TextDisp = screen.transform.Find(btnNav + "CategoryButton_Audio/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();

            TextMeshProUGUI backBtn_TextDisp = screen.transform.Find("ModOptionsMenu/BackButton/Image_ControlsExplanationLeftBackground/TextMeshPro Text_Back").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI applyBtn_TextDisp = screen.transform.Find("ModOptionsMenu/ApplyButton/Image_ControlsExplanationRightBackground (1)/TextMeshPro Text_Apply").GetComponent<TextMeshProUGUI>();

            // Make sure all labels are visible
            categoryBtn_Game_TMP.alpha = 1;
            categoryBtn_Display_TMP.alpha = 1;
            categoryBtn_Graphics_TextDisp.alpha = 1;
            categoryBtn_Controls_TextDisp.alpha = 1;
            categoryBtn_Audio_TextDisp.alpha = 1;

            backBtn_TextDisp.alpha = 1;
            backBtn_TextDisp.m_text = "Back";   // It seems to default to {0}Back (guessing it's localization related)

            applyBtn_TextDisp.alpha = 1;
            backBtn_TextDisp.m_text = "Apply Changes";   // Fixes localization default text

            // Add onClick function to return to the main menu
            backBtn_TextDisp.GetComponent<Button>().onClick.AddListener((UnityEngine.Events.UnityAction)BackFunctionality);
        }

        static void RemoveChildComponents<T>(GameObject parent) where T : Component
        {
            T[] components = parent.GetComponentsInChildren<T>();

            foreach (var c in components)
            {
                GameObject.Destroy(c);
            }
        }

    }
}
