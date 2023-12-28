using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class drowning : sceneControl
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Animator anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
            anim.Play("drowning");
            GameOver.gameOverOn();
            Rigidbody rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePosition;
            Transform trans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            trans.position -= new Vector3(0, 1.24581271f, 0);
            GameObject map = other.GetComponent<PlayerController>().mapLvl;
            Destroy(map);
        }
    }

}