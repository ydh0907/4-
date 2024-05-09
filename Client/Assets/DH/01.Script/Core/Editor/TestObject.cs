using DH;
using TestClient;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Program))]
public class TestObject : Editor
{
    private static int roomId = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!Application.isPlaying) return;

        if (GUILayout.Button("Make Dummy Room"))
        {
            roomId = Mathf.Clamp(roomId + 1, 0, 100);
            Program.Instance.CreateRoom("Test" + roomId.ToString(), "Dummy" + roomId.ToString());
        }
        if (GUILayout.Button("Delete Dummy Room"))
        {
            Program.Instance.Delete("Dummy" + roomId.ToString(), "Test" + roomId.ToString());
            roomId = Mathf.Clamp(roomId - 1, 0, 100);
        }
    }
}
