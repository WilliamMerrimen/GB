using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYPEPOLY_Dispenser : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float arrowSpeed;
    public float shootingDistance;
    public float shootingSpeed;
    Transform arrowsParent;
    GameObject currentArrow;

    //Arrow Creating Variables (Animation of scaling when arrow instantiated)
    bool arrowReady = true;
    float arrowSize = 0f;

    //Arrow ShootingVaeiables
    List<GameObject> arrows;
    Vector3 targetPosition;
    void Start()
    {
        arrows = new List<GameObject>();
        GetComponent<Animator>().speed = shootingSpeed;
        arrowsParent = transform.GetChild(0);
        targetPosition = transform.TransformPoint(new Vector3(0f, 0f, -shootingDistance));
        targetPosition.y = arrowsParent.position.y;
    }
    public void CreateArrow()
    {
        arrowReady = false;
        arrowSize = 0f;
        currentArrow = GameObject.Instantiate(arrowPrefab, arrowsParent);
        currentArrow.transform.localScale = Vector3.zero;
    }
    public void Shoot()
    {
        currentArrow.transform.parent = null;
        arrows.Add(currentArrow);
    }
    private void FixedUpdate()
    {
        if(!arrowReady)
        {
            arrowSize += (Time.fixedDeltaTime* shootingSpeed);
            currentArrow.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, arrowSize);
            if (arrowSize >= 1f) arrowReady = true;
        }
        if(arrows.Count > 0)
        {
            for (int i = arrows.Count - 1; i >= 0; i--)
            {
                if (Vector3.Distance(arrows[i].transform.position, targetPosition) < 0.1f)
                {
                    GameObject toDestroy = arrows[i];
                    arrows.Remove(toDestroy);
                    Destroy(toDestroy);
                }
            }

            for (int i = 0; i<arrows.Count; i++)
            {
                arrows[i].transform.position = Vector3.MoveTowards(arrows[i].transform.position, targetPosition, arrowSpeed);
            }
        }
    }
}
