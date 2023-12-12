using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class lvlCompleted : MonoBehaviour
{
    private PlayerController _playerController;
    public GameObject menuCanvas;
    public TMP_Text lvlText;
    public GameObject nextLvlButton;

    private void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        nextLvlButton.SetActive(false);
        lvlText.text = "Game Over";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _playerController.hasMap)
        {
            _playerController.lvlCompleted = true;
            lvlText.text = "Level Completed!";
            nextLvlButton.SetActive(true);
            menuCanvas.SetActive(true);
        }
    }
}
