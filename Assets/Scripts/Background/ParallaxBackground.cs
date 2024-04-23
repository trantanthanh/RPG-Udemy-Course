using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] float parallaxEffect;
    private GameObject cam;
    private float xPosition;
    private float length;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > xPosition + length)//check moved right
        {
            xPosition = xPosition + length;
        }
        else if (distanceMoved < xPosition - length)//check moved left
        {
            xPosition -= length;
        }
    }
}
