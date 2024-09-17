using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VignetteColorBlender))]
public class VignetteColorBlenderEditor : Editor
{
    private void OnEnable()
    {
        // 編集中にプロパティが変更されるたびにこのメソッドが呼ばれる
        serializedObject.Update();
    }

    public override void OnInspectorGUI()
    {
        // プロパティを描画
        serializedObject.Update();

        // インスペクターで表示するプロパティを描画
        DrawDefaultInspector();

        VignetteColorBlender blender = (VignetteColorBlender)target;

        // プロパティが変更された場合にUpdateVignetteColorを呼び出す
        if (GUI.changed)
        {
            blender.UpdateVignetteColor();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
