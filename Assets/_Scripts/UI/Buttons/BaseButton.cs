using Menu;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    [Header("Base Button")]
    [SerializeField] protected Button button;//assign button in inspector: make sure buttons which is disabled can be work
    protected UI ui;

    protected virtual void Start()
    {
        this.AddOnClickEvent();
        this.LoadComponents();
        ui = GetComponentInParent<UI>();
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
        this.button.onClick.AddListener(this.PlaySoundFX);
    }

    protected virtual void PlaySoundFX()
    {
        SoundManager.Instance.PlaySFX(SFXDefine.sfx_click);
    }

    protected abstract void OnClick();
}
