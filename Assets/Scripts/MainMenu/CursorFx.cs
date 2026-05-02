using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFx : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject fxCursorTrail;

    [Header("Movement")]
    [SerializeField] float followSpeed = 8f;   // скорость догоняния
    [SerializeField] float smoothness = 10f;   // плавность

    [Header("Noise / Life")]
    [SerializeField] float noiseAmount = 0.1f; // случайное дрожание
    [SerializeField] float waveAmount = 0.05f; // амплитуда волны
    [SerializeField] float waveSpeed = 5f;     // скорость волны

    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current == null || fxCursorTrail == null) return;

        Vector3 targetPos = GetMouseWorldPosition();

       
        targetPos += GetWaveOffset();
        targetPos += GetNoiseOffset();

     
        fxCursorTrail.transform.position = Vector3.Lerp(
            fxCursorTrail.transform.position,
            targetPos,
            smoothness * Time.deltaTime
        );
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();

        Vector3 screenPos = new Vector3(mouseScreen.x, mouseScreen.y, 5f);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);

        worldPos.z = -3f;
        return worldPos;
    }

    Vector3 GetWaveOffset()
    {
        float x = Mathf.Sin(Time.time * waveSpeed) * waveAmount;
        float y = Mathf.Cos(Time.time * waveSpeed * 1.3f) * waveAmount;

        return new Vector3(x, y, 0f);
    }

    Vector3 GetNoiseOffset()
    {
        return Random.insideUnitSphere * noiseAmount;
    }
}