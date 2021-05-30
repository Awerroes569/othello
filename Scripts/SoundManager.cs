using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class SoundManager: MonoBehaviour
{
    public static void PlayMusic()
    {
        if (GameObject.Find("sound") == null)
        {
            GameObject soundGameObject = new GameObject("sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(soundGameObject);
            audioSource.clip = ChooseMusic();
            //Debug.Log("MENU name of clip: " + Generals.music1.name);
            audioSource.loop = true;
            if (Generals.onMusic)
            {
                audioSource.Play();
            }
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    public static void PlayClick()
    {
        if (Generals.onSound)
        {
            GameObject soundGameObject = new GameObject("soundClick");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(soundGameObject);
            audioSource.clip = Generals.click;
            //Debug.Log("MENU name of clip: " + Generals.music1.name);
            //audioSource.loop = true;
            audioSource.Play();

            Destroy(soundGameObject, 1);
            
        }
    }

    public static void PlayReverse(int number)
    {
        if (Generals.onSound)
        {
            string name = "soundReverse" + 1;
            GameObject soundGameObject = new GameObject(name);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(soundGameObject);
            audioSource.clip = Generals.reverse;
            //Debug.Log("MENU name of clip: " + Generals.music1.name);
            //audioSource.loop = true;
            audioSource.Play();
            Destroy(soundGameObject, 1);
        }
    }

    private static AudioClip ChooseMusic()
    {
        AudioClip result;
        System.Random rnd = new System.Random();
        int choice = rnd.Next(1, 4);
        switch (choice)
        {
            case 1:
                result = Generals.music1;
                break;
            case 2:
                result = Generals.music2;
                break;
            case 3:
                result = Generals.music3;
                break;
            default:
                return null;
        }

        return result;
    }

    public static void PlayFanfare(bool blacks)
    {
        Pole.PoleStatus winner = blacks ? Pole.PoleStatus.Black : Pole.PoleStatus.White;
        Pole.PoleStatus human=Generals.isYourBlack? Pole.PoleStatus.Black : Pole.PoleStatus.White;

        if (Generals.onSound)
        {
            GameObject soundGameObject = new GameObject("soundFanfare");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(soundGameObject);
            if (!Generals.isHuman && winner != human)
            {
                audioSource.clip = Generals.defeat;
            }
            else
            {
                audioSource.clip = Generals.success;
            }
            //Debug.Log("MENU name of clip: " + Generals.music1.name);
            //audioSource.loop = true;
            audioSource.Play();
            Destroy(soundGameObject, 8);
        }
    }
}
