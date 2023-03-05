using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {


    public UnityEngine.UI.Button StartButton;
    public UnityEngine.UI.Button StepByStepButton;
    public UnityEngine.UI.Button QuickButton;
    public UnityEngine.UI.Button ASAPButton;
    public GameObject StartingScreen;
    public GameObject MainScreen;
    public GameObject TestOverlay;
    public GameObject CreaturePreviev;
    public GridLayout CreaturrePrevievGrid;

    

    #region fields
    public UnityEngine.UI.InputField minOffset;
    public UnityEngine.UI.InputField maxOffset;
    public UnityEngine.UI.InputField CreatureAmount;
    public UnityEngine.UI.InputField MinNodes;
    public UnityEngine.UI.InputField MaxNodes;
    public UnityEngine.UI.InputField MinCreatureSize;
    public UnityEngine.UI.InputField MaxCreatureSize;
    public UnityEngine.UI.InputField MinNodeMass;
    public UnityEngine.UI.InputField MaxNodeMass;
    public UnityEngine.UI.InputField MinNodeFriction;
    public UnityEngine.UI.InputField MaxNodeFriction;
    public UnityEngine.UI.InputField MinMusculeStrenght;
    public UnityEngine.UI.InputField MaxMusculeStrenght;
    public UnityEngine.UI.InputField MinMusculeContractedLenght;
    public UnityEngine.UI.InputField MaxMusculeContractedLenght;
    public UnityEngine.UI.InputField MinMusculeExtendedLenght;
    public UnityEngine.UI.InputField MaxMusculeExtendedLenght;
    public UnityEngine.UI.InputField MinMusculeContractedTime;
    public UnityEngine.UI.InputField MaxMusculeContractedTime;
    public UnityEngine.UI.InputField MinMusculeExtendedTime;
    public UnityEngine.UI.InputField MaxMusculeExtendedTime;
    public UnityEngine.UI.InputField maxSimTime;
    #endregion

    private int StepByStepGeneratedCreatureID = 1;
    bool GenerateCreatures = false;

    void Start () {

        StartButton.onClick.AddListener(Initialize);
        StepByStepButton.onClick.AddListener(DoOneStepByStepGeneration);
        QuickButton.onClick.AddListener(DoOneQuickGeneration);
        ASAPButton.onClick.AddListener(DoOneGenerationASAP);




    }
	
	
	void Update () {

        if(StepByStepGeneratedCreatureID < CreatureManager.instance.CreatureAmount && GenerateCreatures == true)
        {
            CreatureManager.instance.currentGeneration.Add(CreatureManager.instance.GenerateRandomCreatureData());
            StepByStepGeneratedCreatureID++;

            if(StepByStepGeneratedCreatureID == CreatureManager.instance.CreatureAmount)
            {
                GenerateCreatures = false;

                
            }
        }
		
	}

    void Initialize()
    {
        CreatureManager.instance.minOffset = float.Parse(minOffset.text);
        CreatureManager.instance.maxOffset = float.Parse(maxOffset.text);
        CreatureManager.instance.CreatureAmount = int.Parse(CreatureAmount.text);
        CreatureManager.instance.MinNodes = int.Parse(MinNodes.text);
        CreatureManager.instance.MaxNodes = int.Parse(MaxNodes.text);
        CreatureManager.instance.MinCreatureSize = float.Parse(MinCreatureSize.text);
        CreatureManager.instance.MaxCreatureSize = float.Parse(MaxCreatureSize.text);
        CreatureManager.instance.MinNodeMass = float.Parse(MinNodeMass.text);
        CreatureManager.instance.MaxNodeMass = float.Parse(MaxNodeMass.text);
        CreatureManager.instance.MinNodeFriction = float.Parse(MinNodeFriction.text);
        CreatureManager.instance.MaxNodeFriction = float.Parse(MaxNodeFriction.text);
        CreatureManager.instance.MinMusculeStrenght = float.Parse(MinMusculeStrenght.text);
        CreatureManager.instance.MaxMusculeStrenght = float.Parse(MaxMusculeStrenght.text);
        CreatureManager.instance.MinMusculeContractedLenght = float.Parse(MinMusculeContractedLenght.text);
        CreatureManager.instance.MaxMusculeContractedLenght = float.Parse(MaxMusculeContractedLenght.text);
        CreatureManager.instance.MinMusculeExtendedLenght = float.Parse(MinMusculeExtendedLenght.text);
        CreatureManager.instance.MaxMusculeExtendedLenght = float.Parse(MaxMusculeExtendedLenght.text);
        CreatureManager.instance.MinMusculeContractedTime = float.Parse(MinMusculeContractedTime.text);
        CreatureManager.instance.MaxMusculeContractedTime = float.Parse(MaxMusculeContractedTime.text);
        CreatureManager.instance.MinMusculeExtendedTime = float.Parse(MinMusculeExtendedTime.text);
        CreatureManager.instance.MaxMusculeExtendedTime = float.Parse(MaxMusculeExtendedTime.text);
        CreatureManager.instance.maxSimTime = float.Parse(maxSimTime.text);


        StartingScreen.SetActive(false);
        MainScreen.SetActive(true);
    }

    void DoOneStepByStepGeneration()
    {
        MainScreen.SetActive(false);
        CreaturePreviev.SetActive(true);

        if (CreatureManager.instance.GenerationNumber == 0)
        {
            GenerateCreatures = true;
        }
    }

    void DoOneQuickGeneration()
    {

    }

    void DoOneGenerationASAP()
    {

    }
    


}
