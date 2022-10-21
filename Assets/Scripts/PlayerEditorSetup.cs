using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEditor;
using UnityEngine;

public class PlayerEditorSetup : NetworkBehaviour
{
    private void Start()
    {
        if (Object.HasInputAuthority)
        {
            gameObject.name += " - Mine";
            Selection.activeObject = transform;
        }
        else
        {
            gameObject.name += " - Other";
        }
    }
}