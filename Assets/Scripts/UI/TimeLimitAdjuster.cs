using UnityEngine;
using TMPro; // TextMeshProを使用するための名前空間
using System.Collections;

public class TimeLimitAdjuster : MonoBehaviour
{
    [SerializeField] private TMP_Text valueDisplay;   // TextMeshProを使用するテキスト
    [SerializeField] private float cooldownDuration = 1f; // クールダウン時間

    public static int timeLimit = 30; // 調整するゲームの制限時間
    private bool canAdjustTime = true;   // タイム調整を受け付けるかどうか
    private bool inputCooldownActive = false; // 入力クールダウンのフラグ

    private Joycon leftJoycon;
    private Joycon rightJoycon;

    [System.Obsolete]
    private void Awake()
    {
        // 他のインスタンスが既に存在する場合は、現在のインスタンスを破棄する
        if (FindObjectsOfType<TimeLimitAdjuster>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); // シーン切り替え時にオブジェクトを維持
        }
    }

    private void Start()
    {
        // 初期値を表示
        UpdateValueDisplay();

        // Joy-Conの初期化
        var joycons = JoyconManager.Instance.j;
        if (joycons.Count >= 2)
        {
            leftJoycon = joycons.Find(c => c.isLeft);
            rightJoycon = joycons.Find(c => !c.isLeft);
        }
    }

    private void Update()
    {
        // 操作を受け付けない場合は処理しない
        if (inputCooldownActive) return;

        // Lのスティックの入力を取得
        if (leftJoycon != null)
        {
            float verticalInput = leftJoycon.GetStick()[0]; // スティックのY値を取得

            // 縦軸の入力が0以外のときのみ処理
            if (Mathf.Abs(verticalInput) > 0.01f && canAdjustTime)
            {
                AdjustTimeLimit(verticalInput > 0 ? 10 : -10); // 十の位を増減
                StartInputCooldown(); // クールダウンを開始
            }
        }
    }

    int prevTimeLimit = 0;
    private void AdjustTimeLimit(int amount)
    {
        if (!canAdjustTime) return;

        timeLimit += amount;
        timeLimit = Mathf.Clamp(timeLimit, 20, 40); // 最小値20、最大値40に制限

        if (timeLimit != prevTimeLimit)
        {
            ScenesAudio.ClickSe();
        }

        UpdateValueDisplay();

        // タイム調整を一定時間無効にする
        StartCoroutine(DisableTimeAdjustmentTemporarily());
        prevTimeLimit = timeLimit;
    }

    private void UpdateValueDisplay()
    {
        valueDisplay.text = timeLimit.ToString("F0") + "s"; // 整数で表示
    }

    // クールダウンを開始する
    private IEnumerator DisableTimeAdjustmentTemporarily()
    {
        canAdjustTime = false;
        yield return new WaitForSeconds(cooldownDuration); // 一定時間待つ
        canAdjustTime = true;
    }

    // 入力のクールダウンを開始
    private void StartInputCooldown()
    {
        inputCooldownActive = true;
        StartCoroutine(ResetInputCooldown());
    }

    private IEnumerator ResetInputCooldown()
    {
        yield return new WaitForSeconds(cooldownDuration);
        inputCooldownActive = false;
    }
}
