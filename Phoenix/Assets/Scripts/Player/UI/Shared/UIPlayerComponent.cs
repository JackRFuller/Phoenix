using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerComponent : MonoBehaviour
{
    protected UIPlayerView uiView;

    protected virtual void Start()
    {
        uiView = GetComponentInParent<UIPlayerView>();
    }
}
