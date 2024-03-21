#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityToolbarExtender;

[InitializeOnLoad]
public class ToolbarScene {
    private const string ScenesFilePath = "/01_Scenes";

    static ToolbarScene() {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    private static void OnToolbarGUI(IMGUIEvent evt) {
        Debug.Log($"OnToolbarGUI {evt.target}");
    }

    private static void OnToolbarGUI() {
        GUIContent content = new GUIContent(SceneManager.GetActiveScene().name);

        Vector2 size = EditorStyles.toolbarDropDown.CalcSize(content);

        string filePath =
            $"{Application.dataPath}{ScenesFilePath}";

        GUILayout.Space(5);

        if (EditorGUILayout.DropdownButton(content, FocusType.Keyboard,
                EditorStyles.toolbarDropDown, GUILayout.Width(size.x + 5f)) == false) return;

        GenericMenu menu = new GenericMenu();
        MakeSceneMenus(filePath, menu);
        menu.ShowAsContext();
    }

    private static void MakeSceneMenus(string path, GenericMenu menu) {
        string[] scenes = { };

        try {
            scenes = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
        }
        catch {
            // ignored
        }
        foreach (string scene in scenes) {
            int dotIndex = scene.LastIndexOf('.');
            if (dotIndex == -1) continue;

            string substring = scene[dotIndex..];

            if (substring == ".meta") continue;

            string extension = Path.GetFileNameWithoutExtension(scene);



            if (substring == ".unity") {
                int assetsIndex = scene.IndexOf("Assets");
                var sceneFilePathIndex = scene.IndexOf(ScenesFilePath);

                if (assetsIndex == -1) continue;
                if (sceneFilePathIndex == -1) continue;

                sceneFilePathIndex += 1 + ScenesFilePath.Length;

                var contentPath = scene[sceneFilePathIndex..];
                string originalString = @"C:\Your\Path\Here";
                string modifiedString = originalString.Replace("\\", "/");
                contentPath = contentPath.Replace('\\', '/').Replace(".unity", string.Empty);

                menu.AddItem(new GUIContent(contentPath), false, () => {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorSceneManager.OpenScene(scene[assetsIndex..]);
                });
            }
        }
    }
}
#endif