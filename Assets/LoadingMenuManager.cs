using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

public class LoadingMenuManager : MonoBehaviour
{
    public GameObject canvas;

    public Image Background;
    public TMP_Text loading;
    public TMP_Text percentage;
    public float fadingTime = 1.25f;
    public bool isBusy = false;

    public static LoadingMenuManager inst;

    void Awake() {
        if (inst != this && inst != null) { Destroy(canvas); Destroy(gameObject); return; }

        inst = this;
    }

    void Start() {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(canvas);
    }

    public void LoadScene(string name) {
        if (isBusy) return;

        isBusy = true;
        StartCoroutine(LoadSceneAsync(name));
    }

    IEnumerator LoadSceneAsync(string name) {
        canvas.SetActive(true);

        yield return Fade(true);

        float timePassed = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        Debug.Log(operation != null);
        while (!operation.isDone) {
            timePassed += Time.deltaTime;
            loading.text = "Loading" + (timePassed > 1f ? ".." : (timePassed > 0.5f ? "." : "..."));
            percentage.text = $"[{operation.progress * 100}%]";
            if (timePassed >= 1.5f) timePassed = 0f;
            yield return null;
        }

        yield return Fade(false);
        isBusy = false;

        canvas.SetActive(false);
    }

    float easeOutQuart(float x) {
        return 1 - Mathf.Pow(1 - x, 4);
    }

    IEnumerator Fade(bool inout) {
        yield return null;

        float timePassed = 0f;
        Color bgcolor = Background.color;
        while (timePassed < fadingTime) {
            float easing = Mathf.Abs((inout ? 0 : 1) - easeOutQuart(timePassed / fadingTime));
            bgcolor.a = easing;
            loading.alpha = easing;
            percentage.alpha = easing;

            yield return null;
            timePassed += Time.deltaTime;
        }

        yield return null;
    }





    /*

    IEnumerator LoadAsyncScene(string sceneName)
    {
        loadingCanvas.SetActive(true);
        StartCoroutine(Fade(true, 0.75f));

        while (fading) yield return null; // Poczekaj do końca animacji

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 0.9 to warto�� zako�czenia �adowania
            int progressPercentage = Mathf.RoundToInt(progress * 100);
            loadingText.text = progressPercentage + "%";
            loadingSlider.value = progress;

            yield return null;
        }

        StartCoroutine(Fade(false, 0.75f));

        while (fading) yield return null;
        loadingCanvas.SetActive(false);
    }


    #region looks
    float Easing(float x) {
        return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }

    bool fading = false;
    IEnumerator Fade(bool value, float time) { // True - FadeIn; False - FadeOut
        fading = true;
        float f = 0f;

        Color thisGreenMf = new Color(104/255f, 1f, 0f);

        while (true) {
            yield return new WaitForEndOfFrame();
            f += Time.deltaTime;
            if (f >= time) break;

            float ease = Easing(Mathf.Abs((value ? 0 : 1) - f));
            thisGreenMf.a = ease;

            loadingText.alpha = ease;
            loadingCanvas.transform.Find("Image").GetComponent<Image>().color = new Color(0, 0, 0, ease);
            loadingSlider.transform.Find("Background").GetComponent<Image>().color = new Color(1, 1, 1, ease);
            loadingSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = thisGreenMf;
        }

        float end = value ? 1f : 0f;
        thisGreenMf.a = end;
        loadingText.alpha = end;
        loadingCanvas.transform.Find("Image").GetComponent<Image>().color = new Color(0, 0, 0, end);
        loadingSlider.transform.Find("Background").GetComponent<Image>().color = new Color(1, 1, 1, end);
        loadingSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(104/255f, 1f, 0f, end);

        yield return new WaitForSeconds(0.25f);
        fading = false;
    }
    #endregion */
}
