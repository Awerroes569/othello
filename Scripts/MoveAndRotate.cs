using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndRotate : MonoBehaviour
{

    float speed1 = 0;
    float speed2 = 0;
    float speed3 = 0;
    float speedDown = 0;
    // Start is called before the first frame update
    void Start()
    {
        speed1 = RandAI.rnd.Next(-180, 181);
        speed2 = RandAI.rnd.Next(-180, 181);
        speed3 = RandAI.rnd.Next(-180, 181);
        speedDown= RandAI.rnd.Next(5, 31)/10f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(
            speed1 * Time.deltaTime,
            speed2 * Time.deltaTime,
            speed3 * Time.deltaTime);

        transform.Translate(Vector3.down *speedDown* Time.deltaTime,Space.World);

        if(transform.position.y<-6)
        {
            Destroy(gameObject);
        }
    }
}
