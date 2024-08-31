using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
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

        [SerializeField]
        private RectTransform _progressBar;

        private GameObject _target;

        private const float RefTimer = 20f;
        private float _timer;
        private bool _isActive;
        private int _progressIndex;

        private int _clickCount;
        private int RefClickCount = 100;

        private bool _didGameEnd;

        private CubismParameter _param;
        private float _animValue;
        private bool _animGoUp;

        private void Awake()
        {
            _timer = RefTimer;
            _target = Instantiate(_progress[_progressIndex], Vector2.zero, Quaternion.identity);
            _param = _target.GetComponentInChildren<CubismParameter>();
            UpdateUI();
        }

        private void Update()
        {
            if (_animGoUp)
            {
                _animValue += Time.deltaTime;
                if (_animValue > 1f)
                {
                    _animValue = 1f;
                    _animGoUp = false;
                }
            }
            else
            {
                _animValue -= Time.deltaTime;
                if (_animValue < -1f)
                {
                    _animValue = -1f;
                    _animGoUp = true;
                }
            }

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

        private void LateUpdate()
        {
            _param.Value = _animValue;
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

                _progressBar.localScale = new(1f - ((_progressIndex * RefClickCount + _clickCount) / (float)(RefClickCount * _progress.Length)), 1f, 1f);
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
                    _param = _target.GetComponentInChildren<CubismParameter>();
                    _animValue = 0f;
                }
            }
        }
    }
}
