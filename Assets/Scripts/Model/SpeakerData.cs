using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeakerData
{
    // list that contains all of the text any character might say
    public List<string> messages;
    // reference to the speaker's sprite
    public Sprite speaker;
    // reference to the coner of the screen the speaker shows on
    public TextAnchor anchor;


}
