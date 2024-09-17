using UnityEngine;

public class FullscreenParticleSystem : MonoBehaviour
{
    public ParticleSystem particleSystem_;
    [SerializeField] private Camera targetCamera; // インスペクターで設定できるカメラ
    [SerializeField] private float distanceFromCamera = 5f;
    [SerializeField] private Vector3 cameraPositionOffset = new Vector3(0, 0, 0); // カメラ位置のオフセット

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main; // targetCameraが設定されていない場合、メインカメラを使用
        }

        // Particle Systemの位置をカメラの前に設定
        UpdateParticleSystemPositionAndSize();
        Destroy(this);
    }

    void UpdateParticleSystemPositionAndSize()
    {
        // カメラの位置にオフセットを適用してParticle Systemの位置を更新
        Vector3 adjustedCameraPosition = targetCamera.transform.position + cameraPositionOffset;
        particleSystem_.transform.position = adjustedCameraPosition + targetCamera.transform.forward * distanceFromCamera;

        // カメラの視野角に基づいてBoxのサイズを更新
        var shape = particleSystem_.shape;
        float height = 2f * distanceFromCamera * Mathf.Tan(targetCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float width = height * targetCamera.aspect;
        shape.scale = new Vector3(width, height, 1f);
    }
}





