using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible {
    void Interact(Action onInteractionComplete);
}
