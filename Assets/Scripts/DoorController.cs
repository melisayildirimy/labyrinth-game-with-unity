using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    public float openSpeed = 0.75f;

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, 90, 0) * closedRotation; // y ekseninde 90 derece döndürme
    }

    private void Update()
    {
        if (isOpen)
        {
            // Kapýyý aç
            transform.rotation = Quaternion.Lerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
        }
        else
        {
            // Kapýyý kapat
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, Time.deltaTime * openSpeed);
        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }
}