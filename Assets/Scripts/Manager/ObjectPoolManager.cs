using UnityEngine.Pool;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    // プールするオブジェクトのプレハブ
    [SerializeField] private GameObject prefab;

    // ObjectPoolのインスタンス
    private ObjectPool<GameObject> pool;

    private void Start()
    {
        // ObjectPoolの初期化
        pool = new ObjectPool<GameObject>(
            // オブジェクトを生成する方法
            createFunc: () => Instantiate(prefab),

            // オブジェクトをプールから取り出す際に呼ばれるアクション
            actionOnGet: (obj) => obj.SetActive(true),

            // オブジェクトをプールに戻す際に呼ばれるアクション
            actionOnRelease: (obj) => obj.SetActive(false),

            // プールをクリアする際に呼ばれるアクション
            actionOnDestroy: (obj) => Destroy(obj),

            // プールの初期容量
            defaultCapacity: 10,

            // プールの最大容量
            maxSize: 20
        );
    }

    // オブジェクトをプールから取得するメソッド
    public GameObject GetPooledObject()
    {
        return pool.Get();
    }

    // オブジェクトをプールに戻すメソッド
    public void ReleasePooledObject(GameObject obj)
    {
        pool.Release(obj);
    }
}

