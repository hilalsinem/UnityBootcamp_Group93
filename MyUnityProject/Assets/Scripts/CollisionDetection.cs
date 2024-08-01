using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        // Diðer nesnenin tagine göre iþlemler
        switch (other.tag)
        {
            case "Player":
                HandlePlayerCollision(other);
                break;
            case "Animal":
                HandleAnimalCollision();
                break;
            case "Geyik":
                HandleGeyikCollision();
                break;
            case "NPCcar":
                HandleNPCCarCollision();
                break;
            case "Koyun":
                HandleKoyunCollision();
                break;
            default:
                // Diðer tagler için iþlemler
                break;
        }
    }

    private void HandlePlayerCollision(GameObject player)
    {
        // Player ile çarpýþma iþlemleri
        Debug.Log("Player ile çarpýþma");

        // Ek iþlemler
        // Örneðin, player's WorldRemainingLife deðerini güncelle
        UISystem uiSystem = player.GetComponent<UISystem>();
        if (uiSystem != null)
        {
            uiSystem.UpdateWorldLife(-500f); // Örneðin, 500 yýl eksilt
        }
    }

    private void HandleAnimalCollision()
    {
        // Animal ile çarpýþma iþlemleri
        Debug.Log("Animal ile çarpýþma");

        // Ek iþlemler
        UISystem uiSystem = FindObjectOfType<UISystem>();
        if (uiSystem != null)
        {
            uiSystem.UpdateWorldLife(-3000000f); // 3,000,000 yýl eksilt
        }
    }

    private void HandleGeyikCollision()
    {
        // Geyik ile çarpýþma iþlemleri
        Debug.Log("Geyik ile çarpýþma");

        // Ek iþlemler
        UISystem uiSystem = FindObjectOfType<UISystem>();
        if (uiSystem != null)
        {
            uiSystem.UpdateWorldLife(-100000f); // 1,00000 yýl eksilt
        }
    }

    private void HandleNPCCarCollision()
    {
        // NPCcar ile çarpýþma iþlemleri
        Debug.Log("NPCcar ile çarpýþma");

        // Ek iþlemler
        UISystem uiSystem = FindObjectOfType<UISystem>();
        if (uiSystem != null)
        {
            uiSystem.UpdateWorldLife(-500f); // 500 yýl eksilt
        }
    }

    private void HandleKoyunCollision()
    {
        // Koyun ile çarpýþma iþlemleri
        Debug.Log("Koyun ile çarpýþma");

        // Ek iþlemler
        UISystem uiSystem = FindObjectOfType<UISystem>();
        if (uiSystem != null)
        {
            uiSystem.UpdateWorldLife(-300f); // 300 yýl eksilt
        }
    }
}
