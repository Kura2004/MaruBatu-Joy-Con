using UnityEngine;
using System.Collections.Generic;

public class GridPointObjectSpawner : MonoBehaviour
{
    /*[SerializeField] private GameObject objectToSpawn; // 生成するオブジェクト
    [SerializeField] private string generatorTag = "DefaultTag"; // 生成器のタグ
    [SerializeField] private int indexToUse = 0; // 使用する位置のインデクス
    [SerializeField] private Vector3 offset;

    bool isTrigger = false;

    private void LateUpdate()
    {
        if (!isTrigger)
        {
            SpawnObjectAtGridPoint();
            Destroy(this);
        }
    }

    private void SpawnObjectAtGridPoint()
    {
        // シーン内の全ての生成器を取得
        GridObjectGenerator[] generators = FindObjectsByType<GridObjectGenerator>(FindObjectsSortMode.None);
        // 指定したタグの生成器をリスト化
        List<GridObjectGenerator> generatorList = new List<GridObjectGenerator>();
        foreach (GridObjectGenerator generatorObj in generators)
        {
            if (generatorObj.IsTagMatching(generatorTag))
            {
                generatorList.Add(generatorObj);
            }
        }

        // 生成器が見つからない場合はエラーメッセージ
        if (generatorList.Count == 0)
        {
            Debug.LogError($"No GridObjectGenerator found with tag '{generatorTag}'");
            return;
        }

        // 指定したインデクスの位置を確認し、オブジェクトを生成
        foreach (GridObjectGenerator generator in generatorList)
        {
            if (indexToUse >= 0 && indexToUse < generator.GeneratedPositions.Count)
            {
                Transform spawnPosition = generator.GeneratedPositions[indexToUse];
                spawnPosition.localPosition += offset;
                if (objectToSpawn != null)
                {
                    Instantiate(objectToSpawn, spawnPosition.position, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("ObjectToSpawn is not assigned.");
                }
            }
            else
            {
                Debug.LogWarning($"Index {indexToUse} is out of range for generator with tag '{generatorTag}'.");
            }
        }
    }*/
}
