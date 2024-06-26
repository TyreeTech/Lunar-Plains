using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformAnchorPositionTweener : Vector3Tweener
{
    // Start is called before the first frame update
    RectTransform rt;

    protected override void Awake()
    {
        base.Awake();
        rt = transform as RectTransform;
    }

    protected override void OnUpdate(object sender, System.EventArgs e)
    {
        base.OnUpdate (sender, e);
        rt.anchoredPosition = currentValue;
    }
}
