using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIn(bool playFromStart = true)
    {
        if (playFromStart)
        {
            animator.Play("FadeIn", -1, 0f);
        }
        else
        {
            animator.Play("FadeIn");
        }
    }

    public void FadeOut(bool playFromStart = true)
    {
        if (playFromStart)
        {
            animator.Play("FadeOut", -1, 0f);
        }
        else
        {
            animator.Play("FadeOut");
        }
    }
}
