using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField]
    private AudioSource[] placePieceSounds;
    [SerializeField]
    private AudioSource laserSound;

    private void Start()
    {
        Instance = this;
    }

    public void playPlacePieceSound()
    {
        int rd = Random.Range(0, placePieceSounds.Length);
        placePieceSounds[rd].Play();
    }

    public void playLaserSound()
    {
        laserSound.Play();
    }
}
