using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Creature : MonoBehaviour
{

    public CreatureData data;
    public List<GameObject> Nodes = new List<GameObject>();
    public List<SpringJoint> Muscles = new List<SpringJoint>();
    public int DataNodeAmount;
    public int DataMusculeAmount;
    private float CurrentTime = 0f;
    


    void Start()
    {
        
    }





    void Update()
    {
        
    }


}

