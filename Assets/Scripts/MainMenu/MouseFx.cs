using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFx : MonoBehaviour
{
    [SerializeField] bool isEnabled = true;

    [SerializeField] float followTime = 5f;
    [SerializeField] float pauseTime = 10f;

    [SerializeField] float followSpeed = 10f;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;

        if (isEnabled)
            StartCoroutine(FxLoop());
    }

    IEnumerator FxLoop()
    {
        while (isEnabled)
        {
        
            yield return StartCoroutine(FollowMouse());

         
            yield return new WaitForSeconds(pauseTime);
        }
    }

    IEnumerator FollowMouse()
    {
        float timer = 0f;

        while (timer < followTime)
        {
            timer += Time.deltaTime;

            Vector3 mousePos = GetMouseWorldPosition();

           
            transform.position = Vector3.Lerp(
                transform.position,
                mousePos,
                followSpeed * Time.deltaTime
            );

            yield return null;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        Vector3 screenPos = new Vector3(mouseScreenPos.x, mouseScreenPos.y, 5f);

        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
       // worldPos.z = 0f;

        return worldPos;
    }
}