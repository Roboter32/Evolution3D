using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleController : MonoBehaviour {


    public SpringJoint muscule;
    public float ContractedTime;
    public float ExtendedTime;
    public float ContractedLenght;
    public float ExtendedLenght;

    private float CurrentTime = 0f;
    private bool FirstSegmentActive = true;
    private bool SecondSegmentActive = false;



    void Start () {
		
	}
	
	
	void Update () {

        
        CurrentTime += Time.deltaTime;

        if (CurrentTime >= ContractedTime && FirstSegmentActive)
        {

            muscule.minDistance = ExtendedLenght;
            muscule.maxDistance = ExtendedLenght;
        
            CurrentTime = 0;
            FirstSegmentActive = false;
            SecondSegmentActive = true;
        }

        if(CurrentTime >= ExtendedTime && SecondSegmentActive)
        {

            muscule.minDistance = ContractedLenght;
            muscule.maxDistance = ContractedLenght;
 
            CurrentTime = 0;
            FirstSegmentActive = true;
            SecondSegmentActive = false;

        }

        





       



    }
}
