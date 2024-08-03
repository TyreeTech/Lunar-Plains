using TMPro;
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
    [SerializeField] TextMeshProUGUI titleLabel;
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
    AbilityMenuEntry Dequeue()
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
        for (int i = menuEntries.Count - 1; i >= 0; --i)
          Enqueue(menuEntries[i]);
        menuEntries.Clear();
    }

    //hide the panel and disable the canvas to start
    void Start()
    {
      panel.SetPosition(HideKey, false);
      canvas.SetActive(false);
    }

    //a method that animates the movement into a select SetPosition
    Tweener TogglePos(string pos)
    {
        Tweener t = panel.SetPosition(pos, true);
        t.easingControl.duration = 0.5f;
        t.easingControl.equation = EasingEquations.EaseOutQuad;
        return t;

    }

    //finds a menu item that isn't locked and highlights by default
    bool SetSelection(int value)
    {
        if (menuEntries[value].IsLocked)
          return false;

        //deselect the previously selected entry
        if(selection >= 0 && selection < menuEntries.Count)
          menuEntries[selection].IsSelected = false;

        selection = value;

        //Select the new entry
        if(selection >= 0 && selection < menuEntries.Count)
          menuEntries[selection].IsSelected = true;

        return true;
    }

    //select the next entry in the list. checks to see if the entry is Locked
    public void Next()
    {
        //loops through and sets the selection
        for (int i = selection + 1; i < selection + menuEntries.Count; ++i)
        {
            int index = i % menuEntries.Count;
            if(SetSelection(index))
              break;
        }
    }

    //select the previous entry in the list. checks to see if the entry is Locked
    public void Previous()
    {
        //loops through and sets the selection
        for (int i = selection - 1 + menuEntries.Count; i > selection; --i)
        {
            int index = i % menuEntries.Count;
            if(SetSelection(index))
              break;
        }
    }

    public void Show(string title, List<string> options)
    {
      //shows the canvas, clears it, adds the new title, adds the list of entries
        canvas.SetActive(true);
        Clear();
        titleLabel.text = title;
        for(int i = 0; i < options.Count; ++i)
        {
            AbilityMenuEntry entry = Dequeue();
            entry.Title = options[i];
            menuEntries.Add(entry);
        }
        //brings the selection to the top
        SetSelection(0);
        TogglePos(ShowKey);
    }

    //locks or unlocks a specific action when called.
    public void SetLocked (int index, bool value)
    {
        if(index < 0 || index >= menuEntries.Count)
          return;

        menuEntries[index].IsLocked = value;
        if(value && selection == index)
          Next();
    }

    //dismisses the menu.
    public void Hide()
    {
        Tweener t = TogglePos(HideKey);
        t.easingControl.completedEvent += delegate(object sender, System.EventArgs e)
        {
            if(panel.CurrentPosition == panel[HideKey])
            {
                Clear();
                canvas.SetActive(false);
            }
        };
    }

}
