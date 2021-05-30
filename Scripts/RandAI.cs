using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RandAI:MonoBehaviour
{
    public static System.Random rnd = new System.Random();

    public static int possibilitiesBase = 10;
    public static int iterationBase = 500;
    public static int depthBase = 8;
    public static int minDepth = 6;
    public static int maxDepth = 16;

    public static int CalculateFromDifficulty()
    {
        switch (Generals.difficulty)
        {
            case 0:
                return 50;
            case 1:
                return 100;
            case 2:
                return 200;
            case 3:
                return 300;
            case 4:
                return 400;
            case 5:
                return 500;
            case 6:
                return 600;
            case 7:
                return 700;
            case 8:
                return 800;
            case 9:
                return 1000;
            case 10:
                return 2000;
            default:
                break;
        }

        return 500;
    }

    public static int[] CalculateParameters(int possibilities,int toEnd)
    {
        var result = new int[2];

        iterationBase = CalculateFromDifficulty();

        int abstractDepth = (depthBase * possibilitiesBase) / possibilities;

        int realDepth = Math.Max(Math.Min(Math.Min(abstractDepth, toEnd), maxDepth),minDepth);

        float excess = 1f*Math.Max(0, abstractDepth - Math.Min(toEnd, maxDepth))/abstractDepth;

        float iterationReal = iterationBase * (1 + excess);

        int iterationFinal = (int)iterationReal;

        result[0] = iterationFinal;
        result[1] = realDepth;

        return result;
    }

    public static int EstimateScenario(Board oldBoard,int counter)
    {
        int result=0;
        //int counter = 8;

        Pole.PoleStatus opponent = oldBoard.WhoNext;

        var board = oldBoard.DeepCopy();

        while (counter>0)
        {

            if (board.IsFinished)
            {
                result = board.CountScorePos(board,opponent);
                return result;
            }

            var list = board.FindPossibles(board);

            if (list.Count < 1)
            {
                board.WhoNext =
                    board.WhoNext == Pole.PoleStatus.White ? Pole.PoleStatus.Black : Pole.PoleStatus.White;
                list = board.FindPossibles(board);
            }
            if (list.Count < 1)
            {
                result = board.CountScorePos(board,opponent);
                return result;
            }

            int number = rnd.Next(1, list.Count + 1);
            string toGo = list[number-1];
            string[] coordinates = toGo.Split('x');
            board.SetAndReverse(int.Parse(coordinates[0]), int.Parse(coordinates[1]));

            counter--;
        }
        result = board.CountScorePos(board,opponent);
        return result;
    }

    public static float CalculateScenarios(Board oldBoard,int total,int depth)
    {
        var board = oldBoard.DeepCopy();

        //int total = 3000;
        int sum = 0;

        var list = new List<int>();

        for(int i=0;i<total;i++)
        {
            list.Add(EstimateScenario(board,depth));
        }

        var sortedList= list.OrderByDescending(x => x);

        int toRemoveFront = total * Generals.removeFront / 100;
        int toRemoveEnd = total * Generals.removeEnd / 100;

        for (int i = 0; i < total; i++)
        {
            if (i > toRemoveFront && i < total - toRemoveEnd)
            {
                //Debug.Log(sortedList.ElementAt(i));
                sum += sortedList.ElementAt(i);
            }
        }

        float result = sum / (float)(total-Generals.removeFront-Generals.removeEnd);

        return result;

    }

    public static string ChooseBestMove(Board oldBoard,int toEnd)
    {
        //Generals.isThinking = true;
        Debug.Log("Start real thinking");

        List<string> possibles = oldBoard.FindPossibles(oldBoard);
        if(possibles.Count<1)
        {
            return "no move";
        }
        else if(possibles.Count==1)
        {
            return possibles[0];
        }

        var parameters = CalculateParameters(possibles.Count, toEnd);

        var results = new Dictionary<int[], float>();

        foreach(var item in possibles)
        {
            var board = oldBoard.DeepCopy();
            
            string[] coordinates = item.Split('x');
            board.SetAndReverse(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
            float result = CalculateScenarios(board,parameters[0],parameters[1]);
            var key = new int[2];
            key[0] = int.Parse(coordinates[0]);
            key[1]=int.Parse(coordinates[1]);
            results.Add(key, result);
        }

        var sortedResults = from entry in results orderby entry.Value ascending select entry;

        string best = sortedResults.Last().Key[0].ToString() + "x" +
            sortedResults.Last().Key[1].ToString();// +"   "+
                                                   //sortedResults.Last().Value.ToString();

        Debug.Log(best + "  " + sortedResults.Last().Value.ToString());
        //Generals.isThinking = false;
        Debug.Log("End thinking");
        return best; 
    }


}
