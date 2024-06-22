using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool isPlayingBGM;
    private int bgmIndex;

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
            sfx[_sfxIndex].pitch = Random.Range(0.8f, 1.2f);
            sfx[_sfxIndex].Play();
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
