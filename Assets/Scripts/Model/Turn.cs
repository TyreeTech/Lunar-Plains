using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    public Unit actor;
    public bool hasUnitMoved;
    public bool hasUnitActed;
    public bool lockMove;
    Tile startTile;
    Directions startDir;

    // Start is called before the first frame update
    public void Change (Unit current)
    {
        actor = current;
        hasUnitMoved = false;
        hasUnitActed = false;
        lockMove = false;
        startTile = actor.tile;
        startDir = actor.dir;

    }

    // Undoes the move 
    public void UndoMove()
    {

        hasUnitMoved = false;
        actor.Place(startTile);
        actor.dir = startDir;
        actor.Match();

    }
}
