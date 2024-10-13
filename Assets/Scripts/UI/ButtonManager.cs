using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

[System.Serializable]
public class ButtonData
{
    // ボタン本体の参照
    public Button button;

    // ボタンのタグ
    public string tag;
}

public class ButtonManager : MonoBehaviour
{
    // 複数のボタンとタグのペアをインスペクターで設定できるようにするリスト
    public List<ButtonData> buttons = new List<ButtonData>();

    // ボタンがロックされているかどうかを管理するフラグ
    public bool isLocked = false;

    public bool onGuide = false;

    [SerializeField] string specialTag;

    void Start()
    {
        // 登録された各ボタンにリスナーを設定
        foreach (ButtonData buttonData in buttons)
        {
            if (buttonData.button != null)
            {
                // ボタンがクリックされたときにOnButtonClickedメソッドを呼び出す
                buttonData.button.onClick.AddListener(() => OnButtonClicked(buttonData));
            }
        }

        onGuide = false;
    }

    // ボタンがクリックされたときの処理
    private void OnButtonClicked(ButtonData buttonData)
    {
        // ボタンがロックされている場合、クリック処理を無視
        if (isLocked)
        {
            Debug.Log("Buttons are locked, no action performed.");
            return;
        }

        // ロックされていない場合にボタンがクリックされた時の処理を実行
        Debug.Log($"Button {buttonData.button.name} with tag {buttonData.tag} clicked!");
        // ここでボタンごとに異なる処理を実装する
    }

    // ボタンをロックするメソッド
    public void LockButtons()
    {
        isLocked = true;
        foreach (ButtonData buttonData in buttons)
        {
            buttonData.button.interactable = buttonData.tag == specialTag;
        }
    }

    // ボタンのロックを解除するメソッド
    public void UnlockButtons()
    {
        isLocked = false;
        foreach (ButtonData buttonData in buttons)
        {
            buttonData.button.interactable = true; // ボタンのインタラクションを有効にする
        }
    }

    // ボタンのロック状態をトグルするメソッド
    public void ToggleLockButtons()
    {
        if (isLocked)
        {
            UnlockButtons(); // ロック解除
        }
        else
        {
            LockButtons(); // ボタンをロック
        }
    }

    public void ToggleGuide()
    {
        onGuide = !onGuide;
    }
}
