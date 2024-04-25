using System;
using UnityEngine;
using UnityEngine.XR;

public class RayGroundInteractable : MonoBehaviour, RayInteractable
{

    public GameObject cam;

    protected GameObject leftHand;
    protected GameObject rightHand;

    // Start is called before the first frame update
    void Start()
    {
        leftHand = GameObject.Find("LeftHandCube");
        rightHand = GameObject.Find("RightHandCube");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnHold(XRNode node)
    {
        //throw new NotImplementedException();
        // Find the location pointed at by controller
        
        var obj = node == XRNode.LeftHand ? leftHand : rightHand;

        Ray ray = new Ray(obj.transform.position, obj.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Move camera to the location
            Vector3 pos = cam.transform.position;
            pos.x = hit.point.x;
            pos.z = hit.point.z;
            cam.transform.position = pos;
        }

    }

    public virtual void OnHolding(XRNode node)
    {
        //throw new NotImplementedException();
        var obj = node == XRNode.LeftHand ? leftHand : rightHand;

        // If Grip button is pressed, rotate the camera
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.gripButton, out bool leftGripped);
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.gripButton, out bool rightGripped);
    
        if (rightGripped){
            cam.transform.RotateAround(obj.transform.position, Vector3.up, -3.0f);
        } else if (leftGripped){
            cam.transform.RotateAround(obj.transform.position, Vector3.up, 3.0f);
        }
    }

    public virtual void OnRelease()
    {
        //throw new NotImplementedException();
    }
    
    public virtual void OnHover(){
        //throw new NotImplementedException();
    }

    public virtual void OnBlur(){
        //throw new NotImplementedException();
    }
}
