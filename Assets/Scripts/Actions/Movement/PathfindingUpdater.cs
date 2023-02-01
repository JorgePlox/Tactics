using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tactics.Core;

namespace Tactics.Movement{
public class PathfindingUpdater : MonoBehaviour
{
    private void Start() {
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
    }

    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }
}
}
