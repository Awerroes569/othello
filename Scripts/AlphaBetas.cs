using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AlphaBetas
{
    public static System.Random rnd = new System.Random();

    public static int EstimateScenario(Board oldBoard)
    {
        int result = 0;
        int counter = 12;

        var board = oldBoard.DeepCopy();

        while (counter > 0)
        {

            if (board.IsFinished)
            {
                result = board.CountScore(board);
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
                result = result = board.CountScore(board);
                return result;
            }

            int number = rnd.Next(1, list.Count + 1);
            string toGo = list[number - 1];
            string[] coordinates = toGo.Split('x');
            board.SetAndReverse(int.Parse(coordinates[0]), int.Parse(coordinates[1]));

            counter--;
        }
        result = result = board.CountScore(board);
        return result;
    }

    public static float CalculateScenarios(Board oldBoard)
    {
        var board = oldBoard.DeepCopy();

        int total = 1000;
        int sum = 0;

        var list = new List<int>();

        for (int i = 0; i < total; i++)
        {
            list.Add(EstimateScenario(board));
        }

        var sortedList = list.OrderByDescending(x => x);

        for (int i = 0; i < total; i++)
        {
            if (i > Generals.removeFront && i < total - Generals.removeEnd)
            {
                //Debug.Log(sortedList.ElementAt(i));
                sum += sortedList.ElementAt(i);
            }
        }

        float result = sum / (float)(total - Generals.removeFront - Generals.removeEnd);

        return result;

    }

    public static string ChooseBestMove(Board oldBoard)
    {
        List<string> possibles = oldBoard.FindPossibles(oldBoard);
        if (possibles.Count < 1)
        {
            return "no move";
        }

        var results = new Dictionary<int[], float>();

        foreach (var item in possibles)
        {
            var board = oldBoard.DeepCopy();

            string[] coordinates = item.Split('x');
            board.SetAndReverse(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
            float result = CalculateScenarios(board);
            var key = new int[2];
            key[0] = int.Parse(coordinates[0]);
            key[1] = int.Parse(coordinates[1]);
            results.Add(key, result);
        }

        var sortedResults = from entry in results orderby entry.Value ascending select entry;

        string best = sortedResults.Last().Key[0].ToString() + "x" +
            sortedResults.Last().Key[1].ToString();// +"   "+
                                                   //sortedResults.Last().Value.ToString();

        Debug.Log(best + "  " + sortedResults.Last().Value.ToString());
        return best;
    }


}
