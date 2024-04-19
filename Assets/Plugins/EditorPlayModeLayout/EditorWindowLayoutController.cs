using System;
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
