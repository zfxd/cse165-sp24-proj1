using UnityEngine;
using UnityEngine.XR;

public class RayButtonInteractable : MonoBehaviour, RayInteractable
{
    protected Renderer r;
    protected Color defaultColor;
    protected Color hoverColor = new Color(0.9f, 0.9f, 1.0f, 1.0f);
    protected Color pressedColor = new Color(0.4f, 1.0f, 0.4f, 1.0f);

    void Start()
    {
        r = GetComponent<Renderer>();
        defaultColor = r.material.color;
        StartAfter();
    }

    protected virtual void StartAfter()
    {
    }

    public void OnHold(XRNode node)
    {
        r.material.SetColor("_Color", pressedColor);
    }

    public void OnHolding(XRNode node)
    {
    }

    virtual public void OnRelease()
    {
    }

    public void OnHover()
    {
        r.material.SetColor("_Color", hoverColor);
    }

    virtual public void OnBlur()
    {
        r.material.SetColor("_Color", defaultColor);
    }
}
