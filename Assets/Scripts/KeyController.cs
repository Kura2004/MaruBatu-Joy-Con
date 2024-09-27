using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyController : MonoBehaviour
{
    private void Update()
    {
        // ESCキーが押されたらアプリケーションを終了
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("アプリを終了します");
            Quit();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Saito.SoundManager.SoundManager.Instance.MuteBgm();
            Saito.SoundManager.SoundManager.Instance.MuteSe();
            Debug.Log("BGMとSEをミュートしました");
        }

        // Eキーが押されたらBGMとSEのミュートを解除
        if (Input.GetKeyDown(KeyCode.E))
        {
            Saito.SoundManager.SoundManager.Instance.UnMuteBgm();
            Saito.SoundManager.SoundManager.Instance.UnMuteSe();
            Debug.Log("BGMとSEのミュートを解除しました");
        }

        // デバッグ用
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ScenesLoader.Instance.LoadGameOver(0);
            Debug.Log("ゲームオーバー画面に行きます");
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
                    UnityEngine.Application.Quit();
#endif
    }
}
