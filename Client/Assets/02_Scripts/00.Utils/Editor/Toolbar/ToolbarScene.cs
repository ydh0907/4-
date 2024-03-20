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
    private const string ScenesFilePath = "/01_Scenes"; // ���� scene�� ������ ��θ� �������ش�

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

    private static void MakeSceneMenus(string path, GenericMenu menu, string addPath = "") { // ����Լ�
        string[] scenes = { };
        try { // �־��� ��� ���� scene������ �ƴ϶� �ٸ� ������ ��� ���� �� ���� 
            scenes = Directory.GetFileSystemEntries(path);
        }
        catch {
            Debug.LogError("The file is in an incorrect format");
        }

        foreach (string scene in scenes) { // ����Լ��� �̿��ϴ� ������ ScenesFilePath ���� ������ �ְ� �� �ȿ� scene�� ���� �Ҷ�
                                           // �̸� ȭ�鿡 �ѹ��� �����ִ� ���� �ƴ� ���� ����ó�� ui�� �����ش�
            int dotIndex = scene.LastIndexOf('.');
            if (dotIndex == -1) continue;

            string substring = scene[dotIndex..];

            if (substring == ".meta") continue;

            string extension = Path.GetFileNameWithoutExtension(scene);

            if (substring == ".unity") { // ���� ������ Ȯ���ڰ� .unity�϶�
                int assetsIndex = scene.IndexOf("Assets");

                if (assetsIndex == -1) continue;

                menu.AddItem(new GUIContent($"{addPath}{extension}"), false, () => {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                        EditorSceneManager.OpenScene(scene[assetsIndex..]);
                    }
                });
            }
            else {
                if (addPath == "") { // scene�� ����
                Debug.Log($"{path}/{addPath}");
                    MakeSceneMenus(scene, menu, extension + "/");
                }
                else { // file�� ����
                    MakeSceneMenus(scene, menu, addPath + extension + "/");
                }
            }
        }
    }
}
#endif