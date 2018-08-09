using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManagerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TutorialManager.Instance.ShowTutorial("Blue");
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
            TutorialManager.Instance.BroadcastTutorialAction("Space");
	    }
	}

    public void OnTutorialsFinished()
    {
        Debug.Log("Tutorials Completed!");
    }
}
