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

    private void FixedUpdate()
   {
       Vector2 inputVector = _playerInputActions.PlayerAction.Movement.ReadValue<Vector2>();
       if (!isHit)
           _playerPosition.position += new Vector3(inputVector.x, 0, inputVector.y) * speed * Time.fixedDeltaTime;
       else
           _playerPosition.position += new Vector3(0, Math.Abs(inputVector.x !=0 ? inputVector.x : inputVector.y), 0) * speed * Time.fixedDeltaTime;
       Vector3 position = _playerPosition.position;
       _rayStart = new Vector3(position.x,position.y - 0.35f, position.z);
       
       if (inputVector.x != 0 || inputVector.y != 0)
       {
           if (inputVector.x == 1)
               _rayDirection = transform.right;

           else if (inputVector.x == -1)
               _rayDirection = -transform.right;
           
           if (inputVector.y == 1)
               _rayDirection = transform.forward;
           else if (inputVector.y == -1)
               _rayDirection = -transform.forward;
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
            _playerRigidbody.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isHit = false;
            _playerRigidbody.useGravity = true;
        }
    }
}
