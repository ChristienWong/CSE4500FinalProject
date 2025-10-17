using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace finalProject
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField]
        GameObject _panelRoot;

        [SerializeField]
        Button _restartButton;

        [SerializeField]
        bool _hideOnStart = true;

        [SerializeField]
        string _fallbackSceneName = "Q1";

        void Awake()
        {
            if (_panelRoot == null)
            {
                _panelRoot = gameObject;
            }

            if (_restartButton == null)
            {
                _restartButton = GetComponentInChildren<Button>(true);
            }
        }

        void OnEnable()
        {
            if (_restartButton != null)
            {
                _restartButton.onClick.AddListener(HandleRestartClicked);
            }
        }

        void Start()
        {
            if (_hideOnStart)
            {
                Hide();
            }
        }

        void OnDisable()
        {
            if (_restartButton != null)
            {
                _restartButton.onClick.RemoveListener(HandleRestartClicked);
            }
        }

        public void Show()
        {
            if (_panelRoot != null && !_panelRoot.activeSelf)
            {
                _panelRoot.SetActive(true);
            }
        }

        public void Hide()
        {
            if (_panelRoot != null && _panelRoot.activeSelf)
            {
                _panelRoot.SetActive(false);
            }
        }

        void HandleRestartClicked()
        {
            Hide();

            SceneManager.LoadScene(_fallbackSceneName);
        }
    }
}
