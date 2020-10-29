using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField]
    private float _speed = 5.0f, _pushPower = 2.0f;
    [SerializeField]
    private float _gravity = 1.0f;
    [SerializeField]
    private float _jumpHeight = 15.0f;
    private float _yVelocity;
    private bool _canDoubleJump = false;
    [SerializeField]
    private int _coins;
    private UIManager _uiManager;
    [SerializeField]
    private int _lives = 3;

    private Vector3 _direction, _velocity;
    private Vector3 _wallSurfaceNormal;

    private bool _canWallJump = false;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        _uiManager.UpdateLivesDisplay(_lives);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");


        if (_controller.isGrounded == true)
        {
            _canWallJump = true;
            _direction = new Vector3(horizontalInput, 0, 0);
            _velocity = _direction * _speed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && _canWallJump == false)
            {
                if (_canDoubleJump == true)
                {
                    _yVelocity += _jumpHeight;
                    _canDoubleJump = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && _canWallJump == true)
            {
                _yVelocity = _jumpHeight;
                _velocity = _wallSurfaceNormal * _speed;               
            }

            _yVelocity -= _gravity;
        }

        _velocity.y = _yVelocity;

        _controller.Move(_velocity * Time.deltaTime);
    }

    public void AddCoins()
    {
        _coins++;

        _uiManager.UpdateCoinDisplay(_coins);
    }

    public int GetCoins()
    {
        return _coins;
    }

    public void Damage()
    {
        _lives--;

        _uiManager.UpdateLivesDisplay(_lives);

        if (_lives < 1)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_controller.isGrounded == false && hit.transform.tag == "Wall")
        {
            _wallSurfaceNormal = hit.normal;
            Debug.DrawRay(hit.point, hit.normal, Color.blue, 256.0f);
            _canWallJump = true;
        }

        if (_controller.isGrounded == true && hit.transform.tag == "Moveable")
        {
            Rigidbody body = hit.collider.attachedRigidbody;


            if (body == null || body.isKinematic) //makes sure the collider has a rigidbody
                return;

            if (hit.transform.position.y < -0.3f) //makes sure the collider isn't below us
                return;

            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, 0);

            body.velocity = pushDirection * _pushPower;
        }
    }
}
