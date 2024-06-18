using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AbilityMenuPanelController : MonoBehaviour
{

    //constant declarations for the panel and pool Manager
    const string ShowKey = "Show";
    const string HideKey = "Hide";
    const string EntryPoolKey = "AbilityMenuPanel.Entry";
    const int MenuCount = 4;

    //exposing a reference to the menu prefab to instantiate the pooled objects
    [SerializeField] GameObject entryPrefab;
    // headings title label to show context
    [SerializeField] Text titleLabel;
    //Panel reference to toggle visibility and for a container for entries for the menu
    [SerializeField] Panel panel;
    //canvas reference to disable when not using
    [SerializeField] GameObject canvas;
    //list holding all active menu entries and the values that represent the currently selected index
    List<AbilityMenuEntry> menuEntries = new List<AbilityMenuEntry>(MenuCount);
    // a value representing the currently selected index in the menu
    public int selection{ get; private set; }

    //configure the pool manager so that it can generate menu entries
    void Awake()
    {
        GameObjectPoolController.AddEntry(EntryPoolKey, entryPrefab, MenuCount, int.MaxValue);
    }

    //Methods to get the menue entires and return them to the pool manager;
    AbilityMenuEntry Deque()
    {
        Poolable p = GameObjectPoolController.Dequeue(EntryPoolKey);
        AbilityMenuEntry entry = p.GetComponent<AbilityMenuEntry>();
        entry.transform.SetParent(panel.transform, false);
        entry.transform.localScale = Vector3.one;
        entry.gameObject.SetActive(true);
        entry.Reset();
        return entry;
    }

    void Enqueue (AbilityMenuEntry entry)
    {
        Poolable p = entry.GetComponent<Poolable>();
        GameObjectPoolController.Enqueue(p);
    }

    void Clear()
    {
        //here future ME
    }

}
