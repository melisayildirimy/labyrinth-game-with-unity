using UnityEngine;

public class ChestController : MonoBehaviour
{
    public Transform movablePart; // Hareket eden parça için referans ekledik
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    public float openSpeed = 1.25f;

    private void Start()
    {
        // Baþlangýç rotasyonunu ayarla
        closedRotation = movablePart.rotation;
        openRotation = Quaternion.Euler(0, 0, 90) * closedRotation;  // z ekseninde 90 derece döndürme
    }

    private void Update()
    {
        if (isOpen)
        {
            // Sandýðý aç
            movablePart.rotation = Quaternion.Lerp(movablePart.rotation, openRotation, Time.deltaTime * openSpeed);
        }
        else
        {
            // Sandýðý kapat
            movablePart.rotation = Quaternion.Lerp(movablePart.rotation, closedRotation, Time.deltaTime * openSpeed);
        }
    }

    public void ToggleChest()
    {
        isOpen = !isOpen;
    }
}
