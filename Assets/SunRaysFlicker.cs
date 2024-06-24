using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class SunRaysFlicker : MonoBehaviour
{
    [Header("Small Ray Vars")]
    public Image smallRayImg;
    public float minFlickerIntervalS;
    public float maxFlickerIntervalS;
    public float minTransitionDurationS;
    public float maxTransitionDurationS;
    private Color originalSmallRayColor;
    private bool isFadingOutS = true;
    private Coroutine raySCoroutine;

    [Header("Big Ray Vars")]
    public Image bigRayImg;
    public float minFlickerIntervalB;
    public float maxFlickerIntervalB;
    public float minFlickerIntervalLitB;
    public float maxFlickerIntervalLitB;
    public float minTransitionDurationB;
    public float maxTransitionDurationB;
    private bool rayBTransitioningToOriginalColor = false;
    private Color originalBigRayColor;
    private bool isFadingOutB = true;
    private Coroutine rayBCoroutine;

    private void Start()
    {
        originalSmallRayColor = smallRayImg.color;
        originalBigRayColor = bigRayImg.color;
    }

    private IEnumerator FlickerS()
    {
        while (true)
        {
            float elapsedTime = 0f;
            float transitionDuration = Random.Range(minTransitionDurationS, maxTransitionDurationS);
            Color startColor = smallRayImg.color;
            Color targetEndColor = isFadingOutS ? new Color(originalSmallRayColor.r, originalSmallRayColor.g, originalSmallRayColor.b, 0f) : originalSmallRayColor;

            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                smallRayImg.color = Color.Lerp(startColor, targetEndColor, elapsedTime / transitionDuration);
                yield return null;
            }

            smallRayImg.color = targetEndColor;
            isFadingOutS = !isFadingOutS;

            float randomInterval = Random.Range(minFlickerIntervalS, maxFlickerIntervalS);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    private IEnumerator FlickerB()
    {
        while (true)
        {
            float elapsedTime = 0f;
            float transitionDuration = Random.Range(minTransitionDurationB, maxTransitionDurationB);
            Color startColor = bigRayImg.color;
            Color targetEndColor = isFadingOutB ? new Color(originalBigRayColor.r, originalBigRayColor.g, originalBigRayColor.b, 0f) : originalBigRayColor;

            rayBTransitioningToOriginalColor = !isFadingOutB;

            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                bigRayImg.color = Color.Lerp(startColor, targetEndColor, elapsedTime / transitionDuration);
                yield return null;
            }

            bigRayImg.color = targetEndColor;
            isFadingOutB = !isFadingOutB;

            float randomInterval = 0f;
            if (rayBTransitioningToOriginalColor) randomInterval = Random.Range(minFlickerIntervalLitB, maxFlickerIntervalLitB);
            else randomInterval = Random.Range(minFlickerIntervalB, maxFlickerIntervalB);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    private void OnDisable()
    {
        if (raySCoroutine != null)
        {
            StopCoroutine(raySCoroutine);
            rayBCoroutine = null;
        }
        if (rayBCoroutine != null)
        {
            StopCoroutine(rayBCoroutine);
            rayBCoroutine = null;
        }
    }

    private void OnEnable()
    {
        raySCoroutine = StartCoroutine(FlickerS());
        rayBCoroutine = StartCoroutine(FlickerB());
    }
}