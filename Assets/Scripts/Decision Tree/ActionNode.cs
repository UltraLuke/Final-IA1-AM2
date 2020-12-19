using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : INode
{
    Action actionn;// HACER CON ACTION

    public delegate void myDelegate();
    myDelegate action;

    public ActionNode(myDelegate action)
    {
        this.action = action;
    }

    public void Excecute()
    {
        action();
    }



}
