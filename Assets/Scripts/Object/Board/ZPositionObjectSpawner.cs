using UnityEngine;

public class ZPositionObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab; // 生成するオブジェクトのプレハブ

    [SerializeField]
    private float zSpacing = 2f; // z方向の間隔

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
        float zOffset = (objectCount - 1) * zSpacing / 2;

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 position = new Vector3(0, 0, i * zSpacing - zOffset);
            Quaternion rotation = Quaternion.Euler(0, 90, 0); // y軸回りに90度回転
            Instantiate(prefab, position, rotation);
        }
    }
}

