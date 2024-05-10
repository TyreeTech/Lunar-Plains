using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tag that requires the RectTransform
[RequireComponent(typeof(RectTransform))]

RectTransform myRT;
RectTransform parentRT;


public class LayoutAnchor : MonoBehaviour
{

    void Awake()
    {
      // adds fields for the object and any parent object's RectTransform
      myRT = transform as RectTransform;
      parentRT = transform.parent as RectTransform;
      //if the event is not available in the heirarchy, error message;

      if(parentRT == null)
        Debug.LogError("This component requires a RectTransform parent to work.", gameObject);
    }
      // function that gets the offset
    Vector2 GetPosition(RectTransform rt, TextAnchor anchor)
    {
        Vector2 retValue = Vector2.zero;

        switch(anchor)
        {
          case TextAnchor.LowerCenter:
          case TextAnchor.MiddleCenter:
          case TextAnchor.UpperCenter:
            retValue.x += rt.rect.width * 0.5f;
            break;
          case TextAnchor.LowerRight:
          case TextAnchor.MiddleRight:
          case TextAnchor.UpperRight:
            retValue.x += rt.rect.width;
            break;
        }

        switch(anchor)
        {
          case TextAnchor.MiddLeft:
          case TextAnchor.MiddleCenter:
          case TextAnchor.MiddleRight:
              retValue.y += rt.rect.height * 0.5f;
              break;
          case TextAnchor.UpperLeft:
          case TextAnchor.UpperCenter:
          case TextAnchor.UpperRight:
              retValue.y += rt.rect.height;
              break;
        }

        return retValue;
    }

    public Vector2 AnchorPosition(TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset)
    {
        Vector2 myOffset = GetPosition(myRT, myAnchor);
        Vector2 parentOffset = GetPosition(parentRT, parentAnchor);
        Vector2 anchorCenter = new Vector2(Mathf.Lerp(myRT.anchorMin.x, myRT.anchorMax.x, myRT.pivot.x),
      Mathf.Lerp(myRT.anchorMin.y, myRT.anchorMax.y, myRT.pivot.y));
        Vector2 myAnchorOffset = new Vector2(parentRT.rect.width * anchorCenter.x,
      parentRT.rect.height * anchorCenter.y);
        Vector2 myPivotOffset = new Vector2(myRT.rect.width * myRT.pivot.x,
      myRT.rect.height * myRT.pivot.y);
        Vector2 pos = parentOffset - myAnchorOffset - myOffset + myPivotOffset + offset;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = MAthf.RoundToInt(pos.y);

        return pos;
    }

    //FUTURE TYREE, NEXT IS SnapToAnchorPosi funciton!!!
}