using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.IO;

public class Record 
{
    string moves;
    int counter;

    public string Moves { get => moves; set => moves = value; }
    public int Counter { get => counter; set => counter = value; }

    public Record()
    {
       this.Counter = 1;
        this.Moves = "";
    }

    public Record(int counter,string moves)
    {
        this.Counter = counter;
        this.Moves = moves;
    }
        

    public void AddRecord(int x,int y)
    {
        StringBuilder builder = new StringBuilder();
        string newRecord = (this.Counter<10?"  ":" ")+this.Counter + "  " 
            + WhichColor() + "  "
            + FindLetter(y) + "  " + (x+1);
        this.Counter++;
        builder.Append(this.Moves);
        if(builder.Length>0)
        {
            builder.Append("\n");
        }
        builder.Append(newRecord);
        this.Moves = builder.ToString();
    }

    public void UndoRecord(bool isHuman)
    {
        Debug.Log("before " + Generals.currentRecord.Moves);
        StringBuilder builder = new StringBuilder();
        StringReader reader = new StringReader(Generals.currentRecord.Moves);
        using (reader)
        {
            //int toDeduct = isHuman ? 1 : 2;


            for (int i = 0; i < Generals.currentRecord.Counter - 2;i++)//toDeduct-1; i++)
            {
                string move = reader.ReadLine();
                Debug.Log(move);
                builder.Append(move);
                builder.Append("\n");
            }
            Generals.currentRecord.Counter -= 1;// toDeduct;

            Generals.currentRecord.Moves = builder.ToString();

            Debug.Log("after "+ Generals.currentRecord.Moves);
        }
    }

    private string WhichColor()
    {
        string result;

        if (Generals.isYourBlack)
        {
            if(this.Counter%2==0)
            {
                result = "White";
            }
            else
            {
                result = "Black";
            }
        }
        else
        {
            if (this.Counter % 2 == 0)
            {
                result = "Black";
            }
            else
            {
                result = "White";
            }
        }

        return result;
    }

    private char FindLetter(int a)
    {
        switch(a)
        {
            case 0:
                return 'A';
            case 1:
                return 'B';
            case 2:
                return 'C';
            case 3:
                return 'D';
            case 4:
                return 'E';
            case 5:
                return 'F';
            case 6:
                return 'G';
            case 7:
                return 'H';
            default:
                return ' ';

        }
    }

    public static int[] FindCoordinatesFromString(string line)
    {
        if(line.Length<11)
        {
            Debug.Log("too short string in a record");
            //throw new ArgumentException("too short string");
            return null;
        }
        else
        {
            var coordinates = new int[3];
            char[] separators = { ' ' };
            string[] elements = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            //counter
            coordinates[0] = int.Parse(elements[0]);
            //y position
            coordinates[1] = FindIntegerFromLetter(elements[2]);
            //x position
            coordinates[2]= int.Parse(elements[3])-1;

            return coordinates;
        }
    }

    public static int FindIntegerFromLetter(string letter)
    {
        switch (letter)
        {
            case "A":
                return 0;
            case "B":
                return 1;
            case "C":
                return 2;
            case "D":
                return 3;
            case "E":
                return 4;
            case "F":
                return 5;
            case "G":
                return 6;
            case "H":
                return 7;
            default:
                return -1;

        }
    }
}
