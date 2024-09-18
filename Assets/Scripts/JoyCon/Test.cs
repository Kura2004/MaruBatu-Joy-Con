using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class Test : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    void Update()
    {
        SwitchControllerAnyKeyDown();
    }

    private void SwitchControllerAnyKeyDown()
    {
        if (Input.anyKeyDown)
        {
            foreach (SwitchController code in Enum.GetValues(typeof(SwitchController)))
            {
                if (Input.GetKeyDown((KeyCode)code))
                {
                    Debug.Log(code);
                    text.text = code.ToString();
                }
                
            }
        }
    }
}

public enum SwitchController
{
    A = 350,
    B = 352,
    X = 351,
    Y = 353,
    UpArrow = 372,
    LeftArrow = 370,
    RightArrow = 373,
    DownArrow = 371,
    LStick = 380,
    RStick = 361,
    L = 384,
    R = 364,
    ZL = 385,
    ZR = 365,
    LeftSL = 374,
    LeftSR = 375,
    RightSL = 354,
    RightSR = 355,
    Minus = 378,
    Plus = 359,
    HOME = 362,
    Capture = 383
}