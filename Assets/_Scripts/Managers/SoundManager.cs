using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXDefine
{
    sfx_attack1 = 0,
    sfx_attack2,
    sfx_attack3,
    sfx_bankai,
    sfx_burning,
    sfx_checkpoint,
    sfx_chronosphere,
    sfx_click,
    sfx_clock_tick_,
    sfx_clock_tick_2,
    sfx_clock,
    sfx_death_screen,
    sfx_evil_voice,
    sfx_fire_magic,
    sfx_footsteps,
    sfx_girl_sigh_2,
    sfx_granfather_clock,
    sfx_gril_sigh,
    sfx_item_pickup,
    sfx_monster_breathing,
    sfx_monster_growl,
    sfx_mosnter_growl_1,
    sfx_open_chest,
    sfx_quick_time_event_key,
    sfx_skeleton_bones,
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

public enum BGMDefine
{
    BGM_a_fateful_encounter = 0,
    BGM_crawl_in_the_dark,
    BGM_man_of_the_hour,
    BGM_nemesis,
    BGM_the_fallen,
    BGM_the_village
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    [SerializeField] float sfxMininumDistance;

    public bool isPlayingBGM;
    [SerializeField] BGMDefine bgmIndex;

    private void Start()
    {
        //CopyListNames();//only for generate list
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
            if (!bgm[(int)bgmIndex].isPlaying)
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

    public void PlaySFX(SFXDefine sfxIndex, Transform _source = null, bool _isLoop = false)
    {
        int _sfxIndex = (int)sfxIndex;
        //if (sfx[_sfxIndex].isPlaying) return;

        if (_source != null && Vector2.Distance(PlayerManager.Instance.player.transform.position, _source.position) > sfxMininumDistance)
            return;
        sfx[_sfxIndex].loop = _isLoop;
        sfx[_sfxIndex].pitch = Random.Range(0.8f, 1.2f);
        sfx[_sfxIndex].Play();
    }

    public void StopSFX(SFXDefine _sfxIndex) => sfx[(int)_sfxIndex].Stop();
    public void StopSFXWithTime(SFXDefine _sfxIndex)
    {
        StartCoroutine(DecreaseVolume(sfx[(int)_sfxIndex]));
    }

    IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;
        while (_audio.volume > 0.1f)
        {
            _audio.volume -= Time.deltaTime;//Decrease in 1s
            yield return new WaitForEndOfFrame();

            if (_audio.volume <= 0.1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayBGM(BGMDefine bgmIndex, bool _isLoop = false)
    {
        int _bgmIndex = (int)bgmIndex;
        StopAllBGM();

        bgm[_bgmIndex].loop = _isLoop;
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
