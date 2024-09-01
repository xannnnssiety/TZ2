using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 100.0f;  // �������� �������� ������
    public float zoomSpeed = 10.0f;       // �������� �����������/��������� ������
    public float minDistance = 1.0f;      // ����������� ���������� �� ����
    public float maxDistance = 10.0f;     // ������������ ���������� �� ����
    public float defaultDistance = 3.0f;  // ���������� �� ��������� ��� �����������

    public Transform target;              // ���� ����������� ������

    private float currentDistance;        // ������� ��������� �� ����
    private Vector3 initialPosition;      // ��������� ������� ������
    private Quaternion initialRotation;   // ��������� ���������� ������
    private float initialDefaultDistance; // ����������� �������� defaultDistance

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialDefaultDistance = defaultDistance; // ��������� ��������� �������� defaultDistance
        currentDistance = defaultDistance;        // ������������� ��������� ���������
    }

    void Update()
    {
        if (target != null)
        {
            // �������� ������ ������ ����, ���� ������ ������ ������ ����
            if (Input.GetMouseButton(1))
            {
                float horizontal = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                float vertical = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

                transform.RotateAround(target.position, Vector3.up, horizontal);
                transform.RotateAround(target.position, transform.right, -vertical);
            }

            // �����������/��������� ������ � ������� �������� ����
            float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance - scroll, minDistance, maxDistance);

            // ��������� defaultDistance � ����������� �� ���������
            defaultDistance = currentDistance;

            // ���������� ������� ������
            UpdateCameraPosition();
        }
    }

    public void FocusOnObject(Transform newTarget)
    {
        target = newTarget;
        currentDistance = defaultDistance; // ������������� ������� ���������� �� ������ defaultDistance
        UpdateCameraPosition();            // ��������� ������� ������
        transform.LookAt(target);          // ������������ ������ � ����
    }

    public void ResetCameraPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        target = null;

        // ����� defaultDistance � ���������� ��������
        defaultDistance = initialDefaultDistance;
        currentDistance = defaultDistance;
    }

    private void UpdateCameraPosition()
    {
        if (target != null)
        {
            // ��������� ������� ������ �� ������ �������� ���������� �� ����
            Vector3 direction = (transform.position - target.position).normalized;
            transform.position = target.position + direction * currentDistance;
        }
    }
}
