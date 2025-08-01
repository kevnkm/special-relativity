using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event_6 : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitBeforeNextNode());
    }

    private IEnumerator WaitBeforeNextNode()
    {
        var camera = Camera.main.GetComponent<FadeCamera>();

        yield return new WaitForSeconds(1f);
        yield return camera.SetUIFadeTrigger(FadeCamera.FadeType.FadeIn, 1f);

        var currentScene = SceneManager.GetActiveScene().name;
        var nextScene = $"Scene_{int.Parse(currentScene.Split('_')[1]) + 1}";
        SceneManager.LoadScene(nextScene);

        Destroy(gameObject);
    }
}
