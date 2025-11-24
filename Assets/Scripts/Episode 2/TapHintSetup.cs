using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro namespace

public class TapHintSetup : MonoBehaviour
{
    [Header("Settings")]
    public float marginTop = 50f;
    public float marginRight = 50f;
    // Köşeleri yuvarlak, modern bir buton için yarı saydam siyah arka plan
    public Color backgroundColor = new Color(0f, 0f, 0f, 0.7f); 
    public string hintText = "EKRANA BAS ÇEK";
    public Color textColor = Color.white;
    public float fontSize = 36f;
    public Sprite customKnobSprite; // Kullanıcının UI Sprite/Knob atayabileceği alan

    private GameObject canvasObj;
    private GameObject buttonObj;

    void Start()
    {
        CreateTapIndicator();
    }

    void CreateTapIndicator()
    {
        // 1. Canvas Bulma/Oluşturma
        string canvasName = "TapHintCanvas";
        canvasObj = GameObject.Find(canvasName);
        
        if (canvasObj == null)
        {
            canvasObj = new GameObject(canvasName);
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // Eski objeleri temizle
        Transform oldBtn = canvasObj.transform.Find("TapButtonModern");
        if (oldBtn != null) Destroy(oldBtn.gameObject);

        // 2. Buton Container
        buttonObj = new GameObject("TapButtonModern");
        buttonObj.transform.SetParent(canvasObj.transform, false);

        // Arka Plan Resmi
        Image img = buttonObj.AddComponent<Image>();
        
        // Yuvarlak köşe için Sprite ayarı
        if (customKnobSprite != null)
        {
            img.sprite = customKnobSprite;
            img.type = Image.Type.Sliced; // Sliced mod, köşelerin bozulmamasını sağlar
        }
        else
        {
            // Eğer custom sprite yoksa, Unity'nin varsayılan 'UISprite'ını bulmaya çalışalım
            // Not: Kodla 'UISprite' bulmak her zaman garanti değildir, bu yüzden 
            // kullanıcıya Inspector'dan atama yapmasını öneririz. 
            // Şimdilik null bırakıyoruz, kare olacak.
        }
        
        img.color = backgroundColor;

        // Layout Group (Otomatik Boyutlandırma)
        HorizontalLayoutGroup layout = buttonObj.AddComponent<HorizontalLayoutGroup>();
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        // Padding'i biraz artırarak ferah bir görünüm sağlıyoruz
        layout.padding = new RectOffset(60, 60, 30, 30); 
        layout.childAlignment = TextAnchor.MiddleCenter;

        ContentSizeFitter fitter = buttonObj.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Konumlandırma (Sağ Üst)
        RectTransform rect = buttonObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);
        rect.anchoredPosition = new Vector2(-marginRight, -marginTop);

        // 3. Metin Ekleme
        GameObject textObj = new GameObject("TapText");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = hintText;
            tmp.color = textColor;
            tmp.fontSize = fontSize;
            // Daha kalın ve okunaklı font
            tmp.fontWeight = FontWeight.Bold; 
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableWordWrapping = false;
        }
        else
        {
            Text txt = textObj.AddComponent<Text>();
            txt.text = hintText;
            txt.color = textColor;
            txt.fontSize = (int)fontSize;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.fontStyle = FontStyle.Bold;
        }

        // 4. Animasyon Scripti (Hızlı Pulse)
        buttonObj.AddComponent<TapPulseAnimation>();
    }
}

public class TapPulseAnimation : MonoBehaviour
{
    // Daha hızlı ve dikkat çekici hız
    public float pulseSpeed = 8f; 
    public float minScale = 0.9f;
    public float maxScale = 1.1f;

    private Vector3 baseScale;
    private Image targetImage;
    private Color baseColor;

    void Start()
    {
        baseScale = transform.localScale;
        targetImage = GetComponent<Image>();
        if (targetImage != null) baseColor = targetImage.color;
    }

    void Update()
    {
        // Sinüs dalgası (-1 ile 1 arası)
        float wave = Mathf.Sin(Time.time * pulseSpeed);
        
        // Ölçek değişimi (Büyüme/Küçülme)
        // wave + 1 => (0 ile 2) -> /2 => (0 ile 1)
        float scaleFactor = Mathf.Lerp(minScale, maxScale, (wave + 1f) / 2f);
        transform.localScale = baseScale * scaleFactor;

        // İsteğe bağlı: Renk parlaklığı da değişebilir (daha dikkat çekici)
        if (targetImage != null)
        {
            // Alpha değerini hafifçe oynat
            float alphaChange = Mathf.Lerp(0.5f, 1f, (wave + 1f) / 2f);
            Color c = baseColor;
            c.a = baseColor.a * alphaChange; // Mevcut alpha üzerinden varyasyon
           // targetImage.color = c; // İstersen bunu açabilirsin
        }
    }
}
