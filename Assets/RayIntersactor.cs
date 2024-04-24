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
        rightCtrl.Update();
        leftCtrl.Update();
    }

    private void OnDestroy()
    {
        rightCtrl.Destory();
        leftCtrl.Destory();
    }
}
