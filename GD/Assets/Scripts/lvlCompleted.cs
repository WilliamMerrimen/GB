using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class lvlCompleted : MonoBehaviour
{
    private PlayerController _playerController;
    public GameObject menuCanvas;
    public GameObject nextLvlButton;
    public GameObject over;
    public GameObject win;

    private void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        nextLvlButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _playerController.hasMap)
        {
            _playerController.lvlCompleted = true;
            win.SetActive(true);            
            _playerController.GetComponent<Animator>().Play("Idle");
            _playerController.GetComponent<Animator>().SetBool("isRunning", false);
            nextLvlButton.SetActive(true);
            menuCanvas.SetActive(true);
        }
    }

}