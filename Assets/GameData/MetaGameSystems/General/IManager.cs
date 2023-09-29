using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager {
    Guid instanceId { get; }
    
    void init();
}