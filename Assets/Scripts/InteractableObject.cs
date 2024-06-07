using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    // Nesne özellikleri tanımlanıyor
    private SkinnedMeshRenderer skinnedMeshRenderer; // Modelin animasyonlu mesh'ini yönetme
    private MeshRenderer meshRenderer; // Modelin statik mesh'ini yönetme
    private Color originalColor; // Nesnenin orijinal rengi
    public Color highlightedColor = Color.yellow; // Vurgulama rengi
    public bool isHighlighted = false; // Vurgulama durumu
    private DoorController doorController; // Kapı kontrolü için
    private ChestController chestController; // Sandık kontrolü için
    private TextMeshPro[] interactionTexts; // Etkileşim metinleri
    private Transform player; // Oyuncunun konumu

    private void Start()
    {
        // Mesh Renderer bileşenlerini al
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Orijinal rengi kaydet
        if (skinnedMeshRenderer != null)
        {
            originalColor = skinnedMeshRenderer.material.color;
        }
        else if (meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
        }

        // Diğer bileşenleri al
        doorController = GetComponent<DoorController>();
        chestController = GetComponent<ChestController>();
        interactionTexts = GetComponentsInChildren<TextMeshPro>();

        // Etkileşim metinlerini başlangıçta gizle
        foreach (var text in interactionTexts)
        {
            text.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Etkileşim metinlerinin oyuncuya dönmesini sağla
        foreach (var text in interactionTexts)
        {
            if (text.gameObject.activeSelf && player)
            {
                text.transform.LookAt(player);
                text.transform.Rotate(0, 180, 0);
            }
        }
    }

    public void HighlightObject()
    {
        // Nesneyi vurgula
        if (skinnedMeshRenderer != null)
        {
            foreach (Material mat in skinnedMeshRenderer.materials)
            {
                mat.color = highlightedColor;
            }
        }
        else if (meshRenderer != null)
        {
            foreach (Material mat in meshRenderer.materials)
            {
                mat.color = highlightedColor;
            }
        }
        isHighlighted = true;

        // Etkileşim metinlerini göster
        foreach (var text in interactionTexts)
        {
            text.gameObject.SetActive(true);
        }
    }

    public void ResetHighlight()
    {
        // Vurgulamayı kaldır
        if (skinnedMeshRenderer != null)
        {
            foreach (Material mat in skinnedMeshRenderer.materials)
            {
                mat.color = originalColor;
            }
        }
        else if (meshRenderer != null)
        {
            foreach (Material mat in meshRenderer.materials)
            {
                mat.color = originalColor;
            }
        }
        isHighlighted = false;

        // Etkileşim metinlerini gizle
        foreach (var text in interactionTexts)
        {
            text.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        // Etkileşim fonksiyonları
        if (doorController)
        {
            doorController.ToggleDoor();
        }
        else if (chestController)
        {
            chestController.ToggleChest();
        }
    }
}