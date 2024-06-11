using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private GameObject characterUI;
        [SerializeField] private GameObject skillTreeUI;
        [SerializeField] private GameObject craftUI;
        [SerializeField] private GameObject optionsUI;
        public GameObject CharacterUI { get { return characterUI; } }
        public GameObject SkillTreeUI { get { return skillTreeUI; } }
        public GameObject CraftUI { get { return craftUI; } }
        public GameObject OptionsUI { get { return optionsUI; } }

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

            SwitchMenuTo(null);
            itemTooltip.Hide();
            statTooltip.Hide();
            skillToolTip.Hide();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (!isMenuShow)
                {
                    SwitchWithKeyTo(menuDictionary[currentMenuIndex]);
                }
                else
                {
                    SwitchMenuTo(null);
                }
            }

            if (isMenuShow && Input.GetKeyDown(KeyCode.Tab))
            {
                currentMenuIndex = ++currentMenuIndex % 4;
                SwitchWithKeyTo(menuDictionary[currentMenuIndex]);
            }
        }

        public void SwitchMenuTo(GameObject _menu)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
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
                _menu.SetActive(true);
            }
        }

        public void SwitchWithKeyTo(GameObject _menu)
        {
            if (_menu != null && _menu.activeSelf)
            {
                _menu.SetActive(false);
            }

            SwitchMenuTo(_menu);
        }
    }
}