using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorPlayModeLayout
{
    [InitializeOnLoad]
    public static class EditorWindowLayoutController
    {
        static EditorWindowLayoutController()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChange;
        }

        [MenuItem("File/Save Current Layout as Playmode Layout", false, 173)]
        private static void SaveCurrentLayoutAsPlayModeLayout()
        {
            var settings = EditorPlayModeLayoutSettings.instance;
            var playModeLayoutPath = Path.Combine(Directory.GetCurrentDirectory(), settings.PlayModeLayoutPath);

            if (!File.Exists(playModeLayoutPath))
            {
                playModeLayoutPath = EditorPlayModeLayoutSettings.DEFAULT_PLAYMODE_LAYOUT_PATH;
                settings.PlayModeLayoutPath = playModeLayoutPath;
                settings.Save();
            }

            EditorWindowLayoutUtility.SaveLayoutToAsset(playModeLayoutPath);
        }

        private static async void OnPlayModeStateChange(PlayModeStateChange state)
        {
            var settings = EditorPlayModeLayoutSettings.instance;

            if (!settings.Enable)
            {
                return;
            }

            if (!EditorWindowLayoutUtility.IsAvailable)
            {
                Debug.LogWarning($"{nameof(EditorWindowLayoutController)} : internal editor api not available...");
                return;
            }

            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                {
                    await Task.Delay(1);

                    string currentPlayModeLayoutPath = settings.PlayModeLayoutPath;
                    bool shouldChangeLayout = !string.IsNullOrEmpty(currentPlayModeLayoutPath);

                    if (shouldChangeLayout)
                    {
                        EditorWindowLayoutUtility.SaveDefaultWindowPreferences();
                        EditorWindowLayoutUtility.LoadLayoutFromAsset(currentPlayModeLayoutPath);
                        EditorWindowLayoutUtility.UpdateWindowLayoutMenu();
                    }
                }
                break;

                case PlayModeStateChange.EnteredEditMode:
                {
                    EditorWindowLayoutUtility.LoadCurrentModeLayout(keepMainWindow: true);
                    EditorWindowLayoutUtility.UpdateWindowLayoutMenu();
                }
                break;
            }
        }
    }
}
