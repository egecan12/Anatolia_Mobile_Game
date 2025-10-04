using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class AddContinueButton : MonoBehaviour
{
    [MenuItem("Tools/Add Continue Button to Episode1")]
    static void AddContinueButtonToScene()
    {
        // Find the Episode1 scene Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene!");
            return;
        }

        // Find Player component
        Player player = FindObjectOfType<Player>();
        if (player == null)
        {
            Debug.LogError("No Player component found in the scene!");
            return;
        }

        // Check if ContinueButton already exists
        GameObject existingButton = GameObject.Find("ContinueButton");
        if (existingButton != null)
        {
            Debug.LogWarning("ContinueButton already exists! Removing old one and creating new one.");
            DestroyImmediate(existingButton);
        }

        // Create Continue Button GameObject
        GameObject continueButton = new GameObject("ContinueButton");
        continueButton.transform.SetParent(canvas.transform);
        continueButton.layer = 5; // UI Layer

        // Add RectTransform component
        RectTransform rectTransform = continueButton.AddComponent<RectTransform>();
        
        // Set RectTransform properties
        rectTransform.localScale = Vector3.one;
        rectTransform.sizeDelta = new Vector2(200, 60);
        rectTransform.anchoredPosition = new Vector2(0, -100); // Below Game Over text
        
        // Set anchoring to center-bottom
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // Add Image component for button background
        Image buttonImage = continueButton.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.6f, 1f, 0.8f); // Blue color
        buttonImage.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/Knob.psd");

        // Add Button component
        Button button = continueButton.AddComponent<Button>();
        
        // Add ContinueButtonManager component for automatic OnClick setup
        continueButton.AddComponent<ContinueButtonManager>();
        
        // Create Text child for button
        GameObject textObject = new GameObject("Text (TMP)");
        textObject.transform.SetParent(continueButton.transform);
        textObject.layer = 5; // UI Layer

        // Add TextMeshProUGUI component
        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        
        // Set text properties
        textComponent.text = "Restart";
        textComponent.color = Color.white;
        textComponent.fontSize = 30;
        textComponent.alignment = TextAlignmentOptions.Center;
        
        // Set Text RectTransform
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.localScale = Vector3.one;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        // Assign button to Player component
        SerializedObject playerSerialized = new SerializedObject(player);
        SerializedProperty continueButtonProperty = playerSerialized.FindProperty("continueButton");
        if (continueButtonProperty != null)
        {
            continueButtonProperty.objectReferenceValue = button;
            playerSerialized.ApplyModifiedProperties();
        }

        // Initially hide the button (will be shown when game over)
        continueButton.SetActive(false);

        Debug.Log("✅ Continue Button successfully added to Episode1 scene!");
        Debug.Log("📍 Button is now connected to Player component.");
        Debug.Log("🔵 Button will appear below Game Over text when player dies.");
        
        // Mark scene as dirty to save changes
        EditorUtility.SetDirty(canvas.gameObject);
        EditorUtility.SetDirty(player.gameObject);
    }
}
