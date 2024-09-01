using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 100.0f;  // Скорость вращения камеры
    public float zoomSpeed = 10.0f;       // Скорость приближения/отдаления камеры
    public float minDistance = 1.0f;      // Минимальное расстояние до цели
    public float maxDistance = 10.0f;     // Максимальное расстояние до цели
    public float defaultDistance = 3.0f;  // Расстояние по умолчанию при фокусировке

    public Transform target;              // Цель фокусировки камеры

    private float currentDistance;        // Текущая дистанция до цели
    private Vector3 initialPosition;      // Начальная позиция камеры
    private Quaternion initialRotation;   // Начальная ориентация камеры
    private float initialDefaultDistance; // Изначальное значение defaultDistance

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialDefaultDistance = defaultDistance; // Сохраняем начальное значение defaultDistance
        currentDistance = defaultDistance;        // Устанавливаем начальную дистанцию
    }

    void Update()
    {
        if (target != null)
        {
            // Вращение камеры вокруг цели, если нажата правая кнопка мыши
            if (Input.GetMouseButton(1))
            {
                float horizontal = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                float vertical = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

                transform.RotateAround(target.position, Vector3.up, horizontal);
                transform.RotateAround(target.position, transform.right, -vertical);
            }

            // Приближение/отдаление камеры с помощью колесика мыши
            float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance - scroll, minDistance, maxDistance);

            // Обновляем defaultDistance в зависимости от прокрутки
            defaultDistance = currentDistance;

            // Обновление позиции камеры
            UpdateCameraPosition();
        }
    }

    public void FocusOnObject(Transform newTarget)
    {
        target = newTarget;
        currentDistance = defaultDistance; // Устанавливаем текущее расстояние на основе defaultDistance
        UpdateCameraPosition();            // Обновляем позицию камеры
        transform.LookAt(target);          // Поворачиваем камеру к цели
    }

    public void ResetCameraPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        target = null;

        // Сброс defaultDistance к начальному значению
        defaultDistance = initialDefaultDistance;
        currentDistance = defaultDistance;
    }

    private void UpdateCameraPosition()
    {
        if (target != null)
        {
            // Обновляем позицию камеры на основе текущего расстояния до цели
            Vector3 direction = (transform.position - target.position).normalized;
            transform.position = target.position + direction * currentDistance;
        }
    }
}
