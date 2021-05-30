using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{

    [SerializeField]
    private GameObject name;
    [SerializeField]
    private GameObject presents;

    int positionY;
    int alpha;
    int step = 20;
    TextMeshPro invitation;
    float howlong;
    float start;

    // Start is called before the first frame update
    void Start()
    {
        alpha = 0;
        positionY = 1600;
        name.transform.position = new Vector3(0,10,positionY);
        invitation= presents.GetComponent<TextMeshPro>();
        howlong = 3f;

    }

    // Update is called once per frame
    void Update()
    {
        if(positionY>100)
        {
            positionY -= step;
            name.transform.position = new Vector3(0, 10, positionY);
            invitation.faceColor = new Color32(255, 255, 255, 0);

        }

        if (positionY < 110 && alpha<250)
        {
            TextMeshPro textmeshPro = GetComponent<TextMeshPro>();
            invitation.faceColor = new Color32(255, 255, 255, (byte)alpha);
            alpha += 2;
        }

        if(Time.time-start>howlong)
        {
            SceneManager.LoadScene("Start");
        }
        
    }
}
