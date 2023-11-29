using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    private Animator _playerAnivator;
    
    private Rigidbody _playerRigidbody;
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;
    private Transform _playerPosition;
    private Vector3 _inputVector;

    public Transform playerPos;
    public float speed = 5f;
    public float jumpForse = 6f;
    public bool onGraund = true;
    public string vector;
    public bool smallPlayer = false;
    public float sizeSmallPlayer = 0.3f;
    public Material materialVisible;
    public Material materialInvisible;
    public float delayToInvis;
    public float kdInvisible;
    public bool invisible = false;
    public bool canToInvis = true;
    private bool _isChest = false;
    
    public GameObject teleportMenu;
    public GameObject tipPressE;
    public GameObject nextLevel;

    private bool _teleportMenuCunOpen = false;
    private MeshRenderer _meshRenderer;
    private short _jumpCount = 0;
    private short _maxCountJump = 1;
    public bool isEnableDublJump = false;
    public bool isHit = false;
    private bool _hasKey = false;


    public int Money = 0;
    
    private void Awake()
    {
        _meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _playerPosition = GetComponent<Transform>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.PlayerAction.Jump.performed += Jump;
        _playerInputActions.PlayerAction.Skill.performed += Skill;
        _playerInputActions.PlayerAction.InteractionButton.performed += InteractionButton;
        _playerInputActions.PlayerAction.Resizeble.performed += Resizeble;
        nextLevel.SetActive(false);
        _playerAnivator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (smallPlayer)
        {
            transform.localScale = new Vector3(0.8f - sizeSmallPlayer, 0.8f - sizeSmallPlayer, 0.8f - sizeSmallPlayer);
        }
        else if (smallPlayer == false)
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        if (_teleportMenuCunOpen == false)
            teleportMenu.SetActive(false);
        
        if (invisible & canToInvis)
        {
            _meshRenderer.material = materialInvisible;
            StartCoroutine(WaitForInvOne());
        }
        
        else if (invisible == false)
            _meshRenderer.material = materialVisible;
        
        Vector2 inputVector = _playerInputActions.PlayerAction.Movement.ReadValue<Vector2>();

        if (!isHit)
        {
            _playerPosition.position += new Vector3(inputVector.x, 0, inputVector.y) * speed * Time.fixedDeltaTime;
            if (inputVector.x != 0 || inputVector.y != 0)
            {
                _playerAnivator.SetBool("isRunning", true);
                if (inputVector.x == -1)
                    _playerPosition.eulerAngles = new Vector3(0,-90,0);
                if(inputVector.x == 1)
                    _playerPosition.eulerAngles = new Vector3(0,90,0);
                if(inputVector.y == 1)
                    _playerPosition.eulerAngles = new Vector3(0,0,0);
                if(inputVector.y == -1)
                    _playerPosition.eulerAngles = new Vector3(0,180,0);
            }

            if(inputVector.x == 0 && inputVector.y == 0)
                _playerAnivator.SetBool("isRunning", false);
        }
        else
        {
            if (!Input.GetKey(KeyCode.LeftShift))
                _playerPosition.position += new Vector3(inputVector.x * (speed-2), Math.Abs(inputVector.x != 0 ? inputVector.x  : inputVector.y) * speed, inputVector.y * (speed-2)) * Time.fixedDeltaTime;
            
            
            else if (Input.GetKey(KeyCode.LeftShift) && !onGraund)
            {
                Debug.Log("Shift!!!");
                _playerPosition.position -= new Vector3(0, 0.5f, 0) * Time.fixedDeltaTime;
            }   
        }
        
        if (onGraund)
            _jumpCount = 0;
        
        if (isEnableDublJump)
            _maxCountJump = 2;
        playerPos = _playerPosition;
    }
    private IEnumerator WaitForInvOne()
    {
        yield return new WaitForSeconds(delayToInvis);
        invisible = false;
        canToInvis = false;
        StartCoroutine(KDInvisible());
    }
    private IEnumerator KDInvisible()
    {
        yield return new WaitForSeconds(kdInvisible);
        canToInvis = true;
    }

    public void Jump(InputAction.CallbackContext context)
    { 
      if (context.performed & _jumpCount < _maxCountJump) 
      {
         _playerRigidbody.AddForce(Vector3.up * jumpForse, ForceMode.Impulse);
         Debug.Log("Jump! " + context.phase);
         _jumpCount += 1;
         onGraund = false;
         _playerAnivator.Play("Jumping Up");
      }
    }
    public void Big()
    {
        smallPlayer = false;
    }
    public void Small()
    {
        smallPlayer = true;
    }
    public void Resizeble(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (smallPlayer)
            {
                Debug.Log("Big");
                if (onGraund)
                {
                    _playerRigidbody.AddForce(Vector3.up * (jumpForse - 3), ForceMode.Impulse);
                    onGraund = false;
                }
                Invoke("Big", 0.2f);
            }
            else if (!smallPlayer)
            {
                Debug.Log("Small");
                
                Invoke("Small", 0.2f);
            }
        }
    }
    public void Skill(InputAction.CallbackContext context)
    {
        if (context.performed)
            invisible = true;
        
    }
    public void InteractionButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_teleportMenuCunOpen)
                teleportMenu.SetActive(true);
            if (_isChest)
            {
                Debug.Log("Chest opened!");
            }
        }
    }
    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Graund"))
            onGraund = true;
        if (other.collider.CompareTag("NextLevel") && _hasKey)
            nextLevel.SetActive(true);

        if (other.collider.CompareTag("Chest"))
        {
            tipPressE.SetActive(true);
            _isChest = true;
        }
        
        if (other.collider.CompareTag("Teleport"))
        {
            _teleportMenuCunOpen = true;
            tipPressE.SetActive(true);
        }
    }
    public void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Chest"))
        {
            tipPressE.SetActive(false);
            _isChest = false;
        }
        
        if (other.collider.CompareTag("NextLevel"))
            nextLevel.SetActive(false);

        if (other.collider.CompareTag("Teleport"))
        {
            _teleportMenuCunOpen = false;
            tipPressE.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("needlesTrap"))
        {
            Debug.Log("You dead");
        }
        if (other.CompareTag("Ladder"))
        {
            onGraund = false;
            isHit = true;
            _playerRigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            _playerRigidbody.freezeRotation = true;
        }

        if (other.CompareTag("Key"))
        {
            _hasKey = true;
            Debug.Log("Has Key!");
            Destroy(other.gameObject);
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
