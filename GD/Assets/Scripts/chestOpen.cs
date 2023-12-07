using UnityEngine;
using UnityEngine.InputSystem;
public class chestOpen : MonoBehaviour
{
    public bool cunOpenChest = false;
    public GameObject item;
    public Transform pointSpawn;
    public GameObject[] CheckPlayer;
    public PlayerController scriptToCheckPlayer;
    public int moneyInChest;

    private int clickOnChest = 0;
    private float _distance;
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;
    private void Awake()
    {
        CheckPlayer = GameObject.FindGameObjectsWithTag("Player");
        scriptToCheckPlayer = CheckPlayer[0].GetComponent<PlayerController>();
        _playerInput = CheckPlayer[0].GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.PlayerAction.InteractionButton.performed += InteractionButton;
    }
    public void InteractionButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (cunOpenChest)
            {
                clickOnChest += 1;
                Debug.Log("Chest Open");
                //Instantiate(item, pointSpawn.transform.position, Quaternion.identity);
                if (clickOnChest == moneyInChest)
                {
                    Destroy(gameObject);
                    clickOnChest = 0;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (scriptToCheckPlayer._hasKey)
            {
                cunOpenChest = true;
                scriptToCheckPlayer.tipPressE.SetActive(true);
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (scriptToCheckPlayer._hasKey)
            {

            }
            cunOpenChest = false;
            scriptToCheckPlayer.tipPressE.SetActive(false);
        }
    }

}