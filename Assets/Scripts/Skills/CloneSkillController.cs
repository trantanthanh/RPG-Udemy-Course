using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] Vector3 offsetPos;
    [SerializeField] float cloneColorLosingSpeed;
    private float cloneTimer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void SetupClone(Transform _newTransform, float _cloneDuration)
    {
        cloneTimer = _cloneDuration;
        transform.position = _newTransform.position + offsetPos;
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * cloneColorLosingSpeed);

            if (sr.color.a < 0)
            {
                Destroy(gameObject);
            }

        }
    }
}
