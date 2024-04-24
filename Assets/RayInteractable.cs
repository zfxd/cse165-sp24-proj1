using UnityEngine;
using UnityEngine.XR;

public interface RayInteractable
{
    public void OnHold(XRNode node);
    public void OnHolding(XRNode node);

    public void OnRelease();
    public void OnHover();
    public void OnBlur();
}
