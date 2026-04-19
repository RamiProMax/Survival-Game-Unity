using UnityEngine;
using UnityEngine.UI;

public class DamageOverlay : MonoBehaviour
{
    public Image overlayImage;
    public float flashAlpha = 0.5f;
    public float fadeSpeed = 2f;

    void Update()
    {
        if (overlayImage.color.a > 0)
        {
            Color c = overlayImage.color;
            c.a -= fadeSpeed * Time.deltaTime;
            overlayImage.color = c;
        }
    }

    public void ShowDamage()
    {
        Color c = overlayImage.color;
        c.a = flashAlpha;
        overlayImage.color = c;
    }
}