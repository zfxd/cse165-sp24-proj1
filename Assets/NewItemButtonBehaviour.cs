using UnityEngine;

public class NewItemButtonBehaviour : RayButtonInteractable
{
    public GameObject spawnTable;
    public GameObject newItem;
    public GameObject flipXBtn;
    public GameObject flipYBtn;
    public GameObject flipZBtn;

    protected OrientationToggleButtonBehaviour toggleX;
    protected OrientationToggleButtonBehaviour toggleY;
    protected OrientationToggleButtonBehaviour toggleZ;

    GameObject group;

    protected override void StartAfter()
    {
        group = GameObject.Find("Interactables");
        flipXBtn.TryGetComponent(out toggleX);
        flipYBtn.TryGetComponent(out toggleY);
        flipZBtn.TryGetComponent(out toggleZ);
    }

    public override void OnRelease()
    {
        Quaternion rotate = Quaternion.Euler(
                toggleX.isFlipped ? 180.0f : 0.0f,
                toggleY.isFlipped ? 180.0f : 0.0f,
                toggleZ.isFlipped ? 180.0f : 0.0f
                );

        var obj = Instantiate(newItem, spawnTable.transform.position + spawnTable.transform.up * 0.1f, rotate);

        obj.AddComponent<BoxCollider>();
        obj.AddComponent<Rigidbody>();
        obj.AddComponent<RayObjectInteractable>();

        obj.transform.SetParent(group.transform);
    }
}
