/// <summary>
/// This script handles the behavior of the boosts UI.
/// </summary>
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostsUI : Singleton<BoostsUI>
{
    private CanvasGroup canvasGroup;

    [SerializeField] private Button boost1Button;
    [SerializeField] private TextMeshProUGUI boost1Name;
    [SerializeField] private TextMeshProUGUI boost1Description;
    private Boost boost1;


    [SerializeField] private Button boost2Button;
    [SerializeField] private TextMeshProUGUI boost2Name;
    [SerializeField] private TextMeshProUGUI boost2Description;
    private Boost boost2;

    [SerializeField] private Button boost3Button;
    [SerializeField] private TextMeshProUGUI boost3Name;
    [SerializeField] private TextMeshProUGUI boost3Description;
    private Boost boost3;

    private float fadeTime = 1.5f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        boost1Button.onClick.AddListener(() => {
            boost1.Select();
            Close();
        });
        boost2Button.onClick.AddListener(() => {
            boost2.Select();
            Close();
        });
        boost3Button.onClick.AddListener(() => {
            boost3.Select();
            Close();
        });
        boost1Button.enabled = false;
        boost2Button.enabled = false;
        boost3Button.enabled = false;
    }

    public void Open()
    {
        Time.timeScale = 0.0f; // pause game
        SetBoosts();
        StartCoroutine(FadeIn());
    }

    public void SetBoosts()
    {
        Boost[] boosts = Player.Instance.Boosts.GetBoosts();
        boost1 = boosts[0];
        boost1Name.text = boost1.Name;
        boost1Description.text = boost1.Description;

        boost2 = boosts[1];
        boost2Name.text = boost2.Name;
        boost2Description.text = boost2.Description;

        boost3 = boosts[2];
        boost3Name.text = boost3.Name;
        boost3Description.text = boost3.Description;
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        canvasGroup.alpha = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }
        canvasGroup.alpha = 1;
        boost1Button.enabled = true;
        boost2Button.enabled = true;
        boost3Button.enabled = true;
        yield return null;
    }

    void Close()
    {
        boost1Button.enabled = false;
        boost2Button.enabled = false;
        boost3Button.enabled = false;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        canvasGroup.alpha = 1;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            yield return null;
        }
        canvasGroup.alpha = 0;
        UIManager.Instance.PreviousState();
        yield return null;
    }
}
