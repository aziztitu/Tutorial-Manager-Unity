using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TutorialManager.Instance.ShowTutorial("A");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTutorialsFinished()
    {
        Debug.Log("Tutorials Completed!");
    }
}
