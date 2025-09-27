using UnityEngine;

namespace finalProject
{
    public class Respawn : MonoBehaviour
    {
        public Transform respawnPoint; 

        Rigidbody2D _rb;
        Vector3 _spawnPos;
        int _respawnLayer;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spawnPos = respawnPoint ? respawnPoint.position : transform.position;
            _respawnLayer = LayerMask.NameToLayer("Respawn");   
        }

        public void DoRespawn()
        {
            if (_rb != null)
            {
                _rb.velocity = Vector2.zero;
                _rb.angularVelocity = 0f;
            }
            transform.position = _spawnPos;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == _respawnLayer) DoRespawn();
        }
    }
}

