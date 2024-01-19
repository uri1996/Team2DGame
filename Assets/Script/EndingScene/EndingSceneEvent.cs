using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndingSceneEvent
{
    public static Action EndingListScrollOver;
    public static void CallEndingListScrollOver()
    {
        EndingListScrollOver?.Invoke();
    }
}
