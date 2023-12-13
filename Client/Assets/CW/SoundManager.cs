using UnityEngine;

namespace Karin
{

    public enum Sound : ushort
    {
        NONE = 0,
        WALK,
        DEAD,
        BACKSOUND,
        ATTACKFORWARD,
        ATTACKBACK,
        HIT,
        REVIVE,
        GAMEENDWIN,
        GAMEENDLOSE,
        GAMESTART,
        CLICKSOUND,
    }

    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {

        [SerializeField] private AudioClip Walk;
        [SerializeField] private AudioClip Dead;
        [SerializeField] private AudioClip BackSound;
        [SerializeField] private AudioClip AttackForward;
        [SerializeField] private AudioClip AttackBack;
        [SerializeField] private AudioClip Hit;
        [SerializeField] private AudioClip Revive;
        [SerializeField] private AudioClip GameEndWin;
        [SerializeField] private AudioClip GameEndLose;
        [SerializeField] private AudioClip GameStart;
        [SerializeField] private AudioClip ClickSound;

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void SoundHandler(Sound sound = Sound.NONE)
        {
            switch (sound)
            {
                case Sound.NONE:
                    Debug.LogWarning("None Is Not Sound");
                    break;
                case Sound.WALK: //함
                    _audioSource.PlayOneShot(Walk);
                    break;
                case Sound.DEAD: //함
                    _audioSource.PlayOneShot(Dead);
                    break;
                case Sound.BACKSOUND: //
                    _audioSource.PlayOneShot(BackSound);
                    break;
                case Sound.ATTACKFORWARD: //함
                    _audioSource.PlayOneShot(AttackForward);
                    break;
                case Sound.ATTACKBACK: // 함
                    _audioSource.PlayOneShot(AttackBack);
                    break;
                case Sound.HIT: // 함
                    _audioSource.PlayOneShot(Hit);
                    break;
                case Sound.REVIVE: // 함
                    _audioSource.PlayOneShot(Revive);
                    break;
                case Sound.GAMEENDWIN: // 함
                    _audioSource.PlayOndeShot(GameEndWin); 
                    break;
                case Sound.GAMEENDLOSE: // 함
                    _audioSource.PlayOneShot(GameEndLose);
                    break;
                case Sound.GAMESTART: //
                    _audioSource.PlayOneShot(GameStart);
                    break;
                case Sound.CLICKSOUND: //
                    _audioSource.PlayOneShot(ClickSound);
                    break;
                default:
                    break;
            }
        }





    }
}
