using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BasicTools.ButtonInspector;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager _instance;

    public static TutorialManager Instance
    {
        get { return _instance; }
    }

    [Serializable]
    class TutorialPageWithName
    {
        [ReadOnly] public string Name;
        [ReadOnly] public TutorialPage TutorialPage;
    }

    [Serializable]
    public class TutorialAwaitingAction
    {
        [ReadOnly] public string AwaitingAction = "";
        [ReadOnly] public TutorialPage TutorialPage;
    }

    [ReadOnly] [SerializeField] private List<TutorialAwaitingAction> TutorialsAwaitingAction = new List<TutorialAwaitingAction>();


    private readonly Dictionary<string, TutorialPage> _tutorialPages = new Dictionary<string, TutorialPage>();

    [SerializeField] private List<TutorialPageWithName> _tutorialPageEntries = new List<TutorialPageWithName>();

    [SerializeField] [Button("Find tutorial pages in children", "FindTutorialPagesInChildren")]
    private bool _buttonFindTutorialPagesInChildren;

    [ReadOnly] [SerializeField] private bool _isEnabled = true;

    public TutorialManager() : base()
    {
        _instance = this;
    }

    void FindTutorialPagesInChildren()
    {
        TutorialPage[] tutorialPagesInChildren = GetComponentsInChildren<TutorialPage>(true);

        _tutorialPages.Clear();
        _tutorialPageEntries.Clear();
        foreach (TutorialPage tutorialPage in tutorialPagesInChildren)
        {
            if (_tutorialPages.ContainsKey(tutorialPage.Name))
            {
                Debug.LogError("Duplicate Tutorial Name found: " + tutorialPage.Name);
            }
            else
            {
                _tutorialPages.Add(tutorialPage.Name, tutorialPage);
                _tutorialPageEntries.Add(new TutorialPageWithName
                {
                    Name = tutorialPage.Name,
                    TutorialPage = tutorialPage
                });
            }
        }
    }

    void Awake()
    {
        RecoverTutorialPages();
    }

    // Use this for initialization
    void Start()
    {
//        Debug.Log("Tutorial Pages: " + _tutorialPages.Count);
//        DeactivateTutorialPages();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /*
     * Dictionary Objects get destroyed by Unity once the game is started.
     * So we recover the dictionary's data from the entries.
     */
    private void RecoverTutorialPages()
    {
        _tutorialPages.Clear();
        foreach (TutorialPageWithName tutorialPageWithName in _tutorialPageEntries)
        {
            if (_tutorialPages.ContainsKey(tutorialPageWithName.Name))
            {
                Debug.LogError("Duplicate Tutorial Name found: " + tutorialPageWithName.Name);
            }
            else
            {
                _tutorialPages.Add(tutorialPageWithName.Name, tutorialPageWithName.TutorialPage);
            }
        }
    }

    private void DeactivateTutorialPages()
    {
        foreach (TutorialPage tutorialPage in _tutorialPages.Values)
        {
            tutorialPage.gameObject.SetActive(false);
        }
    }


    public Dictionary<string, TutorialPage> GetTutorialPages()
    {
        return _tutorialPages;
    }

    public void ShowTutorial(string name)
    {
//        _tutorialPages[name].gameObject.SetActive(true);
        _tutorialPages[name].Begin();
    }

    public void HideTutorial(string name)
    {
        //        _tutorialPages[name].gameObject.SetActive(false);
        _tutorialPages[name].End();
    }

    public void CancelTutorial(string name)
    {
        //        _tutorialPages[name].gameObject.SetActive(false);
        _tutorialPages[name].Cancel();
    }

    public void CancelAllTutorials()
    {
        foreach (TutorialPage tutorialPage in _tutorialPages.Values)
        {
            tutorialPage.Cancel();
        }
    }

    public void AddAwaitingActionForTutorial(TutorialPage tutorialPage, string action)
    {
        bool changed = false;
        foreach (TutorialAwaitingAction tutorialAwaitingAction in TutorialsAwaitingAction)
        {
            if (tutorialAwaitingAction.TutorialPage == tutorialPage)
            {
                tutorialAwaitingAction.AwaitingAction = action;
                changed = true;
            }
        }

        if (!changed)
        {
            TutorialsAwaitingAction.Add(new TutorialAwaitingAction()
            {
                TutorialPage = tutorialPage,
                AwaitingAction = action
            });
        }
    }

    public void BroadcastTutorialAction(string action)
    {
        List<TutorialAwaitingAction> deleteList = new List<TutorialAwaitingAction>();

        foreach (TutorialAwaitingAction tutorialAwaitingAction in TutorialsAwaitingAction)
        {
            if (tutorialAwaitingAction.AwaitingAction.Equals(action))
            {
                tutorialAwaitingAction.TutorialPage.End();
                deleteList.Add(tutorialAwaitingAction);
            }
        }

        foreach (TutorialAwaitingAction deleteObj in deleteList)
        {
            TutorialsAwaitingAction.Remove(deleteObj);
        }

        deleteList.Clear();
    }

    public void EnableTutorialManager()
    {
        _isEnabled = true;
    }

    public void DisableTutorialManager()
    {
        _isEnabled = false;

        CancelAllTutorials();
    }

    public bool IsTutorialManagerEnabled()
    {
        return _isEnabled;
    }
}