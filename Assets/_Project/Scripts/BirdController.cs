using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    public float jumpForce = 0;
    private Camera _camera;
    [HideInInspector] public GameManager m_manager;

    [SerializeField]private bool isDead;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_rigidbody2D == null) Debug.LogError("BirdController: No Rigidbody2D found!");
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        
        if (!(Mathf.Abs(transform.position.y) > _camera.orthographicSize)) return;
        
        m_manager.GameOver();
        enabled = false;

    }

    private void HandleInput()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _rigidbody2D.AddForceY(jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        m_manager.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        m_manager.AddPoint();
    }
}
