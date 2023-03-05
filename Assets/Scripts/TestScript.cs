using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {


    public bool Create = false;
    public bool Test = false;
    public bool TestAll = false;
    public CreatureData data = new CreatureData();

    void Start () {

        
        

        


    }
	
	



	void Update () {
        if(Create)
        {
            Create = false;
            CreatureManager.instance.CreateCreature(CreatureManager.instance.GenerateRandomCreatureData());
            
        }


        if (Test)
        {
            Test = false;
            CreatureManager.instance.InitializeSim(CreatureManager.instance.GenerateRandomCreatureData());

        }


        if (TestAll)
        {
            TestAll = false;
            for(int i = 0; i <CreatureManager.instance.CreatureAmount; i++)
            {
                CreatureData RandomData = new CreatureData();
                RandomData = CreatureManager.instance.GenerateRandomCreatureData();
                CreatureManager.instance.currentGeneration.Add(RandomData);
            }



            CreatureManager.instance.TestAllCreatures();

        }

    }
}
