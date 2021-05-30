using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using UnityEngine;


public class Node
    {

        static System.Random rnd = new System.Random();
        static int min = -10000;
        static int max = 10000;
        public static Stack<Node> many = new Stack<Node>();

        int alpha;
        int beta;
        bool isAlpha;
        Node parent;
        List<Board> possibilities;
        List<Node> children;
        int evaluation;
        int target;
        int current;
        int depth;
        Board nodeBoard;


        public int Alpha { get => alpha; set => alpha = value; }
        public int Beta { get => beta; set => beta = value; }
        public bool IsAlpha { get => isAlpha; set => isAlpha = value; }
        public List<Board> Possibilities { get => possibilities; set => possibilities = value; }
        public List<Node> Children { get => children; set => children = value; }
        public int Depth { get => depth; set => depth = value; }
        public Node Parent { get => parent; set => parent = value; }
        public int Target { get => target; set => target = value; }
        public int Current { get => current; set => current = value; }
        public int Evaluation { get => evaluation; set => evaluation = value; }
        public Board NodeBoard { get => nodeBoard; set => nodeBoard = value; }

        public Node(int deep)
        {
            this.Alpha = Node.min;
            this.Beta = Node.max;
            this.IsAlpha = true;
            this.Possibilities = new List<Board>();
            this.Children = new List<Node>();
            this.Depth = deep;
            this.Parent = null;

            this.Evaluation = this.IsAlpha ? Node.min : Node.max;

            this.Target = 0;
            this.Current = 0;
        }


        public static Node CreateNode(Node parent, Board possibility)
        {

            var newNode = new Node(parent.Depth - 1);

            parent.Children.Add(newNode);
            //add to parent target????

            newNode.Alpha = parent.Alpha;
            newNode.Beta = parent.Beta;
            newNode.IsAlpha = parent.IsAlpha ? false : true;
            newNode.Possibilities = Board.CreateNewPossibilities(possibility);
            //newNode.Children = new List<Node>(); just created before

            newNode.Parent = parent;

            newNode.Evaluation = newNode.IsAlpha ? Node.min : Node.max;

            newNode.Target = newNode.Possibilities.Count;
            newNode.Current = 0;
            newNode.NodeBoard = possibility;

            Node.many.Push(newNode);


            return newNode;

        }


        public void CalculateEvaluation()
        {
            int result = this.NodeBoard.CountScore(this.NodeBoard);


            this.Evaluation = result;
            if (this.Parent != null)
            {
                this.Parent.UpdateByChild(this);
            }

        }

        //public static List<Board> CreateNewPossibilities(Board possibility)
        //{
        //    var list = new List<int>();
        //    list.Add(possibility * 10 + 1);
        //    list.Add(possibility * 10 + 2);
        //    return list;
        //}

        public void UpdateByChild(Node child)
        {
            var childEvaluation = child.Evaluation;
            if (this.IsAlpha)
            {
                this.Evaluation = Math.Max(this.Evaluation, childEvaluation);
                this.Alpha = Math.Max(this.Alpha, this.Evaluation);
            }
            else
            {
                this.Evaluation = Math.Min(this.Evaluation, childEvaluation);
                this.Beta = Math.Min(this.Beta, this.Evaluation);
            }
            this.Children.Remove(child);
            this.Current += 1;

        }

        public override string ToString()
        {
            string result = "IsA: " + this.IsAlpha +
                " A: " + this.Alpha + " B: " + this.Beta +
                " E: " + this.Evaluation + " T: " + this.Target +
                " C: " + this.Current + " D: " + this.Depth;
            return result;
        }

        public Board TakePossibility(Node node)
        {
            if (node.Possibilities.Count > 0)
            {
                Board possibility = node.Possibilities[0];
                node.Possibilities.Remove(possibility);
                return possibility;
            }

            return null;

        }


        public static string CalculateRoot(Board table, int depth)
        {
            int counter = 0;
            char further = 'y';

            var firstGen = new Dictionary<Board, int>();
            var boardLast = new Dictionary<Pole, Node>();

            Node begin = new Node(depth);
            var beforeSort= Board.CreateNewPossibilities(table);

        foreach (var item in beforeSort)
        {
            
            Debug.Log("Last move "+item.LastMove.XVal+"  "+item.LastMove.YVal);
        }

        foreach (var item in beforeSort)
            {
                int outcome = item.CountScore(item);
                firstGen.Add(item, outcome);
            Debug.Log("Success firstGen");
            }

            var afterSort = from entry in firstGen orderby entry.Value descending select entry.Key;

            foreach( var item in afterSort)
        {
            begin.Possibilities.Add(item);
        }

            //begin.Possibilities = (List<Board>)afterSort;//Board.CreateNewPossibilities(table);

            begin.Target = begin.Possibilities.Count;
            //Console.WriteLine(begin.Alpha + "   " + begin.Beta);

            Node.many.Push(begin);

        int peeks = 0;

            while (Node.many.Count > 0)
            {
                var current = Node.many.Peek();
                if(++peeks%5000==0)
            {
                Debug.Log("peeks: " + peeks);
            }


                if (current.Parent == null & current.Current == current.Target)
                {
                    //Console.WriteLine("End: ");
                    //Console.WriteLine(current.ToString());
                    break;

                }
                else if (current.Alpha >= current.Beta || current.Target == current.Current)
                {
                    //if (current.Alpha >= current.Beta)
                    //{
                    //    Console.Write("A>=B!!! ");
                    //}
                    //Console.WriteLine("Update: ");
                    current.Parent.UpdateByChild(current);
                    var toRemove = Node.many.Pop();
                    toRemove = null;//buba
                }
                else if (current.Target == 0 || current.Depth == 0)
                {
                    //Console.WriteLine("Evaluation: ");
                    current.CalculateEvaluation();
                    var toRemove = Node.many.Pop();
                    toRemove = null;//buba
            }
                else
                {
                    //Console.WriteLine("Creation: ");
                    Board toCreateNode = current.TakePossibility(current);
                    Node newNode = Node.CreateNode(current, toCreateNode);
                    if (newNode.Depth==depth-1)
                    {
                   // Debug.Log("ChildAdded");
                    boardLast.Add(toCreateNode.LastMove, newNode);
                    }

                }

                //Console.WriteLine((counter++) + "\t" + current.ToString());
                //char pass = Console.ReadKey().KeyChar;
                //if (pass != further)
                //{
                //    break;
                //}
            }

            //var boardLast = new Dictionary<Pole, Node>();
            var filtered= from entry in boardLast orderby entry.Value.Evaluation descending select entry.Key;
            List<Pole> diamonds = new List<Pole>(filtered);
            //(List<Pole>)filtered;
            string result = diamonds[0].XVal + "x" + diamonds[0].YVal;
            return result;
        }

    }


