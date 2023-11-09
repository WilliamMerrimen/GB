using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRigidbody;
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;
    private Transform _playerPosition;
    private Vector3 _inputVector;
    private int _statusToSprint;
    private int _canToSprint;

    public float speed = 5f;
    public float jumpForse = 6f;
    public bool onGraund = true;
    public float kdSprint;
    public float timer;

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
       timer -= Time.fixedDeltaTime;
        if(timer >= kdSprint)
        {
            _canToSprint = 1;
        }
        else
        {
            _canToSprint = 0;
        }
       Vector2 inputVector = _playerInputActions.PlayerAction.Movement.ReadValue<Vector2>();
       
      _playerPosition.position += new Vector3(inputVector.x, 0 , inputVector.y) * speed * Time.fixedDeltaTime;

      if(inputVector.x == 1)
            _statusToSprint = 1;
        
      else if(inputVector.x == -1)
            _statusToSprint = -1;
        
      if (inputVector.y == -1)
            _statusToSprint = -2;
        
      else if (inputVector.y == 1)
            _statusToSprint = 2;


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
                if(timer <= 0f)
                {
                _playerRigidbody.AddForce(Vector3.left * 400f * Time.deltaTime, ForceMode.VelocityChange);
                }
            }
            if (_statusToSprint == 1)
            {
                if (timer <= 0f)
                {
                _playerRigidbody.AddForce(Vector3.right * 400f * Time.deltaTime, ForceMode.VelocityChange);
                }
            }
            if (_statusToSprint == 2)
            {
                if (timer <= 0f)
                {
                _playerRigidbody.AddForce(Vector3.forward * 400f * Time.deltaTime, ForceMode.VelocityChange);
                }
            }
            if (_statusToSprint == -2)
            {
                if (timer <= 0f)
                {
                _playerRigidbody.AddForce(Vector3.forward * -400f * Time.deltaTime, ForceMode.VelocityChange);
                }
            }
            Debug.Log("Sprint! " + context.phase);
            speed = 5f;


        }
        else if(context.canceled)
        {
            speed = 3f;
            timer = kdSprint;
            //            if (_statusToSprint == -1)
            //            {
            //                _playerRigidbody.AddForce(Vector3.left * -300f * Time.deltaTime, ForceMode.Impulse);
            //                speed = 3f;
            //            }
            //            if (_statusToSprint == 1)
            //            {
            //                _playerRigidbody.AddForce(Vector3.right * -300f * Time.deltaTime, ForceMode.Impulse);
            //                speed = 3f;
            //            }
            //            if (_statusToSprint == 2)
            //            {
            //                _playerRigidbody.AddForce(Vector3.forward *-300f * Time.deltaTime, ForceMode.Impulse);
            //                speed = 3f;
            //            }
            //            if (_statusToSprint == -2)
            //            {
            //                _playerRigidbody.AddForce(Vector3.forward * 300f * Time.deltaTime, ForceMode.Impulse);
            //                speed = 3f;
            //            }
        }
    }
    
    public void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Graund"))
        {
            onGraund = true;
        }
    }
    public void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Graund"))
        {
            onGraund = false;
            _jumpCount += 1;
        }
    }
}
