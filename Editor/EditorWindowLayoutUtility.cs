using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Type = System.Type;

namespace EditorPlayModeLayout
{
    // Note : Reflection by refs (Unity 2022.3) : https://github.com/Unity-Technologies/UnityCsReference/tree/2022.3
    public static class EditorWindowLayoutUtility
    {
        private static MethodInfo _miLoadDefaultLayout;
        private static MethodInfo _miLoadWindowLayout;
        private static MethodInfo _miLoadCurrentModeLayout;
        private static MethodInfo _miTryLoadWindowLayout;
        private static MethodInfo _miLoadLastUsedLayoutForCurrentMode;
        private static MethodInfo _miSaveWindowLayout;
        private static MethodInfo _miSaveDefaultWindowPreferences;
        private static MethodInfo _miUpdateWindowLayoutMenu;

        public static bool IsAvailable => _available;
        private static bool _available;

        static EditorWindowLayoutUtility()
        {
            Initialize();
        }

        public static void Initialize()
        {
            Type tyWindowLayout = Type.GetType("UnityEditor.WindowLayout, UnityEditor");

            _miLoadDefaultLayout = tyWindowLayout.GetMethod("LoadDefaultLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            _miLoadWindowLayout = tyWindowLayout.GetMethod("LoadWindowLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string), typeof(bool), typeof(bool), typeof(bool) }, null);
            _miLoadCurrentModeLayout = tyWindowLayout.GetMethod("LoadCurrentModeLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(bool) }, null);
            _miTryLoadWindowLayout = tyWindowLayout.GetMethod("TryLoadWindowLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string), typeof(bool) }, null);
            _miLoadLastUsedLayoutForCurrentMode = tyWindowLayout.GetMethod("LoadLastUsedLayoutForCurrentMode", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(bool) }, null);
            _miSaveWindowLayout = tyWindowLayout.GetMethod("SaveWindowLayout", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null);
            _miSaveDefaultWindowPreferences = tyWindowLayout.GetMethod("SaveDefaultWindowPreferences", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            _miUpdateWindowLayoutMenu = tyWindowLayout.GetMethod("UpdateWindowLayoutMenu", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

            bool isValid = (tyWindowLayout != null)
                        && (_miLoadDefaultLayout != null)
                        && (_miLoadWindowLayout != null)
                        && (_miLoadCurrentModeLayout != null)
                        && (_miTryLoadWindowLayout != null)
                        && (_miLoadLastUsedLayoutForCurrentMode != null)
                        && (_miSaveWindowLayout != null)
                        && (_miSaveDefaultWindowPreferences != null)
                        && (_miUpdateWindowLayoutMenu != null);

            _available = isValid;
        }

        // Save current window layout to asset file. `assetPath` must be relative to project directory.
        public static void SaveLayoutToAsset(string assetPath)
        {
            string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
            SaveLayout(absolutePath);
        }

        // Load window layout from asset file. `assetPath` must be relative to project directory.
        public static void LoadLayoutFromAsset(string assetPath)
        {
            string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
            LoadLayout(absolutePath);
        }

        // Save current window layout to file. `path` must be absolute.
        public static void SaveLayout(string path)
        {
            if (_miSaveWindowLayout != null)
            {
                _miSaveWindowLayout.Invoke(null, new object[] { path });
            }
        }

        public static void LoadLayout(string path)
        {
            if (_miLoadWindowLayout == null)
            {
                return;
            }

            bool result = (bool)_miLoadWindowLayout.Invoke(null, new object[] { path, true, true, true });

            if (!result)
            {
                LoadCurrentModeLayout(keepMainWindow: true);
            }
        }

        public static void TryLoadLayout(string path, bool newProjectLayoutWasCreated = false)
        {
            if (_miTryLoadWindowLayout != null)
            {
                _miTryLoadWindowLayout.Invoke(null, new object[] { path, newProjectLayoutWasCreated });
            }
        }

        public static void LoadLastUsedLayoutForCurrentMode(bool keepMainWindow)
        {
            if (_miLoadLastUsedLayoutForCurrentMode != null)
            {
                _miLoadLastUsedLayoutForCurrentMode.Invoke(null, new object[] { keepMainWindow });
            }
        }

        public static void LoadCurrentModeLayout(bool keepMainWindow)
        {
            if (_miLoadCurrentModeLayout != null)
            {
                _miLoadCurrentModeLayout.Invoke(null, new object[] { keepMainWindow });
            }
        }

        public static void SaveDefaultWindowPreferences()
        {
            if (_miSaveDefaultWindowPreferences != null)
            {
                _miSaveDefaultWindowPreferences.Invoke(null, null);
            }
        }

        public static void LoadDefaultLayout()
        {
            if (_miLoadDefaultLayout != null)
            {
                _miLoadDefaultLayout.Invoke(null, null);
            }
        }

        public static void UpdateWindowLayoutMenu()
        {
            if (_miUpdateWindowLayoutMenu != null)
            {
                _miUpdateWindowLayoutMenu.Invoke(null, null);
            }
        }
    }
}
