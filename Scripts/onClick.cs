using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class onClick : MonoBehaviour
{

    [SerializeField]
    public GameObject receiver;
    //receiverScript= ;

    private void Start()
    {
        receiver = GameObject.Find("Controller");
        //receiverScript = receiver.GetComponent<ScriptableObject>();
    }

    public void OnMouseDown()
    {
        //Debug.Log("testing 1 2 3");
        //foreach (Transform eachChild in transform)
        //{
        //    if (eachChild.name == "Pawn")
        //    {
        //        Debug.Log("Child found. ...");
        //        eachChild.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
        //   }
        //}

        //Positions.GoWhite(this.transform);
        //Positions.GoWhite(this.transform);
        //Positions.RotatePawn(this.transform);

        foreach (Transform eachChild in transform)
        {
            if (eachChild.name == "Possible")
            {
                //Debug.Log("Child found. ...");
                if(eachChild.transform.position.z <0)
                {
                    //BroadcastMessage("Clickedd", transform.gameObject.tag,SendMessageOptions.RequireReceiver);
                    //receiver.GetComponent(InitalizeBoard);
                    var goScript = (InitializeBoard)receiver.GetComponent(typeof(InitializeBoard));
                    if (Generals.canClick)
                    {
                        goScript.Clickedd(transform.tag);
                    }
                    return;
                }
           }
            else if(eachChild.parent.name == "EndTable"
                || eachChild.parent.name == "EndTable")
            {
                Destroy(eachChild.gameObject);
            }
        }

       
    }

    public void Clicked(string message)
    {
        Debug.Log(message);
    }

    public void SaveGame()
    {
        SaveData.SaveGame(Generals.currentRecord);
    }

    
}
