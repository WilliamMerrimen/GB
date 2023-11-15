using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRigidbody;
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;
    private Transform _playerPosition;
    private Vector3 _inputVector;
    private Vector3 _rayStart;
    private Vector3 _rayDirection;

    public Transform playerPos;
    public float speed = 5f;
    public float jumpForse = 6f;
    public bool onGraund = true;
    public string vector;
    public Material materialVisible;
    public Material materialInvisible;
    public float delayToInvis;
    public float kdInvisible;
    public bool invisible = false;
    public bool canToInvis = true;
    public GameObject teleportMenu;
    public GameObject tipPressE;

    private bool _teleportMenuCunOpen = false;
    private MeshRenderer meshRenderer;
    private short _jumpCount = 0;
    private short _maxCountJump = 1;
    public bool isEnableDublJump = false;
    public bool isHit = false;

    public int Money = 0;
    
    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _playerPosition = GetComponent<Transform>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.PlayerAction.Jump.performed += Jump;
        _playerInputActions.PlayerAction.Skill.performed += Skill;
        _playerInputActions.PlayerAction.InteractionButton.performed += InteractionButton;
        _rayDirection = transform.right;
    }

    private void FixedUpdate()
    {
        if(_teleportMenuCunOpen == false)
            teleportMenu.SetActive(false);
        
        if (invisible & canToInvis)
        {
            meshRenderer.material = materialInvisible;
            StartCoroutine(WaitForInvOne());
        }
        
        else if (invisible == false)
            meshRenderer.material = materialVisible;
        
        Vector2 inputVector = _playerInputActions.PlayerAction.Movement.ReadValue<Vector2>();
        
        if (!isHit)
            _playerPosition.position += new Vector3(inputVector.x, 0, inputVector.y) * speed * Time.fixedDeltaTime;
        
        else
        {
            if (!Input.GetKey(KeyCode.LeftShift))
                _playerPosition.position += new Vector3(inputVector.x * (speed-2), Math.Abs(inputVector.x != 0 ? inputVector.x  : inputVector.y) * speed, inputVector.y * (speed-2)) * Time.fixedDeltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftShift) && !onGraund)
            {
                Debug.Log("Shift!!!");
                _playerPosition.position -= new Vector3(0, 0.01f, 0) * Time.fixedDeltaTime;
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
            if (_teleportMenuCunOpen)
                teleportMenu.SetActive(true);
    }
    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Graund"))
            onGraund = true;
        
        if (other.collider.CompareTag("Teleport"))
        {
            _teleportMenuCunOpen = true;
            tipPressE.SetActive(true);
        }
    }
    public void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Teleport"))
        {
            _teleportMenuCunOpen = false;
            tipPressE.SetActive(false);
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
