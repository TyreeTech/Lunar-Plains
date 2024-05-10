using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Tile tile { get; protected set; }
    public Directions dir;

    public void Place (Tile target)
    {
        //Make sure old tile location is not still pointing to the Unit
        if(tile != null && tile.content == gameObject)
          tile.content = null;

        // Link unit and tile reference
        tile = target;

        if(target != null)
          target.content = gameObject;
    }

    // Update is called once per frame
    public void Match()
    {
        transform.localPosition = tile.center;
        transform.localEulerAngles = dir.ToEuler();
    }
}
