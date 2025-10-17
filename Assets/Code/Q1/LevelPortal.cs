using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace finalProject
{
    public class LevelPortal : MonoBehaviour
    {
        [SerializeField]
        string _targetSceneName;

        [SerializeField]
        float _loadDelaySeconds = 0.25f;

        [SerializeField]
        bool _showGameOverOnTrigger;

        [SerializeField]
        GameOverScreen _gameOverScreen;

        bool _hasTriggered;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (_hasTriggered) return;
            if (!other.CompareTag("Player")) return;

            _hasTriggered = true;
            Debug.Log($"LevelPortal on {name} triggered by {other.name} in scene {SceneManager.GetActiveScene().name}.");

            if (ShouldShowGameOver()) return;

            string activeScene = SceneManager.GetActiveScene().name;
            string destinationScene = ResolveDestinationScene(activeScene);
            if (string.IsNullOrWhiteSpace(destinationScene))
            {
                Debug.LogWarning($"LevelPortal on {name} could not resolve a destination scene from '{activeScene}'.");
                return;
            }

            if (_loadDelaySeconds <= 0f) LoadTargetScene(destinationScene);
            else StartCoroutine(LoadAfterDelay(destinationScene));
        }

        bool ShouldShowGameOver()
        {
            if (!_showGameOverOnTrigger) return false;

            if (_gameOverScreen == null)
            {
                _gameOverScreen = FindObjectOfType<GameOverScreen>(true);
                Debug.Log(_gameOverScreen == null
                    ? "LevelPortal could not find a GameOverScreen instance."
                    : $"LevelPortal using GameOverScreen on {_gameOverScreen.name}.");
            }

            if (_gameOverScreen == null)
            {
                Debug.LogWarning("LevelPortal was asked to show a GameOverScreen, but none were found.");
                return false;
            }

            _gameOverScreen.Show();
            Debug.Log("LevelPortal displayed GameOverScreen and stopped scene transition.");
            return true;
        }

        IEnumerator LoadAfterDelay(string destinationScene)
        {
            yield return new WaitForSeconds(_loadDelaySeconds);
            LoadTargetScene(destinationScene);
        }

        void LoadTargetScene(string destinationScene)
        {
            if (string.IsNullOrWhiteSpace(destinationScene))
            {
                Debug.LogWarning($"LevelPortal on {name} attempted to load an empty scene name.");
                return;
            }

            SceneManager.LoadScene(destinationScene);
        }

        string ResolveDestinationScene(string currentScene)
        {
            if (!string.IsNullOrWhiteSpace(_targetSceneName))
            {
                return _targetSceneName;
            }

            if (string.Equals(currentScene, "Q1", System.StringComparison.OrdinalIgnoreCase))
            {
                return "Q2";
            }

            if (string.Equals(currentScene, "Q2", System.StringComparison.OrdinalIgnoreCase))
            {
                return "Q3";
            }

            return string.Empty;
        }
    }
}
