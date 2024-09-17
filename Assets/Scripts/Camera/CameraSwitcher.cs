using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

[System.Serializable]
public class CameraMapping
{
    public CameraSwitcher.CameraType cameraType;
    public CinemachineVirtualCamera camera;
}

public class CameraSwitcher : SingletonMonoBehaviour<CameraSwitcher>
{
    public enum CameraType
    {
        Main,
        Secondary,
        Action,
        // 必要に応じて他のカメラタイプを追加
    }

    [Header("Camera Settings")]
    [SerializeField] private List<CameraMapping> cameraMappings = new List<CameraMapping>();
    [SerializeField] private CameraType defaultCamera = CameraType.Main;

    private Dictionary<CameraType, CinemachineVirtualCamera> cameraDictionary;

    protected override void OnEnable()
    {
        InitializeCameraDictionary();
        base.OnEnable();
    }

    private void InitializeCameraDictionary()
    {
        cameraDictionary = new Dictionary<CameraType, CinemachineVirtualCamera>();

        foreach (var mapping in cameraMappings)
        {
            if (mapping.camera != null)
            {
                cameraDictionary[mapping.cameraType] = mapping.camera;
            }
        }
    }

    private void Start()
    {
        // デフォルトのカメラをアクティブにする
        SwitchCamera(defaultCamera);
    }

    /// <summary>
    /// カメラを切り替えるメソッド
    /// </summary>
    /// <param name="cameraType">切り替えるカメラのタイプ</param>
    public void SwitchCamera(CameraType cameraType)
    {
        // 全てのカメラを非アクティブにする
        foreach (var cam in cameraDictionary.Values)
        {
            cam.gameObject.SetActive(false);
        }

        // 指定されたカメラだけをアクティブにする
        if (cameraDictionary.TryGetValue(cameraType, out var camera))
        {
            camera.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// カメラリストにカメラを追加するメソッド
    /// </summary>
    /// <param name="camera">追加するカメラ</param>
    public void AddCamera(CameraType type, CinemachineVirtualCamera camera)
    {
        if (camera != null)
        {
            if (!cameraDictionary.ContainsKey(type))
            {
                cameraDictionary[type] = camera;
                cameraMappings.Add(new CameraMapping { cameraType = type, camera = camera });
            }
        }
    }

    /// <summary>
    /// カメラリストからカメラを削除するメソッド
    /// </summary>
    /// <param name="camera">削除するカメラ</param>
    public void RemoveCamera(CinemachineVirtualCamera camera)
    {
        if (camera != null)
        {
            var entry = cameraMappings.Find(mapping => mapping.camera == camera);
            if (entry != null)
            {
                cameraDictionary.Remove(entry.cameraType);
                cameraMappings.Remove(entry);
            }
        }
    }
}
