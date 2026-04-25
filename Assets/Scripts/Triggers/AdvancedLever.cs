using System.Collections.Generic;
using UnityEngine;

public class AdvancedLever : Interactable
{
    [SerializeField]
    private List<GameObject> setActiveList;

    [SerializeField]
    private List<GameObject> setInactiveList;
    public override void Interact()
    {
        base.Interact();

        foreach (GameObject activation in setActiveList)
        {
            activation.SetActive(true);
        }

        foreach (GameObject activation in setInactiveList)
        {
            activation.SetActive(false);
        }
    }
}
