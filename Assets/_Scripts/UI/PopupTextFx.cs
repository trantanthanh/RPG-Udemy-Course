using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupTextFx : MonoBehaviour
{
    private TextMeshPro myText;
    [SerializeField] float speed;
    [SerializeField] float disappearingSpeed;
    [SerializeField] float colorDisappearingSpeed;//alpha color decrease after lifeTime over
    [SerializeField] float lifeTime;//Time show on the screen

    private float textTimer;

    private void Start()
    {
        myText = GetComponent<TextMeshPro>();
        textTimer = lifeTime;
    }

    private void Update()
    {
        UpdateFlyText();
    }

    private void UpdateFlyText()
    {
        textTimer -= Time.deltaTime;
        if (textTimer <= 0)
        {
            float alpha = myText.color.a - colorDisappearingSpeed * Time.deltaTime;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

            if (alpha <= 50)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), disappearingSpeed * Time.deltaTime);
            }

            if (alpha <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);//fly up
        }
    }
}
