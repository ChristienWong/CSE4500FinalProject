using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace finalProject
{
    public class Controller : MonoBehaviour
    {
        Rigidbody2D _rb;
        Collider2D _col;
        public Animator _animator;

        public float speed;
        public float jumpForce;
        public LayerMask ground;

        public GameObject projectilePrefab;
        public Transform ShootPoint;
        public float projectileSpeed = 13f;
        [SerializeField] Ammo ammoSystem;
                                              
        private Vector3 _spawnPos;
        private int _actualGroundLayer;
        private bool facingRight = true;
        private bool isJumping = false;
        float maxSpeed = 10f;
        [SerializeField] float shootCooldown = 0.25f; 
        float lastShootTime = -Mathf.Infinity;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip shootClip;
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
            _spawnPos = transform.position;
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            if (ammoSystem == null)
            {
                ammoSystem = GetComponentInChildren<Ammo>();

                if (ammoSystem == null)
                {
                    ammoSystem = FindObjectOfType<Ammo>();
                }
            }
        }
        
        void FixedUpdate()
        {
            _animator.SetFloat("Speed", _rb.velocity.magnitude);
            if (_rb.velocity.magnitude > 0)
            {
                _animator.speed = _rb.velocity.magnitude / 3f;
            }
            else
            {
                _animator.speed = 1f;
            }
        }


        void Update()
        {
            bool isMoving = false;

            // Move Left
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (_rb.velocity.x > -maxSpeed){
                    _rb.AddForce(Vector2.left * speed * Time.deltaTime);
                }
                isMoving = true;
                
                if(facingRight){
                    Flip();
                }
            }

            // Move Right
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (_rb.velocity.x < maxSpeed){
                    _rb.AddForce(Vector2.right * speed * Time.deltaTime);
                }               
                isMoving = true;

                if(!facingRight){
                    Flip();
                }
            }

            _animator.SetBool("isRunning", isMoving);

            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                _rb.AddForce(Vector2.up * jumpForce);
                isJumping = true; //set true on jump
                _animator.SetBool("isJumping", true);
                _animator.SetTrigger("JumpTrigger");
            }

             if (isJumping && IsGrounded())
            {
                isJumping = false; // reset when landed
                _animator.SetBool("isJumping", false);

            }

            float input = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Z) & !(Time.time < lastShootTime + shootCooldown)){
                Shoot();
                _animator.SetTrigger("ShootTrigger");
            }
        }
        bool IsGrounded()
        {
            Bounds bounds = _col.bounds;
            float extraHeight = 0.05f;
            RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, extraHeight, ground);
            return hit.collider != null;
        }

        void Flip()
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        void Shoot()
        {
            if (Time.time < lastShootTime + shootCooldown){
                return; //Cooldown
            }

            if (ammoSystem != null && !ammoSystem.TrySpendAmmo(1))
            {
                return;
            }

            if (projectilePrefab == null || ShootPoint == null)
            {
                return;
            }

            lastShootTime = Time.time;
            Debug.Log("Ammo before shooting: " + ammoSystem.CurrentAmmo);

            GameObject projectile = Instantiate(projectilePrefab, ShootPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(facingRight ? projectileSpeed : -projectileSpeed, 0);
            audioSource.PlayOneShot(shootClip);
        }
        
    }
}
