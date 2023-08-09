using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalSkillRange : MonoBehaviour
{
    public float skillRadius = 5f;
    public LayerMask targetLayer;
    public GameObject skill1;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider[] hits = Physics.OverlapSphere(mousePosition, skillRadius, targetLayer);
            Debug.Log("¿€µø");

            foreach (Collider hit in hits)
            {
                // Handle skill effect on the hit target

            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, skillRadius);
    }
}
