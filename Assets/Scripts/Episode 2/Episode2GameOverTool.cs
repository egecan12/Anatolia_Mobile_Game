using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

#if UNITY_EDITOR
public class Episode2GameOverTool : MonoBehaviour
{
    [MenuItem("Tools/Episode 2/Add Game Over UI")]
    public static void CreateGameOverUI()
    {
        Debug.Log("🎮 Episode 2 Game Over UI Tool başlatılıyor...");
        
        // Find or get the main Canvas
        Canvas canvas = FindMainCanvas();
        if (canvas == null)
        {
            Debug.LogError("❌ Canvas bulunamadı! Önce bir Canvas oluşturun.");
            return;
        }
        
        // Check if Game Over UI already exists
        if (IsGameOverUIAlreadyExists(canvas))
        {
            Debug.LogWarning("⚠️ Game Over UI zaten mevcut!");
            return;
        }
        
        // Create Game Over Panel
        GameObject gameOverPanel = CreateGameOverPanel(canvas);
        
        // Create Game Over Text
        GameObject gameOverText = CreateGameOverText(gameOverPanel);
        
        // Create Restart Button
        GameObject restartButton = CreateRestartButton(gameOverPanel);
        
        // Assign references to PlayerBalloon script
        AssignUIReferencesToPlayerBalloon(gameOverText, restartButton);
        
        Debug.Log("✅ Episode 2 Game Over UI başarıyla oluşturuldu!");
        EditorUtility.SetDirty(canvas);
    }
    
    static Canvas FindMainCanvas()
    {
        // First try to find any canvas in scene
        Canvas[] allCanvas = FindObjectsOfType<Canvas>();
        if (allCanvas.Length > 0)
        {
            Debug.Log($"✅ Canvas bulundu: {allCanvas[0].name}");
            return allCanvas[0]; // Return the first canvas found
        }
        
        // If no canvas found, try to find by tag (only if UI tag exists)
        try
        {
            GameObject[] canvasObjects = GameObject.FindGameObjectsWithTag("UI");
            foreach (GameObject obj in canvasObjects)
            {
                Canvas canvas = obj.GetComponent<Canvas>();
                if (canvas != null)
                {
                    Debug.Log($"✅ UI tagli Canvas bulundu: {canvas.name}");
                    return canvas;
                }
            }
        }
        catch (UnityException)
        {
            Debug.Log("ℹ️ UI tag tanımlı değil, normal Canvas araması yapılıyor...");
        }
        
        Debug.LogWarning("Canvas bulunamadı, yeni bir Canvas oluşturuluyor...");
        return CreateNewCanvas();
    }
    
    static Canvas CreateNewCanvas()
    {
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        return canvas;
    }
    
    static bool IsGameOverUIAlreadyExists(Canvas canvas)
    {
        return canvas.transform.Find("GameOverPanel") != null;
    }
    
    static GameObject CreateGameOverPanel(Canvas canvas)
    {
        GameObject panel = new GameObject("GameOverPanel");
        panel.transform.SetParent(canvas.transform, false);
        
        // Add Image component for background
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f); // Semi-transparent black background
        
        // Set RectTransform properties
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        
        // Hide by default
        panel.SetActive(false);
        
        Debug.Log($"✅ Game Over Panel oluşturuldu - Aktif: {panel.activeInHierarchy}, Canvas: {canvas.name}");
        return panel;
    }
    
    static GameObject CreateGameOverText(GameObject parent)
    {
        GameObject textObj = new GameObject("GameOverText");
        textObj.transform.SetParent(parent.transform, false);

        // Add TextMeshPro component
        TextMeshProUGUI textComponent = textObj.AddComponent<TextMeshProUGUI>();
        textComponent.text = "Game Over";
        textComponent.fontSize = 48;
        textComponent.color = Color.white;
        textComponent.alignment = TextAlignmentOptions.Center;
        
        // Use default TextMeshPro font
        textComponent.font = Resources.GetBuiltinResource<TMP_FontAsset>("LegacyRuntime TextMeshPro/Textures/ARIAL SDF");

        // Set RectTransform properties - center of screen
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = new Vector2(0, 50);
        textRect.sizeDelta = new Vector2(400, 60);
        
        Debug.Log("💬 Game Over Text oluşturuldu");
        return textObj;
    }
    
    static GameObject CreateRestartButton(GameObject parent)
    {
        GameObject buttonObj = new GameObject("RestartButton");
        buttonObj.transform.SetParent(parent.transform, false);
        
        // Add Button component
        Button button = buttonObj.AddComponent<Button>();
        
        // Add Image component for button background
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.6f, 1f, 1f); // Nice blue color
        
        // Create button text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        TextMeshProUGUI buttonText = textObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = "RESTART";
        buttonText.fontSize = 24;
        buttonText.color = Color.white;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.font = Resources.GetBuiltinResource<TMP_FontAsset>("LegacyRuntime TextMeshPro/Textures/ARIAL SDF");
        
        // Set button text RectTransform
        RectTransform buttonTextRect = textObj.GetComponent<RectTransform>();
        buttonTextRect.anchorMin = Vector2.zero;
        buttonTextRect.anchorMax = Vector2.one;
        buttonTextRect.offsetMin = Vector2.zero;
        buttonTextRect.offsetMax = Vector2.zero;
        
        // Set button RectTransform properties - below text
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.anchoredPosition = new Vector2(0, -20);
        buttonRect.sizeDelta = new Vector2(150, 50);
        
        Debug.Log("🔵 Restart Button oluşturuldu");
        return buttonObj;
    }
    
    static void AssignUIReferencesToPlayerBalloon(GameObject gameOverText, GameObject restartButton)
    {
        // Find PlayerBalloon in scene
        PlayerBalloon playerBalloon = FindObjectOfType<PlayerBalloon>();
        if (playerBalloon == null)
        {
            Debug.LogError("❌ PlayerBalloon bulunamadı! Episode 2 sahnesinde PlayerBalloon objesi olmalı.");
            return;
        }
        
        // Get the components we created
        TextMeshProUGUI gameOverTextComponent = gameOverText.GetComponent<TextMeshProUGUI>();
        Button restartButtonComponent = restartButton.GetComponent<Button>();
        
        // Use SerializedObject to modify the PlayerBalloon script
        SerializedObject serializedBalloon = new SerializedObject(playerBalloon);
        
        SerializedProperty gameOverTextProp = serializedBalloon.FindProperty("gameOverText");
        SerializedProperty continueButtonProp = serializedBalloon.FindProperty("continueButton");
        
        if (gameOverTextProp != null)
        {
            gameOverTextProp.objectReferenceValue = gameOverTextComponent;
            serializedBalloon.ApplyModifiedProperties();
        }
        
        if (continueButtonProp != null)
        {
            continueButtonProp.objectReferenceValue = restartButtonComponent;
            serializedBalloon.ApplyModifiedProperties();
        }
        
        // Set up button click event
        SetupButtonClickEvent(restartButtonComponent, playerBalloon);
        
        Debug.Log("✅ UI referansları PlayerBalloon scriptine atandı");
        
        // Ensure Unity saves the changes
        EditorUtility.SetDirty(playerBalloon);
    }
    
    static void SetupButtonClickEvent(Button button, PlayerBalloon playerBalloon)
    {
        if (button == null || playerBalloon == null)
        {
            Debug.LogError("❌ Button veya PlayerBalloon null!");
            return;
        }
        
        Debug.Log($"🔧 Button event atanıyor - Button: {button.name}, PlayerBalloon: {playerBalloon.name}");
        
        // Clear existing listeners
        button.onClick.RemoveAllListeners();
        
        // Add SimpleRestart method as listener (most reliable)
        button.onClick.AddListener(() => {
            Debug.Log("🔄 Restart butonu tıklandı!");
            playerBalloon.SimpleRestart();
        });
        
        // Verify the listener was added
        int listenerCount = button.onClick.GetPersistentEventCount();
        Debug.Log($"✅ Button click event atandı - OnContinueClick() - Listener sayısı: {listenerCount}");
        
        // Force Unity to mark the object as dirty
        EditorUtility.SetDirty(button);
    }
    
    // Additional tool to remove Game Over UI
    [MenuItem("Tools/Episode 2/Remove Game Over UI")]
    public static void RemoveGameOverUI()
    {
        Canvas canvas = FindMainCanvas();
        if (canvas == null)
        {
            Debug.LogError("❌ Canvas bulunamadı!");
            return;
        }
        
        Transform gameOverPanelTransform = canvas.transform.Find("GameOverPanel");
        if (gameOverPanelTransform != null)
        {
            DestroyImmediate(gameOverPanelTransform.gameObject);
            Debug.Log("🗑️ Game Over UI kaldırıldı");
        }
        else
        {
            Debug.LogWarning("⚠️ Game Over UI bulunamadı!");
        }
    }
    
    // Another tool to clean PlayerBalloon references
    [MenuItem("Tools/Episode 2/Clear PlayerBalloon UI References")]
    public static void ClearPlayerBalloonUIRefences()
    {
        PlayerBalloon playerBalloon = FindObjectOfType<PlayerBalloon>();
        if (playerBalloon == null)
        {
            Debug.LogError("❌ PlayerBalloon bulunamadı!");
            return;
        }
        
        SerializedObject serializedBalloon = new SerializedObject(playerBalloon);
        
        SerializedProperty gameOverTextProp = serializedBalloon.FindProperty("gameOverText");
        SerializedProperty continueButtonProp = serializedBalloon.FindProperty("continueButton");
        
        if (gameOverTextProp != null)
            gameOverTextProp.objectReferenceValue = null;
        
        if (continueButtonProp != null)
            continueButtonProp.objectReferenceValue = null;
        
        serializedBalloon.ApplyModifiedProperties();
        EditorUtility.SetDirty(playerBalloon);
        
        Debug.Log("🧹 PlayerBalloon UI referansları temizlendi");
    }
    
    // Tool to fix existing button click events
    [MenuItem("Tools/Episode 2/Fix Restart Button")]
    public static void FixRestartButton()
    {
        PlayerBalloon playerBalloon = FindObjectOfType<PlayerBalloon>();
        if (playerBalloon == null)
        {
            Debug.LogError("❌ PlayerBalloon bulunamadı!");
            return;
        }
        
        if (playerBalloon.continueButton == null)
        {
            Debug.LogError("❌ continueButton referansı null!");
            return;
        }
        
        SetupButtonClickEvent(playerBalloon.continueButton, playerBalloon);
        EditorUtility.SetDirty(playerBalloon);
        
        Debug.Log("✅ Mevcut restart butonu düzeltildi!");
    }
}
#endif
