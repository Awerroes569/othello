using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class MetaBoard
{
    private Board currentBoard;
    private int outcome;
    List<MetaBoard> kids;
    private int targetLevel;
    private int currentLevel;
    private Pole.PoleStatus status;

    public Board CurrentBoard { get => currentBoard; set => currentBoard = value; }
    public int Outcome { get => CountOutcome(this.Status);}
    public List<MetaBoard> Kids { get => kids; set => kids = value; }
    public int TargetLevel { get => targetLevel; set => targetLevel = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public Pole.PoleStatus Status { get => status; set => status = value; }

    public MetaBoard(Board oldBoard,int level,int target,Pole.PoleStatus status)
    {
        this.Kids = new List<MetaBoard>();
        this.CurrentBoard =oldBoard.DeepCopy();
        //{
         //   Table = new Pole[8,8],
        //    WhoNext = oldBoard.WhoNext,
        //    LastMove=oldBoard.LastMove,
        //    IsFinished=oldBoard.IsFinished
        //};
        
        //foreach(var item in this.CurrentBoard.Table)
        //{
           // item.WhatInside = oldBoard.Table[item.XVal, item.YVal].WhatInside;
           // item.IsSafe = oldBoard.Table[item.XVal, item.YVal].IsSafe;
        //}
        
        //this.CurrentBoard =oldBoard;
        TargetLevel = target;
        CurrentLevel = level+1;
        Board processedBoard = oldBoard.DeepCopy();
        List<string> preKids = processedBoard.FindPossibles(processedBoard);
        if(preKids.Count<1)
        {
            
            processedBoard.WhoNext =
               processedBoard.WhoNext == Pole.PoleStatus.White ?
                Pole.PoleStatus.Black : Pole.PoleStatus.White;
            preKids= processedBoard.FindPossibles(CurrentBoard);
        }
        if(preKids.Count>0&&this.CurrentLevel<TargetLevel)
        {
            foreach(var item in preKids)
            {
                Board ignite = processedBoard.DeepCopy();
                string[] coordinates = item.Split('x');
                ignite.SetAndReverse(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
                Kids.Add(new MetaBoard(ignite, this.CurrentLevel,this.TargetLevel,this.Status));
            }
        }
    }
    private int CountOutcome(Pole.PoleStatus status)
    {
        int result;
        if (this.Kids.Count < 1)
        {
            if (status == Pole.PoleStatus.White)
            {
                result =-this.CurrentBoard.CountScore(this.CurrentBoard);// SafeWhites - CurrentBoard.SafeBlacks;
                return result;
            }
            else
            {
                result =this.CurrentBoard.CountScore(this.CurrentBoard);// this.CurrentBoard.SafeBlacks - CurrentBoard.SafeWhites;
                return result;
            }
        }

        if (status == Pole.PoleStatus.White)
        {
            if(this.CurrentBoard.WhoNext==Pole.PoleStatus.White)
            {
                return MinimumFromKids();
            }
            else
            {
                return MaximumFromKids();
            }
        }

        if (status == Pole.PoleStatus.Black)
        {
            if (this.CurrentBoard.WhoNext == Pole.PoleStatus.Black)
            {
                return MinimumFromKids();
            }
            else
            {
                return MaximumFromKids();
            }
        }

        return -1000;
    }

    private int MaximumFromKids()
    {
        int result=-64;

        foreach(var item in this.Kids)
        {
            if(item.Outcome>result)
            {
                result = item.Outcome;
            }
        }
        return result;
    }

    private int MinimumFromKids()
    {
        int result = 64;

        foreach (var item in this.Kids)
        {
            if (item.Outcome < result)
            {
                result = item.Outcome;
            }
        }
        return result;
    }

    public string FindBest()
    {
        var listMax = new List<string>();

        foreach(var item in this.Kids)
        {
            if(item.Outcome>=this.Outcome)
            {
                string coordinates;
                coordinates = item.CurrentBoard.LastMove.XVal + "x" +
                    item.CurrentBoard.LastMove.YVal;
                listMax.Add(coordinates);
            }
        }

        StringBuilder builder = new StringBuilder();
        //builder.Append(this.Outcome + "\n");
        // foreach(var item in listMax)
        //{
        builder.Append(listMax[listMax.Count-1]);//item+"\n");
        //}
        return builder.ToString();
    }
}
