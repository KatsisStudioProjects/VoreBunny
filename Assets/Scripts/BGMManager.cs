using UnityEngine;

namespace VoreBunny
{
    public class BGMManager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
