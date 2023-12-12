using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class lvlCompleted : MonoBehaviour
{
    private PlayerController _playerController;
    public GameObject menuCanvas;
    public GameObject gameOverGb;
    public GameObject lvlCopleteGb;
    public GameObject nextLvlButton;

    private void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        nextLvlButton.SetActive(false);
        gameOverGb.SetActive(true);
        lvlCopleteGb.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _playerController.hasMap)
        {
            _playerController.lvlConpleted = true;
            gameOverGb.SetActive(false);
            lvlCopleteGb.SetActive(true);
            nextLvlButton.SetActive(true);
            menuCanvas.SetActive(true);
        }
    }
}
