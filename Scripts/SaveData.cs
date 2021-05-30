using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[Serializable]
public class SaveData
{
    private bool onPossibilities;
    private bool onLastMove;
    private bool onScore;
    private bool onAnimation;
    private bool onSound;
    private bool onMusic;
    private bool isYourBlack;
    private bool isHuman;
    private int difficulty;
    private int handicap;
    int counter;
    string moves;


    public bool OnPossibilities { get => onPossibilities; set => onPossibilities = value; }
    public bool OnLastMove { get => onLastMove; set => onLastMove = value; }
    public bool OnScore { get => onScore; set => onScore = value; }
    public bool OnAnimation { get => onAnimation; set => onAnimation = value; }
    public bool OnSound { get => onSound; set => onSound = value; }
    public bool OnMusic { get => onMusic; set => onMusic = value; }
    public bool IsYourBlack { get => isYourBlack; set => isYourBlack = value; }
    public int Counter { get => counter; set => counter = value; }
    public bool IsHuman { get => isHuman; set => isHuman = value; }
    public int Difficulty { get => difficulty; set => difficulty = value; }
    public int Handicap { get => handicap; set => handicap = value; }
    public string Moves { get => moves; set => moves = value; }

    public SaveData(bool a,bool b,bool c,bool d,bool e,
        bool f,bool g,bool h,int i,int j,int count,string mov)
    {
        this.OnPossibilities = a;
        this.OnLastMove = b;
        this.OnScore = c;
        this.OnAnimation = d;
        this.OnSound = e;
        this.OnMusic = f;
        this.IsYourBlack = g;
        this.IsHuman=h;
        this.Difficulty=i;
        this.Handicap=j;

        this.Counter = count;
        this.Moves = mov;
    }

    public static void SaveGame(Record currentGame)
    {
        var toSave = new SaveData(
            Generals.onPossibilities,
            Generals.onLastMove,
            Generals.onScore,
            Generals.onAnimation,
            Generals.onSound,
            Generals.onMusic,
            Generals.isYourBlack,
            Generals.isHuman,
            Generals.difficulty,
            Generals.handicap,
            Generals.currentRecord.Counter,//currentGame.Counter,
            Generals.currentRecord.Moves//currentGame.Moves
            );

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "//" + SaveData.CreateSaveFileName();
        FileStream stream = new FileStream(path, FileMode.Create);

        

        formatter.Serialize(stream, toSave);
        stream.Close();
    }

    public static void SaveGame(Record currentGame,string name)
    {
        var toSave = new SaveData(
            Generals.onPossibilities,
            Generals.onLastMove,
            Generals.onScore,
            Generals.onAnimation,
            Generals.onSound,
            Generals.onMusic,
            Generals.isYourBlack,
            Generals.isHuman,
            Generals.difficulty,
            Generals.handicap,
            Generals.currentRecord.Counter,//currentGame.Counter,
            Generals.currentRecord.Moves//currentGame.Moves
            );
        //File extFiles = getExternalFilesDir(null);
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath  + "//"+name+".sav";//+ "\\"
        FileStream stream = new FileStream(path, FileMode.Create);



        formatter.Serialize(stream, toSave);
        stream.Close();
    }

    public static string CreateSaveFileName()
    {
        string date = DateTime.Now.ToString();
        date=date.Replace('.', '_');
        date = date.Replace(':', '_');
        date = date.Replace('/', '_');
        date = date.Replace('\\', '_');
        string result;
        result = (Generals.isHuman == true ? "HvH" :
            (Generals.isYourBlack?"HvC":"CvH"))+" " 
            +date+".sav";
        return result;
    }

    private static string CreateSaveFileName(string name)
    {
        string date = DateTime.Now.ToString();
        date = date.Replace('.', '_');
        date = date.Replace(':', '_');
        string result;
        result =name+" "+ (Generals.isHuman == true ? "HvH" : "HvC")+" "
            + date + ".sav";
        return result;
    }

    public static SaveData LoadGame(string name)
    {
        string path = Application.persistentDataPath +  "//"+name+".sav";//"\\" +
        if (File.Exists(path))
        {
            Debug.Log("File exists");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    public static List<string> GetFilesOfType()//SaveType fileType)
    {
        Debug.Log($"gettting files");
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "//");
        FileInfo[] info = dir.GetFiles("*" + ".sav");
        List<string> fileNames = null;

        SortedDictionary<DateTime, string> filesDict = new SortedDictionary<DateTime, string>();



        foreach (FileInfo f in info)
        {
            //fileNames.Add(Path.GetFileNameWithoutExtension(f.FullName));
            filesDict.Add(f.LastWriteTime, Path.GetFileNameWithoutExtension(f.FullName));
        }
        //return fileNames;

        if (filesDict.Count > 10)
        {
            string path=null;
            DateTime key=default(DateTime);

            foreach (var item in filesDict)
            {
                path = dir + item.Value+ ".sav";
                key = item.Key;
                break;
                
            }

            

            filesDict.Remove(key);
            Debug.Log(path);
            File.Delete(path);

            //fileNames = new List<string>(filesDict.Values);
        }
        fileNames = new List<string>(filesDict.Values);
        //Debug.Log("number of files: " + fileNames.Count);
        return fileNames;
    }

    
}
