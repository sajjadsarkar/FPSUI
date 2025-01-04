using UnityEngine;
using System.Collections;

public class USE : MonoBehaviour
{
    public float maxRayDistance = 2.0f;
    public LayerMask layerMask;
    bool showGui = false;
    RaycastHit hit;

    void Update()
    {
        Vector3 dir = gameObject.transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, dir, out hit, maxRayDistance, layerMask))
        {
			if(!showGui){
				CanvasManager.instance.note.text = "Press key <color=#88FF6AFF> << E >> </color> to Use";
				showGui = true;
			}	
            if (Input.GetButtonDown("Use"))
            {
                GameObject target = hit.collider.gameObject;
                target.BroadcastMessage("Action");
            }
        }
        else
        {
			if(showGui)
			{
				CanvasManager.instance.note.text = "";
				showGui = false;
			}
        }
    }
}