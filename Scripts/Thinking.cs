using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Thinking : MonoBehaviour
{

    [SerializeField]
    private Text digits;

    float delay;

    int digit0;
    int digit1;
    int digit2;
    int digit3;
    int digit4;
    int digit5;
    int digit6;
    int digit7;

    float now;

    // Start is called before the first frame update
    void Start()
    {
        digits.text = null;
        now = Time.time;
        Invoke("PublishDigits", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Generals.canClick)
        {
            if (Time.time - now > 0.5f)
            {
                digits.text = GenerateBinNumber();
                now = Time.time;
            }
        }
        else
        {
            digits.text = null;
        }
    }

     string GenerateBinNumber()
    {
        digit0 = RandAI.rnd.Next(0, 2);
        digit1 = RandAI.rnd.Next(0, 2);
        digit2 = RandAI.rnd.Next(0, 2);
        digit3 = RandAI.rnd.Next(0, 2);
        digit4 = RandAI.rnd.Next(0, 2);
        digit5 = RandAI.rnd.Next(0, 2);
        digit6 = RandAI.rnd.Next(0, 2);
        digit7 = RandAI.rnd.Next(0, 2);

        string result =
            digit0.ToString() +
            digit1.ToString() +
            digit2.ToString() +
            digit3.ToString() +
            digit4.ToString() +
            digit5.ToString() +
            digit6.ToString() +
            digit7.ToString();
        return result;

    }

    public void PublishDigits()
    {


        
            if (!Generals.aiAllowed)
            {
                if (Time.time - now > 0.5f)
                {
                    digits.text = GenerateBinNumber();
                    now = Time.time;
                }
            }
            else
            {
                digits.text = null;
            }
        

        
    }
}
