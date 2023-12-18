using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneLoadingManager : MonoBehaviour
{
    public Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(LoadScene);
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsync(1));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            //float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            yield return null;
        }
    }
}