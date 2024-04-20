using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorPlayModeLayout
{
    public static class EditorPlayModeLayoutSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateCustomSettingProvider()
        {
            var provider = new SettingsProvider("Preferences/EditorPlayModeLayout", SettingsScope.User)
            {
                label = "EditorPlayModeLayout",
                guiHandler = (searchContext) => 
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying || EditorApplication.isPaused);

                    var settings = EditorPlayModeLayoutSettings.instance;

                    bool isEnable = settings.Enable;
                    string playModeLayoutPath = settings.PlayModeLayoutPath;


                    isEnable = EditorGUILayout.Toggle(new GUIContent("Enable"), isEnable);
                    playModeLayoutPath = EditorGUILayout.TextField(new GUIContent("PlayMode layout path"), playModeLayoutPath);

                    if (GUILayout.Button("Reset", GUILayout.Height(30)))
                    {
                        isEnable = false;
                        playModeLayoutPath = EditorPlayModeLayoutSettings.DEFAULT_PLAYMODE_LAYOUT_PATH;
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        settings.Enable = isEnable;
                        settings.PlayModeLayoutPath = playModeLayoutPath;
                        settings.Save();
                    }

                    EditorGUI.EndDisabledGroup();
                },
                keywords = new HashSet<string>(new [] { "EditorPlayMode", "EditorPlayModeLayout", "layout" })
            };

            return provider;
        }
    }
}
