using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Start")
        {
            InitialSetupOfGenerals();
            Canvas.ForceUpdateCanvases();
        }

        if (SceneManager.GetActiveScene().name == "NewGameMenu")
        {
            Generals.isYourBlack = true;
            Generals.isHuman = false;
            Generals.difficulty = 5;
            Generals.handicap = 0;

            //new
            Generals.nameOfLoadedFile=null;
            Generals.currentRecord=null;
            Generals.aiAllowed = true;

            //Debug.Log("things done   "+Generals.isYourBlack);

    Canvas.ForceUpdateCanvases();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void InitialSetupOfGenerals()
    {
        if (Generals.isClear == true)
        {
            Generals.onPossibilities = true;
            Generals.onLastMove = true;
            Generals.onScore = true;
            Generals.onAnimation = true;
            Generals.onSound = true;
            Generals.onMusic = true;
            Generals.isYourBlack = true;
            Generals.isHuman = false;
            Generals.difficulty = 5;
            Generals.handicap = 0;

        Generals.currentLoad = null;

            Generals.nameOfLoadedFile = null;

            Generals.currentRecord = new Record();
        }

        Generals.isClear = false;
    }

    public void StartNewGame()
    {
        SoundManager.PlayClick();
        SceneManager.LoadScene("HH_load");
    }

    public void GoToNewGame()
    {
        SoundManager.PlayClick();
        SceneManager.LoadScene("NewGameMenu");
    }

    public void GoToMenu()
    {
        SoundManager.PlayClick();
        SceneManager.LoadScene("Menu");
    }

    public void GoToLoad()
    {
        SoundManager.PlayClick();
        SceneManager.LoadScene("newestlist");
    }

    public void GoToSettings()
    {
        SoundManager.PlayClick();
        SceneManager.LoadScene("Settings");
    }

    public void Quit()
    {
        SoundManager.PlayClick();
        Application.Quit();
    }

    public void OpponentSliderReaction()
    {
        SoundManager.PlayClick();
        var value = GameObject.Find("OpponentSlider").GetComponent<Slider>().value;
        var difficulty = GameObject.Find("DifficultySlider").GetComponent<Slider>();

        if (value == 0)
        {
            Generals.isHuman = true;
            difficulty.interactable=false;
            
        }
        else
        {
            Generals.isHuman = false;
            difficulty.interactable=true;
        }
        Debug.Log("isHuman is now: " + Generals.isHuman);
        
    }

    public void StoneSliderReaction()
    {
        SoundManager.PlayClick();
        var value = GameObject.Find("StoneSlider").GetComponent<Slider>().value;

        if (value == 0)
        {
            Generals.isYourBlack = true;
        }
        else
        {
            Generals.isYourBlack = false;
        }
        Debug.Log("isYourBlack is now: " + Generals.isYourBlack);

    }

    public void DifficultySliderReaction()
    {
        SoundManager.PlayClick();
        var value = GameObject.Find("DifficultySlider").GetComponent<Slider>().value;
        var mesh = GameObject.Find("DifficultyText").GetComponent<TextMeshProUGUI>();//.SetText;
        Generals.difficulty = (int)value;
        string text = "DIFFICULTY LEVEL " + Generals.difficulty;
        mesh.SetText(text);
        Debug.Log("difficulty is now: " + Generals.difficulty);

    }

    public void HandicapSliderReaction()
    {
        SoundManager.PlayClick();
        var value = GameObject.Find("HandicapSlider").GetComponent<Slider>().value;
        var mesh = GameObject.Find("HandicapText").GetComponent<TextMeshProUGUI>();//.SetText;
        Generals.handicap = (int)value;
        string text = "HANDICAP STONES " + Generals.handicap;
        mesh.SetText(text);

        Debug.Log("handicap is now: " + Generals.handicap);

    }

    public void AnimationToggleReaction()
    {
        SoundManager.PlayClick();
        var value = GameObject.Find("AnimationToggle").GetComponent<Toggle>().isOn;
        Generals.onAnimation = value;
        

        Debug.Log("onAnimation is now: " + Generals.onAnimation);

    }

    

    public void LastToggleReaction()
    {
        SoundManager.PlayClick();
        var value = GameObject.Find("LastToggle").GetComponent<Toggle>().isOn;
        Generals.onLastMove = value;


        Debug.Log("onLastMove is now: " + Generals.onLastMove);

    }

    public void CoordinateToggleReaction()
    {
        SoundManager.PlayClick();
        var value = GameObject.Find("CoordinateToggle").GetComponent<Toggle>().isOn;
        Generals.onCoordinates = value;


        Debug.Log("onLastMove is now: " + Generals.onLastMove);

    }

    public void MusicToggleReaction()
    {
        SoundManager.PlayClick();
        var value = GameObject.Find("MusicToggle").GetComponent<Toggle>().isOn;
        Generals.onMusic = value;

        GameObject sound;

        if(value)
        {
            SoundManager.PlayMusic();
        }
        else
        {
            sound = GameObject.Find("sound");
            if(sound!=null)
            {
                Debug.Log("sound found and destroyed");
                Destroy(sound);
            }
        }


        Debug.Log("onMusic is now: " + Generals.onMusic);

    }

    public void PossibilietesToggleReaction()
    {
        SoundManager.PlayClick();
        var value = GameObject.Find("PossibilitiesToggle").GetComponent<Toggle>().isOn;
        Generals.onPossibilities = value;


        Debug.Log("onPossibilities is now: " + Generals.onPossibilities);

    }

    public void ScoreToggleReaction()
    {
        SoundManager.PlayClick();
        var value = GameObject.Find("ScoreToggle").GetComponent<Toggle>().isOn;
        Generals.onScore = value;


        Debug.Log("onScore is now: " + Generals.onScore);

    }

    public void SoundToggleReaction()
    {
        SoundManager.PlayClick();
        var value = GameObject.Find("SoundToggle").GetComponent<Toggle>().isOn;
        Generals.onSound = value;


        Debug.Log("onSound is now: " + Generals.onSound);

    }
}
