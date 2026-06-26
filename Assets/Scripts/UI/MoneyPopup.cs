/// <summary>
/// This script handles the behavior of the money popup.
/// </summary>
using System.Collections;
using TMPro;
using UnityEngine;

public class MoneyPopup : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float speed;

    private TextMeshProUGUI text;
    private float t;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        t = 0;
        text.alpha = 1.0f;
        StartCoroutine(DestroyAfterDelay());
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += Vector3.up * speed * Time.deltaTime;
        t += Time.deltaTime;
        text.alpha = Mathf.Lerp(1.0f, 0.0f, t / duration);
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
