using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 7f; // Etkileþim için maksimum mesafe
    private InteractableObject currentInteractable; // Þu anki etkileþimli nesne

    private void Update()
    {
        // Etkileþimli nesneleri kontrol et
        CheckForInteractable();

        // Eðer bir etkileþimli nesne varsa ve E tuþuna basýldýysa etkileþim gerçekleþtir
        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact();
        }
    }

    public void CheckForInteractable()
    {
        RaycastHit hit;
        InteractableObject closestInteractable = null;
        float closestDistance = interactionDistance;

        // Sahnedeki tüm etkileþimli nesneleri tarayarak en yakýn olaný bul
        foreach (var obj in FindObjectsOfType<InteractableObject>())
        {
            // Raycast kullanarak oyuncunun önündeki nesneleri kontrol et
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance) && hit.collider.gameObject == obj.gameObject)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = obj;
                }
            }
            else
            {
                // Eðer nesne etkileþim mesafesinde deðilse vurgulamayý kaldýr
                obj.ResetHighlight();
            }
        }

        // Eðer yeni tespit edilen nesne önceki nesneden farklýysa önceki nesnenin vurgusunu kaldýr
        if (currentInteractable != closestInteractable && currentInteractable != null)
        {
            currentInteractable.ResetHighlight();
        }

        // Þu anki etkileþimli nesneyi güncelle
        currentInteractable = closestInteractable;

        // Eðer bir etkileþimli nesne varsa, vurgula
        if (currentInteractable != null)
        {
            currentInteractable.HighlightObject();
        }
    }
}