using Karin;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Sound {
    Bgm,
    Effect,
    MaxCount //�׳� enum�� ������ ���� ���� ����(�ƹ��͵� �ƴ�)
}
public class SoundManager : MonoSingleton<SoundManager> {
    protected SoundManager() { }
    AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClip = new Dictionary<string, AudioClip>();

    public int bgmValue = 9;
    public int effectValue = 9;
    /*private void Awake() {
        Debug.Log(bgmValue);
        Debug.Log(effectValue);
    }*/
    /// <summary>
    /// SoundManager��� ������Ʈ�� ���� �� �Ʒ��� Sound�� �ִ� Ÿ�� ��ŭ�� ������Ʈ ���� �� ���� AudioSource�� �ٿ���
    /// </summary>
    public void Init() {
        GameObject root = GameObject.Find("SoundManager"); // "SoundManager��� �̸��� ������Ʈ�� ã��
        if (root == null) { // ���ٸ�
            root = new GameObject { name = "SoundManager" }; // SoundManager������Ʈ�� ����� 
            root.AddComponent<SoundManager>();
            Object.DontDestroyOnLoad(root); // �ı� ��ȣ ����

            MakeSoundManager(root);
        }
        else {
            Object.DontDestroyOnLoad(root);
            MakeSoundManager(root);
        }
    }
    private void MakeSoundManager(GameObject root) {
        int child = root.transform.childCount;
        Debug.Log(child);
        if (child <= 0) {
            string[] soundName = System.Enum.GetNames(typeof(Sound)); // BGM Effect
            for (int i = 0; i < soundName.Length - 1; i++) { // -1 : MaxCount����
                GameObject soundObj = new GameObject { name = soundName[i] }; // �̸� �θ�� ��� Bgm, Effect��� �̸��� ������Ʈ��
                _audioSources[i] = soundObj.AddComponent<AudioSource>(); // AudioSource�� ���δ�
                _audioSources[i] = soundObj.GetComponent<AudioSource>();
                soundObj.transform.parent = root.transform;
            }
            _audioSources[0] = root.transform.GetChild(0).GetComponent<AudioSource>();
            _audioSources[1] = root.transform.GetChild(1).GetComponent<AudioSource>();

            _audioSources[(int)Sound.Bgm].loop = true; // bgm ���� �ݺ� ���

        }
    }

    /// <summary> Clear �Լ� �����
    /// public void LoadScene(Define.Scene type){
    ///     Managers.Clear();
    ///     SceneManager.LoadScene(GetSceneName(type));
    /// }
    ///
    /// public void Clear() {
    ///     CurrentScene.Clear();
    /// }
    /// </summary> ���� �ٲ� �� ȣ������!
    public void Clear() {
        // ����� ���� ��� ����, ���� ����
        foreach (AudioSource audioSource in _audioSources) {
            audioSource.clip = null;
            audioSource.Stop();
        }
        // ȿ���� Dictionary ����
        _audioClip.Clear(); // �׷��Ͼ����� ������ �ʹ� ���� ���� �� _audioClip�� Dictionary�� ��� �߰��Ǿ� �޸𸮰� �������� �� ����
    }
    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f) { // ������ audioClip�� �޾� �����Ŵ
        if (audioClip == null) {
            return;
        }
        if (type == Sound.Bgm) { // bgm���� ���
            AudioSource audioSource = _audioSources[(int)Sound.Bgm]; // _audioSources[(int)Sound.Bgm]�� ���� ���
            if (audioSource.isPlaying) { // ���࿡ ������̶�� ���߰�
                audioSource.Stop();
            }
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else if (type == Sound.Effect) {
            AudioSource audioSource = _audioSources[(int)(Sound.Effect)];// _audioSources[(int)Sound.Bgm]�� ���� ���
            // ȿ������ ��ø�Ǿ �Ǳ� ������ if���� ����
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip); // ���ϴ� Ŭ���� ��ø�ؼ� ��� �� �� �ְ� PlayOneShot�Լ� ���
        }
    }
    /// <summary> (path)Play �Լ� �����
    /// Managers.Sound.Play("UnityChan/univ0001", Define.Sound.Effect); 
    /// Managers.Sound.Play("UnityChan/univ0002"); // Effect �� ����Ʈ
    /// </summary>
    /// <param name="path">���</param>
    /// <param name="type">Sound type</param>
    /// <param name="pitch">��� �ӵ�</param>
    public void Play(string path, Sound type = Sound.Effect, float pitch = 1.0f) { // ������ path�� �޾Ƽ� �����Ŵ
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    private AudioClip GetOrAddAudioClip(string path, Sound type = Sound.Effect) { // path�� ���� �ش� Ŭ���� �ε��ϰ� �����Ѵ�
        if (path.Contains("Sounds/") == false) {
            path = $"Sounds/{path}"; // Sounds ���� �ȿ� ������ �ֱ�
        }
        AudioClip audioClip = null;

        if (type == Sound.Bgm) {
            //audioClip = Manager.Resource.Load<AudioClip>(path);
        }
        // ȿ������ ��� �ſ� ���� ����ϱ� ������ Dictionary�� �����صΰ� �����´�
        else if (type == Sound.Effect) {
            //_audioClip�� Dictionary�� �ش�path(Key)�� �����ϴ��� Ȯ���Ѵ� 
            if (_audioClip.TryGetValue(path, out audioClip) == false) { // ���� ���ٸ� �߰��Ѵ�
                                                                        //audioClip = Manager.Resource.Load<AudioClip>(path);
                _audioClip.Add(path, audioClip);
            }
            // ���� �ִٸ� �׳� �״�� return
        }

        if (audioClip == null) {
            Debug.Log($"Missing AudioSource...{path}");
        }
        return audioClip;
    }
    public void RegulateSound(Sound type, int volume) {
        if (type == Sound.Bgm) {
            _audioSources[0].volume = (float)((volume + 1) * 0.1);
        }
        else if (type == Sound.Effect) {
            _audioSources[1].volume = (float)((volume + 1) * 0.1);
        }
        else {
            Debug.LogError("sound error...");
        }
    }
}
