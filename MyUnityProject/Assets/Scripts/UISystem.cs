using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISystem : MonoBehaviour
{
    public Transform player; // Oyuncunun transformu
    public TextMeshProUGUI distanceTraveledText; // Gidilen mesafeyi gösteren UI texti
    public TextMeshProUGUI distanceToDestinationText; // Varış noktasına kalan mesafeyi gösteren UI texti
    public TextMeshProUGUI worldRemainingLifeText; // Dünyanın kalan ömrünü gösteren UI texti
    public TextMeshProUGUI truckHealthText; // Tırın kalan canını gösteren UI texti
    public TextMeshProUGUI gameOverText; // Oyun bitiş mesajını gösteren UI texti
    public float resetDistanceThreshold = 15f; // Kalan mesafe için sıfırlama eşiği
    public float minDistanceToDestination = 50f; // Minimum varış mesafesi
    public float maxDistanceToDestination = 100f; // Maksimum varış mesafesi
    public float startingWorldLife = 5000000f; // Başlangıçta dünyanın ömrü (yıl cinsinden)
    public int maxCollisions = 5; // Maksimum çarpma sayısı
    public int truckMaxHealth = 5; // Tırın maksimum canı

    private float previousZ; // Oyuncunun önceki Z pozisyonu
    private float distanceTraveled; // Gidilen mesafe
    private float distanceToDestination; // Varış noktasına kalan mesafe
    private float worldRemainingLife; // Dünyanın kalan ömrü
    private int collisionCount = 0; // Çarpma sayısı
    private int truckHealth; // Tırın kalan canı

    private void Start()
    {
        // Oyuncunun başlangıç pozisyonunu al
        previousZ = player.position.z;

        // Dünyanın kalan ömrünü PlayerPrefs'den yükle
        worldRemainingLife = PlayerPrefs.GetFloat("WorldRemainingLife", startingWorldLife);

        // İlk varış mesafesini belirle
        distanceToDestination = Random.Range(minDistanceToDestination, maxDistanceToDestination);

        // Tırın başlangıç canını ayarla
        truckHealth = truckMaxHealth;

        UpdateUI();
    }

    private void Update()
    {
        // R tuşuna basıldığında PlayerPrefs'i sıfırla
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayerPrefs();
        }

        // Oyuncunun hareket edip etmediğini kontrol et
        float deltaZ = player.position.z - previousZ;
        if (deltaZ > 0)
        {
            // Gidilen mesafeyi hesapla
            distanceTraveled += deltaZ / 10f; // Metreden km'ye çevir

            // Varış noktasına kalan mesafeyi güncelle
            distanceToDestination -= deltaZ / 10f;
            if (distanceToDestination < resetDistanceThreshold)
            {
                distanceToDestination = Random.Range(minDistanceToDestination, maxDistanceToDestination);
            }

            // Dünyanın kalan ömrünü güncelle
            worldRemainingLife -= deltaZ * 50f; // Her metre için 500.000 yıl eksilt

            // UI'yi güncelle
            UpdateUI();

            // PlayerPrefs'e dünyanın kalan ömrünü kaydet
            PlayerPrefs.SetFloat("WorldRemainingLife", worldRemainingLife);
            PlayerPrefs.Save();

            // Önceki pozisyonu güncelle
            previousZ = player.position.z;
        }
    }

    private void ResetPlayerPrefs()
    {
        // PlayerPrefs'i temizle
        PlayerPrefs.DeleteAll();

        // Dünyanın ömrünü başlangıç değerine ayarla
        PlayerPrefs.SetFloat("WorldRemainingLife", startingWorldLife);
        PlayerPrefs.Save();

        // Dünyanın kalan ömrünü sıfırla ve UI'yi güncelle
        worldRemainingLife = startingWorldLife;
        collisionCount = 0;
        truckHealth = truckMaxHealth;
        UpdateUI();
    }

    public void UpdateWorldLife(float delta)
    {
        // Dünyanın kalan ömrünü güncelle
        worldRemainingLife += delta;
        PlayerPrefs.SetFloat("WorldRemainingLife", worldRemainingLife);
        PlayerPrefs.Save();
        UpdateUI();
    }

    public void OnCollision()
    {
        // Çarpma sayısını artır ve tırın canını azalt
        collisionCount++;
        truckHealth--;
        if (collisionCount >= maxCollisions)
        {
            // Oyun bitiş mesajını göster
            gameOverText.text = "Oyun Bitti!";
            Time.timeScale = 0f; // Oyunu durdur
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        distanceTraveledText.text = $"Gidilen Mesafe: {distanceTraveled:F2} km";
        distanceToDestinationText.text = $"Varış Noktasına Kalan Mesafe: {distanceToDestination:F2} km";
        worldRemainingLifeText.text = $"Dünyanın Kalan Ömrü: {worldRemainingLife:F0} yıl";
        truckHealthText.text = $"Tırın Kalan Canı: {truckHealth} / {truckMaxHealth}";
    }
}
