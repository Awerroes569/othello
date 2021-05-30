using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole 
{
    public enum PoleStatus { Black,White,None}

    private int xVal;
    private int yVal;
    private PoleStatus whatInside;
    private bool isSafe;

    public int XVal { get => xVal; set => xVal = value; }
    public int YVal { get => yVal; set => yVal = value; }
    public PoleStatus WhatInside { get => whatInside; set => whatInside = value; }
    public bool IsSafe { get => isSafe; set => isSafe = value; }

    public Pole(int x, int y)
    {
        XVal = x;
        YVal = y;
        WhatInside = PoleStatus.None;
        IsSafe = false;
    }

    
}
