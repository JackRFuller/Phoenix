using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : PlayerComponent
{
    public event Action PlayerCancelled;

    private bool selectInput;
    private bool deselectInput;

    public bool SelectInput { get { return selectInput; } }
    public bool DeselectInput { get { return deselectInput; } }

    #region UnityMethods

    private void Update()
    {
        DetectSelectInput();
        DetectDeSelectInput();
    }

    #endregion

    private void DetectSelectInput()
    {
        selectInput = Input.GetMouseButton(0);
    }

    private void DetectDeSelectInput()
    {
        deselectInput = Input.GetMouseButtonDown(1);

        if(deselectInput)
        {
            if (PlayerCancelled != null)
                PlayerCancelled();
        }           
    }
}
