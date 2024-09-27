using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class JoyconInputTest : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private List<Joycon> joycons;

    void Start()
    {
        joycons = JoyconManager.Instance.j; // Joy-Conのリストを取得
    }

    void Update()
    {
        CheckJoyconInputs();
    }

    private void CheckJoyconInputs()
    {
        if (joycons == null || joycons.Count <= 0) return;

        for (int i = 0; i < joycons.Count; i++)
        {
            var joycon = joycons[i];

            // 各Joy-Conのボタンをチェック
            foreach (Joycon.Button button in Enum.GetValues(typeof(Joycon.Button)))
            {
                if (joycon.GetButtonDown(button))
                {
                    string joyconType = joycon.isLeft ? "Left" : "Right";
                    Debug.Log($"{joyconType} Joycon - {button} Pressed");
                    text.text = $"{joyconType} Joycon - {button} Pressed";
                }
            }

            // 各Joy-Conのスティックの傾きをチェック
            Vector2 stickInput = new Vector2(joycon.GetStick()[0], joycon.GetStick()[1]);
            if (stickInput != Vector2.zero) // スティックがニュートラル位置から動いている場合のみ
            {
                string joyconType = joycon.isLeft ? "Left" : "Right";
                Debug.Log($"{joyconType} Joycon - Stick X: {stickInput.x}, Y: {stickInput.y}");
                text.text = $"{joyconType} Joycon - Stick X: {stickInput.x}, Y: {stickInput.y}";
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