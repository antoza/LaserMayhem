using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // TODO : SoundManager abstrait, créer des sous-classes MenuSoundManager et GameSoundManager

    public static SoundManager Instance;
    [SerializeField] private AudioSource[] placePieceSounds;
    [SerializeField] private AudioSource laserSound;
    [SerializeField] private AudioSource music;

    private void Awake()
    {
        Instance = this;
        InitializeMasterVolume();
        //music.Play();
    }

    // Master volume functions
    private void InitializeMasterVolume()
    {
        if (!PlayerPrefs.HasKey("masterVolume"))
        {
            PlayerPrefs.SetFloat("masterVolume", 1);
        }
        AudioListener.volume = PlayerPrefs.GetFloat("masterVolume");
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("masterVolume", value);
    }

    // Sound effects functions
    public void PlayPlacePieceSound()
    {
        int rd = Random.Range(0, placePieceSounds.Length);
        placePieceSounds[rd].Play();
    }

    public void PlayLaserSound()
    {
        laserSound.Play();
    }
}
