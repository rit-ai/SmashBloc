using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Script to manage a laser gun.
 * **/
public class Laser : MonoBehaviour
{

    private const float DEFAULT_TIME_LINGER = 0.5f;
    private const float DEFAULT_FORCE_MULT = 10f;
    private LineRenderer laser;
    private CanvasRenderer canvas;
    private Unit parent;
	
	public void Shoot(float range, float damage, float time = DEFAULT_TIME_LINGER)
    {
        StartCoroutine(Fire(range, damage, time));
    }

    /// <summary>
    /// Fires the laser.
    /// </summary>
    /// <param name="range">The range to which the laser will fire. Experiences falloff.</param>
    /// <param name="damage">The damage the laser deals. Experiences falloff.</param>
    /// <param name="time">The time the laser will linger.</param>
    /// NOTE: Because of some uninitutitive properties of Line Renderer and 
    /// Raycasating, please exercise caution before you change where the 
    /// position of the start and end of the laser are set.
    private IEnumerator Fire(float range, float damage, float time)
    {
        laser.enabled = true;
        bool hitOnce = false; // don't hit more than once per shot

        while (time > 0f)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            laser.SetPosition(0, Vector3.zero);
            laser.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Time.time);
            if (Toolbox.Debuggy.Lasers) { DebugFire(ray.origin, ray.direction * range); }

            if (Physics.Raycast(ray, out hit, range))
            {
                // Did we hit a unit?
                Unit contact = hit.collider.GetComponent<Unit>();
                if (contact != null)
                {
                    // Was the unit an enemy?
                    if (contact.Team != parent.Team)
                    {
                        Debug.Assert(range > 0);
                        float falloff = 1 - ((hit.point - transform.position).magnitude / range);
                        if (!hitOnce)
                        {
                            contact.UpdateHealth(-(damage * falloff), parent);
                            Debug.Log("damaged: " + contact.Identity() + contact.Team.title);
                            hitOnce = true;
                        }
                        // If it's a mobile unit, we apply a force.
                        if (contact is MobileUnit)
                        {
                            Rigidbody body = contact.GetComponent<Rigidbody>();
                            body.AddForceAtPosition(transform.forward * falloff * DEFAULT_FORCE_MULT, hit.point);
                        }
                    }
                }
                // We hit something, so don't let the laser go through it.
                float distance = (transform.position - hit.point).magnitude * 1.05f; // grace value, would rather stop
                                                                                    // too late than too early
                laser.SetPosition(1, Vector3.forward * distance);
            }
            else
            {
                // The laser extends to full range if we don't hit anything.
                laser.SetPosition(1, Vector3.forward * range);
            }

            time -= Time.deltaTime;

            yield return null;
        }

        laser.enabled = false;

        yield return null;
    }

    /// <summary>
    /// Draws where the raycast is actually occurring.
    /// </summary>
    private void DebugFire(Vector3 start, Vector3 end)
    {
        Debug.DrawRay(start, end, Color.green, 2f, false);
    }

    private void Start()
    {
        laser = GetComponent<LineRenderer>();
        canvas = GetComponent<CanvasRenderer>();
        parent = GetComponentInParent<Unit>();
        Debug.Assert(laser != null);
        Debug.Assert(canvas != null);
        Debug.Assert(parent != null);

        laser.material.color = parent.Team.color;

        laser.enabled = false;
    }
}
