using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : INode
{
    public delegate bool myDelegate();
    myDelegate question;
    INode trueNode, falseNode;

    public QuestionNode(myDelegate question, INode trueNode, INode falseNode)
    {
        this.trueNode = trueNode;
        this.falseNode = falseNode;
        this.question = question;
    }

    public void Execute()
    {
        if (question())
        {
            //lo que debo hacer si es positivo 
            trueNode.Execute();
        }
        else
        {
            falseNode.Execute();
            //lo que debo hacer si es negativo
        }
    }

}
