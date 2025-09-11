using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace finalProject
{
    public class Controller : MonoBehaviour
    {
        // Outlets
        Rigidbody2D _rb;

        // Configuration
        public float speed;
        public float jumpForce;
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rb.AddForce(Vector2.up * jumpForce);
            }
        }
    }
}
