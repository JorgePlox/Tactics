using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tactics.Core{
public interface IInteractable
{
    void Interact(Action onInteractionComplete);
}
}
