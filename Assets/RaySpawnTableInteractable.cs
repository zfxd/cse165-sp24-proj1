using UnityEngine;
using UnityEngine.XR;

public class RaySpawnTableInteractable : RayObjectInteractable
{
    private GameObject table;
    private GameObject canvas;
    private Vector3 caretPositionOffset;
    private Vector3 canvasPositionOffset;
    private float tableY;
    private float canvasY;
    private float y;

    public override void StartAfter() {
        table = GameObject.Find("SpawnTable");
        canvas = GameObject.Find("ObjectCreatorCanvas");
        caretPositionOffset = gameObject.transform.position - table.transform.position;
        canvasPositionOffset = gameObject.transform.position - canvas.transform.position;
        tableY = table.transform.position.y;
        canvasY = canvas.transform.position.y;
        y = gameObject.transform.position.y;
    }

    public override void OnHolding(XRNode node)
    {
        base.OnHolding(node);

        Vector3 newPosition = gameObject.transform.position - caretPositionOffset;
        newPosition.y = tableY;
        table.transform.position = newPosition;

        Vector3 newCanvasPosition = gameObject.transform.position - canvasPositionOffset;
        newCanvasPosition.y = canvasY;
        canvas.transform.position = newCanvasPosition;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
    }
}
