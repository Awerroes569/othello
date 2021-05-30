using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public static class Generals 
{
    public static bool isClear = true;
    public static bool canClick = true;

    public static bool onCoordinates;
    public static bool onPossibilities;
    public static bool onLastMove;
    public static bool onScore;
    public static bool onAnimation;
    public static bool onSound;
    public static bool onMusic;
    public static bool isYourBlack;
    public static bool isHuman;
    public static int difficulty;
    public static int handicap;

    public static SaveData currentLoad;

    public static string nameOfLoadedFile;

    public static Record currentRecord;

    //algo coefficients

    public static int safeCoeff = 30;
    public static int mobilityCoeff = 7;
    public static int stabilityCoeff = 1;
    public static int cornerCoeff = 1;
    public static int xCoeff = 150;

    public static int removeFront = 45;
    public static int removeEnd=5;

    
    public static AudioClip click;
    public static AudioClip reverse;
    public static AudioClip success;
    public static AudioClip defeat;

    public static AudioClip music1;
    public static AudioClip music2;
    public static AudioClip music3;

    public static RuntimeAnimatorController anime;

    public static int delay;

    public static GameObject tableMenu;

    public static bool aiAllowed = true;


    

    //not used onClick.AdListener used
    public static void SetSavedGameName()
    {
        var thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        var thisButName = thisButton.GetComponentInChildren<Text>().text;

        Generals.nameOfLoadedFile = thisButName;

        SceneManager.LoadScene("Menu");
    }

    

}
