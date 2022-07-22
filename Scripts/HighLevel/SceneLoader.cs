using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneIndexes _scene;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        DontDestroyOnLoad(this);
        StartCoroutine(LoadScene(_scene));
    }

    private IEnumerator LoadScene(SceneIndexes scene)
    {
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync((int)scene);

        while (!sceneLoading.isDone)
        {
            _progressBar.value = sceneLoading.progress;
            yield return null;
        }

        OnLoad();
    }

    private void OnLoad()
    {
        _progressBar.value = _progressBar.maxValue;
        _animator.SetTrigger("Start");
    }

    private enum SceneIndexes
    {
        Initial = 0,
        Main = 1
    }
}
