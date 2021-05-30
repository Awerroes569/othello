using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;


public class Board
{
    private Pole[,] table;
    private Pole.PoleStatus whoNext;
    private int blacks;
    private int whites;
    private int safeBlacks;
    private int safeWhites;
    private Pole lastMove;
    private bool isFinished;
    

    public Pole[,] Table { get => table; set => table = value; }
    public Pole.PoleStatus WhoNext { get => whoNext; set => whoNext = value; }
    public int Blacks { get => CountBlacks(); }//set => blacks = value; }
    public int Whites { get => CountWhites(); }//set => whites = value; }
    public int SafeBlacks { get => CountSafeBlacks(); }
    public int SafeWhites { get => CountSafeWhites(); }
    public Pole LastMove { get => lastMove; set => lastMove = value; }
    public bool IsFinished { get => isFinished; set => isFinished = value; }

    public Board()
    {
        Table = new Pole[8, 8];

        CreatingNullPoles(Table);

        WhoNext = Pole.PoleStatus.Black;


        Table[3, 3].WhatInside = Pole.PoleStatus.White;
        Table[4, 4].WhatInside = Pole.PoleStatus.White;
        Table[3, 4].WhatInside = Pole.PoleStatus.Black;
        Table[4, 3].WhatInside = Pole.PoleStatus.Black;

        IsFinished = false;


    }

    private int CountWhites()
    {
        int counter = 0;

        foreach (var item in this.Table)
        {
            if(item.WhatInside==Pole.PoleStatus.White)
            {
                counter++;
            }
        }

        return counter;
    }

    private int CountBlacks()
    {
        int counter = 0;

        foreach (var item in this.Table)
        {
            if (item.WhatInside == Pole.PoleStatus.Black)
            {
                counter++;
            }
        }

        return counter;
    }

    private int CountSafeWhites()
    {
        int counter = 0;

        foreach (var item in this.Table)
        {
            if (item.WhatInside == Pole.PoleStatus.White
                &&
                item.IsSafe)
            {
                counter++;
            }
        }

        return counter;
    }

    private int CountSafeBlacks()
    {
        int counter = 0;

        foreach (var item in this.Table)
        {
            if (item.WhatInside == Pole.PoleStatus.Black
                &&
                item.IsSafe)
            {
                counter++;
            }
        }

        return counter;
    }

    void CreatingNullPoles(Pole[,] nothing)
    {
        for(int i=0;i<nothing.GetLength(0);i++)
        {
            for(int j=0;j<nothing.GetLength(1);j++)
            {
                nothing[i, j] = new Pole(i, j);
            }
        }
    }

    public void RefreshLook()
    {
        foreach(var item in this.Table)
        {
            string location = item.XVal + "x" + item.YVal;
            Transform toModify = GameObject.FindGameObjectWithTag(location).transform;

            //Positions.GoPossibleDown(toModify);
            //Positions.GoLastDown(toModify);
            
            switch (item.WhatInside)
            {
                case Pole.PoleStatus.White:
                    Positions.GoWhite(toModify);
                    break;
                case Pole.PoleStatus.Black:
                    Positions.GoBlack(toModify);
                    break;
                default:
                    break;

            }
        }

        if (this.LastMove != null&&Generals.onLastMove)
        {
            string location = this.LastMove.XVal + "x" + this.LastMove.YVal;
            Transform toModify = GameObject.FindGameObjectWithTag(location).transform;
            Positions.GoLastUp(toModify);
        }

        var possibilities = this.FindPossibles(this);

        if(possibilities.Count<1)
        {
            this.WhoNext =
                this.WhoNext == Pole.PoleStatus.White ? Pole.PoleStatus.Black:Pole.PoleStatus.White;
            possibilities = this.FindPossibles(this);

        }

        if (possibilities.Count > 0)
        {
            foreach (var item in possibilities)
            {
                Transform toModify = GameObject.FindGameObjectWithTag(item).transform;
                Positions.GoPossibleUp(toModify);
                //try to make invisible
                
                

                
            }
        }
        else
        {
            IsFinished = true;
            
        }

        



    }

    public List<string> FindPossibles(Board table)
    {
        var result = new List<string>();

        //Board toAnalyze = table;
        
        foreach(var item in table.Table)
        {
            string toAdd= IsPossible(item, table.Table,table.WhoNext);
            if (toAdd!=null)
            {
              result.Add(toAdd);
            }
        }

        return result;
        
    }

    public string IsPossible(Pole point, Pole[,] table,Pole.PoleStatus whonext)
    {
        if(!point.WhatInside.Equals(Pole.PoleStatus.None))
        {
            return null;
        }
        
        int xPoint = point.XVal;
        int yPoint = point.YVal;

        Pole.PoleStatus toFind = whonext == Pole.PoleStatus.Black ? Pole.PoleStatus.White: Pole.PoleStatus.Black;
        Pole.PoleStatus notWelcome = whonext == Pole.PoleStatus.Black ? Pole.PoleStatus.Black : Pole.PoleStatus.White;

        //up
        if (GoPossible(-1,0,xPoint,yPoint,toFind,notWelcome,table))
        {
            return xPoint + "x" + yPoint;
        }
        //up right
        if (GoPossible(-1,1,xPoint, yPoint, toFind, notWelcome, table))
        {
            return xPoint + "x" + yPoint;
        }
        //right
        if (GoPossible(0,1,xPoint, yPoint, toFind, notWelcome, table))
        {
            return xPoint + "x" + yPoint;
        }
        //down right
        if (GoPossible(1,1,xPoint, yPoint, toFind, notWelcome, table))
        {
            return xPoint + "x" + yPoint;
        }
        //down
        if (GoPossible(1,0,xPoint, yPoint, toFind, notWelcome, table))
        {
            return xPoint + "x" + yPoint;
        }
        //down left
        if (GoPossible(1,-1,xPoint, yPoint, toFind, notWelcome, table))
        {
            return xPoint + "x" + yPoint;
        }
        //left
        if (GoPossible(0,-1,xPoint, yPoint, toFind, notWelcome, table))
        {
            return xPoint + "x" + yPoint;
        }
        //up left
        if (GoPossible(-1,-1,xPoint, yPoint, toFind, notWelcome, table))
        {
            return xPoint + "x" + yPoint;
        }
        return null;

    }

    public int CountMobility(Board table)
    {
        int result = 0;

        //Board toAnalyze = table;

        foreach (var item in table.Table)
        {
            string toAdd = IsPossible(item, table.Table, Pole.PoleStatus.Black);
            if (toAdd != null)
            {
                result++;
            }
        }
        foreach (var item in table.Table)
        {
            string toAdd = IsPossible(item, table.Table, Pole.PoleStatus.White);
            if (toAdd != null)
            {
   
             result--;
            }
        }

        return result;

    }
    public int CountMobilityCombo(Board table)
    {
        int result = 0;

        //Board toAnalyze = table;

        foreach (var item in table.Table)
        {
            string toAddW = IsPossible(item, table.Table, Pole.PoleStatus.White);
            string toAdd = IsPossible(item, table.Table, Pole.PoleStatus.Black);
            if (toAdd != null)
            {
                result++;
            }
            else if(toAddW != null)
            {
                result--;
            }
        }

        //Debug.Log("Mobility:" + result);
        return result;

    }

    public int CountMobilityComboNew(Board table)
    {
        //int result = 0;

        //Board toAnalyze = table;

        int whites = 0;
        int blacks = 0;

        foreach (var item in table.Table)
        {
            string toAddW = IsPossible(item, table.Table, Pole.PoleStatus.White);
            string toAdd = IsPossible(item, table.Table, Pole.PoleStatus.Black);

            

            if (toAdd != null&&!item.IsSafe)
            {
                blacks++;
                //Debug.Log("whites and blacks   "+whites + "   " + blacks);
            }
            else if (toAddW != null && !item.IsSafe)
            {
                whites++;
            }
        }

        float result = 0;
        //Debug.Log(whites + "   " + blacks);
        if (blacks + whites == 0)
        {
            return 0;
        }
        else
        {
            result = 1f*(blacks - whites) / (blacks + whites) * 100f;
        }
        int corners = 20*CountCorners(table);

        result += corners;

        Debug.Log("Mobility:" + result);
        return (int)result+corners;

    }

    public int CountMobilityBlack(Board table)
    {
        int result = 0;

        //Board toAnalyze = table;

        foreach (var item in table.Table)
        {
            string toAdd = IsPossible(item, table.Table, Pole.PoleStatus.Black);
            if (toAdd != null)
            {
                result++;
            }
        }
        

        return result;

    }

    public int CountMobilityWhite(Board table)
    {
        int result = 0;

        //Board toAnalyze = table;

        
        foreach (var item in table.Table)
        {
            string toAdd = IsPossible(item, table.Table, Pole.PoleStatus.Black);
            if (toAdd != null)
            {
                result--;
            }
        }

        return result;

    }

    public bool GoPossible(int xIncrement,int yIncrement,int xPoint, int yPoint,
        Pole.PoleStatus toFind, Pole.PoleStatus notWelcome, Pole[,] table)
    {
        bool start = false;

        while (true)
        {
            xPoint += xIncrement;
            yPoint += yIncrement;

            if (xPoint < 0 || xPoint > 7 || yPoint < 0 || yPoint > 7)
            {
                return false;
            }

            Pole current = table[xPoint, yPoint];

            if (current.WhatInside == notWelcome && start == false)
            {
                return false;
            }

            if (current.WhatInside == toFind)
            {
                start = true;
            }

            if (current.WhatInside == notWelcome && start == true)
            {
                return true;
            }

            if (current.WhatInside == Pole.PoleStatus.None)
            {
                return false;
            }

        }
    }

    

    

    public void SetAndReverse(int x,int y)
    {
        //var numbers = position.Split('x');
        //int x = int.Parse(numbers[0]);
        //int y = int.Parse(numbers[1]);

        Pole.PoleStatus toFind = this.WhoNext == Pole.PoleStatus.Black ? Pole.PoleStatus.White : Pole.PoleStatus.Black;
        Pole.PoleStatus notWelcome = this.WhoNext == Pole.PoleStatus.Black ? Pole.PoleStatus.Black : Pole.PoleStatus.White;

        Pole current = this.Table[x, y];
        current.WhatInside = notWelcome;
        


        this.LastMove = current;
        

        var toReverse = new List<int[]>();
        //up
        toReverse.AddRange(FindToReverse(-1,0,current.XVal,current.YVal,toFind,notWelcome,this.Table));
        //up right
        toReverse.AddRange(FindToReverse(-1, 1, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //right
        toReverse.AddRange(FindToReverse(0, 1, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //down right
        toReverse.AddRange(FindToReverse(1, 1, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //down
        toReverse.AddRange(FindToReverse(1, 0, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //down left
        toReverse.AddRange(FindToReverse(1, -1, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //left
        toReverse.AddRange(FindToReverse(0, -1, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //up left
        toReverse.AddRange(FindToReverse(-1, -1, current.XVal, current.YVal, toFind, notWelcome, this.Table));

        

        foreach(var item in toReverse)
        {
            Pole currentPole = this.Table[item[0], item[1]];
            if (currentPole.WhatInside==Pole.PoleStatus.White)
            {
                currentPole.WhatInside = Pole.PoleStatus.Black;
            }
            else
            {
                currentPole.WhatInside = Pole.PoleStatus.White;
            }
        }

        MarkSafe();
        this.WhoNext =
   this.WhoNext == Pole.PoleStatus.White ? Pole.PoleStatus.Black : Pole.PoleStatus.White;
    }

    public List<int[]> FindToReverse(int xIncrement,int yIncrement,int xPoint,int yPoint, Pole.PoleStatus toFind, Pole.PoleStatus notWelcome, Pole[,] table)
    {
        bool start = false;
        var result = new List<int[]>();

        while (true)
        {
            xPoint += xIncrement;
            yPoint += yIncrement;

            if (xPoint < 0||xPoint>7||yPoint<0||yPoint>7)
            {
                result.Clear();
                return result;
            }

            Pole current = table[xPoint, yPoint];

            if (current.WhatInside == notWelcome && start == false)
            {
                result.Clear();
                return result;
            }

            if (current.WhatInside == toFind)
            {
                start = true;
                var item = new int[] { xPoint, yPoint };
                //Debug.Log(item[0]+"  "+item[1]+" direct: "+xIncrement+" "+yIncrement);
                result.Add(item);
            }

            if (current.WhatInside == notWelcome && start == true)
            {
                return result;
            }

            if (current.WhatInside == Pole.PoleStatus.None)
            {
                result.Clear();
                return result;
            }
    
        }
    }
    private bool CheckIfSafe(int x, int y)
    {
        Pole current = this.Table[x,y];
        if(current.WhatInside == Pole.PoleStatus.None)
        {
            return false;
        }
        int counter = 0;
        var directions = new int[,] { 
            { -1, 0 },//1
            { -1, 1 },//2
            { 0, 1 },//3
            { 1, 1 },//4
            { 1, 0 },//5
            { 1, -1 },//6
            { 0, -1 },//7
            { -1, -1 },//8

            { -1, 0 },//9
            { -1, 1 },//10
            { 0, 1 }//11
        };

        for(int i=0;i<11;i++)
        {
            int xToCheck = x + directions[i,0];
            int yToCheck = y + directions[i,1];

            if (xToCheck < 0 || xToCheck > 7 || yToCheck < 0 || yToCheck > 7)
            {
                counter++;
            }
            else if (
                (this.Table[xToCheck, yToCheck].WhatInside ==
                current.WhatInside
                &&
                this.Table[xToCheck, yToCheck].IsSafe)
                ||
                CheckIsFull(x,y,directions[i,0],directions[i,1])
                )
            {
                counter++;
            }
            else
            {
                counter = 0;
            }

            if (counter > 3)
            {
                //Debug.Log("counter" + counter);
                return true;
            }

        }

        return false;

    }

    private void MarkSafe()
    {
        for(int i=0;i<8;i++)
        {
            for(int j=0;j<8;j++)
            {
                Pole current = this.Table[i, j];
                if (current.IsSafe&&current.WhatInside!=Pole.PoleStatus.None)
                {
                    continue;
                }
                else if(CheckIfSafe(i,j))
                {
                    current.IsSafe = true;
                    //Debug.Log(i + "  " + j);
                }
            }

            for (int j = 7; j > -1; j--)
            {
                Pole current = this.Table[i, j];
                if (current.IsSafe)
                {
                    continue;
                }
                else if (CheckIfSafe(i, j))
                {
                    current.IsSafe = true;
                }
            }
        }

        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                Pole current = this.Table[i, j];
                if (current.IsSafe)
                {
                    continue;
                }
                else if (CheckIfSafe(i, j))
                {
                    current.IsSafe = true;
                }
            }

            for (int i = 7; i > -1; i--)
            {
                Pole current = this.Table[i, j];
                if (current.IsSafe)
                {
                    continue;
                }
                else if (CheckIfSafe(i, j))
                {
                    current.IsSafe = true;
                }
            }
        }
    }

    private bool CheckIsFull(int x, int y, int xVector, int yVector)
    {
        int a = x;
        int b = y;
        int counter = 0;

        while(true)
        {
            a += xVector;
            b += yVector;

            if(a>7||a<0||b>7||b<0)
            {
                break;
            }

            Pole toCheck = this.Table[a, b];
            if(toCheck.WhatInside==Pole.PoleStatus.None)
            {
                counter++;
            }

        }

        a = x;
        b = y;


        while (true)
        {
            a -= xVector;
            b -= yVector;

            if (a > 7 || a < 0 || b > 7 || b < 0)
            {
                break;
            }

            Pole toCheck = this.Table[a, b];
            if (toCheck.WhatInside == Pole.PoleStatus.None)
            {
                counter++;
            }

        }

        if(counter>0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public Board ShallowCopy()
    {
        return (Board)this.MemberwiseClone();
    }

    public Board DeepCopy()
    {
        Board temp = new Board();// (Board)this.MemberwiseClone();

        temp.whoNext=this.WhoNext;
        temp.LastMove=this.LastMove;
        temp.IsFinished=this.IsFinished;

        foreach(var item in temp.Table)
        {
            item.WhatInside = this.Table[item.XVal, item.YVal].WhatInside;
            item.IsSafe = this.Table[item.XVal, item.YVal].IsSafe;
        }
        return temp;
    }

    public bool IsStable(Pole point, Pole[,] table)//, Pole.PoleStatus whonext)
    {
        if (
            point.WhatInside.Equals(Pole.PoleStatus.None)
            ||
            point.IsSafe==true)
        {
            return false;
        }

        int xPoint = point.XVal;
        int yPoint = point.YVal;

        //Pole.PoleStatus toFind = whonext == Pole.PoleStatus.Black ? Pole.PoleStatus.White : Pole.PoleStatus.Black;
        //Pole.PoleStatus notWelcome = whonext == Pole.PoleStatus.Black ? Pole.PoleStatus.Black : Pole.PoleStatus.White;

        //up
        if (GoStable(-1, 0, xPoint, yPoint, table))
        {
            return true;
        }
        //up right
        if (GoStable(-1, 1, xPoint, yPoint, table))
        {
            return true;
        }
        //right
        if (GoStable(0, 1, xPoint, yPoint, table))
        {
            return true;
        }
        //down right
        if (GoStable(1, 1, xPoint, yPoint, table))
        {
            return true;
        }
        //down
        if (GoStable(1, 0, xPoint, yPoint, table))
        {
            return true;
        }
        //down left
        if (GoStable(1, -1, xPoint, yPoint, table))
        {
            return true;
        }
        //left
        if (GoStable(0, -1, xPoint, yPoint, table))
        {
            return true;
        }
        //up left
        if (GoStable(-1, -1, xPoint, yPoint, table))
        {
            return true;
        }
        return false;

    }

    public bool GoStable(int xIncrement, int yIncrement, int xPoint, int yPoint,
         Pole[,] table)  //Pole.PoleStatus toFind, Pole.PoleStatus notWelcome,
    {
        //bool start = false;

        
            xPoint += xIncrement;
            yPoint += yIncrement;

            if (xPoint < 0 || xPoint > 7 || yPoint < 0 || yPoint > 7)
            {
                return false;
            }

            Pole current = table[xPoint, yPoint];

            if (current.WhatInside == Pole.PoleStatus.None)
            {
                return true;
            }

            return false;

        
    }

    public int CountStability(Board table)
    {
        int result = 0;

        //Board toAnalyze = table;

        foreach (var item in table.Table)
        {
            if(IsStable(item,table.Table))
            {
                result+=item.WhatInside == Pole.PoleStatus.Black ? 1 : -1;
            }

        }
        //Debug.Log("Stability is " + result);
        return result;

    }
    public int CountScore(Board table)
    {
        int result = 0;

        int safe = table.SafeBlacks - table.SafeWhites;
        int mobility = table.CountMobilityCombo(table);
        int stability = table.CountStability(table);
        int corners = table.CountCorners(table);
        int xses = table.CountX(table);

        result =
            (Generals.safeCoeff * safe)
            +
            (Generals.mobilityCoeff * mobility)
            -
            (Generals.stabilityCoeff * stability)
            +
            (Generals.cornerCoeff * corners)
            -
            (Generals.xCoeff * xses);
        if(table.Blacks+table.Whites>156)
        {
            result = table.Blacks - table.Whites;
        }


        //Debug.Log("Score is " + result);
        return result;
    }

    public int CountScore(Board table,Pole.PoleStatus opponent)
    {
        int result = 0;

        int safe = table.SafeBlacks - table.SafeWhites;
        int mobility = table.CountMobilityCombo(table);
        int stability = table.CountStability(table);
        int corners = table.CountCorners(table);
        int xses = table.CountX(table);

        result =
            (Generals.safeCoeff * safe)
            +
            (Generals.mobilityCoeff * mobility)
            -
            (Generals.stabilityCoeff * stability)
            +
            (Generals.cornerCoeff * corners)
            -
            (Generals.xCoeff * xses);
        if (table.IsFinished)
        {
            if(table.Blacks >table.Whites)
            {
                result += 1000;
            }
            else if(table.Blacks < table.Whites)
            {
                result -= 1000;
            }
            else
            {
                result = 0;
            }
        }

        if(opponent==Pole.PoleStatus.Black)
        {
            result = -result;
        }


        //Debug.Log("Score is " + result);
        return result;
    }

    public int CountScorePos(Board table, Pole.PoleStatus opponent)
    {
        int result = 0;

        int safe = table.SafeBlacks - table.SafeWhites;
        int mobility = table.CountMobilityComboNew(table);
        int stability = table.CountStability(table);
        int corners = table.CountPositional(table);
        int xses = table.CountX(table);

        result =
            (Generals.safeCoeff * safe)
            +
            (Generals.mobilityCoeff * mobility)
            -
            (Generals.stabilityCoeff * stability)
            +
            (Generals.cornerCoeff * corners);
            //-
            //(Generals.xCoeff * xses);
        if (table.IsFinished)
        {
            if (table.Blacks > table.Whites)
            {
                result += 20000;
            }
            else if (table.Blacks < table.Whites)
            {
                result -= 20000;
            }
            else
            {
                result = 0;
            }
        }

        if (opponent == Pole.PoleStatus.Black)
        {
            result = -result;
        }


        Debug.Log("Score is " + result);
        return result;
    }

    public int CountCorners(Board board)
    {
        var corners = new int[,] { { 0, 0 }, { 0, 7 }, { 7, 0 }, { 7, 7 } };
        int result = 0;
        
        for(int i=0;i<4;i++)
        {
            var currentPole = board.table[corners[i,0], corners[i, 1]];
            
            if (currentPole.WhatInside!=Pole.PoleStatus.None)
            {
                int toAdd = currentPole.WhatInside 
                    == Pole.PoleStatus.Black ? 1 : -1;
                result += toAdd;
            }
        }

        return result;
    }

    public int CountPositional(Board board)
    {
        var corners = new int[,] 
        { 
            //line 0
            { 0, 0,100 }, 
            { 0, 1,-20 }, 
            { 0, 2, 10}, 
            { 0, 3, 5},
            { 0, 4, 5},
            { 0, 5, 10},
            { 0, 6, -20},
            { 0, 7, 100},
            //line 1
            { 1, 0,-20 },
            { 1, 1,-50 },
            { 1, 2, -2},
            { 1, 3, -2},
            { 1, 4, -2},
            { 1, 5, -2},
            { 1, 6, -50},
            { 1, 7, -20},
            //line 2
            { 2, 0,10 },
            { 2, 1,-2 },
            { 2, 2, -1},
            { 2, 3, -1},
            { 2, 4, -1},
            { 2, 5, -1},
            { 2, 6, -2},
            { 2, 7, 10},
            { 2, 0,10 },
            //line 3
            { 3, 0, 5},
            { 3, 1,-2 },
            { 3, 2, -1},
            { 3, 3, -1},
            { 3, 4, -1},
            { 3, 5, -1},
            { 3, 6, -2},
            { 3, 7, 5},
            //line 4
            { 4, 0, 5},
            { 4, 1,-2 },
            { 4, 2, -1},
            { 4, 3, -1},
            { 4, 4, -1},
            { 4, 5, -1},
            { 4, 6, -2},
            { 4, 7, 5},
            //line 5
            { 5, 0,10 },
            { 5, 1,-2 },
            { 5, 2, -1},
            { 5, 3, -1},
            { 5, 4, -1},
            { 5, 5, -1},
            { 5, 6, -2},
            { 5, 7, 10},
            { 5, 0,10 },
            //line 6
            { 6, 0,-20 },
            { 6, 1,-50 },
            { 6, 2, -2},
            { 6, 3, -2},
            { 6, 4, -2},
            { 6, 5, -2},
            { 6, 6, -50},
            { 6, 7, -20},
            //line 7
            { 7, 0,100 },
            { 7, 1,-20 },
            { 7, 2, 10},
            { 7, 3, 5},
            { 7, 4, 5},
            { 7, 5, 10},
            { 7, 6, -20},
            { 7, 7, 100}
             };
        int result = 0;

        for (int i = 0; i < 64; i++)
        {
            var currentPole = board.table[corners[i, 0], corners[i, 1]];

            if (currentPole.WhatInside != Pole.PoleStatus.None)
            {
                int toAdd = currentPole.WhatInside
                    == Pole.PoleStatus.Black ? corners[i, 2] : -corners[i, 2];
                result += toAdd;
            }
        }

        return result;
    }

    public int CountX(Board board)
    {
        var corners = new int[,] { { 0, 0 }, { 0, 7 }, { 7, 0 }, { 7, 7 } };
        var xPoles = new int[,] { { 1, 1 }, { 1, 6 }, { 6, 1 }, { 6, 6 } };


        int result = 0;

        for (int i = 0; i < 4; i++)
        {
            var currentX= board.table[xPoles[i, 0], xPoles[i, 1]];
            var currentPole = board.table[corners[i, 0], corners[i, 1]];

            if (currentPole.WhatInside == Pole.PoleStatus.None)
            {
                if (currentX.WhatInside != Pole.PoleStatus.None)
                {
                    int toAdd = currentX.WhatInside
                    == Pole.PoleStatus.Black ? 1 : -1;
                    result += toAdd;
                }
                
            }
        }

        return result;
    }


    public async void SetToRotate(int x, int y,Pole.PoleStatus status)
    {
        //var numbers = position.Split('x');
        //int x = int.Parse(numbers[0]);
        //int y = int.Parse(numbers[1]);
        if(!Generals.onAnimation)
        {
            return;
        }

        Pole.PoleStatus toFind = this.WhoNext == Pole.PoleStatus.Black ? Pole.PoleStatus.White : Pole.PoleStatus.Black;
        Pole.PoleStatus notWelcome = this.WhoNext == Pole.PoleStatus.Black ? Pole.PoleStatus.Black : Pole.PoleStatus.White;

        Pole current = this.Table[x, y];
        //current.WhatInside = notWelcome;



        //this.LastMove = current;


        var toReverse = new List<int[]>();
        //up
        toReverse.AddRange(FindToReverse(-1, 0, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //up right
        toReverse.AddRange(FindToReverse(-1, 1, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //right
        toReverse.AddRange(FindToReverse(0, 1, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //down right
        toReverse.AddRange(FindToReverse(1, 1, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //down
        toReverse.AddRange(FindToReverse(1, 0, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //down left
        toReverse.AddRange(FindToReverse(1, -1, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //left
        toReverse.AddRange(FindToReverse(0, -1, current.XVal, current.YVal, toFind, notWelcome, this.Table));
        //up left
        toReverse.AddRange(FindToReverse(-1, -1, current.XVal, current.YVal, toFind, notWelcome, this.Table));

        Generals.delay = toReverse.Count * 350 + 1000;
        int counter = 0;

        foreach (var item in toReverse)
        {
            string location = item[0] + "x" + item[1];
            Transform toModify = GameObject.
                FindGameObjectWithTag(location).transform;
            foreach (Transform eachChild in toModify)
            {
                if (eachChild.name == "Pawn")
                {
                    //if(eachChild.rotation.x==0)
                    //Debug.Log("Child found. ...");
                    //Positions.RotatePawn(eachChild.transform);
                    eachChild.gameObject.AddComponent<Animator>();
                    Animator animator = eachChild.gameObject.GetComponent<Animator>();
                    animator.runtimeAnimatorController = Generals.anime;

                    if (status==Pole.PoleStatus.White)
                    {
                        animator.SetTrigger("makeRead");
                        //await Task.Delay(200);
                        //StartCoroutine(DelayReverseSound(0.0f, counter++));
                        //SoundManager.PlayReverse(counter++);

                    }
                    else
                    {
                        animator.SetTrigger("makegreen");
                        
                        //StartCoroutine(DelayReverseSound(0.0f, counter++));
                        //await Task.Delay(200);
                        //SoundManager.PlayReverse(counter++);


                    }
                    await Task.Delay(350);
                    SoundManager.PlayReverse(counter++);

                }
                
               
            }
        }

        
    }

    static IEnumerator DelayReverseSound(float Count,int counter)
    {
        SceneManager.LoadScene("Menu");

        yield return new WaitForSeconds(Count); //Count is the amount of time in seconds that you want to wait.
        
        //SceneManager.LoadScene("Menu");
        SoundManager.PlayReverse(counter);                                       //And here goes your method of resetting the game...
        //Debug.Log("success");
        yield return null;
    }

    public void CurrentCleaning()
    {
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);
        foreach (var item in rootObjects)
        {
            foreach (Transform eachChild in item.transform)
            {

                if (eachChild.name == "Possible")
                {
                    Positions.GoPossibleDown(item.transform);
                    //SoundManager.PlayReverse();
                }
                else if (eachChild.name == "LastMove")
                {
                    Positions.GoLastDown(item.transform);
                    
                }

            }
        }
    }

    internal string BlackPlayer()
    {
        if(Generals.isHuman)
        {
            return "HUMAN";
        }
        else if(Generals.isYourBlack)
        {
            return "HUMAN";
        }
        else
        {
            return "CPU " + Generals.difficulty;
        }
    }

    internal string WhitePlayer()
    {
        if (Generals.isHuman)
        {
            return "HUMAN";
        }
        else if (!Generals.isYourBlack)
        {
            return "HUMAN";
        }
        else
        {
            return "CPU " + Generals.difficulty;
        }
    }

    internal void HideCoordintates()
    {
        var objects = GameObject.FindGameObjectsWithTag("coo");

        foreach(var item in objects)
        {
            item.SetActive(false);
        }
    }

    public int CountMobilityForAI(Board table)
    {
        
        int result = 0;
        int adjustment = 0;
        int toEnd = 64 - table.Blacks - table.Whites;

        if (table.Blacks+table.Whites<30)
        {
            return 5+adjustment;
        }

        //Board toAnalyze = table;

        foreach (var item in table.Table)
        {
            string toAdd = IsPossible(item, table.Table, Pole.PoleStatus.Black);
            if (toAdd != null)
            {
                result++;
            }
        }
        foreach (var item in table.Table)
        {
            string toAdd = IsPossible(item, table.Table, Pole.PoleStatus.White);
            if (toAdd != null)
            {

                result++;
            }
        }

        int toCompare = result / 2;

        if (toCompare > 10)
        {
            return Math.Min(4+adjustment,toEnd);
        }
        else if (toCompare>8)
        {
            return Math.Min(5 + adjustment, toEnd);
        }
        else if(toCompare > 6)
        {
            return Math.Min(6 + adjustment, toEnd);
        }
        else if (toCompare > 5)
        {
            return Math.Min(7 + adjustment, toEnd);
        }
        else if (toCompare > 4)
        {
            return Math.Min(8 + adjustment, toEnd);
        }
        else if (toCompare >3 )
        {
            return Math.Min(9 + adjustment, toEnd);
        }
        else if (toCompare > 2)
        {
            return Math.Min(11 + adjustment, toEnd);
        }

        return Math.Min(12 + adjustment, toEnd);

    }

    public static List<Board> CreateNewPossibilities(Board table)
    {
        //var result = new List<string>();
        var result = new List<Board>();

        //Board toAnalyze = table;

        foreach (var item in table.Table)
        {
            int[] toAdd = table.IsPossibleInt(item, table.Table, table.WhoNext);
            if (toAdd != null)
            {
                var newBoard = table.DeepCopy();
                newBoard.SetAndReverse(toAdd[0], toAdd[1]);
                result.Add(newBoard);
            }
        }


        return result;

    }

    public int[] IsPossibleInt(Pole point, Pole[,] table, Pole.PoleStatus whonext)
    {
        int[] result = new int[2];

        if (!point.WhatInside.Equals(Pole.PoleStatus.None))
        {
            return null;
        }



        int xPoint = point.XVal;
        int yPoint = point.YVal;

        Pole.PoleStatus toFind = whonext == Pole.PoleStatus.Black ? Pole.PoleStatus.White : Pole.PoleStatus.Black;
        Pole.PoleStatus notWelcome = whonext == Pole.PoleStatus.Black ? Pole.PoleStatus.Black : Pole.PoleStatus.White;

        //up
        if (GoPossible(-1, 0, xPoint, yPoint, toFind, notWelcome, table))
        {
            result[0] = xPoint;
            result[1] = yPoint;
            return result;
        }
        //up right
        if (GoPossible(-1, 1, xPoint, yPoint, toFind, notWelcome, table))
        {
            result[0] = xPoint;
            result[1] = yPoint;
            return result;
        }
        //right
        if (GoPossible(0, 1, xPoint, yPoint, toFind, notWelcome, table))
        {
            result[0] = xPoint;
            result[1] = yPoint;
            return result;
        }
        //down right
        if (GoPossible(1, 1, xPoint, yPoint, toFind, notWelcome, table))
        {
            result[0] = xPoint;
            result[1] = yPoint;
            return result;
        }
        //down
        if (GoPossible(1, 0, xPoint, yPoint, toFind, notWelcome, table))
        {
            result[0] = xPoint;
            result[1] = yPoint;
            return result;
        }
        //down left
        if (GoPossible(1, -1, xPoint, yPoint, toFind, notWelcome, table))
        {
            result[0] = xPoint;
            result[1] = yPoint;
            return result;
        }
        //left
        if (GoPossible(0, -1, xPoint, yPoint, toFind, notWelcome, table))
        {
            result[0] = xPoint;
            result[1] = yPoint;
            return result;
        }
        //up left
        if (GoPossible(-1, -1, xPoint, yPoint, toFind, notWelcome, table))
        {
            result[0] = xPoint;
            result[1] = yPoint;
            return result;
        }
        return null;

    }

    public override bool Equals(object board1)//, Board board2)
    {
        Board toCompare = (Board)board1;
        for (int i=0;i<8;i++)
        {
            for(int j=0;j<8;j++)
            {
                if(this.Table[i,j].WhatInside!=toCompare.Table[i,j].WhatInside)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

