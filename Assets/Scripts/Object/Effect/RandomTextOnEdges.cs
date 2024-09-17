using UnityEngine;
using TMPro;

public class RandomTextOnEdges : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TMP_FontAsset font; // 使用するフォント
    [SerializeField] private int textLength = 5; // ランダム文字列の長さ
    [SerializeField] private Color textColor = Color.black; // テキストの色
    [SerializeField] private float textChangeInterval = 1f; // 文字が変わる間隔 (秒)
    [SerializeField] private float fontSize = 40f; // フォントサイズ

    private TextMeshPro textMeshPro;
    private float timeSinceLastChange;
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    void Start()
    {
        // テキストオブジェクトを生成
        GameObject textObject = new GameObject("EdgeText");
        textObject.transform.SetParent(transform);
        textObject.transform.localPosition = Vector3.zero;
        textObject.transform.Rotate(90, 0, 0);

        textMeshPro = textObject.AddComponent<TextMeshPro>();
        textMeshPro.font = font;
        textMeshPro.fontSize = fontSize; // フォントサイズを設定
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.color = textColor;

        // 初期文字を設定
        UpdateRandomText();
    }

    void Update()
    {
        timeSinceLastChange += Time.deltaTime;

        if (ShouldUpdateText())
        {
            UpdateRandomText();
            timeSinceLastChange = 0;
        }
    }

    private bool ShouldUpdateText()
    {
        // 条件式をメソッド化
        return timeSinceLastChange >= textChangeInterval && !TimeControllerToggle.isTimeStopped;
    }

    private void UpdateRandomText()
    {
        // ランダムな文字列を生成
        string randomText = GenerateRandomString(textLength);

        // テキストを更新
        textMeshPro.text = randomText;
    }

    private string GenerateRandomString(int length)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        System.Random random = new System.Random();
        for (int i = 0; i < length; i++)
        {
            sb.Append(chars[random.Next(chars.Length)]);
        }
        return sb.ToString();
    }
}
