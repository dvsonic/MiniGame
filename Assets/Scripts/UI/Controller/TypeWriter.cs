using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TypeWriter : MonoBehaviour
{
    public float delay = 0.1f;
    private string fullText;
    private string currentText = "";
    TextMeshProUGUI tf;
    LayoutElement le;
    void Start()
    {
    }

    public void SetText(string fullText)
    {
        this.fullText = fullText;
        if (string.IsNullOrEmpty(fullText))
        {
            tf.text = "";
            StopAllCoroutines();
        }
        else
            StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            if(tf == null)
                tf = GetComponent<TextMeshProUGUI>();
            if (le == null)
                le = GetComponent<LayoutElement>();
            tf.text = currentText;
            float length = tf.preferredWidth;
            if (length > 800)
                le.preferredWidth = 800;
            else
                le.preferredWidth = length;
            yield return new WaitForSeconds(delay);
        }
    }
}
