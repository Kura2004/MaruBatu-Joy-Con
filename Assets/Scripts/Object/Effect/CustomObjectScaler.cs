using UnityEngine;

public class CustomObjectScaler : ObjectScaler
{
    private void Update()
    {
        // 基本クラスの機能を呼び出し、オブジェクトのサイズをチェックする
        if (IsObjectSizeReset())
        {
            ResetObjectSize();
        }
    }
}

