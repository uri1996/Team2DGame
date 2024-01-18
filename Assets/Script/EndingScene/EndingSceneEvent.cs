using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndingSceneEvent
{
    public static Action PressTextWakeUp;
    public static void CallPressTextWakeUp()
    {
        PressTextWakeUp?.Invoke();
    }


    public static Action CanChangeToNextScene;
    public static void CallCanChangeToNextScene()
    {
        CanChangeToNextScene?.Invoke();
    }
}
