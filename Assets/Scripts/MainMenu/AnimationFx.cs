using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationFx : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 baseScale;
    private Quaternion baseRotation;

    private bool isHover;

    [Header("Settings")]
    public float scaleAmount = 0.15f;
    public float speed = 8f;
    public float rotationAmount = 8f;

    void Start()
    {
        baseScale = transform.localScale;
        baseRotation = transform.localRotation;
    }

    void Update()
    {
        if (isHover)
        {
            float t = Mathf.Sin(Time.time * speed) * scaleAmount;

            
            Vector3 targetScale = baseScale + new Vector3(t, -t * 0.5f, 0);
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 10f);

           
            float rot = Mathf.Sin(Time.time * speed) * rotationAmount;
            Quaternion targetRot = Quaternion.Euler(0, 0, rot);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * 10f);
        }
        else
        {
           
            transform.localScale = Vector3.Lerp(transform.localScale, baseScale, Time.deltaTime * 10f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, baseRotation, Time.deltaTime * 10f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
    }
}