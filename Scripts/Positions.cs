using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Positions : MonoBehaviour
{
    
    public static void GoBlack(Transform square)
    {
        foreach (Transform eachChild in square)
        {
            if (eachChild.name == "Pawn")
            {
                //Debug.Log("Child found. ...");
                eachChild.transform.position = new Vector3(eachChild.transform.position.x, eachChild.position.y, -2);
            }
        }
    }

    

    public static void GoWhite(Transform square)
    {
        foreach (Transform eachChild in square)
        {
            if (eachChild.name == "Pawn")
            {
                //Debug.Log("Child found. ...");
                eachChild.transform.position = 
                    new Vector3(eachChild.transform.position.x, eachChild.position.y, -2);
                

                Quaternion newRotation= new Quaternion(0f,0f,0f,0f);
                eachChild.Rotate(new Vector3(180, 0, 0));//rotation=Quaternion.FromToRotation(Quaternion.identity, new Vector3(180,0,0));
            }
        }
    }

    public static void GoLastUp(Transform square)
    {
        foreach (Transform eachChild in square)
        {
            if (eachChild.name == "LastMove")
            {
                //Debug.Log("Child found. ...");
                eachChild.transform.position =
                    new Vector3(eachChild.transform.position.x, eachChild.position.y, -5);
                //Quaternion newRotation = new Quaternion(0f, 0f, 0f, 0f);
                //eachChild.Rotate(new Vector3(180, 0, 0));//rotation=Quaternion.FromToRotation(Quaternion.identity, new Vector3(180,0,0));
            }
        }
    }

    public static void GoLastDown(Transform square)
    {
        foreach (Transform eachChild in square)
        {
            if (eachChild.name == "LastMove")
            {
                //Debug.Log("Child found. ...");
                eachChild.transform.position =
                    new Vector3(eachChild.transform.position.x, eachChild.position.y, 10);
                //Quaternion newRotation = new Quaternion(0f, 0f, 0f, 0f);
                //eachChild.Rotate(new Vector3(180, 0, 0));//rotation=Quaternion.FromToRotation(Quaternion.identity, new Vector3(180,0,0));
            }
        }
    }

    public static void GoPossibleUp(Transform square)
    {
        foreach (Transform eachChild in square)
        {
            if (eachChild.name == "Possible")
            {
                //Debug.Log("Child found. ...");
                eachChild.transform.position =
                    new Vector3(eachChild.transform.position.x, eachChild.position.y, -5);
                //Quaternion newRotation = new Quaternion(0f, 0f, 0f, 0f);
                //eachChild.Rotate(new Vector3(180, 0, 0));//rotation=Quaternion.FromToRotation(Quaternion.identity, new Vector3(180,0,0));
                MeshRenderer m = eachChild.gameObject.GetComponent<MeshRenderer>();
                if (Generals.onPossibilities)
                {
                    m.enabled = true;
                }
                else
                {
                    m.enabled = false;
                }
            }
        }
    }

    public static void GoPossibleDown(Transform square)
    {
        foreach (Transform eachChild in square)
        {
            if (eachChild.name == "Possible")
            {
                //Debug.Log("Child found. ...");
                eachChild.transform.position =
                    new Vector3(eachChild.transform.position.x, eachChild.position.y, 10);
                //Quaternion newRotation = new Quaternion(0f, 0f, 0f, 0f);
                //eachChild.Rotate(new Vector3(180, 0, 0));//rotation=Quaternion.FromToRotation(Quaternion.identity, new Vector3(180,0,0));
            }
        }
    }

    public async static Task RotatePawns()
    {
        List<GameObject> pawns = new List<GameObject>();
        foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (gameObj.name == "Pawn")
            {
                pawns.Add(gameObj);
            }
        }
        Debug.Log("pawns found " + pawns.Count);
        foreach (var item in pawns)
        {
            //if (eachChild.name == "Pawn")
            //{
            //Debug.Log("Child found. ...");
            //eachChild.transform.position =
            //    new Vector3(eachChild.transform.position.x, eachChild.position.y, -5);
            //Quaternion newRotation = new Quaternion(0f, 0f, 0f, 0f);
            //eachChild.Rotate(new Vector3(180, 0, 0));//rotation=Quaternion.FromToRotation(Quaternion.identity, new Vector3(180,0,0));
            item.transform.Rotate(Vector3.left * Time.deltaTime * 180);//rotation=Quaternion.FromToRotation(Quaternion.identity, new Vector3(180,0,0));
            //item.transform.rotation = Quaternion.RotateTowards(item.transform.rotation, Quaternion.Euler(45, 45, 0), 10 * Time.deltaTime);
        }

        await Task.Delay(5000);
    }
    public static void RotatePawn(Transform toRotate)
    {
        toRotate.transform.Rotate(Vector3.left * Time.deltaTime * 180);
    }
}
