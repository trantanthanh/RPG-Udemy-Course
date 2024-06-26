using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class UI : MonoBehaviour, ISaveManager
    {
        [SerializeField] UI_FadeScreen fakeScreen;
        [SerializeField] GameObject dieScreen;

        [SerializeField] private GameObject characterUI;
        [SerializeField] private GameObject skillTreeUI;
        [SerializeField] private GameObject craftUI;
        [SerializeField] private GameObject optionsUI;

        [SerializeField] private GameObject closeBTN;
        [SerializeField] private GameObject ingameMenu;

        [SerializeField] private UI_VolumeSlider[] volumeSettings;

        public GameObject CharacterUI { get { return characterUI; } }
        public GameObject SkillTreeUI { get { return skillTreeUI; } }
        public GameObject CraftUI { get { return craftUI; } }
        public GameObject OptionsUI { get { return optionsUI; } }
        public GameObject CloseBTN { get { return closeBTN; } }
        public GameObject IngameMenu { get { return ingameMenu; } }

        private bool isMenuShow = false;
        private int currentMenuIndex = 0;
        private Dictionary<int, GameObject> menuDictionary = new Dictionary<int, GameObject>();

        public UI_ItemTooltip itemTooltip;
        public UI_StatTooltip statTooltip;
        public UI_CraftWindow craftWindow;
        public UI_SkillTooltip skillToolTip;

        private void Start()
        {
            menuDictionary.Add(0, characterUI);
            menuDictionary.Add(1, skillTreeUI);
            menuDictionary.Add(2, craftUI);
            menuDictionary.Add(3, optionsUI);

            SwitchMenuTo(ingameMenu);
            itemTooltip.Hide();
            statTooltip.Hide();
            skillToolTip.Hide();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && PlayerManager.Instance.player.stats.IsAlive())
            {
                if (!isMenuShow)
                {
                    SwitchMenuTo(menuDictionary[currentMenuIndex]);
                }
                else
                {
                    SwitchMenuTo(ingameMenu);
                }
            }

            if (isMenuShow && Input.GetKeyDown(KeyCode.Tab))
            {
                currentMenuIndex = ++currentMenuIndex % 4;
                SwitchMenuTo(menuDictionary[currentMenuIndex]);
            }
        }

        public void Die()
        {
            GameManager.Instance.lostCurrencyAmount = PlayerManager.Instance.GetCurrency();
            PlayerManager.Instance.UpdateCurrency(-PlayerManager.Instance.GetCurrency());

            SaveManager.Instance.SaveGame();//save when die
            SwitchMenuTo(null);
            fakeScreen.FadeOut(true);
            dieScreen.SetActive(true);
        }

        public void RestartGameButton()
        {
            GameManager.Instance.RestartScene();
        }

        public void SwitchMenuTo(GameObject _menu)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                UI_FadeScreen fakeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>();
                bool isFakeScreen = fakeScreen != null;//Keep fakeScreen gameObject active
                if (!isFakeScreen)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                else
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                    fakeScreen.FadeIn(false);//play once
                }
            }
            isMenuShow = false;

            if (_menu != null)
            {
                isMenuShow = true;
                foreach (KeyValuePair<int, GameObject> menu in menuDictionary)
                {
                    if (menu.Value == _menu)
                    {
                        currentMenuIndex = menu.Key;
                        break;
                    }
                }
                if (_menu != ingameMenu)
                {
                    closeBTN.SetActive(true);
                }
                else
                {
                    isMenuShow = false;
                }

                GameManager.Instance.PauseGame(isMenuShow);
                _menu.SetActive(true);
            }
        }

        public void LoadData(GameData _data)
        {
            foreach (KeyValuePair<string, float> pair in _data.volumeSettings)
            {
                foreach (UI_VolumeSlider item in volumeSettings)
                {
                    if (pair.Key == item.parameter)
                    {
                        item.LoadSlider(pair.Value);
                    }
                }
            }
        }

        public void SaveData(ref GameData _data)
        {
            _data.volumeSettings.Clear();
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                _data.volumeSettings.Add(item.parameter, item.slider.value);
            }
        }
    }
}