using System;
using UnityEngine;
using UnityEngine.XR;

public class RayObjectInteractable : MonoBehaviour, RayInteractable
{
    public bool isOnHold = false;
    GameObject leftHand;
    GameObject rightHand;
    Renderer r;
    Color defaultColor;
    Color highlightColor = new Color(0.5f, 0.5f, 1.0f, 1.0f);
    Color resizingColor = new Color(0.5f, 1.0f, 0.5f, 0.5f);

    private Quaternion initialRotationOffset;

    private float initDistance;
    private bool scalerEnabled;

    void Start()
    {
        leftHand = GameObject.Find("LeftHandCube");
        rightHand = GameObject.Find("RightHandCube");
        r = GetComponent<Renderer>();

        defaultColor = r.material.color;
    }

    public void OnHold(XRNode node)
    {
        if (!isOnHold)
        {
            isOnHold = true;

            var obj = node == XRNode.LeftHand ? leftHand : rightHand;

            if (obj != null)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Collider>().enabled = false;

                initialRotationOffset = Quaternion.Inverse(obj.transform.rotation) * transform.rotation;
                removeHighlight();
                OnHolding(node);
            }
        }
    }

    public void OnHolding(XRNode node)
    {
        var obj = node == XRNode.LeftHand ? leftHand : rightHand;

        // holding location
        gameObject.transform.position = obj.transform.position + obj.transform.forward * 0.2f;
        gameObject.transform.rotation = obj.transform.rotation * initialRotationOffset;

        if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand)
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

    public void OnRelease()
    {
        if (isOnHold)
        {
            isOnHold = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Collider>().enabled = true;
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
