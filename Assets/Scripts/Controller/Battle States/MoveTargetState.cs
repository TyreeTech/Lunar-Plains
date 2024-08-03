using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetState : BattleState
{
    // grabs the list of tiles
    List<Tile> tiles;

    // when the state has started
    public override void Enter()
    {
        base.Enter ();
        Movement mover = turn.actor.GetComponent<Movement>();
        tiles = mover.GetTilesInRange(board);
        board.SelectTiles(tiles);
    }

    //when the state has ended
    public override void Exit()
    {
        base.Exit();
        board.DeSelectTiles(tiles);
        tiles = null;
    }

    protected override void OnMove (object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
      if(e.info == 0)
      {
        if(tiles.Contains(owner.currentTile))
          owner.ChangeState<MoveSequenceState>();
      }
      else
      {
        owner.ChangeState<CommandSelectionState>();
      }
    }
}
