using UnityEngine;

public class RemoveInteractablesButtonBehaviour : RayButtonInteractable
{
    public GameObject groups;

    public override void OnRelease()
    {
        foreach (Transform child in groups.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
