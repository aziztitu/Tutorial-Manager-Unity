using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialPage : MonoBehaviour
{
    public string Name = "TutorialName";
    public bool ToggleActive = true;

    [Header("Timer")]
    public bool EnableTimer = false;
    public float TimerPeriod = 5f;

    [Header("Events")] public UnityEvent OnBeginEvent;
    public UnityEvent OnEndEvent;

    private bool _isShowing = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Begin()
    {
        if (!_isShowing)
        {
            _isShowing = true;

            if (ToggleActive)
            {
                gameObject.SetActive(true);
            }

            OnBeginEvent.Invoke();

            if (EnableTimer)
            {
                StartCoroutine(EndTutorialAfterTimer());
            }
        }
    }

    public void End()
    {
        if (_isShowing)
        {
            _isShowing = false;

            if (ToggleActive)
            {
                gameObject.SetActive(false);
            }

            OnEndEvent.Invoke();
        }
    }

    private IEnumerator EndTutorialAfterTimer()
    {
        yield return new WaitForSeconds(TimerPeriod);
        End();
    }
}