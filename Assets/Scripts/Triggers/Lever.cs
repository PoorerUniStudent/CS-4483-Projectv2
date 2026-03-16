using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Activates/deactivates certain objects
public class Lever : Interactable
{
    [SerializeField]
    private List<GameObject> activationList;

    [SerializeField]
    private bool oneTimeUse;

    private bool used;
    public override void Interact()
    {
        if (oneTimeUse && used)
        {
            return;
        }

        base.Interact();

        foreach (GameObject activation in activationList)
        {
            Light2D light = activation.GetComponentInChildren<Light2D>(true);
            SpriteRenderer spriteRenderer = activation.GetComponent<SpriteRenderer>();
            Debug.Log(activation);
            if (light)
            {
                ChangeLightIntensity(light);
            }
            else if (spriteRenderer)
            {
                ChangeSpriteColorBlackWhite(spriteRenderer);
            }
        }

        if (oneTimeUse)
        {
            used = true;
            messageTrigger.Disable();
        }
    }

    public void ChangeLightIntensity(Light2D light)
    {
        light.intensity = light.intensity > 0 ? 0 : 1; ;
        light.gameObject.transform.parent.gameObject.SetActive(!light.gameObject.transform.parent.gameObject.activeSelf);
    }

    // Changes between black and white
    public void ChangeSpriteColorBlackWhite(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer.color == Color.white)
        {
            spriteRenderer.color = Color.black;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }   
    }
}
