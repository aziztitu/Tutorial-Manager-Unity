using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialPage : MonoBehaviour
{
    public string Name = "TutorialName";

    public UnityEvent OnBeginEvent;
    public UnityEvent OnEndEvent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Begin()
    {
        OnBeginEvent.Invoke();
    }

    public void End()
    {
        OnEndEvent.Invoke();
    }
}
