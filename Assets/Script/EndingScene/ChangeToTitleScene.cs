using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChangeToTitleScene : MonoBehaviour
{
    private bool isCanPressKey;
    private void Start()
    {
        isCanPressKey = false;
    }
    private void Update()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            if (isCanPressKey)
            {
                if (gamepad.buttonSouth.wasPressedThisFrame)
                {
                    SceneManager.LoadScene("TitleScene");
                }
            }
        }
    }
    private void OnEnable()
    {
        EndingSceneEvent.EndingListScrollOver += SetIsCanPressKey_True;
    }
    private void OnDisable()
    {
        EndingSceneEvent.EndingListScrollOver -= SetIsCanPressKey_True;
    }

    void SetIsCanPressKey_True()
    {
        isCanPressKey = true;
    }
}
