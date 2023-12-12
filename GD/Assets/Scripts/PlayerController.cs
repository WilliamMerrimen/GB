using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Cinemachine;
public class PlayerController : MonoBehaviour
{
    private Animator _playerAnivator;
    private Rigidbody _playerRigidbody;
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;
    private Transform _playerPosition;
    private Vector3 _inputVector;

    public CinemachineVirtualCamera cine;
    public Transform playerPos;
    public float rotationSpeed;
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
    public GameObject keyLocateDel;
    public bool keyLocate = false;

    public GameObject mapLvl;
    public GameObject teleportMenu;
    public GameObject tipPressE;
    public GameObject nextLevel;
    public GameObject stepRayUpper;
    public GameObject stepRayLower;
    public GameObject gameOverGameObject;

    public chestOpen scrptChestOpen;
    public bool stopAnim = false;
    private Vector3 inputVector;
    private bool _teleportMenuCunOpen = false;
    private MeshRenderer _meshRenderer;
    public float stepHeight = 0.4f;
    public float stepSmooth;
    public bool isHit = false;
    public bool _hasKey = false;
    public int Money = 0;
    public bool step;

    public bool hasMap = false;
    public bool lvlCompleted = false;
    
    private void Awake()
    {
        Application.targetFrameRate = -1;
        _meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _playerPosition = GetComponent<Transform>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.PlayerAction.Skill.performed += Skill;
        _playerInputActions.PlayerAction.InteractionButton.performed += InteractionButton;
        _playerInputActions.PlayerAction.Resizeble.performed += Resizeble;
        _playerAnivator = GetComponent<Animator>();
        GameOver._gameOverScreen = gameOverGameObject;
        
        hasMap = false;
        lvlCompleted = false;
        GameOver.GameOverOff();
    }

    private void FixedUpdate()
    {
        if (!GameOver.GameOverBl && !lvlCompleted)
        {
            smallLogic();
            if (stopAnim == false)
            {
                moveAndAnimation();
                Jump();
            }
            stepClimb();
        }
    }

    public void moveAndAnimation()
    {
        if (!GameOver.GameOverBl)
        {
            inputVector = _playerInputActions.PlayerAction.Movement.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
            moveDirection.Normalize();
            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            }

            if (!isHit)
            {
                _playerAnivator.SetBool("isClimbing", false);
                _playerAnivator.speed = 1.0f;
                _playerPosition.position += new Vector3(inputVector.x, 0f, inputVector.y) * speed * Time.fixedDeltaTime;
                if (inputVector.x != 0 || inputVector.y != 0)
                {
                    _playerAnivator.SetBool("isRunning", true);

                }

                if (inputVector.x == 0 && inputVector.y == 0)
                {
                    _playerAnivator.SetBool("isRunning", false);
                }
            }
            else
            {
                _playerAnivator.SetBool("isClimbing", true);
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    Vector3 climbVec = new Vector3(inputVector.x * (speed - 2f),
                        Math.Abs(inputVector.x != 0f ? inputVector.x : inputVector.y) * (speed - 2f),
                        inputVector.y * (speed - 2f)) * Time.fixedDeltaTime;
                    _playerPosition.position += climbVec;
                    if (climbVec.y == 0)
                    {
                        _playerAnivator.speed = 0.0f;
                    }
                    else
                    {
                        _playerAnivator.speed = 1.0f;
                    }
                }
                else if (Input.GetKey(KeyCode.LeftShift) && !onGraund)
                {
                    Debug.Log("Shift!!!");
                    _playerPosition.position -= new Vector3(0, 1f, 0);
                    _playerAnivator.speed = 1.0f;
                    _playerAnivator.SetBool("isClimbing", false);
                    _playerAnivator.Play("Climbing Ladder Down");
                }
            }
        }
        //playerPos = _playerPosition;
    }

    public void smallLogic()
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
        {
            teleportMenu.SetActive(false);
        }
        if (invisible & canToInvis)
        {
            _meshRenderer.material = materialInvisible;
            StartCoroutine(WaitForInvOne());
        }
        else if (invisible == false)
        {
            _meshRenderer.material = materialVisible;
        }
    }

    public void stepClimb()
    {
        RaycastHit hitLower;
        if(Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.1f))
            {
                if(inputVector != Vector3.zero)
                {
                    
                    _playerPosition.position += new Vector3(0f, stepSmooth, 0f) * Time.fixedDeltaTime;
                    Debug.Log("123qwe123qwe");
                }
            }
            
        }
    }

    public void Jump()
    { 
        if (Input.GetKeyDown(KeyCode.Space) && onGraund) 
        {
            StartCoroutine(JumpTimeStabil());
            onGraund = false;
            _playerAnivator.Play("Jumping Up");
        }
    }

    private IEnumerator JumpKD()
    {
        float time = 0.3f;
        yield return new WaitForSeconds(time);
        onGraund = true;
    }

    private IEnumerator WaitForInvOne()
    {
        float time = delayToInvis;
        yield return new WaitForSeconds(time);
        invisible = false;
        canToInvis = false;
        time = delayToInvis;
        StartCoroutine(KDInvisible());
    }

    private IEnumerator KDInvisible()
    {
        float time = kdInvisible;
        yield return new WaitForSeconds(time);
        canToInvis = true;
        time = kdInvisible;
    }

    private IEnumerator JumpTimeStabil()
    {
        float time = 0.15f;
        int countJump = 1;
        if(countJump != 0)
        {
            yield return new WaitForSeconds(time);
            _playerRigidbody.AddForce(Vector3.up * jumpForse, ForceMode.Impulse);
            Debug.Log("Jump!");
            time = 0.15f;
            countJump = 0;
        }
    }

    public void ToChestAnim()
    {
        scrptChestOpen.ChestOpened();
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
    public void DigKey()
    {
        cine.m_Lens.FieldOfView = 65f;
        stopAnim = false;
        _hasKey = true;
        Destroy(keyLocateDel);
        keyLocate = false;
        if(tipPressE != null)
        {
            tipPressE.SetActive(false);
        }
        else
        {
            Debug.Log("bug is " + tipPressE);
        }
    }
    public void InteractionButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_teleportMenuCunOpen)
            {
                teleportMenu.SetActive(true);
            }
            
            if (keyLocate)
            {
                _playerAnivator.Play("digging");
                stopAnim = true;
            }
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Steps"))
            onGraund = false;
        if (other.gameObject.CompareTag("Graund"))
            StartCoroutine(JumpKD());
        if (other.collider.CompareTag("NextLevel"))
        {
            nextLevel.SetActive(true);
        }
        if (other.collider.CompareTag("Teleport"))
        {
            _teleportMenuCunOpen = true;
            tipPressE.SetActive(true);
        }
    }

    public void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("NextLevel"))
        {
            nextLevel.SetActive(false);
        }
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