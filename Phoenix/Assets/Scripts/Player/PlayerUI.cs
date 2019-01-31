using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : PlayerComponent
{
    [SerializeField] private GameObject playerUIObj;
    private UIPlayerView uiPlayerView;

    public UIPlayerView GetPlayerUIView { get { return uiPlayerView; } }

    protected override void Start()
    {
        base.Start();

        GameObject ui = Instantiate(playerUIObj);
        ui.GetComponent<UIPlayerView>().SetupPlayerUI(playerView);

        uiPlayerView = ui.GetComponent<UIPlayerView>();
    }
}
