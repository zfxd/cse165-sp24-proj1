using UnityEngine;

public class NewItemButtonBehaviour : RayButtonInteractable
{
    public GameObject spawnTable;
    public GameObject newItem;

    GameObject group;

    protected override void StartAfter()
    {
        group = GameObject.Find("Interactables");
    }

    public override void OnRelease()
    {
        var obj = Instantiate(newItem, spawnTable.transform.position + spawnTable.transform.up * 0.1f, Quaternion.identity);

        obj.AddComponent<BoxCollider>();
        obj.AddComponent<Rigidbody>();
        obj.AddComponent<RayObjectInteractable>();

        obj.transform.SetParent(group.transform);
    }
}
