using UnityEngine;

public class FloorMaterialZone : MonoBehaviour
{
    public FloorMaterial material;
   
    void OnTriggerStay(Collider other)
    {
        if (!other) return;

        Player player = other.GetComponentInParent<Player>();
        if (!player) return;

        player.floorMaterial = material;
    }
    void OnTriggerExit(Collider other)
    {
        if (!other) return;

        Player player = other.GetComponentInParent<Player>();
        if (!player) return;

        player.floorMaterial = FloorMaterial.Default;
    }
}
