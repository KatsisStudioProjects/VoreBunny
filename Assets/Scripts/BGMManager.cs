using UnityEngine;
using UnityEngine.UI;

namespace VoreBunny
{
    public class BGMManager : MonoBehaviour
    {
        public static bool UseSFX = true;

        [SerializeField]
        private Image _btnSfx, _btnBgm;

        [SerializeField]
        private Sprite _sfxOn, _sfxOff, _bgmOn, _bgmOff;

        private AudioSource _bgm;

        private void Awake()
        {
            _bgm = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }

        public void ToggleSfx()
        {
            UseSFX = !UseSFX;
            _btnSfx.sprite = UseSFX ? _sfxOn : _sfxOff;
        }

        public void ToggleBgm()
        {
            _bgm.volume = _bgm.volume == 0 ? 0.15f : 0f;
            _btnBgm.sprite = _bgm.volume == 0f ? _bgmOff : _bgmOn;
        }
    }
}
