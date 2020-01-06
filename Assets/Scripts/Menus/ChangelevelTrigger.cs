using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangelevelTrigger : MonoBehaviour
{
    private void Awake()
    {
        HubManagerScript.Instance.levelExit = this.gameObject;

        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(2);
    }
}
