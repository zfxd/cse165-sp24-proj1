using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RayIntersactor : MonoBehaviour
{
    public GameObject leftController;
    public GameObject rightController;

    protected RayController leftCtrl;
    protected RayController rightCtrl;
    // Start is called before the first frame update
    void Start()
    {
        leftCtrl = new RayController(leftController, XRNode.LeftHand);
        rightCtrl = new RayController(rightController, XRNode.RightHand);

    }

    // Update is called once per frame
    void Update()
    {
        leftCtrl.Update();
        rightCtrl.Update();
    }

    private void OnDestroy()
    {
        leftCtrl.Destory();
        rightCtrl.Destory();
    }
}
