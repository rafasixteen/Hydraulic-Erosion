using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        private float cameraDistance = 40f;
        private readonly float cameraRotationSpeed = 2f;
        private float cameraZoomSpeed = 10.0f;
        private Vector3 cameraOffset = new(0, 2f, -5f);
        private Vector3 lookAt = new(0, 0, 0);

        private float currentRotationX = 0f;
        private float currentRotationY = 0f;
        private float cameraMaxDst = 40f;

        private void UpdateCameraVariables(int terrainSize)
        {
            cameraMaxDst = terrainSize * 0.4f;
            cameraZoomSpeed = terrainSize * 0.2f;
            cameraDistance = cameraMaxDst;
        }

        private void Update()
        {
            float smoothFactor = Time.deltaTime * 5f;
            bool isRightMouseButtonPressed = Input.GetMouseButton(1);

            if (isRightMouseButtonPressed)
            {
                float mouseX = Input.GetAxis("Mouse X");
                currentRotationX += mouseX * cameraRotationSpeed;

                float mouseY = Input.GetAxis("Mouse Y");
                currentRotationY -= mouseY * cameraRotationSpeed;
                currentRotationY = Mathf.Clamp(currentRotationY, -10f, 20f);
            }

            Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0f);

            cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * cameraZoomSpeed;
            cameraDistance = Mathf.Clamp(cameraDistance, 0.1f, cameraMaxDst);

            Vector3 desiredPosition = lookAt + rotation * cameraOffset * cameraDistance;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothFactor);

            transform.LookAt(lookAt);
        }

        private void Awake()
        {
            EventManager.OnTerrainChanged += UpdateCameraVariables;
        }

        private void OnDestroy()
        {
            EventManager.OnTerrainChanged -= UpdateCameraVariables;
        }
    }
}