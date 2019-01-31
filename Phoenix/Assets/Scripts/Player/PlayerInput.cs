using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : PlayerComponent
{
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
        deselectInput = Input.GetMouseButton(1);
    }
}
