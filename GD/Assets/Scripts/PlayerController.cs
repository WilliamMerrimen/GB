using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRigidbody;
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;
    private Transform _playerPosition;
    private Vector3 inputVector;
    private int _statusToSprint;

    public float speed = 5f;
    public float jumpForse = 6f;
    public bool onGraund = true;
    public float mouseSens = 100f;
    public Transform gameCamera;

    private short _jumpCount = 0;
    private short _maxCountJump = 1;
    public bool isEnableDublJump = false;



    private void Awake()
   {
      _playerRigidbody = GetComponent<Rigidbody>();
      _playerInput = GetComponent<PlayerInput>();
      _playerPosition = GetComponent<Transform>();
      
      _playerInputActions = new PlayerInputActions();
      _playerInputActions.Enable();
      _playerInputActions.PlayerAction.Jump.performed += Jump;
      _playerInputActions.PlayerAction.Sprint.started += Sprint;
      _playerInputActions.PlayerAction.Sprint.performed += Sprint;
      _playerInputActions.PlayerAction.Sprint.canceled += Sprint;
    }

    

    private void FixedUpdate()
   {
       Vector2 inputVector = _playerInputActions.PlayerAction.Movement.ReadValue<Vector2>();
      
      _playerPosition.position += new Vector3(inputVector.x, 0 , inputVector.y) * speed * Time.deltaTime;
        if(inputVector.x == 1)
        {
            _statusToSprint = 1;
        }
        else if(inputVector.x == -1)
        {
            _statusToSprint = -1;
        }
        else if (inputVector.y == -1)
        {
            _statusToSprint = -2;
        }
        else if (inputVector.y == 1)
        {
            _statusToSprint = 2;
        }

        gameCamera.position += new Vector3(inputVector.x, 0f, inputVector.y ) * speed * Time.deltaTime;

        if (onGraund)
            _jumpCount = 0;
        if (isEnableDublJump)
            _maxCountJump = 2;
   }

   public void Jump(InputAction.CallbackContext context)
   {
       if (context.performed & _jumpCount < _maxCountJump) 
      {
         _playerRigidbody.AddForce(Vector3.up * jumpForse, ForceMode.Impulse);
         Debug.Log("Jump! " + context.phase);
         _jumpCount += 1;
      }
   }
    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(_statusToSprint == -1)
            {
                _playerRigidbody.AddForce(Vector3.left * 400f * Time.deltaTime, ForceMode.Impulse);
                gameCamera.position += new Vector3(-1f, 0f, 0f) * Time.deltaTime;
            }
            if (_statusToSprint == 1)
            {
                _playerRigidbody.AddForce(Vector3.right * 400f * Time.deltaTime, ForceMode.Impulse);
                gameCamera.position += new Vector3(1f, 0f, 0f) * Time.deltaTime;
            }
            if (_statusToSprint == 2)
            {
                _playerRigidbody.AddForce(Vector3.forward * 400f * Time.deltaTime, ForceMode.Impulse);
                gameCamera.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;
            }
            if (_statusToSprint == -2)
            {
                _playerRigidbody.AddForce(Vector3.forward * -400f * Time.deltaTime, ForceMode.Impulse);
                gameCamera.position += new Vector3(0f, 0f, 1f) * Time.deltaTime;
            }
            Debug.Log("Sprint! " + context.phase);
            speed = 5f;


        }
        else if(context.performed && speed == 7f)
        {
            Debug.Log("Sprint! " + context.phase);
            speed = 7f;
        }
        else if(context.canceled)
        {
            if (_statusToSprint == -1)
            {
                _playerRigidbody.AddForce(Vector3.left * -300f * Time.deltaTime, ForceMode.Impulse);
                gameCamera.position += new Vector3(0.5f, 0f, 0f) * Time.deltaTime;
                speed = 5f;
            }
            if (_statusToSprint == 1)
            {
                _playerRigidbody.AddForce(Vector3.right * -300f * Time.deltaTime, ForceMode.Impulse);
                gameCamera.position += new Vector3(-0.5f, 0f, 0f) * Time.deltaTime;
                speed = 5f;
            }
            if (_statusToSprint == 2)
            {
                _playerRigidbody.AddForce(Vector3.forward *-300f * Time.deltaTime, ForceMode.Impulse);
                gameCamera.position += new Vector3(0f, 0f, -0.5f) * Time.deltaTime;
                speed = 5f;
            }
            if (_statusToSprint == -2)
            {
                _playerRigidbody.AddForce(Vector3.forward * 300f * Time.deltaTime, ForceMode.Impulse);
                gameCamera.position += new Vector3(0f, 0f, -0.5f) * Time.deltaTime;
                speed = 5f;
            }
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Graund")
        {
            onGraund = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Graund")
        {
            onGraund = false;
            _jumpCount += 1;
        }
    }
}
