using System.Collections.Generic;
using UnityEngine;



public class CreatureManager : MonoBehaviour
{
    

    public static CreatureManager instance;

    //[SerializeField]
    public List<CreatureData> currentGeneration = new List<CreatureData>();
    //[SerializeField]
    public List<CreatureData> previousGeneration = new List<CreatureData>();

    

    public GameObject nodePrefab;
    public GameObject nodeNoRendererPrefab;
    public GameObject musculeRendererPrefab;

    public GameObject CameraRig;

    public UnityEngine.UI.Text GenerationIndicator;
    public UnityEngine.UI.Text CreatureIndicator;
    public UnityEngine.UI.Image CreatureProgressBar;
    public int GenerationNumber=0;

    


    #region sim_vars
    public float currentSimTime;
    public float maxSimTime;
    public bool simRunning = false;
    private GameObject CreatureInstance;
    public int CreaturesToTest;
    public int TestedCreatureID = 0;
    public bool CreatureRendering;
    public float SimSpeed=1;
    #endregion

    #region gen_vars
    //===================
    //GENERATOR VARIABLES
    //===================
    public float maxOffset;
    public float minOffset;
    public int CreatureAmount;
    public int MinNodes;
    public int MaxNodes;
    public float MinCreatureSize;
    public float MaxCreatureSize;
    public float MinNodeMass;
    public float MaxNodeMass;
    public float MinNodeFriction;
    public float MaxNodeFriction;
    public float MinMusculeStrenght;
    public float MaxMusculeStrenght;
    public float MinMusculeContractedLenght;
    public float MaxMusculeContractedLenght;
    public float MinMusculeExtendedLenght;
    public float MaxMusculeExtendedLenght;
    public float MinMusculeContractedTime;
    public float MaxMusculeContractedTime;
    public float MinMusculeExtendedTime;
    public float MaxMusculeExtendedTime;
    #endregion


    public void Start()
    {
        instance = this;

        GenerationIndicator.text = "Generation 0";


    }



    

    


    public GameObject CreateCreature(CreatureData data)
    {
        GameObject c = new GameObject();
        c.AddComponent<Creature>().data = data;
        c.transform.name = "Creature";

        Debug.Log("Instantiating creature...");

        foreach (Node n in data.Nodes)
        {
            //Instantiate nodes in the right positions

           

            



            GameObject node = Instantiate(nodePrefab, n.NodePosition, gameObject.transform.rotation, c.transform);
            //Debug.Log("Instantiating node...");
            //Tweak node settings           
            node.GetComponent<SphereCollider>().material.staticFriction = n.NodeFriction + 0.05f;
            node.GetComponent<SphereCollider>().material.dynamicFriction = n.NodeFriction;
            node.GetComponent<Rigidbody>().mass = n.NodeMass;
            node.GetComponent<Rigidbody>().freezeRotation = true;
            //Add nodes to the creature node list
            c.GetComponent<Creature>().Nodes.Add(node);
        }




        foreach (Muscule m in data.Muscles)
        {
            //Attach muscles to the right nodes
            SpringJoint muscule = c.GetComponent<Creature>().Nodes[m.NodeOneID].AddComponent<SpringJoint>();
            muscule.connectedBody = c.GetComponent<Creature>().Nodes[m.NodeTwoID].GetComponent<Rigidbody>();
            //Set anchors to the center of the nodes
            muscule.autoConfigureConnectedAnchor = false;
            muscule.anchor = new Vector3(0, 0, 0);
            muscule.connectedAnchor = new Vector3(0, 0, 0);
            //Tweak muscule settings
            muscule.spring = m.Strength;
            muscule.damper = m.Strength / 10;
            //Initialize muscule internal clock system
            MuscleController clock = c.GetComponent<Creature>().Nodes[m.NodeOneID].AddComponent<MuscleController>();
            clock.muscule = muscule;
            clock.ContractedTime = m.ContractedTime;
            clock.ExtendedTime = m.ExtendedTime;
            clock.ContractedLenght = m.ContractedLength;
            clock.ExtendedLenght = m.ExtendedLength;
            //Initialize muscule rendering system

            
                GameObject renderer = Instantiate(musculeRendererPrefab, c.transform);
                renderer.GetComponent<LineController>().TransformOne = c.GetComponent<Creature>().Nodes[m.NodeOneID].transform;
                renderer.GetComponent<LineController>().TransformTwo = c.GetComponent<Creature>().Nodes[m.NodeTwoID].transform;
            
            
            
            //Add muscles to the creature muscule list
            c.GetComponent<Creature>().Muscles.Add(muscule);

        }


        return c;
    }

    
    

    public CreatureData GenerateRandomCreatureData()
    {
        //Create a CreatureData object
        CreatureData data = new CreatureData();
        //Initialize random number generator
        System.Random randomizer = new System.Random();
        int NodeAmount = randomizer.Next(MinNodes, MaxNodes);
        


        for (int i = 0; i < NodeAmount; i++)
        {
            Node node = new Node();
            int number;
            if (randomizer.Next() % 2 == 0) number = 1;
            else number = -1;
            node.NodePosition.x = UnityEngine.Random.Range(MinCreatureSize / 2, MaxCreatureSize / 2) * number;
            node.NodePosition.y = UnityEngine.Random.Range(MinCreatureSize / 2, MaxCreatureSize / 2) * number;
            node.NodePosition.z = UnityEngine.Random.Range(MinCreatureSize / 2, MaxCreatureSize / 2) * number;

            if (node.NodePosition.y < 0.5) node.NodePosition.y = 0.5f;

            node.NodeMass = UnityEngine.Random.Range(MinNodeMass, MaxNodeMass);
            node.NodeFriction = UnityEngine.Random.Range(MinNodeFriction, MaxNodeFriction);

            data.Nodes.Add(node);

            //Debug.Log("Generating Node...");
        }



        int MaxMusculeAmount = 0;
        MaxMusculeAmount = (NodeAmount * ((NodeAmount - 1))) / 2;

        //Debug.Log("Node amount: " + NodeAmount.ToString() + " Maximum muscule ammount: " + MaxMusculeAmount);
        


        int MusculeAmount = randomizer.Next(NodeAmount, MaxMusculeAmount);
        List<Vector2Int> availbleConnections = new List<Vector2Int>();
        Debug.Log("Node amount: " + NodeAmount + ", muscule amount: " + MusculeAmount);

        //== WARNING ==
        //This solution is possibly a hack, it is very inefficient when number of nodes will be increased! S: O(n^2) T: O(n^2)

        //1. Evaluate all possibilities
        for (int i = 0; i < NodeAmount; i++)
        {
            for (int j = 0; j < NodeAmount; j++)
            {
                //We cannot connect node to it self, so we skip that connection
                if (j == i) continue;
                availbleConnections.Add(new Vector2Int(i, j));
            }
        }



        for (int i = 0; i < MusculeAmount; i++)
        {
            Muscule muscule = new Muscule();

            //Choose one of possible connections
            Vector2Int connectionID = availbleConnections[randomizer.Next(0, availbleConnections.Count)];
            //remove that connection marking it as not availble
            availbleConnections.Remove(connectionID);
            //remove the opposite too, to prevent back and forth connections and the creatures from ripping apart
            availbleConnections.Remove(new Vector2Int(connectionID.y ,connectionID.x));

            //assign stuff
            muscule.NodeOneID = connectionID.x;
            muscule.NodeTwoID = connectionID.y;
            muscule.Strength = UnityEngine.Random.Range(MinMusculeStrenght, MaxMusculeStrenght);
            muscule.ContractedLength = UnityEngine.Random.Range(MinMusculeContractedLenght, MaxMusculeContractedLenght);
            muscule.ExtendedLength = UnityEngine.Random.Range(MinMusculeExtendedLenght, MaxMusculeExtendedLenght);
            muscule.ContractedTime = UnityEngine.Random.Range(MinMusculeContractedTime, MaxMusculeContractedTime);
            muscule.ExtendedTime = UnityEngine.Random.Range(MinMusculeExtendedTime, MaxMusculeExtendedTime);

            data.Muscles.Add(muscule);

        }

        
        return data;


    }



    public void InitializeSim(CreatureData data)
    {
        CreatureInstance = CreateCreature(data);
        currentSimTime = 0f;
        simRunning = true;

        
       

        
        float IndicatedCreatureNumber = TestedCreatureID + 1f;
        float CreatureAmountFloat = CreatureAmount;

        CreatureIndicator.text = "Creature " + IndicatedCreatureNumber + "/" + CreatureAmount;
        CreatureProgressBar.fillAmount =  IndicatedCreatureNumber / CreatureAmountFloat;
        


    }

    void EndSim()
    {
        currentGeneration[TestedCreatureID].fitness = CreatureInstance.transform.position.z - Mathf.Abs(CreatureInstance.transform.position.x);//Make creatures go STRAIGHT forward
        simRunning = false;
        Destroy(CreatureInstance);

        TestedCreatureID++;
        CreaturesToTest--;

        

        if(CreaturesToTest > 0)
        {
            InitializeSim(currentGeneration[TestedCreatureID]);
        }

        else
        {
            Debug.Log("Test Complete!");
        }
        
        

    }



    public void TestAllCreatures()
    {
        CreaturesToTest = CreatureAmount;
        TestedCreatureID = 0;

        InitializeSim(currentGeneration[TestedCreatureID]);
    }


    
    





    private void Update()
    {
        if (simRunning)
        {
            currentSimTime += Time.deltaTime;
            if (currentSimTime >= maxSimTime)
            {
                EndSim();
            }

            if (CreatureRendering)
            {

                Camera.main.gameObject.SetActive(true);


                float SumX = 0;
                float SumY = 0;
                float SumZ = 0;

                foreach (GameObject node in CreatureInstance.GetComponent<Creature>().Nodes)
                {
                    SumX += node.transform.position.x;
                    SumY += node.transform.position.y;
                    SumZ += node.transform.position.z;
                }

                float AvgX = SumX / CreatureInstance.GetComponent<Creature>().Nodes.Count;
                float AvgY = SumY / CreatureInstance.GetComponent<Creature>().Nodes.Count;
                float AvgZ = SumZ / CreatureInstance.GetComponent<Creature>().Nodes.Count;

                CameraRig.transform.position = new Vector3(AvgX, AvgY, AvgZ);
            }

            else Camera.main.gameObject.SetActive(false);


            Time.timeScale = SimSpeed;
        }

}

    


    public void OnGenerationEnd()
    {
        //===========================
        //I. DEATH HANDLING
        //===========================

        //Initialize random number generator
        System.Random randomizer = new System.Random();

        //50% of worst dies
        //1. Find median value
        //1a) Sort list decrementaly
        currentGeneration.Sort((x, y) => x.fitness.CompareTo(y.fitness));
        //1b) Find middle
        int Middle = currentGeneration.Count/2;

        List<CreatureData> CreaturesAboutToDie = new List<CreatureData>();
        List<CreatureData> CreaturesNotAboutToDie = new List<CreatureData>();



        //2. Mark 50% descending as about to die
        for (int i = Middle; i < currentGeneration.Count; i++)
        {
            CreaturesAboutToDie.Add(currentGeneration[i]);
            
        }


        //3. Mark 50% ascending as not about to die
        for (int i = Middle-1; i >= 0; i--)
        {
            CreaturesNotAboutToDie.Add(currentGeneration[i]);

        }


        //4. Invert 10% of creatures from each array
        int AmountToRandomize = Mathf.CeilToInt(currentGeneration.Count / 20);

        for(int i  = 0;i < AmountToRandomize; i++)
        {
            int rand = randomizer.Next(0, CreaturesAboutToDie.Count);
            CreaturesNotAboutToDie.Add(CreaturesAboutToDie[rand]);
            CreaturesAboutToDie.Remove(CreaturesAboutToDie[rand]);
        }


        for (int i = 0; i < AmountToRandomize; i++)
        {
            int rand = randomizer.Next(0, CreaturesAboutToDie.Count);
            CreaturesNotAboutToDie.Add(CreaturesAboutToDie[rand]);
            CreaturesAboutToDie.Remove(CreaturesAboutToDie[rand]);
        }



        //Push current gen to previous, then evaluate next gen
        previousGeneration.AddRange(CreaturesNotAboutToDie);
        currentGeneration.Clear();
        CreaturesNotAboutToDie.Clear();
        CreaturesAboutToDie.Clear();

        /*
        
        
        //===========================
        //II. OFFSPRING CREATION HANDLING
        //===========================

        for (int i = 0; i < CreatureAmount; i++)
        {
            //Create offspring
            CreatureData offspring = previousGeneration[i].data;
            //Randomize data by offset
            foreach (Node n in offspring.Nodes)
            {
                n.NodeFriction = UnityEngine.Random.Range(n.NodeFriction - maxOffset, n.NodeFriction + maxOffset);
                n.NodeMass = UnityEngine.Random.Range(n.NodeMass - maxOffset, n.NodeMass + maxOffset);
                n.NodePosition = new Vector3(UnityEngine.Random.Range(n.NodePosition.x - maxOffset, n.NodePosition.x + maxOffset),
                    UnityEngine.Random.Range(n.NodePosition.y - maxOffset, n.NodePosition.y + maxOffset),
                    UnityEngine.Random.Range(n.NodePosition.z - maxOffset, n.NodePosition.z + maxOffset));

            }
            foreach (Muscule m in offspring.Muscles)
            {
                m.ContractedLength = UnityEngine.Random.Range(m.ContractedLength - maxOffset, m.ContractedLength + maxOffset);
                m.ContractedTime = UnityEngine.Random.Range(m.ContractedTime - maxOffset, m.ContractedTime + maxOffset);
                m.ExtendedLength = UnityEngine.Random.Range(m.ExtendedLength - maxOffset, m.ExtendedLength + maxOffset);
                m.ExtendedTime = UnityEngine.Random.Range(m.ExtendedTime - maxOffset, m.ExtendedTime + maxOffset);
            }


            CreateCreature(offspring);
        }*/

        //========================
        //III. Finalize generation
        //========================


        GenerationNumber++;

        GenerationIndicator.text = "Generation " + GenerationNumber.ToString();


    }

}

