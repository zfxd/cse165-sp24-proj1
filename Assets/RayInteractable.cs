using UnityEngine;

public class RayInteractable : MonoBehaviour
{
    public bool isOnHold = false;

    public void OnHold()
    {
        isOnHold = true;
        gameObject.transform.Translate(0.0f, 0.5f, 0.0f);
    }

    public void OnRelease()
    {
        isOnHold = false;

    }
}
