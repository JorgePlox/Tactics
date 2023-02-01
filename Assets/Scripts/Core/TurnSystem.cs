using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Core{
public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance;
    public event EventHandler OnTurnChanged;
    private int turnNumber = 0;
    private bool isPlayerTurn = true;
    [SerializeField] private Unit selectedUnit;

    private void Awake() 
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one TurnSystem! " + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        
    }
    public void NextTurn()
    {
        selectedUnit = UnitManager.Instance.CheckTurnOrder();
        turnNumber ++;
        isPlayerTurn = !selectedUnit.IsEnemy();

        SetSelectedUnit(selectedUnit);
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    private void SetSelectedUnit(Unit selectedUnit)
    {
        if (isPlayerTurn)
        {
            UnitActionSystem.Instance.SetSelectedUnit(selectedUnit);
        }
        else if(!isPlayerTurn)
        {

        }
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
}
