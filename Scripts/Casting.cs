using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Casting : MonoBehaviour
{
    Camera camera;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit;
            //if (Physics2D.Raycast(ray, out hit))
            hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            {
                Debug.Log("hit!!!");
                switch (hit.transform.name)
                {
                    case "Save":
                        //Generals.canClick = false;
                        Casting.GoToMenu();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public static void GoToMenu()
    {
        SoundManager.PlayClick();

        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);

        foreach (var item in rootObjects)
        {
            foreach (Transform eachChild in item.transform)
            {

                
                    if (eachChild.name == "MenuTable")
                    {
                        //var table = GameObject.Find("MenuTable");
                        item.SetActive(true);
                    }
            }
            
        }

        //SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
