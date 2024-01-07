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
		private GameObject _mainMenuOptionsBtn;

		private GameObject _modMenuBtn;
		private GameObject _modMenuScreen;


		public override void OnEarlyInitializeMelon()
		{
			instance = this;
		}

		public override void OnUpdate()
		{
			if (Input.GetKeyDown(KeyCode.H))
			{
				GenerateMainMenuButton();
			}
		}


		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			// string[] whitelistedLoadScenes = {
			// 	"Menu_Alps_01", "Menu_Autumn_01", "Menu_Canyon_01", "Menu_Rockies_01", "Menu_Island_01",
			// 	"gameplay", "DontDestroyOnLoad", "HideAndDontSave"
			// };

			string[] whitelistedLoadScenes = {"gameplay"};
			if (Array.IndexOf(whitelistedLoadScenes, sceneName) != -1)
			{
				LoggerInstance.Msg($"Scene {sceneName} with build index {buildIndex} has been loaded!");
			}
		}

		void GenerateMainMenuButton()
		{
			// Find the options menu
			// Grab existing buttons and screens
			_mainMenuScreen = GameObject.Find("UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)");
			_settingsScreen = GameObject.Find("UI(Clone)/Canvas3D/Wrapper/SettingsScreen(Clone)");
			_mainMenuOptionsBtn = GameObject.Find("UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)/MainMenuScreen/Layout_ButtonList/ListButton_Options");

			// If already present destroy
			if (GameObject.Find("UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)/MainMenuScreen/Layout_ButtonList/ListButton_Mods") != null)
			{
				GameObject.Destroy(GameObject.Find("UI(Clone)/Canvas3D/Wrapper/MainMenuScreen(Clone)/MainMenuScreen/Layout_ButtonList/ListButton_Mods"));
			}
			if (GameObject.Find("UI(Clone)/Canvas3D/Wrapper/ModMenuScreen") != null)
			{
				GameObject.Destroy(GameObject.Find("UI(Clone)/Canvas3D/Wrapper/ModMenuScreen"));
			}

			// Clone the existing button
			_modMenuBtn = GameObject.Instantiate(_mainMenuOptionsBtn, _mainMenuOptionsBtn.transform.parent);
			_modMenuBtn.name = "ListButton_Mods";
			GameObject modMenuBtnTextDisplay = _modMenuBtn.transform.Find("Elements/TextMeshPro Text_OptionsButton").gameObject;
			TextMeshProUGUI tmpComponent = modMenuBtnTextDisplay.GetComponent<TextMeshProUGUI>();
			tmpComponent.m_text = "Mods";

			// Add onClick function to open the mod menu
			modMenuBtnTextDisplay.GetComponent<UnityEngine.UI.Button>().onClick.AddListener((UnityEngine.Events.UnityAction)OpenModMenuScreen);

			// Destroy all Megagon scripts as I have no way of knowing how they work (thanks il2cpp)
			// Probably aren't important ¯\_(ツ)_/¯
			RemoveChildComponents<ListButton>(_modMenuBtn);
			RemoveChildComponents<UIAnimation_FadeContainer>(_modMenuBtn);
			RemoveChildComponents<InteractiveUIElement>(modMenuBtnTextDisplay);

			// Create a new menu screen
			_modMenuScreen = GameObject.Instantiate(_settingsScreen, _settingsScreen.transform.parent);
			_modMenuScreen.name = "ModMenuScreen";
			_modMenuScreen.transform.Find("SettingsMenu").gameObject.name = "ModOptionsMenu";

			// Remove useless scripts
			RemoveChildComponents<SettingsScreen>(_modMenuScreen);
			RemoveChildComponents<ControlsDisplayAnimations>(_modMenuScreen);
			RemoveChildComponents<DummyShowHideAnimation>(_modMenuScreen);
			RemoveChildComponents<ScreenElement>(_modMenuScreen);
			RemoveChildComponents<InteractiveUIElement>(_modMenuScreen);
			RemoveChildComponents<UIAnimation_FadeContainer>(_modMenuScreen);
			RemoveChildComponents<UIAnimation_MoveContainer>(_modMenuScreen);
			RemoveChildComponents<TextMeshProLocalized>(_modMenuScreen);

			// Set menu header text
			TextMeshProUGUI menuSubHeader_TMP = _modMenuScreen.transform.Find("ModOptionsMenu/MenuHeader/TextMeshPro Text_SubHeadline").GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI menuHeader_TMP = _modMenuScreen.transform.Find("ModOptionsMenu/MenuHeader/TextMeshPro Text_GameMenuHeadline").GetComponent<TextMeshProUGUI>();
			menuHeader_TMP.m_text = "Mod Options";
			menuSubHeader_TMP.m_text = $"Version {MOD_VERSION}";

			// Get the TMP components for each category button
			// These existing buttons are just for debugging and would be replaced in future
			string btnNav = "ModOptionsMenu/Layout_Categories/";
			TextMeshProUGUI categoryBtn_Game_TMP = _modMenuScreen.transform.Find(btnNav+"CategoryButton_Game/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI categoryBtn_Display_TMP = _modMenuScreen.transform.Find(btnNav+"CategoryButton_Display/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI categoryBtn_Graphics_TextDisp = _modMenuScreen.transform.Find(btnNav+"CategoryButton_Graphics/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI categoryBtn_Controls_TextDisp = _modMenuScreen.transform.Find(btnNav+"CategoryButton_Controls/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI categoryBtn_Audio_TextDisp = _modMenuScreen.transform.Find(btnNav+"CategoryButton_Audio/TextMeshPro Text_Display").GetComponent<TextMeshProUGUI>();

			TextMeshProUGUI backBtn_TextDisp = _modMenuScreen.transform.Find("ModOptionsMenu/BackButton/Image_ControlsExplanationLeftBackground/TextMeshPro Text_Back").GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI applyBtn_TextDisp = _modMenuScreen.transform.Find("ModOptionsMenu/ApplyButton/Image_ControlsExplanationRightBackground (1)/TextMeshPro Text_Apply").GetComponent<TextMeshProUGUI>();

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
			backBtn_TextDisp.GetComponent<UnityEngine.UI.Button>().onClick.AddListener((UnityEngine.Events.UnityAction)LeaveModMenuScreen);


			// Remove unwanted buttons
			// GameObject.Destroy(_modMenuScreen.transform.Find("SettingsMenu/Layout_Categories/CategoryButton_Display"));
			// GameObject.Destroy(_modMenuScreen.transform.Find("SettingsMenu/Layout_Categories/CategoryButton_Graphics"));
			// GameObject.Destroy(_modMenuScreen.transform.Find("SettingsMenu/Layout_Categories/CategoryButton_Controls"));
			// GameObject.Destroy(_modMenuScreen.transform.Find("SettingsMenu/Layout_Categories/CategoryButton_Audio"));

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

			foreach (var c in components) {
				GameObject.Destroy(c);
			}
		}
	}
}