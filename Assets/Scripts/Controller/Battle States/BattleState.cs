using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleState : State
{
  //creates a reference to the owner of the script: BattleController
  protected BattleController owner;

  //way to shorten reference calls. EX: instead of saying owner.board, can now say board
  public CameraRig cameraRig { get { return owner.cameraRig; }}
  public Board board { get { return owner.board; }}
  public LevelData levelData { get { return owner.levelData; }}
  public Transform tileSelectionIndicator { get { return owner.tileSelectionIndicator; }}
  public Point pos { get { return owner.pos; } set { owner.pos = value; }}
  public AbilityMenuPanelController abilityMenuPanelController{ get { return owner.abilityMenuPanelController; }}
  public Turn turn { get { return owner.turn; }}
  public List<Unit> units { get { return owner.units; }}

  protected virtual void Awake()
  {
    // owner reference is connected to the BattleController component
      owner = GetComponent<BattleController>();

  }

  protected override void AddListeners()
  {
      InputController.moveEvent += OnMove;
      InputController.fireEvent += OnFire;
  }

  protected override void RemoveListeners()
  {
      InputController.moveEvent -= OnMove;
      InputController.fireEvent -= OnFire;
  }

  // added event handlers for the input events. Empty so not required to override
  protected virtual void OnMove (object sender, InfoEventArgs<Point> e)
  {

  }

  protected virtual void OnFire (object sender, InfoEventArgs<int> e)
  {

  }

  //sets selected tile of the game board
  protected virtual void SelectTile (Point p)
  {
      if(pos == p || !board.tiles.ContainsKey(p))
        return;
      pos = p;
      tileSelectionIndicator.localPosition = board.tiles[p].center;
  }
}
