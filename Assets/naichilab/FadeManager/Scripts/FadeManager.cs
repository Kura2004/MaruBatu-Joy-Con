using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// シーン遷移時のフェードイン・アウトを制御するためのクラス.
/// </summary>
public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    /// <summary>
    /// デバッグモード.
    /// </summary>
    public bool DebugMode = true;
    /// <summary>フェード中の透明度</summary>
    private float fadeAlpha = 0;
    /// <summary>フェード中かどうか</summary>
    private bool isFading = false;
    /// <summary>フェード色</summary>
    public Color fadeColor = Color.black;

    /// <summary>
    /// 現在シーンがロード中かどうか.
    /// </summary>
    public bool IsLoadingScene { get; private set; } = false;

    public void OnGUI()
    {
        // Fade.
        if (this.isFading)
        {
            // 色と透明度を更新して白テクスチャを描画.
            this.fadeColor.a = this.fadeAlpha;
            GUI.color = this.fadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }

        if (this.DebugMode)
        {
            if (!this.isFading)
            {
                // Scene一覧を作成.
                List<string> scenes = new List<string>
                {
                    "SampleScene"
                    // 他のシーン名をここに追加
                };

                // Sceneが一つもない.
                if (scenes.Count == 0)
                {
                    GUI.Box(new Rect(10, 10, 200, 50), "Fade Manager(Debug Mode)");
                    GUI.Label(new Rect(20, 35, 180, 20), "Scene not found.");
                    return;
                }

                GUI.Box(new Rect(10, 10, 300, 50 + scenes.Count * 25), "Fade Manager(Debug Mode)");
                GUI.Label(new Rect(20, 30, 280, 20), "Current Scene : " + SceneManager.GetActiveScene().name);

                int i = 0;
                foreach (string sceneName in scenes)
                {
                    if (GUI.Button(new Rect(20, 55 + i * 25, 100, 20), "Load Level"))
                    {
                        LoadScene(sceneName, 1.0f);
                    }
                    GUI.Label(new Rect(125, 55 + i * 25, 1000, 20), sceneName);
                    i++;
                }
            }
        }
    }

    /// <summary>
    /// 画面遷移.
    /// </summary>
    /// <param name='scene'>シーン名</param>
    /// <param name='interval'>暗転にかかる時間(秒)</param>
    public void LoadScene(string scene, float interval)
    {
        StartCoroutine(TransScene(scene, interval));
    }

    /// <summary>
    /// シーン遷移用コルーチン.
    /// </summary>
    /// <param name='scene'>シーン名</param>
    /// <param name='interval'>暗転にかかる時間(秒)</param>
    private IEnumerator TransScene(string scene, float interval)
    {
        // だんだん暗く.
        this.isFading = true;
        float time = 0;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            yield return null;
        }

        // シーンのロード中であることを示す
        SetLoadingSceneStatus(true);

        // シーンを非同期で追加ロード.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false; // 自動的にシーンが有効化されるのを防ぐ

        // シーンのロードが完了するのを待つ.
        while (!asyncLoad.isDone)
        {
            // シーンのロードが完了しているかどうかを確認.
            if (asyncLoad.progress >= 0.9f)
            {
                // フェードアウトを完了させる.
                this.fadeAlpha = 1f;
                // 新しいシーンをアクティブにする.
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }


        // だんだん明るく.
        time = 0;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            time += Time.deltaTime;
            yield return null;
        }

        this.isFading = false;
        SetLoadingSceneStatus(false);
    }

    /// <summary>
    /// シーンのロード中状態を設定.
    /// </summary>
    /// <param name="status">ロード中状態</param>
    public void SetLoadingSceneStatus(bool status)
    {
        IsLoadingScene = status;
    }
}
