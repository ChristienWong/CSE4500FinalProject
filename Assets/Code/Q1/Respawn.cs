using UnityEngine;
using UnityEngine.SceneManagement;

namespace finalProject
{
    public class Respawn : MonoBehaviour
    {
        public Transform respawnPoint; 

        Rigidbody2D _rb;
        Vector3 _spawnPos;
        int _respawnLayer;

        [SerializeField]
        PlayerHealth _playerHealth;

        void Awake()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            CacheSpawnPoint();
            _respawnLayer = LayerMask.NameToLayer("Respawn");   

            if (_playerHealth == null)
            {
                _playerHealth = GetComponent<PlayerHealth>();
            }
            
        }

        public void DoRespawn()
        {
            if (_rb != null)
            {
                _rb.velocity = Vector2.zero;
                _rb.angularVelocity = 0f;
            }
            transform.position = _spawnPos;

            if (_playerHealth != null)
            {
                _playerHealth.ResetHealth();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == _respawnLayer) DoRespawn();
        }

        void OnDestroy()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            CacheSpawnPoint();
        }

        void CacheSpawnPoint()
        {
            if (respawnPoint == null)
            {
                respawnPoint = FindRespawnPointInScene();
            }

            _spawnPos = respawnPoint ? respawnPoint.position : transform.position;
        }

        Transform FindRespawnPointInScene()
        {
            GameObject taggedPoint = GameObject.FindGameObjectWithTag("Respawn");
            if (taggedPoint != null)
            {
                return taggedPoint.transform;
            }

            return null;
        }
    }
}
