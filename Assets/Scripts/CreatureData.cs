using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CreatureData
{

    public float fitness=0;
    public List<Node> Nodes = new List<Node>();
    public List<Muscule> Muscles = new List<Muscule>();

    
}







public class Node
{
    public Node(Vector3 Position, float Mass, float Friction)
    {
        NodePosition = Position;
        NodeMass = Mass;
        NodeFriction = Friction;
    }

    public Node() { }
    public Vector3 NodePosition;
    public float NodeMass;
    public float NodeFriction;
    //public int ID;
}


public class Muscule
{
    public Muscule(int FirstNodeID, int SecondNodeID, float strenght, float contractedLenght, float extendedLenght, float contractedTime, float extendedTime)
    {
        NodeOneID = FirstNodeID;
        NodeTwoID = SecondNodeID;
        Strength = strenght;
        ContractedLength = contractedLenght;
        ExtendedLength = extendedLenght;
        ContractedTime = contractedTime;
        ExtendedTime = extendedTime;
    }

    public Muscule() { }
    public int NodeOneID;
    public int NodeTwoID;
    public float Strength;
    public float ContractedLength;
    public float ExtendedLength;
    public float ContractedTime;
    public float ExtendedTime;
}

