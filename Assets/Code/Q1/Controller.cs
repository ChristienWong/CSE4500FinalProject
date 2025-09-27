using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace finalProject
{
    public class Controller : MonoBehaviour
    {
        // Outlets
        Rigidbody2D _rb;
        Collider2D _col;

        // Configuration
        public float speed;
        public float jumpForce;
        public LayerMask ground;

                                
        // public Transform respawnPoint;                
        private Vector3 _spawnPos;
        private int _actualGroundLayer;



        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>();

            _spawnPos = transform.position; 
        }


        void Update()
        {
            // Move Left
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _rb.AddForce(Vector2.left * speed * Time.deltaTime);
            }

            // Move Right
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _rb.AddForce(Vector2.right * speed * Time.deltaTime);
            }

            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                _rb.AddForce(Vector2.up * jumpForce);
            }
        }
        bool IsGrounded()
        {
            Bounds bounds = _col.bounds;
            float extraHeight = 0.05f;
            RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, extraHeight, ground);
            return hit.collider != null;
        }
    }
}
