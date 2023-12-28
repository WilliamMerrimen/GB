using System;
using UnityEngine;

public class TakeKey : MonoBehaviour
{
    PlayerController scrptPlayer;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            scrptPlayer = other.GetComponent<PlayerController>();
            scrptPlayer.keyLocateDel = gameObject;
            scrptPlayer.keyLocate = true;
            scrptPlayer.tipPressE.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            scrptPlayer = other.GetComponent<PlayerController>();
            scrptPlayer.keyLocate = false;
            scrptPlayer.tipPressE.SetActive(false);
        }
    }

}