using UnityEngine;
using System.Collections.Generic;

public class CircleSetObjects : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsList; // 生成するオブジェクトのリスト
    [SerializeField] private float radius = 5f; // 円の半径

    private void Start()
    {
        GenerateAndArrangeObjects();
    }

    // オブジェクトを円周上に生成する
    private void GenerateAndArrangeObjects()
    {
        int numberOfObjects = objectsList.Count;
        if (numberOfObjects == 0)
        {
            Debug.LogWarning("No objects to arrange.");
            return;
        }

        float angleStep = 360f / numberOfObjects;

        for (int i = 0; i < numberOfObjects; i++)
        {
            // 1. Instantiate the object
            GameObject obj = Instantiate(objectsList[i]);

            // 2. Set the parent
            obj.transform.SetParent(transform);

            // 3. Calculate the position
            float angle = i * angleStep * Mathf.Deg2Rad; // ラジアンに変換
            Vector3 localPosition = new Vector3(Mathf.Cos(angle), 10, Mathf.Sin(angle)) * radius;

            // 4. Move the object to the correct position
            obj.transform.localPosition = localPosition;
        }
    }
}









