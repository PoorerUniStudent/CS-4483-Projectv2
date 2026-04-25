using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    [SerializeField] private string scene;
    public override void Interact()
    {
        base.Interact();

        if (scene != null && !scene.Equals(""))
        {
            GameManager.instance.LoadScene(scene);
        }
    }
}
