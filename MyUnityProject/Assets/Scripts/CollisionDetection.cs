using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDetection : MonoBehaviour
{
    private int playerGeyikCollisionCount = 0;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        // Di�er nesnenin tagine g�re i�lemler
        switch (other.tag)
        {
            case "Player":
                HandlePlayerCollision(other);
                break;
            case "Animal":
                HandleAnimalCollision();
                break;
            case "Geyik":
                HandleGeyikCollision(other);
                break;
            case "NPCcar":
                HandleNPCCarCollision();
                break;
            case "Koyun":
                HandleKoyunCollision();
                break;
            default:
                // Di�er tagler i�in i�lemler
                break;
        }
    }

    private void HandlePlayerCollision(GameObject player)
    {
        // Player ile �arp��ma i�lemleri
        Debug.Log("Player ile �arp��ma");

        // Ek i�lemler
        // �rne�in, player's WorldRemainingLife de�erini g�ncelle
        UISystem uiSystem = player.GetComponent<UISystem>();
        if (uiSystem != null)
        {
            uiSystem.UpdateWorldLife(-500f); // �rne�in, 500 y�l eksilt
        }
    }

    private void HandleAnimalCollision()
    {
        // Animal ile �arp��ma i�lemleri
        Debug.Log("Animal ile �arp��ma");

        // Ek i�lemler
        UISystem uiSystem = FindObjectOfType<UISystem>();
        if (uiSystem != null)
        {
            uiSystem.UpdateWorldLife(-3000000f); // 3,000,000 y�l eksilt
        }
    }

    private void HandleGeyikCollision(GameObject player)
    {
        // Geyik ile �arp��ma i�lemleri
        Debug.Log("Geyik ile �arp��ma");

        // Ek i�lemler
        UISystem uiSystem = FindObjectOfType<UISystem>();
        if (uiSystem != null)
        {
            uiSystem.UpdateWorldLife(-100000f); // 1,00000 y�l eksilt
        }

        // Player Geyik ile �arp��ma say�s�n� art�r
        playerGeyikCollisionCount++;

        // E�er player toplamda 3 kez Geyik ile �arp��t�ysa
        if (playerGeyikCollisionCount >= 3)
        {
            GameOver();
        }
    }

    private void HandleNPCCarCollision()
    {
        // NPCcar ile �arp��ma i�lemleri
        Debug.Log("NPCcar ile �arp��ma");

        // Ek i�lemler
        UISystem uiSystem = FindObjectOfType<UISystem>();
        if (uiSystem != null)
        {
            uiSystem.UpdateWorldLife(-500f); // 500 y�l eksilt
        }
    }

    private void HandleKoyunCollision()
    {
        // Koyun ile �arp��ma i�lemleri
        Debug.Log("Koyun ile �arp��ma");

        // Ek i�lemler
        UISystem uiSystem = FindObjectOfType<UISystem>();
        if (uiSystem != null)
        {
            uiSystem.UpdateWorldLife(-300f); // 300 y�l eksilt
        }
    }

    private void GameOver()
    {
        // Game Over sahnesini y�kle
        SceneManager.LoadScene("StartScene");
    }
}
