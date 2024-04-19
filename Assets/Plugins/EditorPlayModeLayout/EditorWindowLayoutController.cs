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
            if (!EditorWindowLayoutUtility.IsAvailable)
            {
                Debug.LogWarning($"{nameof(EditorWindowLayoutController)} : internal editor api not available...");
                return;
            }

            //Debug.Log($"{nameof(EditorWindowLayoutController)} : change to state ({state})");

            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                {
                    await Task.Delay(1);
                    EditorWindowLayoutUtility.SaveDefaultWindowPreferences();
                    EditorWindowLayoutUtility.LoadLayoutFromAsset("Assets/Plugins/EditorPlayModeLayout/Resources/playModeLayout.wlt");
                    //EditorWindowLayoutUtility.LoadLayoutFromAsset("Assets/Plugins/EditorPlayModeLayout/Resources/playModeLayoutFullScreen.wlt");
                    EditorWindowLayoutUtility.UpdateWindowLayoutMenu();
                }
                break;

                case PlayModeStateChange.EnteredEditMode:
                {
                    try
                    {
                        EditorWindowLayoutUtility.LoadCurrentModeLayout(keepMainWindow: true);
                    }
                    catch (Exception)
                    {
                        EditorWindowLayoutUtility.LoadDefaultLayout();
                    }

                    EditorWindowLayoutUtility.UpdateWindowLayoutMenu();
                }
                break;
            }
        }
    }
}
