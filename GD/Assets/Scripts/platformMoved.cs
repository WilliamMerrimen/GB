using UnityEngine;

public class platformMoved : MonoBehaviour
{
    public bool forward = false;
    public float vector = 4f;

    private void FixedUpdate()
    {
        if (forward)
        {
            transform.position += new Vector3(vector, 0f, 0f) * Time.fixedDeltaTime;
        }
        if (!forward)
        {
            transform.position += new Vector3(0f, 0f, vector) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Graund"))
        {
            vector *= -1;
            Debug.Log(vector);
        }

        if (other.collider.CompareTag("Player"))
        {
            if (forward)
                other.gameObject.transform.position = new Vector3(transform.position.x, other.gameObject.transform.position.y, other.gameObject.transform.position.z);
        }
    }

}