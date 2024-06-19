using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator animator;
    public bool activeStatus = false;
    public string id;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoint id")]
    private void GenerateID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activeStatus)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                ActivateCheckpoint();
                CheckDeactiveOthers();
            }
        }
    }

    private void CheckDeactiveOthers()
    {
        GameManager.Instance.CheckActiveCheckpoint(this.id);
    }

    public void ActivateCheckpoint()
    {
        activeStatus = true;
        animator.SetBool("active", true);
    }

    public void DeactiveCheckpoint()
    {
        activeStatus = false;
        animator.SetBool("active", false);
    }

}
