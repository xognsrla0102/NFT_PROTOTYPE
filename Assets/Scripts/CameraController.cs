using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float cameraSpd;

    private float xAngle;
    private float yAngle;
    private float cameraRot;

    public void OnDrag(PointerEventData eventData)
    {
        xAngle = eventData.delta.x * Time.deltaTime * cameraSpd;
        yAngle = eventData.delta.y * Time.deltaTime * cameraSpd;

        cameraRot -= yAngle;
        cameraRot = Mathf.Clamp(cameraRot, -90f, 90f);
        cameraTransform.localEulerAngles = Vector3.right * cameraRot;

        playerTransform.Rotate(0, xAngle, 0);
    }
}