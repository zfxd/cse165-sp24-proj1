public class OrientationToggleButtonBehaviour : RayButtonInteractable
{
    public bool isFlipped = false;

    public override void OnRelease()
    {
        isFlipped = !isFlipped;

        if (isFlipped)
        {
            r.material.SetColor("_Color", pressedColor);
        }
        else
        {
            r.material.SetColor("_Color", hoverColor);
        }
    }

    public override void OnBlur()
    {
        if (isFlipped)
        {
            r.material.SetColor("_Color", pressedColor);
        }
        else
        {
            r.material.SetColor("_Color", defaultColor);
        }
    }
}
