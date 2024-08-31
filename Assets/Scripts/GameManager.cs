using Live2D.Cubism.Core;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

        [SerializeField]
        private GameObject _canvas;

        [SerializeField]
        private TMP_Text _victoryText;

        [SerializeField]
        private AudioClip[] _tummyNoises;

        [SerializeField]
        private Animator _anim;

        private AudioSource _source;

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

        private int _gotLastClick;

        private bool _canReset;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _timer = RefTimer;
            _target = Instantiate(_progress[_progressIndex], Vector2.zero, Quaternion.identity);
            _param = _target.GetComponentInChildren<CubismParameter>();
            UpdateUI();
        }

        public void IncreaseAnimValue(float mult)
        {
            if (_animGoUp)
            {
                _animValue += mult;
                if (_animValue > 1f)
                {
                    _animValue = 1f;
                    _animGoUp = false;
                    _source.PlayOneShot(_tummyNoises[Random.Range(0, _tummyNoises.Length)]);
                }
            }
            else
            {
                _animValue -= mult;
                if (_animValue < -1f)
                {
                    _animValue = -1f;
                    _animGoUp = true;
                    _source.PlayOneShot(_tummyNoises[Random.Range(0, _tummyNoises.Length)]);
                }
            }
        }

        private void Update()
        {
            IncreaseAnimValue(Time.deltaTime / 2f * (_gotLastClick > 0 ? 3f : 1f));
            if (_gotLastClick > 0) _gotLastClick--;

            if (_isActive && !_didGameEnd)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    _didGameEnd = true;
                    _canvas.SetActive(false);
                    _victoryText.text = "You got digested";
                    StartCoroutine(WaitAndDo(.8f, () =>
                    {
                        Destroy(_target);
                        _target = Instantiate(_loose, Vector2.zero, Quaternion.identity);
                        _param = _target.GetComponentInChildren<CubismParameter>();
                    }));
                    StartCoroutine(AllowReset());
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
            _timerText.text = $"Time to get digested: {_timer:00}";
        }

        public void OnAction(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started && !_didGameEnd)
            {
                if (_canReset)
                {
                    SceneManager.LoadScene("Main");
                }
                else
                {
                    if (!_isActive) _isActive = true;

                    _clickCount++;
                    _gotLastClick = 10;

                    _progressBar.localScale = new(1f - ((_progressIndex * RefClickCount + _clickCount) / (float)(RefClickCount * _progress.Length)), 1f, 1f);
                    if (_clickCount == RefClickCount)
                    {
                        _clickCount = 0;
                        _progressIndex++;
                        _timer += 10f;

                        _anim.SetTrigger("Play");
                        GameObject t;
                        if (_progressIndex == _progress.Length)
                        {
                            t = _win;
                            _didGameEnd = true;
                            _canvas.SetActive(false);
                            _victoryText.text = "You escaped!";
                            StartCoroutine(AllowReset());
                        }
                        else
                        {
                            t = _progress[_progressIndex];
                        }
                        _animValue = 0f;
                        StartCoroutine(WaitAndDo(.8f, () =>
                        {
                            Destroy(_target);
                            _target = Instantiate(t, Vector2.zero, Quaternion.identity);
                            _param = _target.GetComponentInChildren<CubismParameter>();
                        }));
                    }
                }
            }
        }

        private IEnumerator WaitAndDo(float time, System.Action callback)
        {
            yield return new WaitForSeconds(time);
            callback();
        }

        private IEnumerator AllowReset()
        {
            yield return new WaitForSeconds(2f);
            _canReset = true;
        }
    }
}
