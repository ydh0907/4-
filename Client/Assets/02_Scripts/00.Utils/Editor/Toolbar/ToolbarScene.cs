#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityToolbarExtender;

[InitializeOnLoad]
public class ToolbarScene {
    private const string ScenesFilePath = "/01_Scenes"; // 내가 scene을 가져올 경로를 지정해준다

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

    private static void MakeSceneMenus(string path, GenericMenu menu, string addPath = "") { // 재귀함수
        string[] scenes = { };
        try { // 주어진 경로 내에 scene파일이 아니라 다른 파일이 들어 있을 수 있음 
            scenes = Directory.GetFileSystemEntries(path);
        }
        catch {
            Debug.LogError("The file is in an incorrect format");
        }

        foreach (string scene in scenes) { // 재귀함수를 이용하는 이유는 ScenesFilePath 내에 파일이 있고 그 안에 scene이 존재 할때
                                           // 이를 화면에 한번에 보여주는 것이 아닌 실제 파일처럼 ui로 보여준다
            int dotIndex = scene.LastIndexOf('.');
            if (dotIndex == -1) continue;

            string substring = scene[dotIndex..];

            if (substring == ".meta") continue;

            string extension = Path.GetFileNameWithoutExtension(scene);

            if (substring == ".unity") { // 현재 파일의 확장자가 .unity일때
                int assetsIndex = scene.IndexOf("Assets");

                if (assetsIndex == -1) continue;

                menu.AddItem(new GUIContent($"{addPath}{extension}"), false, () => {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                        EditorSceneManager.OpenScene(scene[assetsIndex..]);
                    }
                });
            }
            else {
                if (addPath == "") { // scene이 존재
                Debug.Log($"{path}/{addPath}");
                    MakeSceneMenus(scene, menu, extension + "/");
                }
                else { // file이 존재
                    MakeSceneMenus(scene, menu, addPath + extension + "/");
                }
            }
        }
    }
}
#endif