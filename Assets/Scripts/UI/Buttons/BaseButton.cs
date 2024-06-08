using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    [Header("Base Button")]
    [SerializeField] protected Button button;

    protected virtual void Start()
    {
        this.AddOnClickEvent();
        this.LoadComponents();
    }

    protected virtual void LoadComponents()
    {
        this.LoadButton();
    }

    private void LoadButton()
    {
        if (this.button != null)
        {
            return;//already loaded
        }

        this.button = this.GetComponent<Button>();
    }

    protected virtual void AddOnClickEvent()
    {
        this.button.onClick.AddListener(this.OnClick);
    }

    protected abstract void OnClick();
}
