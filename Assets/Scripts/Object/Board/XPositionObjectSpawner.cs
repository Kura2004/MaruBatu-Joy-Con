using UnityEngine;

public class XPositionObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab; // 生成するオブジェクトのプレハブ

    [SerializeField]
    private float xSpacing = 2f; // x方向の間隔

    [SerializeField]
    private int objectCount = 4; // 生成するオブジェクトの数

    private void Start()
    {
        SpawnObjects();
        //配置が終わったら削除
        Destroy(this);
    }

    private void SpawnObjects()
    {
        // 配列の中央を0とするためのオフセットを計算
        float xOffset = (objectCount - 1) * xSpacing / 2f;

        for (int i = 0; i < objectCount; i++)
        {
            // オフセットを引いて中心に揃える
            Vector3 position = new Vector3(i * xSpacing - xOffset, 0, 0);
            Instantiate(prefab, position, Quaternion.identity);
        }
    }
}


