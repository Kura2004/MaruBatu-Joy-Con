using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Saito.SoundManager
{
    /// <summary>
    /// 音を統合的に管理
    /// </summary>
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        #region Singleton

        private void Awake()
        {
            // AudioSource の初期化
            InitializeAudioSources();
        }

        #endregion

        [Header("音のボリューム")]
        [SerializeField, Range(0.0f, 1.0f), Tooltip("マスタ-音量")]
        public float masterVolume = 1.0f;

        [SerializeField, Range(0.0f, 1.0f), Tooltip("BGMのマスタ音量")]
        public float bgmMasterVolume = 1.0f;

        [SerializeField, Range(0.0f, 1.0f), Tooltip("SEのマスタ音量")]
        public float seMasterVolume = 1.0f;

        [SerializeField, Tooltip("再生したいBgmをここで設定")]
        List<BgmSoundData> bgmSoundDatas;

        [SerializeField, Tooltip("再生したいSeをここで設定")]
        List<SeSoundData> seSoundDatas;

        private AudioSource bgmAudioSource;
        private Dictionary<SeSoundData.SE, AudioSource> seAudioSources = new Dictionary<SeSoundData.SE, AudioSource>();

        private Dictionary<SeSoundData.SE, bool> seMuteStates = new Dictionary<SeSoundData.SE, bool>(); // SEのミュート状態を管理
        private Dictionary<SeSoundData.SE, bool> seLoopStates = new Dictionary<SeSoundData.SE, bool>(); // SEのループ状態を管理

        private void InitializeAudioSources()
        {
            // BGM用 AudioSource の生成
            bgmAudioSource = gameObject.AddComponent<AudioSource>();

            // SE用 AudioSource の生成
            foreach (var seData in seSoundDatas)
            {
                var seAudioSource = gameObject.AddComponent<AudioSource>();
                seAudioSources[seData.se] = seAudioSource;
            }
        }

        /// <summary>
        /// BGMのマスタ音量を設定
        /// </summary>
        public void SetBgmMasterVolume(float volume)
        {
            bgmMasterVolume = Mathf.Clamp(volume, 0.0f, 1.0f);
            UpdateBgmVolume();
        }

        /// <summary>
        /// SEのマスタ音量を設定
        /// </summary>
        public void SetSeMasterVolume(float volume)
        {
            seMasterVolume = Mathf.Clamp(volume, 0.0f, 1.0f);
            UpdateSeVolumes();
        }

        /// <summary>
        /// BGMの音量を更新
        /// </summary>
        private void UpdateBgmVolume()
        {
            if (bgmAudioSource != null && bgmAudioSource.clip != null)
            {
                BgmSoundData data = bgmSoundDatas.Find(d => d.audioClip == bgmAudioSource.clip);
                if (data != null)
                {
                    bgmAudioSource.volume = Mathf.Clamp(data.volume * bgmMasterVolume * masterVolume, 0.0f, 1.0f);
                }
            }
        }

        /// <summary>
        /// SEの音量を更新
        /// </summary>
        private void UpdateSeVolumes()
        {
            foreach (var kvp in seAudioSources)
            {
                var se = kvp.Key;
                var audioSource = kvp.Value;

                if (se != null)
                {
                    SeSoundData data = seSoundDatas.Find(d => d.se == se);
                    if (data != null)
                    {
                        audioSource.volume = Mathf.Clamp(data.volume * seMasterVolume * masterVolume, 0.0f, 1.0f);
                    }
                }
            }
        }

        /// <summary>
        /// BGMを再生
        /// </summary>
        public void PlayBgm(BgmSoundData.BGM bgm)
        {
            if (bgmAudioSource == null)
            {
                Debug.LogError("BgmのAudioSourceが設定されていません。");
                return;
            }

            BgmSoundData data = bgmSoundDatas.Find(data => data.bgm == bgm);
            if (data == null)
            {
                Debug.LogError("指定されたラベルが見つかりません。設定し忘れていませんか？");
                return;
            }

            bgmAudioSource.volume = Mathf.Clamp(data.volume * bgmMasterVolume * masterVolume, 0.0f, 1.0f);
            bgmAudioSource.clip = data.audioClip;

            if (bgmAudioSource.clip == null)
            {
                Debug.LogError(bgm + "のclipが設定されていません。");
                return;
            }

            bgmAudioSource.loop = true;
            bgmAudioSource.Play();
            Debug.Log(data.audioClip.name + "効果音を鳴らしました");
        }

        /// <summary>
        /// BGMの再生をやめる
        /// </summary>
        public void StopBgm(BgmSoundData.BGM bgm)
        {
            if (bgmAudioSource == null)
            {
                Debug.LogError("BgmのAudioSourceが設定されていません。");
                return;
            }

            BgmSoundData data = bgmSoundDatas.Find(data => data.bgm == bgm);
            if (data == null)
            {
                Debug.LogError("指定されたラベルが見つかりません。設定し忘れていませんか？");
                return;
            }

            bgmAudioSource.Stop();
            bgmAudioSource.clip = null;
            Debug.Log("BGMを鳴らすのをやめました");
        }

        /// <summary>
        /// BGMの再生を一時停止する
        /// </summary>
        public void PauseBgm()
        {
            if (bgmAudioSource == null)
            {
                Debug.LogError("BgmのAudioSourceが設定されていません。");
                return;
            }

            if (bgmAudioSource.clip == null)
            {
                Debug.LogError("BGMのAudioClipが設定されていません");
                return;
            }

            bgmAudioSource.Pause();
        }

        /// <summary>
        /// BGMを一時停止した場所から再生する
        /// </summary>
        public void UnPauseBgm()
        {
            if (bgmAudioSource == null)
            {
                Debug.LogError("BgmのAudioSourceが設定されていません。");
                return;
            }

            if (bgmAudioSource.clip == null)
            {
                Debug.LogError("BGMのAudioClipが設定されていません");
                return;
            }

            bgmAudioSource.UnPause();
        }

        /// <summary>
        /// BGMの音量を0にする（ミュート）
        /// </summary>
        public void MuteBgm()
        {
            if (bgmAudioSource != null)
            {
                bgmAudioSource.mute = true;
                Debug.Log("BGMをミュートしました");
            }
        }

        /// <summary>
        /// BGMのミュートを解除する
        /// </summary>
        public void UnMuteBgm()
        {
            if (bgmAudioSource != null)
            {
                bgmAudioSource.mute = false;
                Debug.Log("BGMのミュートを解除しました");
            }
        }

        /// <summary>
        /// SEを再生
        /// </summary>
        public void PlaySe(SeSoundData.SE se)
        {
            if (!seAudioSources.TryGetValue(se, out var seAudioSource))
            {
                Debug.LogError("指定されたSEのAudioSourceが設定されていません。");
                return;
            }

            SeSoundData data = seSoundDatas.Find(data => data.se == se);
            if (data == null)
            {
                Debug.LogError("指定されたラベルが見つかりません。設定し忘れていませんか？");
                return;
            }

            if (IsSeMuted(se))
            {
                Debug.Log("指定されたSEはミュートされています。");
                return;
            }

            seAudioSource.volume = Mathf.Clamp(data.volume * seMasterVolume * masterVolume, 0.0f, 1.0f);
            if (data.audioClip == null)
            {
                Debug.LogError(se + "のclipが設定されていません。");
                return;
            }

            seAudioSource.PlayOneShot(data.audioClip);
            Debug.Log(data.audioClip.name + "効果音を鳴らしました");
        }

        /// <summary>
        /// SEの再生をやめる
        /// </summary>
        public void StopSe(SeSoundData.SE se)
        {
            if (!seAudioSources.TryGetValue(se, out var seAudioSource))
            {
                Debug.LogError("指定されたSEのAudioSourceが設定されていません。");
                return;
            }

            seAudioSource.Stop();
            seAudioSource.clip = null;
            Debug.Log("効果音を鳴らすのをやめました");
        }

        /// <summary>
        /// SEの音量を0にする（ミュート）
        /// </summary>
        public void MuteSe()
        {
            foreach (var kvp in seAudioSources)
            {
                kvp.Value.mute = true;
            }
            Debug.Log("SEをミュートしました");
        }

        /// <summary>
        /// SEのミュートを解除する
        /// </summary>
        public void UnMuteSe()
        {
            foreach (var kvp in seAudioSources)
            {
                kvp.Value.mute = false;
            }
            Debug.Log("SEのミュートを解除しました");
        }

        /// <summary>
        /// 特定のSEをミュートする
        /// </summary>
        public void MuteSpecificSe(SeSoundData.SE se)
        {
            if (!seMuteStates.ContainsKey(se))
            {
                seMuteStates.Add(se, true);
                Debug.Log($"{se} SEをミュートしました");
            }
        }

        /// <summary>
        /// 特定のSEのミュートを解除する
        /// </summary>
        public void UnMuteSpecificSe(SeSoundData.SE se)
        {
            if (seMuteStates.ContainsKey(se))
            {
                seMuteStates[se] = false;
                Debug.Log($"{se} SEのミュートを解除しました");
            }
        }

        /// <summary>
        /// 特定のSEがミュートされているか確認する
        /// </summary>
        private bool IsSeMuted(SeSoundData.SE se)
        {
            return seMuteStates.ContainsKey(se) && seMuteStates[se];
        }

        /// <summary>
        /// 特定のSEのループ再生をオンにする
        /// </summary>
        public void SetSeLoop(SeSoundData.SE se)
        {
            if (!seLoopStates.ContainsKey(se))
            {
                seLoopStates.Add(se, true);
                Debug.Log($"{se} SEのループ再生をオンにしました");
            }
            else
            {
                seLoopStates[se] = true;
                Debug.Log($"{se} SEのループ再生をオンにしました");
            }

            if (seAudioSources.TryGetValue(se, out var seAudioSource))
            {
                SeSoundData data = seSoundDatas.Find(d => d.se == se);
                if (data != null)
                {
                    seAudioSource.volume = Mathf.Clamp(data.volume * seMasterVolume * masterVolume, 0.0f, 1.0f);
                    seAudioSource.clip = data.audioClip;

                    if (seAudioSource.clip != null)
                    {
                        seAudioSource.loop = true;
                        seAudioSource.Play();
                    }
                }
            }
        }

        /// <summary>
        /// 特定のSEのループ再生をオフにする
        /// </summary>
        public void UnsetSeLoop(SeSoundData.SE se)
        {
            if (!seLoopStates.ContainsKey(se))
            {
                seLoopStates.Add(se, false);
                Debug.Log($"{se} SEのループ再生をオフにしました");
            }
            else
            {
                seLoopStates[se] = false;
                Debug.Log($"{se} SEのループ再生をオフにしました");
            }

            if (seAudioSources.TryGetValue(se, out var seAudioSource))
            {
                seAudioSource.loop = false;
                seAudioSource.Stop();
                seAudioSource.clip = null;
            }
        }

        /* Seは再生を一時停止を利用することが少ないのでコメントアウトしておきます
        /// <summary>
        /// Seの再生を一時停止する
        /// </summary>
        public void PauseSe()
        {
            foreach (var seAudioSource in seAudioSources.Values)
            {
                seAudioSource.Pause();   
            }
        }
        /// <summary>
        /// Seを一時停止した場所から再生する
        /// </summary>
        public void UnPauseSe()
        { 
            foreach (var seAudioSource in seAudioSources.Values)
            {
                seAudioSource.UnPause();
            }
        }
        */

        /// <summary>
        /// BGMを登録・調整するクラス
        /// </summary>
        [Serializable]
        public class BgmSoundData
        {
            /// <summary>
            /// 用途に応じたラベルを設定
            /// </summary>
            public enum BGM
            {
                Home,
                GameOver,
                Title,
                Play,
            }

            [Tooltip("音の種類をラベルで設定")]
            public BGM bgm;
            [Tooltip("使用したい音を設定")]
            public AudioClip audioClip;
            [Range(0.0f, 1.0f), Tooltip("音量")] public float volume = 1.0f;
        }

        /// <summary>
        /// SEを登録・調整するクラス
        /// </summary>
        [Serializable]
        public class SeSoundData
        {
            /// <summary>
            /// 用途に応じたラベルを設定
            /// </summary>
            public enum SE
            {
                Win,
                Lose,
                Clicked,
                Highlited,
                Taped,
                Set,
                Shoot,
                Restart,
                Fall,
                Heart,
                Blocked,
            }

            [Tooltip("音の種類をラベルで設定")]
            public SE se;
            [Tooltip("使用したい音を設定")]
            public AudioClip audioClip;
            [Range(0.0f, 1.0f), Tooltip("音量")] public float volume = 1.0f;
        }
    }
}
