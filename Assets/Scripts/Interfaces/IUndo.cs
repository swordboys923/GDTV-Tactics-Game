using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUndo {
    public event EventHandler<BaseAction> OnUndo;
    public void Undo();
} 

