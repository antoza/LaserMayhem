using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField]
    private AudioSource[] placePieceSounds;

    private void Start()
    {
        Instance = this;
    }

    public void playRandomPlacePieceSound()
    {
        int rd = Random.Range(0, placePieceSounds.Length);
        placePieceSounds[rd].Play();
    }
}
