using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCell : MonoBehaviour
{
    [SerializeField]
    private Image imageToBlink;

    private float blinkSpeed = 0.3f;

    private Color originalColor;

    public Coroutine blinking { get; private set; }

    public static event Action LostHealthPoint;
    public static event Action Damaged;
    public static event Action StoppedBlinking;
    public static event Action BlindOutOfTime;

    public void DestroyMe()
    {
        LostHealthPoint?.Invoke();
        Destroy(gameObject);
    }

    public void Blink(float time)
    {
        blinking = StartCoroutine(BlinkImage(time));
        Damaged?.Invoke();
    }

    public void StopBlink()
    {
        StoppedBlinking?.Invoke();
        if (blinking != null) StopCoroutine(blinking);
        blinking = null;
        imageToBlink.color = originalColor;
    }


    private IEnumerator BlinkImage(float timeDelay)
    {
        originalColor = imageToBlink.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        float elapsedTime = 0f;
        float allElapsedTime = 0f;
        bool fadingOut = true;

        while (allElapsedTime < timeDelay) 
        {
            while (elapsedTime < blinkSpeed)
            {
                elapsedTime += Time.deltaTime;
                allElapsedTime += Time.deltaTime;
                float t = elapsedTime / blinkSpeed;
                imageToBlink.color = Color.Lerp(fadingOut ? originalColor : transparentColor, 
                    fadingOut ? transparentColor : originalColor, t);
                yield return null;
            }

            fadingOut = !fadingOut;
            elapsedTime = 0f; 
        }
        BlindOutOfTime?.Invoke();
        yield break;
    }
}
