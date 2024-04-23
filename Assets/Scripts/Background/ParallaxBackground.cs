using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] float parallaxEffect;
    private GameObject cam;
    private float xPosition;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
    }
}
