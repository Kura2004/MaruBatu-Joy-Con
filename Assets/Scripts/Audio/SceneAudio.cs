using Saito.SoundManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesAudio : MonoBehaviour
{
    private void OnEnable()
    {
        // シーンが読み込まれたときに呼び出されるイベントに登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // イベントの登録解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーンが読み込まれたときに呼び出されるメソッド
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーンの名前に応じてBGMを再生
        switch (scene.name)
        {
            case "StartMenu":
                PlayStartMenuBgm();
                break;
            case "GameOver":
                //PlayGameOverBgm();
                break;
            case "4×4":
                PlayGameBgm();
                PauseBgm();
                break;
            default:
                Debug.LogWarning("Unknown scene name: " + scene.name);
                break;
        }
    }

    public static void PlayGameBgm()
    {
        SoundManager.Instance.PlayBgm(SoundManager.BgmSoundData.BGM.Play);
        Debug.Log("4×4のBGMを再生します");
    }

    public static void PauseBgm()
    {
        SoundManager.Instance.PauseBgm();
    }

    public static void UnPauseBgm()
    {
        SoundManager.Instance.UnPauseBgm();
        Debug.Log("BGMを再生します");
    }

    private void PlayGameOverBgm()
    {
        SoundManager.Instance.PlayBgm(SoundManager.BgmSoundData.BGM.GameOver);
    }

    private void PlayStartMenuBgm()
    {
        SoundManager.Instance.PlayBgm(SoundManager.BgmSoundData.BGM.Title);
    }

    public static void ClickSe()
    {
        SoundManager.Instance.PlaySe(SoundManager.SeSoundData.SE.Clicked);
    }

    public static void WinSe()
    {
        SoundManager.Instance.PlaySe(SoundManager.SeSoundData.SE.Win);
    }

    public static void SetSe()
    {
        SoundManager.Instance.PlaySe(SoundManager.SeSoundData.SE.Set);
    }

    public static void ShootSe()
    {
        SoundManager.Instance.PlaySe(SoundManager.SeSoundData.SE.Shoot);
    }

    public static void RestartSe()
    {
        SoundManager.Instance.PlaySe(SoundManager.SeSoundData.SE.Restart);
    }

    public static void FallSe()
    {
        SoundManager.Instance.PlaySe(SoundManager.SeSoundData.SE.Fall);
    }


    public static void HeartSe()
    {
        SoundManager.Instance.PlaySe(SoundManager.SeSoundData.SE.Heart);
    }

    public static void BlockedSe()
    {
        SoundManager.Instance.PlaySe(SoundManager.SeSoundData.SE.Blocked);
    }

    // 特定のSEをミュートする
    public static void MuteSpecificSe(SoundManager.SeSoundData.SE se)
    {
        SoundManager.Instance.MuteSpecificSe(se);
    }

    // 特定のSEのミュートを解除する
    public static void UnMuteSpecificSe(SoundManager.SeSoundData.SE se)
    {
        SoundManager.Instance.UnMuteSpecificSe(se);
    }

    // 特定のSEのループ再生をオンにする
    public static void SetSeLoop(SoundManager.SeSoundData.SE se)
    {
        SoundManager.Instance.SetSeLoop(se);
    }

    // 特定のSEのループ再生をオフにする
    public static void UnsetSeLoop(SoundManager.SeSoundData.SE se)
    {
        SoundManager.Instance.UnsetSeLoop(se);
    }
}
