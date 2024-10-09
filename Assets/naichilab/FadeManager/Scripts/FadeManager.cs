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
                    "StartMenu",
                    "4×4",
                    "GameOver",
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
                        LoadScene(sceneName, 1.0f, this.fadeColor);
                    }
                    GUI.Label(new Rect(125, 55 + i * 25, 1000, 20), sceneName);
                    i++;
                }
            }
        }
    }

    // 画面遷移.
    // <param name='scene'>シーン名</param>
    // <param name='interval'>暗転にかかる時間(秒)</param>
    // <param name='fadeColor'>フェードに使用する色</param>
    public void LoadScene(string scene, float interval, Color fadeColor)
    {
        this.fadeColor = fadeColor; // 受け取ったフェード色をセット
        StartCoroutine(TransScene(scene, interval));
    }

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

        // 非同期で読み込まれているシーンをすべてアンロード
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);

            if (loadedScene.isLoaded && loadedScene.name != SceneManager.GetActiveScene().name)
            {
                // 非アクティブなシーンをアンロード
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(loadedScene);
                while (!unloadOperation.isDone)
                {
                    yield return null;
                }
            }
        }

        // 新しいシーンを非同期でロード.
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
