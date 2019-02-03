using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    protected CharacterView characterView;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        characterView = GetComponent<CharacterView>();
    }    
}
