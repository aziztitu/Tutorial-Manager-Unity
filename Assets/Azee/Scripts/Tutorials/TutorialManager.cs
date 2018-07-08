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

    [SerializeField]
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
        TutorialPage[] tutorialPagesInChildren = GetComponentsInChildren<TutorialPage>();

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

    public void OnValidate()
    {
        /*Debug.Log("Clearing _tutorialPageEntries");
        _tutorialPageEntries.Clear();
        foreach (KeyValuePair<string, TutorialPage> keyValuePair in _tutorialPages)
        {
            Debug.Log("Adding "+ keyValuePair.Key);
            _tutorialPageEntries.Add(new TutorialPageWithName
            {
                Name = keyValuePair.Key,
                TutorialPage = keyValuePair.Value
            });
        }*/
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    public Dictionary<string, TutorialPage> GetTutorialPages()
    {
        return _tutorialPages;
    }
}