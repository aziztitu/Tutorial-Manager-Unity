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

    private readonly Dictionary<string, TutorialPage> _tutorialPages = new Dictionary<string, TutorialPage>();

    [SerializeField] private List<TutorialPageWithName> _tutorialPageEntries = new List<TutorialPageWithName>();

    [SerializeField] [Button("Find tutorial pages in children", "FindTutorialPagesInChildren")]
    private bool _buttonFindTutorialPagesInChildren;


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
                Debug.Log("Tutorial Pages: " + _tutorialPages.Count);
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
        Debug.Log("Tutorial Pages: " + _tutorialPages.Count);
//        DisableTutorialPages();
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

    private void DisableTutorialPages()
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
}