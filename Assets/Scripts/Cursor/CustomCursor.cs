using UnityEngine;
using UnityEngine.Pool;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private GameObject imagePrefab; // 生成する画像のプレハブ
    [SerializeField] private float initialScale = 0.5f; // 画像の初期スケール
    [SerializeField] private Vector3 initialRotation = Vector3.zero; // 画像の初期回転角度
    [SerializeField] private float spawnInterval = 0.1f; // 生成間隔
    [SerializeField] private float yOffset = 1f; // 画像のY座標オフセット
    [SerializeField] private string targetTag = "Target"; // Raycastが当たるターゲットのタグ

    private bool isSpawning = false; // 生成中かどうかのフラグ
    private float nextSpawnTime = 0f; // 次に生成する時間

    // ObjectPoolのインスタンス
    private ObjectPool<GameObject> pool;

    private void Start()
    {
        // ObjectPoolの初期化
        pool = new ObjectPool<GameObject>(
            // オブジェクトを生成する方法
            createFunc: () => Instantiate(imagePrefab),

            // オブジェクトをプールから取り出す際に呼ばれるアクション
            actionOnGet: (obj) => obj.SetActive(true),

            // オブジェクトをプールに戻す際に呼ばれるアクション
            actionOnRelease: (obj) => obj.SetActive(false),

            // オブジェクトをプールに戻す際に呼ばれるアクション
            actionOnDestroy: (obj) => Destroy(obj),

            // プールの初期容量
            defaultCapacity: 10,

            // プールの最大容量
            maxSize: 20
        );
    }

    private void Update()
    {
        // マウスボタンが押されている間
        if (Input.GetMouseButton(0))
        {
            if (!isSpawning)
            {
                isSpawning = true;
                nextSpawnTime = Time.time;
            }

            // 指定した間隔でオブジェクトを生成
            if (Time.time >= nextSpawnTime)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    // タグが一致する場合にのみオブジェクトを生成
                    if (hit.collider.CompareTag(targetTag))
                    {
                        // ヒットした位置にオブジェクトを生成
                        SpawnImage(hit.point);
                    }
                }
                nextSpawnTime = Time.time + spawnInterval;
            }
        }

        // マウスボタンが離されたら生成を停止
        if (Input.GetMouseButtonUp(0))
        {
            isSpawning = false;
        }
    }

    private void SpawnImage(Vector3 position)
    {
        // Y座標を調整
        Vector3 spawnPosition = new Vector3(position.x, yOffset, position.z);

        // プールからオブジェクトを取得
        GameObject newImage = pool.Get();

        // 位置と回転を設定
        newImage.transform.position = spawnPosition;
        newImage.transform.rotation = Quaternion.Euler(initialRotation);

        // 初期のスケールを設定
        newImage.transform.localScale = Vector3.one * initialScale;
    }

    private void OnDestroy()
    {
        // プールを破棄
        pool.Clear();
    }
}
