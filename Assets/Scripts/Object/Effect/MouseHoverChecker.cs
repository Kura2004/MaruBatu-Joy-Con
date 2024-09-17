using UnityEngine;

public class MouseHoverChecker : MonoBehaviour
{
    // マウスがオブジェクトに触れているかどうかを返すメソッド
    public bool IsMouseOver()
    {
        // マウスの位置からレイキャストを行うためのRayを取得
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // RaycastHit変数を宣言して、ヒット情報を取得
        RaycastHit hit;

        // レイキャストがオブジェクトにヒットしたかどうかを判定
        if (Physics.Raycast(ray, out hit))
        {
            // ヒットしたオブジェクトがこのスクリプトがアタッチされているオブジェクトかどうかを判定
            if (hit.collider.gameObject == gameObject)
            {
                return true; // マウスがオブジェクトに触れている
            }
        }
        return false; // マウスがオブジェクトに触れていない
    }
}

