using UnityEngine;
using System.Collections.Generic;

public class CircularSinWaveMotion : MonoBehaviour
{
    [SerializeField] private List<GameObject> targetPrefabList; // ¶¬‚·‚éƒ^[ƒQƒbƒg‚ÌƒvƒŒƒnƒuƒŠƒXƒg
    [SerializeField] private Transform centerPoint; // ’†S“_
    [SerializeField] private float baseRadius = 5f; // Šî–{‚Ì‰~‚Ì”¼Œa
    [SerializeField] private float amplitude = 1f; // ã‰º‰^“®‚ÌU•
    [SerializeField] private float baseFrequency = 1f; // Šî–{‚Ìã‰º‰^“®‚Ìü”g”
    [SerializeField] private float radiusFrequency = 1f; // ”¼Œa‚Ì•Ï‰»‚Ìü”g”
    [SerializeField] private float rotationSpeed = 1f; // ‰~‰^“®‚Ì‘¬“x

    private Transform[] targetArray;
    private Vector3[] initialPositions;

    void Start()
    {
        int numberOfTargets = targetPrefabList.Count;
        targetArray = new Transform[numberOfTargets];
        initialPositions = new Vector3[numberOfTargets];

        GenerateTargets(numberOfTargets);
        InitializeTargetPositions(numberOfTargets);
    }

    void Update()
    {
        AnimateTargets();
    }

    // ƒ^[ƒQƒbƒg‚ğ¶¬
    private void GenerateTargets(int numberOfTargets)
    {
        for (int i = 0; i < numberOfTargets; i++)
        {
            GameObject newTarget = Instantiate(targetPrefabList[i]);
            targetArray[i] = newTarget.transform;
        }
    }

    // ƒ^[ƒQƒbƒg‚ğ‰~üã‚É‹Ï“™‚É”z’u
    private void InitializeTargetPositions(int numberOfTargets)
    {
        for (int i = 0; i < numberOfTargets; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfTargets;
            Vector3 newPosition = centerPoint.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * baseRadius;
            initialPositions[i] = newPosition;
            targetArray[i].position = newPosition;
        }
    }

    // ƒ^[ƒQƒbƒg‚Ìã‰º‰^“®‚Æ‰~‰^“®‚ğXV
    private void AnimateTargets()
    {
        for (int i = 0; i < targetArray.Length; i++)
        {
            Transform target = targetArray[i];
            if (target != null)
            {
                float time = Time.time;
                float frequency = baseFrequency * (1 + i * 0.1f); // ƒCƒ“ƒfƒbƒNƒX‚É‚æ‚Á‚Äü”g”‚ğ’²®

                // ã‰º‰^“®
                float verticalOffset = Mathf.Sin(time * frequency) * amplitude * 0.3f;

                // ”¼Œa‚Ì•Ï‰»
                float dynamicRadius = baseRadius + Mathf.Sin(time * radiusFrequency) * amplitude;

                // ‰~‰^“®
                float angle = i * Mathf.PI * 2 / targetArray.Length + time * rotationSpeed;
                Vector3 offset = new Vector3(Mathf.Cos(angle), verticalOffset, Mathf.Sin(angle)) * dynamicRadius;
                target.position = centerPoint.position + offset;
            }
        }
    }
}





