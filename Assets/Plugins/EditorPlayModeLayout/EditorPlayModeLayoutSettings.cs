using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorPlayModeLayout
{
    [FilePath(SETTING_PATH, FilePathAttribute.Location.ProjectFolder)]
    public class EditorPlayModeLayoutSettings : ScriptableSingleton<EditorPlayModeLayoutSettings>
    {
        public const string SETTING_PATH = "UserSettings/EditorPlayModeLayoutSettings.asset";
        public const string DEFAULT_PLAYMODE_LAYOUT_PATH = "UserSettings/playModeLayout.wlt";

        public bool Enable = false;
        public string PlayModeLayoutPath = DEFAULT_PLAYMODE_LAYOUT_PATH;

        public void Save()
        {
            base.Save(saveAsText: true);
        }
    }
}
