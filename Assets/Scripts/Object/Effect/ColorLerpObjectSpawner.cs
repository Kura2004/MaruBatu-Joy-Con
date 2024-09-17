using DG.Tweening;
using UnityEngine.Pool;
using UnityEngine;

public class ColorLerpObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;       // 生成するオブジェクトのプレハブ
    [SerializeField] private float spawnInterval = 1f;       // 生成する間隔（秒）
    [SerializeField] private float spawnDuration = 5f;       // オートオフまでの時間（秒）
    [SerializeField] private float yOffset = 0f;             // 生成位置のY座標のオフセット
    [SerializeField] private Color startColor = Color.white; // 補完する最初の色
    [SerializeField] private Color endColor = Color.red;     // 補完する最後の色

    private ObjectPool<GameObject> objectPool;               // オブジェクトプール
    private bool isSpawning = false;                         // 生成のオン/オフ状態
    private Sequence spawnSequence;                          // 生成のシーケンス

    private void Start()
    {
        // ObjectPoolの初期化
        objectPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(prefabToSpawn),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            defaultCapacity: 10,
            maxSize: 20
        );
    }

    /// <summary>
    /// 生成を開始する
    /// </summary>
    public void StartSpawning()
    {
        if (isSpawning) return;

        isSpawning = true;

        // DOTweenシーケンスの作成
        spawnSequence = DOTween.Sequence()
            .AppendInterval(spawnInterval)
            .AppendCallback(() => SpawnObject())
            .SetLoops(-1, LoopType.Restart) // ループさせる
            .OnKill(() => isSpawning = false); // シーケンスが停止したときに生成をオフにする

        // 一定時間後に生成を自動的に停止
        DOVirtual.DelayedCall(spawnDuration, StopSpawning);
    }

    /// <summary>
    /// 生成を停止する
    /// </summary>
    public void StopSpawning()
    {
        if (spawnSequence != null && isSpawning)
        {
            spawnSequence.Kill(); // シーケンスを停止して破棄
        }
    }

    /// <summary>
    /// オブジェクトを生成する
    /// </summary>
    private void SpawnObject()
    {
        // 生成する位置を設定
        Vector3 spawnPosition = transform.position + new Vector3(0, yOffset, 0);

        // オブジェクトをプールから取得
        GameObject spawnedObject = objectPool.Get();

        // オブジェクトの位置を設定
        spawnedObject.transform.position = spawnPosition;

        // 時間に基づいて色を補完
        float t = (Time.time % spawnDuration) / spawnDuration;
        Color currentColor = Color.Lerp(startColor, endColor, t);

        // オブジェクトの色を設定
        SetObjectColor(spawnedObject, currentColor);

        // 一定時間後にオブジェクトをプールに戻す
        DOVirtual.DelayedCall(spawnDuration, () => objectPool.Release(spawnedObject));
    }

    /// <summary>
    /// オブジェクトの色を設定する
    /// </summary>
    private void SetObjectColor(GameObject obj, Color color)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    /// <summary>
    /// 補完色を設定する
    /// </summary>
    public void SetLerpColors(Color newStartColor, Color newEndColor)
    {
        startColor = newStartColor;
        endColor = newEndColor;
    }
}

