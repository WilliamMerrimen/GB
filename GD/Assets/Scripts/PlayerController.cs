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

    public float speed = 5f;
    public float jumpForse = 6f;
    public bool onGraund = true;
    public bool isLadder = false;

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
   }

    private void FixedUpdate()
   {
       Vector2 inputVector = _playerInputActions.PlayerAction.Movement.ReadValue<Vector2>();
       
       if(!isLadder) 
           _playerPosition.position += new Vector3(inputVector.x, 0 , inputVector.y) * speed * Time.fixedDeltaTime;
       else
           _playerPosition.position += new Vector3(0, (inputVector.x != 0 ? inputVector.x : inputVector.y) , 0) * speed * Time.fixedDeltaTime;
       
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
            isLadder = true;
            _playerRigidbody.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isLadder = false;
            _playerRigidbody.useGravity = true;
        }
    }
}
