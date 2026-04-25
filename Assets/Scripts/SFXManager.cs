using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    // 0 = Walk, 1 = Jump, 2 = Attack, 3 = Pull, 4 = Death
    [SerializeField] private List<AudioSource> sfxList;
    public static SFXManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        foreach (AudioSource sfx in sfxList)
        {
            sfx.volume = PlayerPrefs.GetFloat("sfxVolume");
        }
    }

    public void PlaySFX(int index)
    {
        sfxList[index].Play();
    }
}
