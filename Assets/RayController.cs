using UnityEngine;
using UnityEngine.XR;

enum TriggerButtonCondition
{
    Idle,
    Pressing,
    Pressed,
    Releasing,
    Released, // go back to Idle
    NoExist,
}

public class RayController
{
    protected GameObject obj;
    protected XRNode xrNode;
    protected RayGuideline guide = new RayGuideline();

    protected Color idle = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.5f);
    protected Color invalid = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
    protected Color inUse = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.5f);
    protected Color selected = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.5f);

    private float length = 10.0f;
    private bool deviceOn = false;

    private RayInteractable held = null;
    private RayInteractable hovered = null;

    private TriggerButtonCondition condition = TriggerButtonCondition.NoExist;

    public RayController(GameObject obj, XRNode xrNode)
    {
        this.obj = obj;
        this.xrNode = xrNode;
    }

    public void Update()
    {
        Vector3 start = obj.transform.position;
        Vector3 end = obj.transform.position + obj.transform.forward * length;

        UpdateCondition();

        // event handling
        if (condition != TriggerButtonCondition.NoExist && !deviceOn)
        {
            obj.SetActive(true);
            deviceOn = true;
        }
        else if (condition == TriggerButtonCondition.NoExist && deviceOn)
        {
            obj.SetActive(false);
            deviceOn = false;

            guide.DrawLine(start, start, idle);
        }
        else if (condition == TriggerButtonCondition.Idle)
        {
            var hitObj = FindRayInteractable();

            if (hitObj != null)
            {
                if (hovered != null && hovered != hitObj)
                {
                    hovered.OnBlur();
                }
                hovered = hitObj;
                hovered.OnHover();

                guide.DrawLine(start, end, inUse);
            }
            else
            {
                if (hovered != null)
                {
                    hovered.OnBlur();
                    hovered = null;
                }

                guide.DrawLine(start, end, idle);
            }
        }
        else if (condition == TriggerButtonCondition.Pressing)
        {
            var hitObj = FindRayInteractable();

            if (hitObj != null)
            {
                held = hitObj;
                held.OnHold(xrNode);

                guide.DrawLine(start, end, selected);
            }
        }
        else if (condition == TriggerButtonCondition.Pressed)
        {
            if (held != null)
            {
                held.OnHolding(xrNode);
                // hide draw line on holding behavior
                guide.DrawLine(start, start, selected);
            }
            else
            {
                guide.DrawLine(start, end, invalid);
            }
        }
        else if (condition == TriggerButtonCondition.Releasing)
        {
            if (held != null)
            {
                var _held = held;
                held = null;
                _held.OnRelease();
                guide.DrawLine(start, start, inUse);
            }
            else
            {
                guide.DrawLine(start, end, idle);
            }
        }
        else if (condition == TriggerButtonCondition.Released)
        {
            // check any remaining cases
        }
    }

    protected void UpdateCondition()
    {
        if (InputDevices.GetDeviceAtXRNode(xrNode)
                .TryGetFeatureValue(CommonUsages.triggerButton, out bool triggered))
        {
            if (triggered)
            {
                if (
                    condition == TriggerButtonCondition.Pressing
                    || condition == TriggerButtonCondition.Pressed
                )
                {
                    condition = TriggerButtonCondition.Pressed;
                }
                else
                {
                    condition = TriggerButtonCondition.Pressing;
                }
            }
            else
            {
                if (condition == TriggerButtonCondition.Pressing || condition == TriggerButtonCondition.Pressed)
                {
                    condition = TriggerButtonCondition.Releasing;
                }
                else if (condition == TriggerButtonCondition.Releasing)
                {
                    condition = TriggerButtonCondition.Released;
                }
                else if (condition == TriggerButtonCondition.Released)
                {
                    condition = TriggerButtonCondition.Idle;
                }
                else
                {
                    condition = TriggerButtonCondition.Idle;
                }
            }
        }
        else
        {
            // device is not available
            condition = TriggerButtonCondition.NoExist;
        }
    }

    public void Destory()
    {
        guide.Destroy();
    }

    RayInteractable FindRayInteractable()
    {
        Ray ray = new Ray(obj.transform.position, obj.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.gameObject.TryGetComponent(out RayInteractable obj))
            {
                return obj;
            }
        }

        return null;
    }

}
