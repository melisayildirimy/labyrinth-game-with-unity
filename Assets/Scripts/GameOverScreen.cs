using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverPanel; // Game over paneli için GameObject
    public Button restartButton; // Yeniden dene butonu
    public Button mainMenuButton; // Ana menüye dön butonu
    private PlayerController playerController; // PlayerController'dan kalýtým

    private void Start()
    {
        // Buton event'lerini atama
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(LoadMainMenu);

        // PlayerController scripti
        playerController = GameObject.FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController script'i sahnede bulunamadý.");
        }

        // Oyun baþlangýcýnda panel kapalý olmalý
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        // PlayerController script'inden stamina deðerini kontrol et
        if (playerController != null && playerController.stamina <= 0.1f * playerController.maxStamina)
        {
            ShowGameOverPanel();
        }
    }

    public void ShowGameOverPanel()
    {
        if (!gameOverPanel.activeInHierarchy)
        {
            // Oyunu duraklat
            Time.timeScale = 0;
            // Game over panelini göster
            gameOverPanel.SetActive(true);
            // Ýmleci görünür yap ve kilidini aç
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    public void RestartGame()
    {
        // Oyunun zamanýný normale döndür
        Time.timeScale = 1;
        // Oyunu kaldýðý yerden devam ettir
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        // Oyunun zamanýný normale döndür
        Time.timeScale = 1;
        // Ana Menüyü Yükle
        SceneManager.LoadScene(0);
    }
}