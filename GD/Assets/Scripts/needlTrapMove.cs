using System.Collections;
using UnityEngine;
public class needlTrapMove : MonoBehaviour
{
    public bool up = true;
    private void Start()
    {
        StartCoroutine(delayMoveUp());
    }

    private void FixedUpdate()
    {
        if (up)
            transform.position += new Vector3(0f, 0.7f, 0f) * Time.fixedDeltaTime;
        else if (!up)
            transform.position -= new Vector3(0f, 0.7f, 0f) * Time.fixedDeltaTime;
    }

    private IEnumerator delayMoveUp()
    {
        yield return new WaitForSeconds(1.8f);
        up = false;
        StartCoroutine(delayMoveDown());
    }

    private IEnumerator delayMoveDown()
    {
        yield return new WaitForSeconds(1.8f);
        up = true;
        StartCoroutine(delayMoveUp());
    }

}