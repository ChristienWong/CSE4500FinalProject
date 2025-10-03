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

        bool _hasTriggered;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (_hasTriggered) return;
            if (!other.CompareTag("Player")) return;

            _hasTriggered = true;

            if (_loadDelaySeconds <= 0f)
            {
                LoadTargetScene();
            }
            else
            {
                StartCoroutine(LoadAfterDelay());
            }
        }

        IEnumerator LoadAfterDelay()
        {
            yield return new WaitForSeconds(_loadDelaySeconds);
            LoadTargetScene();
        }

        void LoadTargetScene()
        {
            if (!string.IsNullOrWhiteSpace(_targetSceneName))
            {
                SceneManager.LoadScene(_targetSceneName);
                return;
            }

            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextIndex);
            }
            else
            {
                Debug.LogWarning($"LevelPortal on {name} cannot determine the next scene.");
            }
        }
    }
}
