using System;
using UnityEngine;
using UnityEngine.XR;

public class RayObjectInteractable : MonoBehaviour, RayInteractable
{
    public bool isOnHold = false;
    public bool scalable = true;
    public float maxHoldingDistance = 0.2f;
    public float holdingDistance;

    protected GameObject leftHand;
    protected GameObject rightHand;
    protected Renderer r;
    protected Color defaultColor;
    protected Color highlightColor = new Color(0.5f, 0.5f, 1.0f, 1.0f);
    protected Color resizingColor = new Color(0.5f, 1.0f, 0.5f, 0.5f);

    protected Quaternion initialRotationOffset;
    protected float initDistance;
    protected bool scalerEnabled;

    void Start()
    {
        leftHand = GameObject.Find("LeftHandCube");
        rightHand = GameObject.Find("RightHandCube");
        r = GetComponent<Renderer>();

        defaultColor = r.material.color;
        StartAfter();
    }

    public virtual void StartAfter() { }

    public virtual void OnHold(XRNode node)
    {
        if (!isOnHold)
        {
            isOnHold = true;
            holdingDistance = maxHoldingDistance; // use it as init value

            var obj = node == XRNode.LeftHand ? leftHand : rightHand;

            if (obj != null)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                //gameObject.GetComponent<Collider>().enabled = false;

                initialRotationOffset = Quaternion.Inverse(obj.transform.rotation) * transform.rotation;
                removeHighlight();
                if (InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.gripButton, out bool gripped))
                {
                    if (gripped)
                    {
                        holdingDistance = Vector3.Distance(gameObject.transform.position, obj.transform.position);
                    }
                }
                OnHolding(node);
            }
        }
    }

    public virtual void OnHolding(XRNode node)
    {
        var obj = node == XRNode.LeftHand ? leftHand : rightHand;

        // holding location
        gameObject.transform.position = obj.transform.position + obj.transform.forward * holdingDistance;
        gameObject.transform.rotation = obj.transform.rotation * initialRotationOffset;

        if (scalable && InputDevices.GetDeviceAtXRNode(XRNode.LeftHand)
            .TryGetFeatureValue(CommonUsages.gripButton, out bool leftGripped))
        {
            if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand)
                .TryGetFeatureValue(CommonUsages.gripButton, out bool rightGripped))
            {
                if (leftGripped && rightGripped)
                {
                    float dist = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);

                    if (! scalerEnabled)
                    {
                        scalerEnabled = true;
                        initDistance = dist;

                        r.material.SetColor("_Color", resizingColor);
                    }

                    float deltaDist = dist - initDistance;
                    gameObject.transform.localScale *= (1.0f + deltaDist);

                    initDistance = dist;
                } else
                {
                    scalerEnabled = false;
                    r.material.SetColor("_Color", defaultColor);
                }
            }
        }
    }

    public virtual void OnRelease()
    {
        if (isOnHold)
        {
            isOnHold = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            //gameObject.GetComponent<Collider>().enabled = true;
        }
    }

    public void OnHover()
    {
        applyHighlight();
    }

    public void OnBlur()
    {
        removeHighlight();
    }

    void applyHighlight()
    {
        r.material.SetColor("_Color", highlightColor);
    }

    void removeHighlight()
    {
        r.material.SetColor("_Color", defaultColor);
    }

}
