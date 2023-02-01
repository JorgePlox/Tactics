using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tactics.Core;

namespace Tactics.Actions{
public class InteractAction : BaseAction
{
    private int maxInteractDistance = 1;

    private void Update() {
        if (!isActive)
        {
           return; 
        }
    }
    public override string GetActionName()
    {
        return "Interact";
    }

    public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetvalidActionsGridPositionList()
    {
        List<GridPosition> validGridpositionList = new List<GridPosition>();
        
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
        {
            for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxInteractDistance)
                {
                    continue;
                }
                
                IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);
                if (interactable == null)
                {
                    continue;
                }

                validGridpositionList.Add(testGridPosition);
            }
        }

        return validGridpositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
        interactable.Interact(onInteractComplete);
        ActionStart(onActionComplete);
    }

    private void onInteractComplete()
    {
        ActionComplete();
    }
}
}
