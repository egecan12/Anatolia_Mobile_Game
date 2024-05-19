using UnityEngine;
using TMPro;
using System.Collections;

public class BlinkingTextMeshPro : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float blinkTime = 1f;
    private Material textMaterial;

    void Start()
    {
        textMaterial = textComponent.fontMaterial;
        StartCoroutine(BlinkOutline());
    }

    IEnumerator BlinkOutline()
    {
        while (true)
        {
            textMaterial.SetFloat("_OutlineWidth", 0.2f);
            textMaterial.SetColor("_OutlineColor", Color.yellow);
            yield return new WaitForSeconds(blinkTime);
            textMaterial.SetFloat("_OutlineWidth", 0f);
            yield return new WaitForSeconds(blinkTime);
        }
    }
}