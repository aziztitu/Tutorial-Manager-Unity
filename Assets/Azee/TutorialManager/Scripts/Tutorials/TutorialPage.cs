using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class TutorialPage : MonoBehaviour
{
    public string Name = "TutorialName";
    public bool ToggleActive = true;
    public bool PauseTime = false;

    [Header("Timer-Based")] public bool EnableTimer = false;
    public float TimerPeriod = 5f;

    [Header("Broadcast-Action-Based")] public bool EnableAction = false;
    public string AwaitingAction = "";

    [Header("Events")] public UnityEvent OnBeginEvent;
    public UnityEvent OnEndEvent;

    private bool _isShowing = false;

    private Animator _animator;

    private IEnumerator _endAfterTimeCoroutine = null;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

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
        if (!_isShowing && TutorialManager.Instance.IsTutorialManagerEnabled())
        {
            _isShowing = true;

            if (ToggleActive)
            {
                gameObject.SetActive(true);
                if (_animator && _animator.enabled)
                {
                    _animator.SetTrigger("showTutorial");
                }
            }

            OnBeginEvent.Invoke();

            if (EnableTimer)
            {
                if (_endAfterTimeCoroutine != null)
                {
                    StopCoroutine(_endAfterTimeCoroutine);
                }
                _endAfterTimeCoroutine = EndTutorialAfterTimer();
                StartCoroutine(_endAfterTimeCoroutine);
            }

            if (EnableAction)
            {
                TutorialManager.Instance.AddAwaitingActionForTutorial(this, AwaitingAction);
            }

            if (PauseTime)
            {
                Time.timeScale = 0;
            }
        }
    }

    public void End()
    {
        if (_isShowing)
        {
            _isShowing = false;

            if (PauseTime)
            {
                Time.timeScale = 1;
            }

            if (ToggleActive)
            {
                if (_animator && _animator.enabled)
                {
                    _animator.SetTrigger("hideTutorial");
                    return;
                }

                gameObject.SetActive(false);
            }

            OnEndEvent.Invoke();
        }
    }

    public void Cancel()
    {
        if (_isShowing)
        {
            _isShowing = false;

            if (ToggleActive)
            {
                if (_animator && _animator.enabled)
                {
                    _animator.SetTrigger("hideTutorial");
                    return;
                }

                gameObject.SetActive(false);
            }
        }
    }

    /*
     * The hide animation clip must fire this event once the tutorial is hidden.
     */
    public void OnTutorialHidden()
    {
        gameObject.SetActive(false);
        OnEndEvent.Invoke();
    }

    private IEnumerator EndTutorialAfterTimer()
    {
        yield return new WaitForSecondsRealtime(TimerPeriod);

        _endAfterTimeCoroutine = null;
        End();
    }

    void OnDisable()
    {

    }

    void OnEnable()
    {
        if (_endAfterTimeCoroutine != null)
        {
            StartCoroutine(_endAfterTimeCoroutine);
        }
    }
}