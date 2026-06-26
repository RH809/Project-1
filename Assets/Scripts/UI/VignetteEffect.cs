/// <summary>
/// This script handles the vignette effect for taking damage/healing.
/// </summary>
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteEffect : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private Vignette vignette;

    [SerializeField] private float startIntensity;
    [SerializeField] private float endIntensity;
    [SerializeField] private float inDuration;
    [SerializeField] private float outDuration;
    [SerializeField] private Color damageColor;
    [SerializeField] private Color healColor;

    private Coroutine coroutine;

    void Start()    
    {
       volume.profile.TryGet(out vignette);
        vignette.intensity.value = 0;
    }

    void OnEnable()
    {
        Health.OnTakeDamage += Damage;
        Health.OnHeal += Heal;
    }

    void OnDisable()
    {
        Health.OnTakeDamage -= Damage;
        Health.OnHeal -= Heal;
    }

    public void Damage(HealthContext healthContext)
    {
        if (!healthContext.target.Equals(Player.Instance.gameObject)) return;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(PlayEffect(true));
    }

    public void Heal(HealthContext healthContext)
    {
        Debug.Log("Heal: " + healthContext.target);
        if (!healthContext.target.Equals(Player.Instance.gameObject)) return;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(PlayEffect(false));
    }

    IEnumerator PlayEffect(bool damage)
    {
        vignette.color.value = damage ? damageColor : healColor;
        yield return Fade(startIntensity, endIntensity, inDuration);
        yield return Fade(endIntensity, startIntensity, outDuration);
        vignette.intensity.value = 0;
        coroutine = null;
    }

    IEnumerator Fade(float start, float end, float duration)
    {
        float t = 0f;
        vignette.intensity.value = start;
        while (t < duration)
        {
            t += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(start, end, t / duration);
            yield return null;
        }

        vignette.intensity.value = end;
    }
}
