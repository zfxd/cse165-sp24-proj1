using UnityEngine;
using UnityEngine.XR;

public class RayController
{
    protected GameObject obj;
    protected XRNode xrNode;
    protected RayGuideline guide = new RayGuideline();

    protected Color idle = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.5f);
    protected Color inUse = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.5f);
    protected Color selected = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.5f);

    private float length = 10.0f;
    private bool deviceOn = false;
    private RayInteractable holded = null;

    public RayController(GameObject obj, XRNode xrNode)
    {
        this.obj = obj;
        this.xrNode = xrNode;
    }

    public void Update()
    {
        Vector3 start = obj.transform.position;
        Vector3 end = obj.transform.position + obj.transform.forward * length;

        if (InputDevices.GetDeviceAtXRNode(xrNode)
                .TryGetFeatureValue(CommonUsages.triggerButton, out bool triggered))
        {
            if (!deviceOn)
            {
                obj.SetActive(true);
                deviceOn = true;
            }

            if (triggered)
            {
                var hitObj = FindRayInteractable();

                if (hitObj != null)
                {
                    holded = hitObj;
                    hitObj.OnHold(xrNode);
                    guide.DrawLine(start, end, selected);
                }
                else if (holded != null)
                {
                    holded.OnHolding(xrNode);
                    guide.DrawLine(start, start, selected);
                } else
                {
                    guide.DrawLine(start, end, idle);
                }
            }
            else
            {
                if (holded != null)
                {
                    var _holded = holded;
                    holded = null;
                    _holded.OnRelease();

                    guide.DrawLine(start, end, inUse);
                }
                else
                {
                    var hitObj = FindRayInteractable();

                    if (hitObj != null)
                    {
                        guide.DrawLine(start, end, inUse);
                    }
                    else
                    {
                        guide.DrawLine(start, end, idle);
                    }
                }
            }
        }
        else
        {
            // device is not available
            guide.DrawLine(start, start, idle);
            if (deviceOn)
            {
                obj.SetActive(false);
                deviceOn = false;
            }

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
