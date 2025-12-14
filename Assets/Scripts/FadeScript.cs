using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class FadeScript : MonoBehaviour
{
    Image img;
    Color tempColor;

    void Awake()
    {
        img = GetComponent<Image>();
        if (img == null)
            Debug.LogError("FadeScript: No Image component found on this GameObject!", this);
    }

    void Start()
    {
        tempColor = img.color;
        tempColor.a = 1f;
        img.color = tempColor;
        StartCoroutine(FadeIn(0.10f));
    }

    public IEnumerator FadeIn(float fadeSpeed)
    {
        for (float a = 1f; a >= -0.05f; a -= 0.05f)
        {
            tempColor = img.color;
            tempColor.a = a;
            img.color = tempColor;
            yield return new WaitForSecondsRealtime(fadeSpeed);
        }
        img.raycastTarget = false;
    }
    public IEnumerator FadeOut(float fadeSpeed)
    {
        if (img == null)
        {
            Debug.LogError("FadeScript: Image component is missing in FadeOut!", this);
            yield break;
        }
        for (float a = 0f; a <= 1.05f; a += 0.05f)
        {
            tempColor = img.color;
            tempColor.a = a;
            img.color = tempColor;
            yield return new WaitForSecondsRealtime(fadeSpeed);
        }
        img.raycastTarget = true;
    }
}