using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public int IDTeleport;
    public int TeleportEnter;
    public Transform playerTrans;
    public Transform OneTpTrans;
    public Transform TwoTpTrans;

    public void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Debug.Log(IDTeleport);
        }
    }
    public void IdOne()
    {
        playerTrans.transform.position = OneTpTrans.transform.position;
    }
    public void IdTwo()
    {
        playerTrans.transform.position = TwoTpTrans.transform.position;
    }
}
