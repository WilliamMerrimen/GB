using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class chestOpen : MonoBehaviour
{
    public GameObject player;
    public float distanceToOpen;
    public bool cunOpenChest = false;
    public GameObject item;
    public Transform pointSpawn;
    public GameObject[] CheckPlayer;
    public PlayerController scriptToCheckPlayer;

    private float _distance;
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;
    private void Awake()
    {
        CheckPlayer = GameObject.FindGameObjectsWithTag("Player");
        scriptToCheckPlayer = CheckPlayer[0].GetComponent<PlayerController>();
        _playerInput = player.GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.PlayerAction.InteractionButton.performed += InteractionButton;
    }
    private void FixedUpdate()
    {
        _distance = Mathf.Round(Vector3.Distance(player.transform.position, gameObject.transform.position));
        if(_distance <= distanceToOpen)
        {
            cunOpenChest = true;
            scriptToCheckPlayer.tipPressE.SetActive(true);
        }
        if (_distance >= distanceToOpen)
        {
            cunOpenChest = false;
            scriptToCheckPlayer.tipPressE.SetActive(false);
        } 
    }
    public void InteractionButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (cunOpenChest)
            {
                Debug.Log("Chest Open");
                Instantiate(item, pointSpawn.transform.position, Quaternion.identity);
                //Destroy(gameObject);
            }
        }
    }
}