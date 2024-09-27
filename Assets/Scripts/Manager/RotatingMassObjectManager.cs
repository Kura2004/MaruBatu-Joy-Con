using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class RotatingMassObjectManager : MonoBehaviour
{
    [SerializeField]
    private string massTag = "Mass"; // Tag名 (Inspectorで変更可能)

    [SerializeField]
    public float rotationDuration = 1f; // 回転にかかる時間（秒）

    [SerializeField]
    private float rotationDegrees = 90f; // 回転する角度（度）

    private GameObject[] mass = new GameObject[4];
    private int massIndex = 0;

    private Dictionary<MassPoint, GameObject> massPlaceholders = new Dictionary<MassPoint, GameObject>();

    private enum MassPoint
    {
        Point1,
        Point2,
        Point3,
        Point4
    }

    public bool AnyMassClicked()
    {
        foreach (var obj in mass)
        {
            if (obj != null && obj.GetComponent<ObjectColorChanger>()?.isClicked == true)
            {
                return true; // いずれかのオブジェクトがクリックされていた場合
            }
        }
        return false; // 全てのオブジェクトがクリックされていない場合
    }

    public bool isSelected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ButtonSelecter"))
        {
            isSelected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ButtonSelecter"))
        {
            isSelected = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        // TagがMassのオブジェクトに当たっている場合
        if (other.CompareTag(massTag))
        {
            // オブジェクトを登録
            mass[massIndex] = other.gameObject;
            massIndex = (massIndex + 1) % mass.Length;
        }
    }

    public void StartRotationLeft()
    {
        StartCoroutine(RotateObject(-rotationDegrees));
    }

    public void StartRotationRight()
    {
        StartCoroutine(RotateObject(rotationDegrees));
    }

    private IEnumerator RotateObject(float degrees)
    {
        if (GameStateManager.Instance.IsRotating) yield break;

        MakeObjectsChildren();

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, degrees, 0);
        transform.rotation = endRotation; // 最終的な回転を設定

        // オブジェクトの位置を対数関数的に補完して移動
        foreach (var entry in massPlaceholders)
        {
            MassPoint point = entry.Key;
            GameObject placeholder = entry.Value;
            int index = (int)point;

            if (mass[index] != null)
            {
                // 対数関数的なイージングを使って移動
                mass[index].transform.DOMove(placeholder.transform.position, rotationDuration)
                    .SetEase(Ease.InOutQuad); // ここでEaseを調整
            }
        }

        // 回転が終わったら親子関係をリセットし、回転完了処理を実行
        yield return new WaitForSeconds(rotationDuration);

        ResetParentRelationships();
        OnRotationComplete();
    }

    // オブジェクトの位置を更新するメソッドは不要になるので削除

    // MakeObjectsChildren, ResetParentRelationships, OnRotationComplete メソッドはそのまま使用

    private void MakeObjectsChildren()
    {
        if (GameStateManager.Instance.IsRotating) return;
        GameStateManager.Instance.StartRotation(); // 回転開始フラグを立てる

        for (int i = 0; i < mass.Length; i++)
        {
            if (mass[i] != null)
            {
                GameObject placeholder = new GameObject("MassPlaceholder" + i);
                placeholder.transform.position = mass[i].transform.position;
                placeholder.transform.rotation = mass[i].transform.rotation;

                placeholder.transform.SetParent(transform);
                massPlaceholders[(MassPoint)i] = placeholder;

                //mass[i].GetComponent<ObjectScaler>().IgnoreMouseInput();
            }
        }
    }

    private void ResetParentRelationships()
    {
        foreach (var obj in mass)
        {
            if (obj != null)
            {
                obj.transform.SetParent(null);
                //obj.GetComponent<ObjectScaler>().ResumeMouseInput();
            }
        }

        foreach (var placeholder in massPlaceholders.Values)
        {
            Destroy(placeholder);
        }

        massPlaceholders.Clear();
    }

    private void OnRotationComplete()
    {
        GameStateManager.Instance.EndRotation(); // 回転終了フラグを戻す
        TimeLimitController.Instance.ResetTimer();
        TimeLimitController.Instance.StartTimer();
        GameTurnManager.Instance.SetTurnChange(true);
        GameTurnManager.Instance.AdvanceTurn(); // ターンを進める
        ScenesAudio.SetSe();
        ObjectStateManager.Instance.MoveFirstObjectUpDown(true);
        ObjectStateManager.Instance.MoveSecondObjectUpDown(false);
    }
}