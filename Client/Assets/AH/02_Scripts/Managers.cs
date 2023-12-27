using UnityEngine;

public class Managers : MonoBehaviour {
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    SoundManager _sound = new SoundManager();
    public static SoundManager Sound { get { return Instance._sound; } }

    static void Init() {
        // Instance 프로퍼티 get 시 호출되니까 또 여기서 Instance 쓰면 무한 루프 빠짐 주의!
        if (s_instance == null) {
            GameObject go = GameObject.Find("Managers");
            if (go == null) {
                go = new GameObject { name = "Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._sound.Init(); //SoundManager의 Init() 호출
        }
    }
}
