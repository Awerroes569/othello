using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject poss = GameObject.Find("PossibilitiesToggle");
        if (poss != null)
        {
            poss.GetComponent<Toggle>().isOn = Generals.onPossibilities;
        }

        GameObject last = GameObject.Find("LastToggle");
        if (last != null)
        {
            last.GetComponent<Toggle>().isOn = Generals.onLastMove;
        }

        GameObject music = GameObject.Find("MusicToggle");
        if (music != null)
        {
            music.GetComponent<Toggle>().isOn = Generals.onMusic;
        }

        GameObject score = GameObject.Find("ScoreToggle");
            if (score != null)
            {
                score.GetComponent<Toggle>().isOn=Generals.onScore;
            }

        GameObject animation = GameObject.Find("AnimationToggle");
        if (animation != null)
        {
            animation.GetComponent<Toggle>().isOn = Generals.onAnimation;
        }

        GameObject sound = GameObject.Find("SoundToggle");
        if (sound != null)
        {
            sound.GetComponent<Toggle>().isOn = Generals.onSound;
        }

        GameObject coord = GameObject.Find("CoordinateToggle");
        if (coord != null)
        {
            coord.GetComponent<Toggle>().isOn = Generals.onCoordinates;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
