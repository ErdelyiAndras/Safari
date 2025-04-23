using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Camera minimapCamera;
    public CameraMovement cameraMovement;
    public RectTransform minimapRectTransform;

    private Vector3 cameraOffset = new Vector3(-8f, 0f, -15f);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localCursor))
            return;

        Rect rect = minimapRectTransform.rect;

        float normalizedX = (localCursor.x - rect.x) / rect.width;
        float normalizedY = (localCursor.y - rect.y) / rect.height;

        Ray ray = minimapCamera.ViewportPointToRay(new Vector3(normalizedX, normalizedY, 0));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            
            Vector3 targetPos = new Vector3(
                hit.point.x + cameraOffset.x,
                cameraMovement.gameCamera.transform.position.y + cameraOffset.y,
                hit.point.z + cameraOffset.z
            );

            cameraMovement.gameCamera.transform.position = targetPos;
        }
    }
}