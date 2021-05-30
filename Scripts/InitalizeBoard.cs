using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitalizeBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject pawn;
    [SerializeField]
    private Text whitePawns;
    [SerializeField]
    private Text blackPawns;
    [SerializeField]
    private Text info;
    [SerializeField]
    private Text safeWhite;
    [SerializeField]
    private Text safeBlack;
    [SerializeField]
    private Text analysis;
    [SerializeField]
    private Text record;


    public bool isAllowed = false;

    Board newTable;
    MetaBoard newMeta;
    Record newRecord;

    // Start is called before the first frame update
    void Start()
    {
        StartBoard();

        

        newTable = new Board();
        newRecord = new Record();
        //RemoveBoard();
        //StartBoard();
        newTable.RefreshLook();
        //newTable.Table[7, 7].WhatInside = Pole.PoleStatus.White;
        //newTable.RefreshLook();
        //newTable.RefreshLook();
        //RemoveBoard();
        //StartBoard();
        //newTable.RefreshLook();
        //Debug.Log(newTable.Table[7, 7].WhatInside.ToString());
        //SaveData.SaveGame(newRecord);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartBoard()
    {
        int startX = -7;
        int startY = 7;

        for(int i=0;i<8;i++)
        {
            for(int j=0;j<8;j++)
            {
                var position = new Vector3(startX, startY,0);
                string forTag = i + "x" + j;
                GameObject current=Instantiate(pawn, position, Quaternion.Euler(new Vector3(90, 0, 0)));
                current.tag = forTag; //"MainCamera";
                startX += 2;
            }

            startX = -7;
            startY -= 2;
        }

        //GameObject toReverse = GameObject.FindGameObjectWithTag("0x0");
        //Positions.GoWhite(toReverse.transform);
        //Positions.RotatePawn(toReverse.transform);
    }

    public void RemoveBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                string tag = i + "x" + j;
                GameObject toDelete = GameObject.FindGameObjectWithTag(tag);
                if(toDelete!=null)
                {
                    Destroy(toDelete);
                }
            }

        }

    }

    public void Clickedd(string position)
    {
        StartCoroutine(AfterClicking(position));
    }

    public IEnumerator AfterClicking(string position)
    {
        var toParse = position.Split('x');
        int x = int.Parse(toParse[0]);
        int y = int.Parse(toParse[1]);

        yield return StartCoroutine(Cleaning());


        newRecord.AddRecord(x, y);
        newTable.SetAndReverse(x, y);
        yield return new WaitForSeconds(0.1f);
        //newTable.WhoNext =
   //newTable.WhoNext == Pole.PoleStatus.White ? Pole.PoleStatus.Black : Pole.PoleStatus.White;
        yield return new WaitForSeconds(0.1f);
        
        newTable.RefreshLook();
        if (Generals.onScore)
        {
            whitePawns.text = newTable.Whites.ToString();
            blackPawns.text = newTable.Blacks.ToString();
            safeBlack.text = newTable.SafeBlacks.ToString();
            safeWhite.text = newTable.SafeWhites.ToString();
            PreparingInfo();
        }

        if (newTable.WhoNext == Pole.PoleStatus.Black)
        {
            newMeta = new MetaBoard(newTable, 0, 5, newTable.WhoNext);
            //analysis.text = newMeta.FindBest();
        }

        record.text = newRecord.Moves;


    }

    public IEnumerator Cleaning()
    {
        RemoveBoard();
        yield return new WaitForSeconds(0.1f);
        StartBoard();
        yield return 0;
    }

    private void PreparingInfo()
    {
        if(newTable.IsFinished==true)
        {
            info.text = "FINISHED";
            info.color = Color.white;
        }
        else if(newTable.WhoNext==Pole.PoleStatus.White)
        {
            info.text = "WHITE";
            info.color = Color.white;
        }
        else
        {
            info.text = "BLACK";
            info.color = Color.black;
        }
    }
}
