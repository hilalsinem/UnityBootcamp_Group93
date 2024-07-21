using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISystem : MonoBehaviour
{
    public Transform player; // Oyuncunun transformu
    public TextMeshProUGUI distanceTraveledText; // Gidilen mesafeyi g�steren UI texti
    public TextMeshProUGUI distanceToDestinationText; // Var�� noktas�na kalan mesafeyi g�steren UI texti
    public TextMeshProUGUI worldRemainingLifeText; // D�nyan�n kalan �mr�n� g�steren UI texti
    public float resetDistanceThreshold = 15f; // Kalan mesafe i�in s�f�rlama e�i�i
    public float minDistanceToDestination = 50f; // Minimum var�� mesafesi
    public float maxDistanceToDestination = 100f; // Maksimum var�� mesafesi
    public float startingWorldLife = 5000000f; // Ba�lang��ta d�nyan�n �mr� (y�l cinsinden)

    private float previousZ; // Oyuncunun �nceki Z pozisyonu
    private float distanceTraveled; // Gidilen mesafe
    private float distanceToDestination; // Var�� noktas�na kalan mesafe
    private float worldRemainingLife; // D�nyan�n kalan �mr�

    private void Start()
    {
        // Oyuncunun ba�lang�� pozisyonunu al
        previousZ = player.position.z;

        // D�nyan�n kalan �mr�n� PlayerPrefs'den y�kle
        worldRemainingLife = PlayerPrefs.GetFloat("WorldRemainingLife", startingWorldLife);

        // �lk var�� mesafesini belirle
        distanceToDestination = Random.Range(minDistanceToDestination, maxDistanceToDestination);

        UpdateUI();
    }

    private void Update()
    {
        // R tu�una bas�ld���nda PlayerPrefs'i s�f�rla
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayerPrefs();
        }

        // Oyuncunun hareket edip etmedi�ini kontrol et
        float deltaZ = player.position.z - previousZ;
        if (deltaZ > 0)
        {
            // Gidilen mesafeyi hesapla
            distanceTraveled += deltaZ / 1000f; // Metreden km'ye �evir

            // Var�� noktas�na kalan mesafeyi g�ncelle
            distanceToDestination -= deltaZ / 1000f;
            if (distanceToDestination < resetDistanceThreshold)
            {
                distanceToDestination = Random.Range(minDistanceToDestination, maxDistanceToDestination);
            }

            // D�nyan�n kalan �mr�n� g�ncelle
            worldRemainingLife -= deltaZ * 50f; // Her metre i�in 500.000 y�l eksilt

            // UI'yi g�ncelle
            UpdateUI();

            // PlayerPrefs'e d�nyan�n kalan �mr�n� kaydet
            PlayerPrefs.SetFloat("WorldRemainingLife", worldRemainingLife);
            PlayerPrefs.Save();

            // �nceki pozisyonu g�ncelle
            previousZ = player.position.z;
        }
    }

    private void ResetPlayerPrefs()
    {
        // PlayerPrefs'i temizle
        PlayerPrefs.DeleteAll();

        // D�nyan�n �mr�n� ba�lang�� de�erine ayarla
        PlayerPrefs.SetFloat("WorldRemainingLife", startingWorldLife);
        PlayerPrefs.Save();

        // D�nyan�n kalan �mr�n� s�f�rla ve UI'yi g�ncelle
        worldRemainingLife = startingWorldLife;
        UpdateUI();
    }
    public void UpdateWorldLife(float delta)
    {
        // D�nyan�n kalan �mr�n� g�ncelle
        worldRemainingLife += delta;
        PlayerPrefs.SetFloat("WorldRemainingLife", worldRemainingLife);
        PlayerPrefs.Save();
        UpdateUI();
    }

    private void UpdateUI()
    {
        distanceTraveledText.text = $"Gidilen Mesafe: {distanceTraveled:F2} km";
        distanceToDestinationText.text = $"Var�� Noktas�na Kalan Mesafe: {distanceToDestination:F2} km";
        worldRemainingLifeText.text = $"D�nyan�n Kalan �mr�: {worldRemainingLife:F0} y�l";
    }

   
}
