using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VoreBunny
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _timerText;

        [SerializeField]
        private GameObject[] _progress;

        [SerializeField]
        private GameObject _win, _loose;

        [SerializeField]
        private GameObject _instructions;

        private GameObject _target;

        private const float RefTimer = 20f;
        private float _timer;
        private bool _isActive;
        private int _progressIndex;

        private int _clickCount;
        private int RefClickCount = 100;

        private bool _didGameEnd;

        private void Awake()
        {
            _timer = RefTimer;
            _target = Instantiate(_progress[_progressIndex], Vector2.zero, Quaternion.identity);
            UpdateUI();
        }

        private void Update()
        {
            if (_isActive && !_didGameEnd)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    Destroy(_target);
                    _target = Instantiate(_loose, Vector2.zero, Quaternion.identity);
                    _didGameEnd = true;
                }
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            _timerText.text = $"{_timer:00}";
        }

        public void OnAction(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started && !_didGameEnd)
            {
                if (!_isActive) _isActive = true;
                _instructions.SetActive(false);

                _clickCount++;
                if (_clickCount == RefClickCount)
                {
                    _clickCount = 0;
                    _progressIndex++;
                    _timer += 10f;

                    Destroy(_target);
                    GameObject t;
                    if (_progressIndex == _progress.Length)
                    {
                        t = _win;
                        _didGameEnd = true;
                    }
                    else
                    {
                        t = _progress[_progressIndex];
                    }
                    _target = Instantiate(t, Vector2.zero, Quaternion.identity);
                }
            }
        }
    }
}
