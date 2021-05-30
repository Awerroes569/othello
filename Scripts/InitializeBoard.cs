using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class InitializeBoard : MonoBehaviour
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
    [SerializeField]
    private Text mobility;
    [SerializeField]
    private Text stability;
    [SerializeField]
    private Text howgood;
    [SerializeField]
    private TextMeshProUGUI saveText;
    [SerializeField]
    private TextMeshProUGUI tableText;
    [SerializeField]
    public  RuntimeAnimatorController anime;
    [SerializeField]
    public GameObject menuTable;
    [SerializeField]
    private Text blackPlayer;
    [SerializeField]
    private Text whitePlayer;
    [SerializeField]
    private Text wait;

    //private TouchScreenKeyboard keyboard;
    static bool isHinted = false;


    GameObject table;
    GameObject tableYN1;
    GameObject tableYN2;
    GameObject tableYN3;


    public bool isAllowed = false;

    Board newTable;

    Camera camera;///
    //MetaBoard newMeta;
    //Record newRecord;

    // Start is called before the first frame update
    void Start()
    {
        camera=Camera.main;///

        newTable = new Board();
        Generals.currentRecord = new Record();
        if (Generals.nameOfLoadedFile!=null
            &&Generals.nameOfLoadedFile.Length >1)
        {
            //Debug.Log("name is:" + Generals.nameOfLoadedFile);
            Generals.currentLoad = SaveData.LoadGame(Generals.nameOfLoadedFile);

            var load = Generals.currentLoad;

            Generals.onPossibilities = load.OnPossibilities;
            Generals.onLastMove = load.OnLastMove;
            Generals.onScore = load.OnScore;
            Generals.onAnimation = load.OnAnimation;
            Generals.onSound = load.OnSound;
            Generals.onMusic = load.OnMusic;
            Generals.isYourBlack = load.IsYourBlack;
            Generals.isHuman = load.IsHuman;
            Generals.difficulty = load.Difficulty;
            Generals.handicap = load.Handicap;
            //Debug.Log("handicap is " + Generals.handicap);

            Generals.currentRecord = new Record(load.Counter, load.Moves);

            SetAllMoves(Generals.currentRecord,newTable);

            isHinted = false;
        }


        table = GameObject.Find("EndTable");
        tableYN1 = GameObject.Find("MenuTable");
        tableYN2 = GameObject.Find("NewTable");
        tableYN3 = GameObject.Find("SaveTable");
        StartBoard();
        newTable.RefreshLook();
        table.SetActive(false);
        tableYN1.SetActive(false);
        tableYN2.SetActive(false);
        tableYN3.SetActive(false);

        wait.text = null;

        /////////////
        blackPlayer.text = newTable.BlackPlayer();
        whitePlayer.text = newTable.WhitePlayer();

        if(!Generals.onCoordinates)
        {
            newTable.HideCoordintates();
        }

        if (Generals.onScore)
        {
            whitePawns.text = newTable.Whites.ToString();
            blackPawns.text = newTable.Blacks.ToString();
        }

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
                
                //Debug.Log("hit!!!");
                switch (hit.transform.name)
                {
                    case "Menu":
                        SoundManager.PlayClick();
                        if (newTable.IsFinished)
                        {
                            SceneManager.LoadScene("Menu");
                        }
                        Generals.canClick = false;
                        tableYN1.SetActive(true);
                        Generals.tableMenu = tableYN1;
                        break;
                    case "NewGame":
                        SoundManager.PlayClick();
                        if (newTable.IsFinished)
                        {
                            SceneManager.LoadScene("NewGameMenu");
                        }
                        Generals.canClick = false;
                        tableYN2.SetActive(true);
                        Generals.tableMenu = tableYN2;
                        break;
                    case "Save":
                        SoundManager.PlayClick();
                        Generals.canClick = false;
                        tableYN3.SetActive(true);
                        string name=SaveData.CreateSaveFileName();
                        string fullName = "The game will be saved under name:\n"
                            +name+"\nor you can type the name below";
                        Debug.Log(fullName);
                        saveText.text = fullName;
                        //Debug.Log(name);
                        //InputField field = tableYN3.GetComponentInChildren<InputField>();
                        //field.text=name;
                        //field.ActivateInputField();
                        //field.Select();
                        
                        Generals.tableMenu = tableYN3;

                        break;
                    case "Hint":

                        //Debug.Log("Hint hitted");
                        //wait.text = "Wait...";
                        //Debug.Log(wait.text);
                        Pole.PoleStatus cpuSide2 = Generals.isYourBlack ? Pole.PoleStatus.White : Pole.PoleStatus.Black;
                        //if (newTable.WhoNext != cpuSide2&&!Generals.isHuman&&!isHinted)
                        //{
                            //Generals.canClick = false;
                         //   wait.text = "Wait...";
                        //}
                        //else
                        //{
                        //    break;
                        //}
                        SoundManager.PlayClick();
                        int toEnd = 64 - newTable.Blacks - newTable.Whites;
                        string toExecute = RandAI.ChooseBestMove(newTable,toEnd);
                        GameObject toModify = GameObject.
                            FindGameObjectWithTag(toExecute);
                        var square = toModify.GetComponent<MeshRenderer>();
                        square.materials[0].color = Color.grey;
                        wait.text = null;
                        isHinted = true;

                        break;
                    case "Undo":
                        SoundManager.PlayClick();
                        Generals.canClick = false;
                        Debug.Log("undo hitted");
                        Pole.PoleStatus UndidPlayer = newTable.WhoNext;
                        Debug.Log(UndidPlayer);
                        do
                        {
                            Generals.currentRecord.UndoRecord(Generals.isHuman);
                            newTable = new Board();
                            SetAllMoves(Generals.currentRecord, newTable);
                            //StartCoroutine(CleaningN());
                            Debug.Log("inside loop: "+UndidPlayer);
                            if (Generals.isHuman)
                            {
                                Debug.Log("break because of human");
                                break;
                            }
                        } while (newTable.WhoNext != UndidPlayer);
                        StartCoroutine(CleaningN());

                        //RemoveBoard();
                        //StartBoard();
                        //newTable.RefreshLook();

                        //SceneManager.LoadScene("transit");
                        //SceneManager.LoadScene("Menu");

                        break;
                    default:
                        break;
                }
            }
        }


        Pole.PoleStatus cpuSide = Generals.isYourBlack ? Pole.PoleStatus.White : Pole.PoleStatus.Black;
        if (newTable.WhoNext == cpuSide&&!Generals.isHuman&&!newTable.IsFinished)
        {
            //Generals.canClick = false;
            wait.text = "Wait...";
        }
        else
        {
            wait.text = null;
        }

            if (Generals.aiAllowed)
        {
            Debug.Log("Pre-thinking");
            MakeAIMove(newTable);
            
        }


    }

    private void StartBoard()
    {
        int startX = -7;
        int startY = 7;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var position = new Vector3(startX, startY, 0);
                string forTag = i + "x" + j;
                GameObject current = Instantiate(pawn, position, Quaternion.Euler(new Vector3(90, 0, 0)));
                current.tag = forTag; //"MainCamera";
                startX += 2;
            }

            startX = -7;
            startY -= 2;
        }

        PositionHandicaps(newTable);
        //GameObject toReverse = GameObject.FindGameObjectWithTag("0x0");
        //Positions.GoWhite(toReverse.transform);
        //Positions.RotatePawn(toReverse.transform);
        PreparingInfo();
    }

    public void RemoveBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                string tag = i + "x" + j;
                GameObject toDelete = GameObject.FindGameObjectWithTag(tag);
                if (toDelete != null)
                {
                    Destroy(toDelete);
                }
            }

        }

    }

    public async void Clickedd(string position)
    {

        var toParse = position.Split('x');
        int x = int.Parse(toParse[0]);
        int y = int.Parse(toParse[1]);
        SoundManager.PlayClick();
        await this.GoSleep(100);
        //yield return new WaitForSeconds(0.1f);
        newTable.CurrentCleaning();
        Transform toModify = GameObject.
                FindGameObjectWithTag(position).transform;
        if(newTable.WhoNext==Pole.PoleStatus.Black)
        {
            Positions.GoBlack(toModify);
        }
        else
        {
            Positions.GoWhite(toModify);
        }
        ////////////////////
        
        newTable.SetToRotate(x,y,newTable.WhoNext);
        
        ////////////////////
        await this.GoSleep(Generals.delay);
        Generals.delay = 0;

        StartCoroutine(AfterClicking(position));
    }

    public IEnumerator AfterClicking(string position)
    {
        var toParse = position.Split('x');
        int x = int.Parse(toParse[0]);
        int y = int.Parse(toParse[1]);
        //StartCoroutine(Rotate());// newTable,x,y));
        yield return StartCoroutine(Cleaning());


        Generals.currentRecord.AddRecord(x, y);
        ////HERE ROTATION
        //newTable.SetToRotate(x, y);
        //Pole.PoleStatus prev = newTable.WhoNext;
        //newTable.SetAndReverse(x, y);
        
        //Pole.PoleStatus now = newTable.WhoNext;
        //if (prev == now)
        //{
            
            StartCoroutine(MessagePassed(x,y));
        //}
        //yield return new WaitForSeconds(0.1f);
        //newTable.WhoNext =
        //newTable.WhoNext == Pole.PoleStatus.White ? Pole.PoleStatus.Black : Pole.PoleStatus.White;
        //yield return new WaitForSeconds(0.1f);

        newTable.RefreshLook();
        if (Generals.onScore)
        {
            whitePawns.text = newTable.Whites.ToString();
            blackPawns.text = newTable.Blacks.ToString();
            safeBlack.text = newTable.SafeBlacks.ToString();
            safeWhite.text = newTable.SafeWhites.ToString();
        }

        PreparingInfo();

        if (newTable.WhoNext == Pole.PoleStatus.White)
        {
            ///  old   emergency
            //int howfar = newTable.CountMobilityForAI(newTable);
            //var newMeta = new MetaBoard(newTable, 0, howfar, newTable.WhoNext);
            //analysis.text = newMeta.FindBest();
            //Debug.Log("dupa: "+analysis.text);
        }

        record.text = Generals.currentRecord.Moves;
        mobility.text = newTable.CountMobilityCombo(newTable).ToString();
        stability.text=newTable.CountStability(newTable).ToString();
        howgood.text = newTable.CountScore(newTable).ToString();
        Debug.Log("score: " + howgood.text);
        if (newTable.WhoNext == Pole.PoleStatus.Black)
        {
            //Debug.Log("Best move: " + RandAI.ChooseBestMove(newTable));
        }
        if(newTable.IsFinished)
        {
            tableText.text = newTable.Blacks > newTable.Whites ?
                "BLACKS WIN" : "WHITES WIN";
            if(newTable.Blacks == newTable.Whites)
            {
                tableText.text = "DRAW";
            }
            table.SetActive(true);
            SoundManager.PlayFanfare(newTable.Blacks > newTable.Whites);
        }
        //yield return 0;
        Generals.aiAllowed = true;
        Generals.canClick = true;

        Pole.PoleStatus cpuSide = Generals.isYourBlack ? Pole.PoleStatus.White : Pole.PoleStatus.Black;
        if (newTable.WhoNext == cpuSide&&!Generals.isHuman)
        {
            //Generals.canClick = false;
            wait.text = "Wait...";
        }
        else
        {
            wait.text = null;
        }

        isHinted = false;
    }

    public IEnumerator Cleaning()
    {
        RemoveBoard();
        yield return 0;// new WaitForSeconds(0.0f);
        StartBoard();
        yield return 0;
    }

    public IEnumerator CleaningN()
    {
        RemoveBoard();
        yield return 0;// new WaitForSeconds(0.0f);
        StartBoard();
        yield return 0;
        newTable.RefreshLook();
        Generals.canClick = true;

    }

    public IEnumerator MessagePassed(int x,int y)
    {
        Pole.PoleStatus prev = newTable.WhoNext;
        newTable.SetAndReverse(x, y);
        yield return 0;

        Pole.PoleStatus now = newTable.WhoNext;
        if (prev == now&&!newTable.IsFinished)
        {
            tableText.text = "PASSED";
            table.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            table.SetActive(false);
        }
    }

    private void PreparingInfo()
    {
        if (newTable.IsFinished == true)
        {
            info.text = "FINISHED";
            info.color = Color.white;
        }
        else if (newTable.WhoNext == Pole.PoleStatus.White)
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

    private void PositionHandicaps(Board board)
    {
        if (Generals.handicap > 0)
        {
            board.Table[0, 0].WhatInside=
                Generals.isYourBlack == true ? Pole.PoleStatus.Black 
                : Pole.PoleStatus.White;
        }

        if (Generals.handicap > 1)
        {
            board.Table[0, 7].WhatInside =
                Generals.isYourBlack == true ? Pole.PoleStatus.Black
                : Pole.PoleStatus.White;
        }

        if (Generals.handicap > 2)
        {
            board.Table[7, 0].WhatInside =
                Generals.isYourBlack == true ? Pole.PoleStatus.Black
                : Pole.PoleStatus.White;
        }

        if (Generals.handicap > 3)
        {
            board.Table[7, 7].WhatInside =
                Generals.isYourBlack == true ? Pole.PoleStatus.Black
                : Pole.PoleStatus.White;
        }
    }

    private void SetAllMoves(Record record, Board board)
    {
        StringReader reader = new StringReader(record.Moves);
        this.record.text = record.Moves;
        string currentLine;
        int counter = 0;
        using (reader)
        {
            while (true)
            {
                currentLine = reader.ReadLine();
                if (currentLine != null)//&&counter>0)
                {
                    var coordinates = Record.FindCoordinatesFromString(currentLine);
                    board.SetAndReverse(coordinates[2], coordinates[1]);
                    Debug.Log("Move is " + coordinates[2] + " " + coordinates[1]);
                    counter++;
                }
                else
                {
                    break;
                }
            }
        }

    }

    public  void HideTable()
    {
        GameObject table = GameObject.Find("EndTable");
        if(table!=null)
        {
            //Debug.Log("table found");
            table.SetActive(false);
            //table.SetActive(true);

        }
    }

    public static void UnhideTable()
    {
        GameObject table = GameObject.Find("EndTable");
        if (table != null)
        {
            //Debug.Log("table found to be active");
            table.SetActive(true);

        }
    }

    public void SetInactive()
    {
        this.gameObject.SetActive(false);
    }

    public IEnumerator Rotate(Board board,int x,int y)
    {
        //board.SetToRotate(x, y);
        yield return new WaitForSeconds(10f);
        //yield return new WaitForSeconds(10f);
    }

    public IEnumerator Rotate()
    {
        
            yield return new WaitForSeconds(10f);
        
    }

    public Task GoRotate()
    {

        //StartCoroutine(Rotate());
        return Task.Delay(3000);
        
    }
    public Task GoSleep(int time)
    {

        //StartCoroutine(Rotate());
        return Task.Delay(time);

    }

    public static void GoToMenu()
    {
        SoundManager.PlayClick();
        SceneManager.LoadScene("Menu");
    }

    public  void onYesClickMenu()
    {
        SoundManager.PlayClick();
        Generals.canClick = true;
        //Debug.Log("yes yes yes");
        SceneManager.LoadScene("Menu");
    }

    public void onYesClickNew()
    {
        SoundManager.PlayClick();
        Generals.canClick = true;
        //Debug.Log("yes yes yes");
        SceneManager.LoadScene("NewGameMenu");
    }

    public void onNoClickMenu()
    {
        SoundManager.PlayClick();
        Generals.canClick = true;
        Generals.tableMenu.SetActive(false);
        //Transform item=this.transform.parent.transform.parent.transform;
        //.gameObject.SetActive(false);
        //SceneManager.LoadScene("Menu");
    }

    public void onProceedClickMenu()
    {
        SoundManager.PlayClick();
        string text = Generals.tableMenu.GetComponentInChildren<InputField>().text;
        if(text.Length<1)
        {
            SaveData.SaveGame(Generals.currentRecord);
        }
        else
        {
            SaveData.SaveGame(Generals.currentRecord, text);
        }
        Generals.canClick = true;
        //Debug.Log("proceed");
        Generals.tableMenu.SetActive(false);
    }

    public void MakeAIMove(Board board)
    {
        Generals.aiAllowed = false;
        if (Generals.isHuman)
        {
            return;
        }
        
        Pole.PoleStatus cpuSide = Generals.isYourBlack ? Pole.PoleStatus.White : Pole.PoleStatus.Black;
        if (board.WhoNext == cpuSide)
        {
            Generals.canClick = false;
            //wait.text = "Wait...";

            //StartCoroutine(Rotate());

            //yield return new WaitForSeconds(2f);

            Debug.Log("Start thinking");

            string toExecute = RandAI.ChooseBestMove(board,64-newTable.Blacks-newTable.Whites);

            Clickedd(toExecute);
            
            //int depth = newTable.CountMobilityForAI(newTable);
            //Clickedd(Node.CalculateRoot(board,5));
            //int howfar = newTable.CountMobilityForAI(newTable);
            //var newMeta = new MetaBoard(newTable, 0, depth, newTable.WhoNext);
            //analysis.text = newMeta.FindBest();
            //Clickedd(newMeta.FindBest());
        }
    }

}
