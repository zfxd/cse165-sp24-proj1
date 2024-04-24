using UnityEngine;
using UnityEngine.XR;

public class RayInteractable : MonoBehaviour
{
    public bool isOnHold = false;
    GameObject leftHand;
    GameObject rightHand;

    private Quaternion initialRotationOffset;

    void Start()
    {
        leftHand = GameObject.Find("LeftHandCube");
        rightHand = GameObject.Find("RightHandCube");
    }

    public void OnHold(XRNode node)
    {
        if (! isOnHold)
        {
            isOnHold = true;

            var obj = node == XRNode.LeftHand ? leftHand : rightHand;

            if (obj != null)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Collider>().enabled = false;

                initialRotationOffset = Quaternion.Inverse(obj.transform.rotation) * transform.rotation;
                OnHolding(node);
            }
        }
    }

    public void OnHolding(XRNode node)
    {
        var obj = node == XRNode.LeftHand ? leftHand : rightHand;

        // holding location
        gameObject.transform.position = obj.transform.position + obj.transform.forward * 0.01f;
        gameObject.transform.rotation = obj.transform.rotation * initialRotationOffset;
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
}
