using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridObjectGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPrefabs; // 生成するオブジェクトのプレハブの配列
    [SerializeField] private int rows = 5; // 格子の行数 (n)
    [SerializeField] private int columns = 5; // 格子の列数 (m)
    [SerializeField] private float spacing = 1.0f; // 格子点の間隔
    [SerializeField] private Transform gridReferenceTransform; // 格子点の基準となるTransform
    [SerializeField] private float spawnInterval = 2f; // n秒ごとの間隔
    private bool onGenerate = false;
    private List<GameObject> generatedObjects = new List<GameObject>(); // 生成されたオブジェクトを保持

    private void Start()
    {
        GenerateGrid();
    }
    private void Update()
    {
        if (GameTurnManager.Instance.IsGameStarted && !onGenerate)
        {
            onGenerate = true;
            StartCoroutine(ActivateObjects()); // コルーチンでオブジェクトを順次表示
        }
    }

    private void GenerateGrid()
    {
        if (gridReferenceTransform == null)
        {
            Debug.LogError("GridReferenceTransform is not assigned.");
            return;
        }

        if (objectPrefabs.Length == 0)
        {
            Debug.LogError("ObjectPrefabs array is empty.");
            return;
        }

        // 格子点の群の中心を基準にする
        Vector3 gridCenter = gridReferenceTransform.position;

        // プレハブのインデックスを追跡するための変数
        int prefabIndex = 0;

        // すべてのオブジェクトを生成してRendererをオフにする
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                // 格子点の位置を計算
                Vector3 position = gridCenter + new Vector3((column - (columns - 1) / 2f) * spacing, 0, (row - (rows - 1) / 2f) * spacing);

                // プレハブを取得し生成
                GameObject prefab = objectPrefabs[prefabIndex];
                if (prefab != null)
                {
                    GameObject newObj = Instantiate(prefab, position, Quaternion.identity);
                    newObj.SetActive(false);
                    generatedObjects.Add(newObj);
                }
                else
                {
                    Debug.LogWarning($"Prefab at index {prefabIndex} is not assigned.");
                }

                // インデックスを更新（要素数が `objectPrefabs.Length` でリセット）
                prefabIndex = (prefabIndex + 1) % objectPrefabs.Length;
            }
        }
    }

    private IEnumerator ActivateObjects()
    {
        foreach (GameObject obj in generatedObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
                ScenesAudio.SetSe(); // サウンドエフェクトを再生
                yield return new WaitForSeconds(spawnInterval); // 次のオブジェクトを表示するまで待機
            }
        }

        //配置が終わったら削除
        Destroy(this);
    }
}
