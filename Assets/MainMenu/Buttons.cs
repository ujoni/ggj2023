using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{

    public Sprite[] startButtonFrames;

    private bool started = false;

    public void Exit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        if (this.started) return;
        this.started = true;

        this.StartCoroutine(this.StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        var image = this.GetComponent<Image>();
        foreach (var frame in startButtonFrames)
        {
            yield return new WaitForSeconds(0.2f);
            image.sprite = frame;
        }
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
