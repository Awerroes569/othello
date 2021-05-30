using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundAsset: MonoBehaviour
{
    [SerializeField]
    AudioClip [] sounds;

    [SerializeField]
    RuntimeAnimatorController anime;


    void Start()
    {

        //sounds = new AudioClip[10];
        Debug.Log("lenght of sounds array: " + sounds.GetLength(0));
        Generals.click = sounds[0];
            Generals.reverse = sounds[1];
            Generals.success = sounds[2];
            Generals.defeat = sounds[3];
            Generals.music1 = sounds[4];
        Debug.Log("START name of clip: " + Generals.music1.name);
            Generals.music2 = sounds[5];
            Generals.music3 = sounds[6];

        Generals.onCoordinates = true;
        Generals.onAnimation = true;
        Generals.onLastMove = true;
        Generals.onMusic = true;
        Generals.onPossibilities = true;
        Generals.onScore = true;
        Generals.onSound = true;
        Generals.isYourBlack = true;
        Generals.isHuman = false;
        Generals.difficulty = 10;
        Generals.handicap = 0;

        Generals.anime = anime;

        //Debug.LogError("No all sound files found");

        SceneManager.LoadScene("First");
    }
}
