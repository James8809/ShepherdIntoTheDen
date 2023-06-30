using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Slideshow : MonoBehaviour
{
    public Sprite[] images;
    public string[] imageText;
    public float imageDuration;
    public float fadeDuration;
    public float darknessDuration;
    public string nextSceneName;
    public Image image;
    public Image fadeOutImage;
    public Image finalFadeOutImage;
    public TextMeshProUGUI textArea;

    public int currSlide;

    IEnumerator FadeOutNextScene()
    {
        finalFadeOutImage.DOColor(Color.black, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(nextSceneName);
    }

    public void GoToNextScene()
    {
        StartCoroutine(FadeOutNextScene());
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Time.timeScale = 1f;
        Debug.Log("start!");
        currSlide = 0;
        while (currSlide < images.Length)
        {
            yield return StartCoroutine(ShowSlide(images[currSlide], imageText[currSlide]));
            currSlide++;
        }
        GoToNextScene();
    }

    public IEnumerator ShowSlide(Sprite im, string text)
    {
        image.sprite = im;
        textArea.text = text;
        fadeOutImage.color = Color.black;
        yield return new WaitForSeconds(darknessDuration/2);
        fadeOutImage.CrossFadeColor(Color.clear, fadeDuration, true, true);
        yield return new WaitForSeconds(fadeDuration + imageDuration);
        fadeOutImage.CrossFadeColor(Color.black, fadeDuration, true, true);
        yield return new WaitForSeconds(fadeDuration);
        yield return new WaitForSeconds(darknessDuration / 2);
    }
}
