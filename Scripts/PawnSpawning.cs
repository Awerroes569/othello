using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnSpawning : MonoBehaviour
{


    [SerializeField]
    public GameObject pawn;
    public float startTime = 2f;
    public float spawnTime = 50f;
    float position = 0;
    float begin = 0;
    float now = 0;
    // Use this for initialization
    void Start()
    {
        //StartCoroutine(Spawn());
        begin = Time.time;
        now = Time.time;
        spawnTime = 0.3f;
        //InvokeRepeating("SpawnPawn", startTime, spawnTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time-begin>startTime)
        {
         //  StartCoroutine( Spawn());
            if(Time.time-now>spawnTime)
            {
                //Debug.Log(Time.time - now+"  "+spawnTime);
                SpawnPawn();
                now = Time.time;
            }

        }
    }
    void SpawnPawn()
    {
        position = RandAI.rnd.Next(-30, 31) / 10f;

        var newPawn = GameObject.Instantiate(pawn, new Vector3(position, 6, 0), Quaternion.identity);
    }

    IEnumerator Spawn()
    {
        while (true)
        {

            position = RandAI.rnd.Next(-30, 31) / 10f;

            var newPawn = GameObject.Instantiate(pawn, new Vector3(position, 6, 0), Quaternion.identity);
            //repeatTime -= 0.01f;
            //repeatTime = Mathf.Clamp(repeatTime, 3f, maxRepeatTime);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    
}
