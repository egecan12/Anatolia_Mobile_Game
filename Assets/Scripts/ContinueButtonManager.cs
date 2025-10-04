using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonManager : MonoBehaviour
{
    void Start()
    {
        // Find the Continue button automatically
        Button continueButton = GetComponent<Button>();
        if (continueButton != null)
        {
            // Find Player component
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                // Add OnClick event listener
                continueButton.onClick.RemoveAllListeners(); // Clear existing listeners
                continueButton.onClick.AddListener(() => player.OnContinueClick());
                Debug.Log("✅ Restart Button OnClick event successfully connected to Player.OnContinueClick()!");
            }
            else
            {
                Debug.LogError("❌ Player component not found! Cannot connect Continue button.");
            }
        }
    }
}
