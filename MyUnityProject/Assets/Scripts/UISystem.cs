using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISystem : MonoBehaviour
{
    public Transform player; // Oyuncunun transformu
    public TextMeshProUGUI distanceTraveledText; // Gidilen mesafeyi gösteren UI texti
    public TextMeshProUGUI distanceToDestinationText; // Varýþ noktasýna kalan mesafeyi gösteren UI texti
    public TextMeshProUGUI worldRemainingLifeText; // Dünyanýn kalan ömrünü gösteren UI texti
    public float resetDistanceThreshold = 15f; // Kalan mesafe için sýfýrlama eþiði
    public float minDistanceToDestination = 50f; // Minimum varýþ mesafesi
    public float maxDistanceToDestination = 100f; // Maksimum varýþ mesafesi
    public float startingWorldLife = 5000000f; // Baþlangýçta dünyanýn ömrü (yýl cinsinden)

    private float previousZ; // Oyuncunun önceki Z pozisyonu
    private float distanceTraveled; // Gidilen mesafe
    private float distanceToDestination; // Varýþ noktasýna kalan mesafe
    private float worldRemainingLife; // Dünyanýn kalan ömrü

    private void Start()
    {
        // Oyuncunun baþlangýç pozisyonunu al
        previousZ = player.position.z;

        // Dünyanýn kalan ömrünü PlayerPrefs'den yükle
        worldRemainingLife = PlayerPrefs.GetFloat("WorldRemainingLife", startingWorldLife);

        // Ýlk varýþ mesafesini belirle
        distanceToDestination = Random.Range(minDistanceToDestination, maxDistanceToDestination);

        UpdateUI();
    }

    private void Update()
    {
        // R tuþuna basýldýðýnda PlayerPrefs'i sýfýrla
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayerPrefs();
        }

        // Oyuncunun hareket edip etmediðini kontrol et
        float deltaZ = player.position.z - previousZ;
        if (deltaZ > 0)
        {
            // Gidilen mesafeyi hesapla
            distanceTraveled += deltaZ / 1000f; // Metreden km'ye çevir

            // Varýþ noktasýna kalan mesafeyi güncelle
            distanceToDestination -= deltaZ / 1000f;
            if (distanceToDestination < resetDistanceThreshold)
            {
                distanceToDestination = Random.Range(minDistanceToDestination, maxDistanceToDestination);
            }

            // Dünyanýn kalan ömrünü güncelle
            worldRemainingLife -= deltaZ * 50f; // Her metre için 500.000 yýl eksilt

            // UI'yi güncelle
            UpdateUI();

            // PlayerPrefs'e dünyanýn kalan ömrünü kaydet
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

        // Dünyanýn ömrünü baþlangýç deðerine ayarla
        PlayerPrefs.SetFloat("WorldRemainingLife", startingWorldLife);
        PlayerPrefs.Save();

        // Dünyanýn kalan ömrünü sýfýrla ve UI'yi güncelle
        worldRemainingLife = startingWorldLife;
        UpdateUI();
    }
    public void UpdateWorldLife(float delta)
    {
        // Dünyanýn kalan ömrünü güncelle
        worldRemainingLife += delta;
        PlayerPrefs.SetFloat("WorldRemainingLife", worldRemainingLife);
        PlayerPrefs.Save();
        UpdateUI();
    }

    private void UpdateUI()
    {
        distanceTraveledText.text = $"Gidilen Mesafe: {distanceTraveled:F2} km";
        distanceToDestinationText.text = $"Varýþ Noktasýna Kalan Mesafe: {distanceToDestination:F2} km";
        worldRemainingLifeText.text = $"Dünyanýn Kalan Ömrü: {worldRemainingLife:F0} yýl";
    }

   
}
