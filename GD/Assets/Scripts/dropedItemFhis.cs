using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropedItemFhis : MonoBehaviour
{
    private Vector3 delta;
    private int flag = 1;
    private Vector3 chestPos;
    private Vector3 playerPos;
    private Collider colider;

    public bool cunToGo = false;
    public float timeToGo = 2.5f;
    public GameObject[] CheckPlayer;
    public PlayerController scriptToCheckPlayer;
    public float speed;
    public float dropForse;
    private void FixedUpdate()
    {
        CheckPlayer = GameObject.FindGameObjectsWithTag("Player");
        scriptToCheckPlayer = CheckPlayer[0].GetComponent<PlayerController>();
        chestPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        playerPos = scriptToCheckPlayer.playerPos.position;
        delta = gameObject.transform.position - playerPos;
        delta.Normalize();
        if(flag == 1)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(-delta * dropForse, ForceMode.Impulse);
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * (dropForse - 2), ForceMode.Impulse);
            flag += 1;
        }
        StartCoroutine(TimeToGo());
        if (cunToGo)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, playerPos, speed * Time.fixedDeltaTime);
            StartCoroutine(TimeToFastGo());
        }
        
    }
    private IEnumerator TimeToGo()
    {
        yield return new WaitForSeconds(timeToGo);
        cunToGo = true;
    }
    private IEnumerator TimeToFastGo()
    {
        yield return new WaitForSeconds(timeToGo+3);
        speed += 10;
        colider = gameObject.GetComponent<Collider>();
        colider.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            scriptToCheckPlayer.Money += 1;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            scriptToCheckPlayer.Money += 1;
            Destroy(gameObject);
        }
    }
}
