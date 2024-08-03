using System.Collections;
using UnityEngine;

public class ExploreState : BattleState
{
    //creates a state where the player is able to move and select things
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if(e.info == 0)
          owner.ChangeState<CommandSelectionState>();
    }
}
