using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float laserLength = 10f;
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                float maxDistance = Vector3.Distance(transform.position, transform.forward*laserLength);
                float hitDistance = Vector3.Distance(transform.position, hit.point);
                
                if (hitDistance <= maxDistance)
                {
                    lr.SetPosition(1, hit.point);
                }
                else lr.SetPosition(1, transform.forward*laserLength);
            }
        }
        else lr.SetPosition(1, transform.forward*laserLength);
    }
}
