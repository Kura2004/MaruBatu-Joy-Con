using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTweenを使用するための名前空間

public class DebugButtonSelector : MonoBehaviour
{
    [SerializeField] private ButtonManager buttonManager; // ButtonManagerの参照
    [SerializeField] private float inputCooldown = 0.5f; // ボタン切り替え後のクールダウン時間
    [SerializeField] private float colorTransitionDuration = 0.3f; // ボタンの色が変わるまでの時間
    [SerializeField] private float scaleMultiplier = 1.2f; // 選択時のボタンのスケール倍率
    [SerializeField] private Color selectedColor = Color.green; // 選択されているときのボタンの色
    [SerializeField] private Color defaultColor = Color.white; // デフォルトのボタンの色
    [SerializeField] private List<ButtonTagPair> buttonTagPairs; // インデックスとタグのペアリスト

    private int currentIndex = 0; // 現在選択されているボタンのインデックス
    private bool canInput = true; // 入力可能かどうかを管理
    private Dictionary<int, Vector3> originalScales = new Dictionary<int, Vector3>(); // ボタンの元のローカルスケールを保存
    private List<Button> buttonList = new List<Button>(); // ボタンを格納するリスト

    [SerializeField] VolumeSettings volume;

    private void Start()
    {
        // 各ボタンの元のローカルスケールを保存
        if (buttonManager != null && buttonManager.buttons.Count > 0)
        {
            // buttonListを初期化して、buttonManagerのボタンをコピー
            buttonList = new List<Button>(new Button[buttonManager.buttons.Count]);

            // ボタンとタグの対応を設定
            foreach (var pair in buttonTagPairs)
            {
                for (int i = 0; i < buttonManager.buttons.Count; i++)
                {
                    if (buttonManager.buttons[i].tag == pair.tag)
                    {
                        buttonList[pair.index] = buttonManager.buttons[i].button;
                        break;
                    }
                }
            }

            // 各ボタンの元のローカルスケールを保存
            for (int i = 0; i < buttonList.Count; i++)
            {
                if (buttonList[i] != null)
                {
                    originalScales[i] = buttonList[i].transform.localScale;
                }
            }

            // 初期のボタンを選択状態にする
            UpdateButtonSelection();
        }
    }

    private void Update()
    {
        // Enterキーが押されたときに現在のボタンを実行する
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentIndex >= 0 && currentIndex < buttonList.Count && buttonList[currentIndex] != null)
            {
                buttonList[currentIndex].onClick.Invoke();
            }
            StartCooldown(); // クールダウン開始
        }

        if (!canInput || buttonManager == null || buttonManager.onGuide) return;

        // HorizontalとVerticalの入力を取得
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // ボタンを切り替える処理
        if (verticalInput > 0.5f)
        {
            SelectPreviousButton();
            StartCooldown(); // クールダウン開始
        }
        else if (verticalInput < -0.5f)
        {
            SelectNextButton();
            StartCooldown(); // クールダウン開始
        }

        // Enterキーが押されたときに現在のボタンを実行する
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentIndex >= 0 && currentIndex < buttonList.Count && buttonList[currentIndex] != null)
            {
                buttonList[currentIndex].onClick.Invoke();
            }
            StartCooldown(); // クールダウン開始
        }

        // ボリュームコントロール処理
        if (volume != null)
        {
            if (!volume.onSeVolume)
                volume.AddBgmVolume(horizontalInput * 0.01f);
            else
                volume.AddSeVolume(horizontalInput * 0.01f);
        }
    }

    // 前のボタンを選択
    private void SelectPreviousButton()
    {
        currentIndex = (currentIndex - 1 + buttonList.Count) % buttonList.Count; // インデックスの範囲を循環
        UpdateButtonSelection();
    }

    // 次のボタンを選択
    private void SelectNextButton()
    {
        currentIndex = (currentIndex + 1) % buttonList.Count; // インデックスの範囲を循環
        UpdateButtonSelection();
    }

    // 現在選択されているボタンの表示を更新
    private void UpdateButtonSelection()
    {
        if (buttonList == null || buttonList.Count == 0) return; // Null チェック

        for (int i = 0; i < buttonList.Count; i++)
        {
            Button button = buttonList[i];
            if (button == null) continue; // Null チェック

            Transform buttonTransform = button.transform;

            // 現在のボタンを選択状態にする
            if (i == currentIndex)
            {
                button.GetComponent<Image>().DOColor(selectedColor, colorTransitionDuration);
                Vector3 targetScale = originalScales[i] * scaleMultiplier;
                buttonTransform.DOScale(targetScale, colorTransitionDuration);
            }
            else
            {
                button.GetComponent<Image>().DOColor(defaultColor, colorTransitionDuration);
                buttonTransform.DOScale(originalScales[i], colorTransitionDuration);
            }
        }
    }

    // 入力を一定時間無効にする
    private void StartCooldown()
    {
        canInput = false; // 入力を無効にする
        DOVirtual.DelayedCall(inputCooldown, () =>
        {
            canInput = true; // クールダウンが終了したら入力を再度有効にする
        });
    }
}