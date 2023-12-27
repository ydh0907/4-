using System.Collections.Generic;
using UnityEngine;

public enum Sound {
    Bgm,
    Effect,
    MaxCount //�׳� enum�� ������ ���� ���� ����(�ƹ��͵� �ƴ�)
}
public class SoundManager : MonoSingleton<SoundManager> {
    public SoundManager() { }
    AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClip = new Dictionary<string, AudioClip>();

    public int bgmValue = 9;
    public int effectValue = 9;

    /// <summary>
    /// SoundManager��� ������Ʈ�� ���� �� �Ʒ��� Sound�� �ִ� Ÿ�� ��ŭ�� ������Ʈ ���� �� ���� AudioSource�� �ٿ���
    /// </summary>
    public void Init() {
        GameObject root = GameObject.Find("SoundManager");
        if (root == null) { // ���ٸ�
            root = new GameObject { name = "SoundManager" };
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
        if (child <= 0) {
            string[] soundName = System.Enum.GetNames(typeof(Sound));
            for (int i = 0; i < soundName.Length - 1; i++) {
                GameObject soundObj = new GameObject { name = soundName[i] };
                _audioSources[i] = soundObj.AddComponent<AudioSource>();
                _audioSources[i] = soundObj.GetComponent<AudioSource>();
                soundObj.transform.parent = root.transform;
            }
            _audioSources[0] = root.transform.GetChild(0).GetComponent<AudioSource>();
            _audioSources[1] = root.transform.GetChild(1).GetComponent<AudioSource>();

            _audioSources[(int)Sound.Bgm].loop = true;

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
        _audioClip.Clear();
    }
    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f) {
        if (audioClip == null) {
            return;
        }
        if (type == Sound.Bgm) {
            AudioSource audioSource = _audioSources[(int)Sound.Bgm];
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else if (type == Sound.Effect) {
            AudioSource audioSource = _audioSources[(int)(Sound.Effect)];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }
    /// <summary> (path)Play �Լ� �����
    /// Managers.Sound.Play("UnityChan/univ0001", Define.Sound.Effect); 
    /// Managers.Sound.Play("UnityChan/univ0002"); // Effect �� ����Ʈ
    /// </summary>
    /// <param name="path">���</param>
    /// <param name="type">Sound type</param>
    /// <param name="pitch">��� �ӵ�</param>
    public void Play(string path, Sound type = Sound.Effect, float pitch = 1.0f) {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    private AudioClip GetOrAddAudioClip(string path, Sound type = Sound.Effect) {
        if (path.Contains("Sounds/") == false) {
            path = $"Sounds/{path}";
        }
        //Debug.Log(path);
        AudioClip audioClip = null;

        if (type == Sound.Bgm) {
            audioClip = Resources.Load(path, typeof(AudioClip)) as AudioClip;
        }
        else if (type == Sound.Effect) {
            if (_audioClip.TryGetValue(path, out audioClip) == false) {
                audioClip = Resources.Load(path, typeof(AudioClip)) as AudioClip;
                _audioClip.Add(path, audioClip);
            }
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
