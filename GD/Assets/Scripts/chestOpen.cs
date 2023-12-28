using UnityEngine;
using UnityEngine.InputSystem;

public class chestOpen : MonoBehaviour
{
    public bool cunOpenChest = false;
    public GameObject item;
    public Transform posAnim;
    public Transform pointSpawn;
    public GameObject[] CheckPlayer;
    public PlayerController scriptToCheckPlayer;
    public int moneyInChest;

    private bool flag = false;
    private Animator animPl;
    private Transform playerPos;
    private float rotatY;
    private int clickOnChest = 0;
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        CheckPlayer = GameObject.FindGameObjectsWithTag("Player");
        playerPos = CheckPlayer[0].GetComponent<Transform>();
        animPl = CheckPlayer[0].GetComponent<Animator>();
        rotatY = transform.rotation.eulerAngles.y / 2;
        _playerInput = CheckPlayer[0].GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.PlayerAction.InteractionButton.performed += InteractionButton;
    }

    private void FixedUpdate()
    {
        if (flag)
        {
            playerPos.transform.LookAt(gameObject.transform.position);
            playerPos.transform.position = new Vector3(posAnim.position.x, playerPos.position.y, posAnim.position.z);
        }
    }

    public void InteractionButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (cunOpenChest && scriptToCheckPlayer._hasKey)
            {
                animPl.Play("Opening A Lid");
                Animator chestAnim = gameObject.GetComponent<Animator>();
                chestAnim.Play("Opened");
                scriptToCheckPlayer.stopAnim = true;
                clickOnChest += 1;
                flag = true;
                scriptToCheckPlayer.tipPressE.SetActive(false);
                //Instantiate(item, pointSpawn.transform.position, Quaternion.identity);
            }
        }
    }

    public void ChestOpened()
    {
        flag = false;
        scriptToCheckPlayer.stopAnim = false;
        Debug.Log("Chest Open");
        scriptToCheckPlayer.hasMap = true;
        clickOnChest = 0;
        Debug.Log(clickOnChest);
        cunOpenChest = false;
        scriptToCheckPlayer._hasKey = false;
        scriptToCheckPlayer.mapLvl.SetActive(true);
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            scriptToCheckPlayer = other.GetComponent<PlayerController>();
            if (scriptToCheckPlayer._hasKey)
            {
                scriptToCheckPlayer.scrptChestOpen = gameObject.GetComponent<chestOpen>();
                cunOpenChest = true;
                scriptToCheckPlayer.tipPressE.SetActive(true);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cunOpenChest = false;
            scriptToCheckPlayer.tipPressE.SetActive(false);
        }
    }

}