using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour {

    LineRenderer LineRenderer;


    public Transform TransformOne;
    public Transform TransformTwo;




	
	void Start () {

        LineRenderer = GetComponent<LineRenderer>();
		
	}
	
	
	void Update () {

        LineRenderer.SetPosition(0, TransformOne.position);
        LineRenderer.SetPosition(1, TransformTwo.position);

    }
}
