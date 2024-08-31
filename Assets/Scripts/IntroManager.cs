using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace VoreBunny
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private Sprite[] _sprites;

        private int _progress;

        public void OnAction(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started)
            {
                _progress++;

                if (_progress == _sprites.Length) SceneManager.LoadScene("Main");
                else _image.sprite = _sprites[_progress];
            }
        }
    }
}
