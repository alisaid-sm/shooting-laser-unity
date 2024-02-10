using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float laserLength = 10f;
    public GameObject hitLimiter;
    public GameObject DamageArea;
    public FixedJoystick fixedJoystick;
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        hitLimiter.transform.position = transform.forward * laserLength;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = fixedJoystick.Horizontal;
        float vertical = fixedJoystick.Vertical;

        Vector3 rotationDirection = new(horizontal, 0, vertical);

        rotationDirection = Vector3.Normalize(rotationDirection);

        if (rotationDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(rotationDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 50*Time.deltaTime);
        }

        lr.SetPosition(0, transform.position);

        LaserV2();
    }

    void LaserV2()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, laserLength);

        Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.name != "DamageArea")
            {
                lr.SetPosition(1, hits[i].point);
                DamageArea.transform.position = hits[i].point;
                break;
            }
        }
    }

    void LaserV1()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider && hit.transform.name != "DamageArea")
            {
                lr.SetPosition(1, hit.point);
                float distance = Vector3.Distance(transform.position, hit.point);
                if (distance == hit.distance)
                {
                    DamageArea.transform.position = hit.point;
                }
            }
            else if (hit.transform.name == "DamageArea")
            {
                RaycastHit hit2;

                if (Physics.Raycast(hit.point, transform.forward, out hit2))
                {
                    if (hit2.collider)
                    {
                        lr.SetPosition(1, hit2.point);
                        float distance = Vector3.Distance(hit.point, hit2.point);
                        if (distance == hit2.distance)
                        {
                            DamageArea.transform.position = hit2.point;
                        }
                    }
                }
            }
        }
    }
}
