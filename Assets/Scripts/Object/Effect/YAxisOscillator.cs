using UnityEngine;

public class YAxisOscillator : MonoBehaviour
{
    [SerializeField]
    private float frequency = 1f; // 周波数（揺れの速さ）

    [SerializeField]
    private float amplitude = 1f; // 振幅（揺れの大きさ）

    [SerializeField]
    private float initialYRange = 1f; // 初期Y座標の範囲（ランダム性を追加）

    private float initialY; // 初期Y座標
    private float time;

    private void Start()
    {
        // ランダムな初期Y座標を設定
        initialY = transform.position.y + Random.Range(-initialYRange, initialYRange);
    }

    private void Update()
    {
        // 時間に基づいてY座標を計算
        time += Time.deltaTime;
        float newY = initialY + Mathf.Sin(time * frequency) * amplitude;

        // 計算したY座標をオブジェクトに適用
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    /// <summary>
    /// 周波数を設定する
    /// </summary>
    public void SetFrequency(float newFrequency)
    {
        frequency = newFrequency;
    }

    /// <summary>
    /// 振幅を設定する
    /// </summary>
    public void SetAmplitude(float newAmplitude)
    {
        amplitude = newAmplitude;
    }

    /// <summary>
    /// 初期Y座標の範囲を設定する
    /// </summary>
    public void SetInitialYRange(float range)
    {
        initialYRange = range;
    }
}

