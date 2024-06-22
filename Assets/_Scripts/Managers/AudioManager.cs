using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXDefine : int
{
    sfx_attack1 = 0,
    sfx_attack2,
    sfx_attack3,
    sfx_bankai,
    sfx_burning,
    sfx_checkpoint,
    sfx_chronosphere,
    sfx_click,
    sfx_clock_tick,
    sfx_clock_tick_2,
    sfx_clock,
    sfx_evil_voice,
    sfx_fire_magic,
    sfx_footsteps,
    sfx_girl_sigh_2,
    sfx_granfather_clock,
    sfx_gril_sigh,
    sfx_mosnter_growl_1,
    sfx_open_chest,
    sfx_quick_time_event_key,
    sfx_spell_1,
    sfx_spell,
    sfx_sword_throw_2,
    sfx_throw_sword,
    sfx_thunder_strike,
    sfx_wind_sounds,
    sfx_woman_sigh_2,
    sfx_woman_sigh_3,
    sfx_woman_sigh,
    sfx_woman_struggle_2,
    sfx_woman_struggle
}

public enum BGMDefine : int
{
    BGM_a_fateful_encounter = 0,
    BGM_crawl_in_the_dark,
    BGM_man_of_the_hour,
    BGM_nemesis,
    BGM_the_fallen,
    BGM_the_village
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool isPlayingBGM;
    private int bgmIndex;

    private void Start()
    {
        //CopyListNames();
    }

    private void CopyListNames()
    {
        List<string> gameObjectNames = new List<string>();
        foreach (AudioSource obj in sfx)
        {
            gameObjectNames.Add(obj.name);
        }

        foreach (string name in gameObjectNames)
        {
            Debug.Log(name);
        }

#if UNITY_EDITOR
        string names = string.Join(",\n", gameObjectNames);
        GUIUtility.systemCopyBuffer = names;
        Debug.Log("Copied to Clipboard: \n" + names);
#endif
    }

    private void Update()
    {
        if (!isPlayingBGM)
        {
            StopAllBGM();
        }
        else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayBGM(bgmIndex);
            }
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(int _sfxIndex)
    {
        if (_sfxIndex < sfx.Length)
        {
            if (!sfx[_sfxIndex].isPlaying)
            {
                sfx[_sfxIndex].pitch = Random.Range(0.8f, 1.2f);
                sfx[_sfxIndex].Play();
            }
        }
    }

    public void StopSFX(int _sfxIndex)
    {
        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].Stop();
        }
    }

    public void PlayBGM(int _bgmIndex)
    {
        StopAllBGM();

        bgm[_bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
