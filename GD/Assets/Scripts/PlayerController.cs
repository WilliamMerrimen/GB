using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRigidbody;
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;
    private Transform _playerPosition;
    private Vector3 _inputVector;
    private Vector3 _rayStart;
    private Vector3 _rayDirection;

    public float speed = 5f;
    public float jumpForse = 6f;
    public bool onGraund = true;
    public LayerMask layerMask;
    public string vector;

    private int _vectorToClimb;
    private short _jumpCount = 0;
    private short _maxCountJump = 1;
    public bool isEnableDublJump = false;
    public bool isHit = false;



    private void Awake()
   {
      _playerRigidbody = GetComponent<Rigidbody>();
      _playerInput = GetComponent<PlayerInput>();
      _playerPosition = GetComponent<Transform>();
      
      _playerInputActions = new PlayerInputActions();
      _playerInputActions.Enable();
      _playerInputActions.PlayerAction.Jump.performed += Jump;
      _rayDirection = transform.right;
   }

    private void Start()
    {
        
    }
    private void FixedUpdate()
   {
        
        Vector2 inputVector = _playerInputActions.PlayerAction.Movement.ReadValue<Vector2>();
        Vector3 position = _playerPosition.position;
       _rayStart = new Vector3(position.x,position.y - 0.35f, position.z);
        if (!isHit)
            _playerPosition.position += new Vector3(inputVector.x, 0, inputVector.y) * speed * Time.fixedDeltaTime;
        else
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                _playerPosition.position += new Vector3(inputVector.x * (speed-2), Math.Abs(inputVector.x != 0 ? inputVector.x  : inputVector.y) * speed, inputVector.y * (speed-2)) * Time.fixedDeltaTime;
                
            }
            else if (Input.GetKey(KeyCode.LeftShift) && !onGraund)
            {
                Debug.Log("Shift!!!");
                _playerPosition.position -= new Vector3(0, 0.01f, 0);
            }   
        }
        if (inputVector.x != 0 || inputVector.y != 0)
        {
           if (inputVector.x == 1)
            {
                _rayDirection = transform.right;
                vector = "right";
                _vectorToClimb = 1;
            }
               
           else if (inputVector.x == -1)
            {
                _rayDirection = -transform.right;
                vector = "left";
                _vectorToClimb = 2;
            }    
           if (inputVector.y == 1)
            {
                _rayDirection = transform.forward;
                vector = "forward";
                _vectorToClimb = 3;
            }
           else if (inputVector.y == -1)
            {
                _rayDirection = -transform.forward;
                vector = "back";
                _vectorToClimb = 4;
            }   
        }


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
   
   public void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Graund"))
            onGraund = true;
        
    }
   
    public void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Graund"))
        {
            onGraund = false;
            _jumpCount += 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            
            onGraund = false;
            isHit = true;
            _playerRigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            _playerRigidbody.freezeRotation = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isHit = false;
            _playerRigidbody.useGravity = true;
            _playerRigidbody.constraints = RigidbodyConstraints.None;
            _playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
