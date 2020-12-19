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

    public void Excecute()
    {
        if (question())
        {
            //lo que debo hacer si es positivo 
            trueNode.Excecute();
        }
        else
        {
            falseNode.Excecute();
            //lo que debo hacer si es negativo
        }
    }

}
