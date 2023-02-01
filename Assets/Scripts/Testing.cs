using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tactics.Core;

public class Testing : MonoBehaviour
{


    private void Start()
    {

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TurnSystem.Instance.NextTurn();
        }
    }
}
