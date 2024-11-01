// Mod
using MelonLoader;
using Harmony;

// Unity
using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ModUiFramework
{
	public sealed class MenuManager
	{
		MainMenu menu;
		Dictionary<string,ModInfo> modInfoWindows = new Dictionary<string,ModInfo>();

		private static readonly MenuManager instance = new MenuManager();
		public static MenuManager Instance
		{
			get { return instance; }
		}

		static MenuManager() { }
		private MenuManager()
		{
			menu = new MainMenu();
			int count = 1;
			foreach (MelonBase melon in MelonBase.RegisteredMelons)
			{
				modInfoWindows.Add(melon.Info.Name, new ModInfo(melon, count));
				count++;
			}
		}

		/// <summary>
		/// Registers an action button to be drawn.
		/// </summary>
		public void RegisterAction(string name, string buttonName, int callbackID, Action<int> callback)
		{
			modInfoWindows[name].AddAction(buttonName, callbackID, callback);
		}

		/// <summary>
		/// Registers an information label to be drawn.
		/// </summary>
		public void RegisterInfoItem(string name, string infoPrefix, int callbackID, Func<string> infoValueCallback)
		{
			modInfoWindows[name].AddInfoItem(infoPrefix, callbackID, infoValueCallback);
		}
	}
	class MainMenu
	{
		Rect windowRect = new Rect(20, 20, 500, 1000);
		List<string> clickedMods = new List<string>();

		MelonBase[] allMelons;
		public MainMenu()
		{
			allMelons = MelonBase.RegisteredMelons.ToArray();
		}
		public void Draw()
		{
			windowRect = GUI.Window(0, windowRect, (GUI.WindowFunction)ListMods, "Mod Menu");
		}

		void ListMods(int windowID)
		{
			GUILayout.BeginVertical();
			foreach (MelonBase melon in allMelons)
			{
				if (GUILayout.Button(melon.Info.Name))
				{
					clickedMods.Add(melon.Info.Name);
				}
			}
			GUILayout.EndVertical();
			GUI.DragWindow();
		}

		public string[] GetClicked()
		{
			string[] temp = clickedMods.ToArray();
			clickedMods.Clear();
			return temp;
		}
	}

	class ModInfo
	{
		Rect windowRect = new Rect(520, 20, 500, 500);
		public bool IsVisible { get; private set; }
		MelonBase mod;
		int windowID;

		bool loaded = true;

		Dictionary<string, (int, Action<int>)> actions = new Dictionary<string, (int, Action<int>)>();
		Dictionary<string, (int, Func<string>)> infoItems = new Dictionary<string, (int, Func<string>)>();

		public ModInfo(MelonBase melon, int windowID)
		{
			this.mod = melon;
			this.windowID = windowID;
		}

		/// <summary>
		/// Toggle the drawing of the entire mod info window.
		/// </summary>
		public void ToggleVisibility()
		{
			IsVisible = !IsVisible;
		}

		void ToggleLoaded()
		{
			if (loaded)
			{
				mod.Unregister();
				loaded = false;
			}
			else
			{
				mod.Register();
				loaded = true;
			}
		}

		/// <summary>
		/// Adds an action.
		/// </summary>
		public void AddAction(string buttonName, int callbackID, Action<int> callback)
		{
			actions.Add(buttonName, (callbackID, callback));
		}

		/// <summary>
		/// Adds an info label.
		/// </summary>
		public void AddInfoItem(string infoPrefix, int callbackID, Func<string> infoValue)
		{
			infoItems.Add(infoPrefix, (callbackID, infoValue));
		}

		public void Draw()
		{
			if (IsVisible)
			{
				windowRect = GUI.Window(windowID, windowRect, (GUI.WindowFunction)ModSettings, mod.Info.Name + " " + mod.Info.Version + " by " + mod.Info.Author);
			}
		}

		void ModSettings(int windowID)
		{
			if (GUI.Button(new Rect(10, 0, 60, 20), "X"))
			{
				ToggleVisibility();
			}
			GUILayout.BeginVertical();

			GUILayout.Label("Toggle mod state");
			if (GUILayout.Button(loaded ? "disable" : "enable"))
			{
				ToggleLoaded();
			}

			// Render all info labels
			if (infoItems.Count > 0) GUILayout.Label("Info");
			foreach(KeyValuePair<string,(int,Func<string>)> infoItem in infoItems)
			{
				string infoText = $"{infoItem.Key}{infoItem.Value.Item2()}";
				GUILayout.Label(infoText);
			}

			// Render all action buttons
			if (actions.Count > 0) GUILayout.Label("Actions");
			foreach(KeyValuePair<string,(int,Action<int>)> action in actions)
			{
				if (GUILayout.Button(action.Key))
				{
					action.Value.Item2(action.Value.Item1);
				}
			}

			GUILayout.EndVertical();
			GUI.DragWindow();
		}
	}
}
