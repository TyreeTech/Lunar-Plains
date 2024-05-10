using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // creates and defines an empty state called the "Current state"
  public virtual State CurrentState
  {
    //gets the current state, the sets the value of the transition
      get { return _currentState;}
      set { Transition (value); }
  }
  // protected state and bool can't be overridden
  protected State _currentState;
  protected bool _inTransition;

  public virtual T GetState<T> () where T: State
  {
      T target = GetComponent<T>();
      if (target == null)
        target = gameObject.AddComponent<T>();
      return target;
  }

  public virtual void ChangeState<T> () where T: State
  {
      CurrentState = GetState<T>();
  }

//transition method changes the states from current state to the nuw value
  protected virtual void Transition (State value)
  {
    //checks to see if the current state is different from the value or game is in transition
    // if not, set the game to start transitioning
      if(_currentState == value || _inTransition)
        return;

      _inTransition = true;

      //if the previous state exists, tell it to exit
      if(_currentState != null)
          _currentState.Exit();

      // set the value of the current state to the intended state
      _currentState = value;

      //if the new state exists, tell it to enter
      if(_currentState != null)
          _currentState.Enter();

      //end the transition protocol
      _inTransition = false;
  }

}
