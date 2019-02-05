using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Transform diceTransform;
    private Rigidbody diceRB;

    // Start is called before the first frame update
    void Start()
    {
        diceTransform = GetComponent<Transform>();
        diceRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //GetDiceValue();
    }

    private void GetDiceValue()
    {
        if (diceRB.velocity == Vector3.zero)
            Debug.Log(ReturnDiceValue());
    }   

    private float ReturnDiceValue()
    {
        float diceValue = 0;

        if (Vector3.Dot(diceTransform.forward, Vector3.up) > 0.6f)
            diceValue = 1;
        if (Vector3.Dot(-diceTransform.forward, Vector3.up) > 0.6f)
            diceValue = 6;
        if (Vector3.Dot(diceTransform.up, Vector3.up) > 0.6f)
            diceValue = 5;
        if (Vector3.Dot(-diceTransform.up, Vector3.up) > 0.6f)
            diceValue = 2;
        if (Vector3.Dot(diceTransform.right, Vector3.up) > 0.6f)
            diceValue = 4;
        if (Vector3.Dot(-diceTransform.right, Vector3.up) > 0.6f)
            diceValue = 3;

        return diceValue;
    }
}
