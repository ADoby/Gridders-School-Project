using UnityEngine;
using System.Collections;

public class TriggerBool : MonoBehaviour {

    public LayerMask mask;
    public int hits = 0;

    public bool Hitting()
    {
        return (hits > 0);
    }

    private void UpdateHits(bool hit)
    {
        if (hit)
            hits++;
        else
            hits--;
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((mask.value & (1 << obj.layer)) > 0);
    }

    void OnTriggerEnter(Collider info)
    {
        if (IsInLayerMask(info.gameObject, mask)) 
        {
            UpdateHits(true);
        }
    }

    void OnTriggerExit(Collider info)
    {
        if (IsInLayerMask(info.gameObject, mask))
        {
            UpdateHits(false);
        }
            
    }
}
