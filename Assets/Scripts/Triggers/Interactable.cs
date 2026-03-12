using UnityEngine;

public class Interactable : MonoBehaviour
{

    protected NarrativeTrigger messageTrigger;
    private void Start()
    {
        messageTrigger = GetComponent<NarrativeTrigger>();
    }
    public virtual void Interact()
    {

    }
}
