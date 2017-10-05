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

    private const float DEFAULT_TIME_LINGER = 0.25f;
    private const float DEFAULT_FORCE_MULT = 10f;
    private LineRenderer laser;
    private Unit parent;

	private void Start ()
    {
        laser = GetComponent<LineRenderer>();
        parent = GetComponentInParent<Unit>();
        Debug.Assert(laser);
        Debug.Assert(parent);

        laser.startColor = parent.Team.color;
        laser.endColor = Color.Lerp(parent.Team.color, Color.clear, 0.5f);
        laser.enabled = false;
	}
	
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
    /// <returns></returns>
    private IEnumerator Fire(float range, float damage, float time)
    {
        laser.enabled = true;
        bool hitOnce = false; // don't hit more than once per shot

        while (time > 0f)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            laser.SetPosition(0, ray.origin);
            laser.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Time.time);

            // Did we hit a unit?
            if (Physics.Raycast(ray, out hit, range, Toolbox.UnitLayer))
            {
                Unit contact = hit.collider.GetComponent<Unit>();
                // Was the unit an enemy?
                if (contact.Team != parent.Team)
                {
                    Debug.Assert(range > 0);
                    float falloff = 1 - ((hit.point - transform.position).magnitude / range);
                    if (!hitOnce)
                    {
                        contact.UpdateHealth(-(damage * falloff), parent);
                        hitOnce = true;
                    }
                    // We can assume there's a rigidbody, because it's a unit.
                    hit.rigidbody.AddForceAtPosition(transform.forward * falloff * DEFAULT_FORCE_MULT, hit.point);
                    hitOnce = true;
                }
                laser.SetPosition(1, hit.point);
            }
            // Don't let the laser go through walls.
            else if (Physics.Raycast(ray, out hit, range))
            {
                laser.SetPosition(1, hit.point);
            }
            // Don't let the laser go on forever.
            else
            {
                laser.SetPosition(1, ray.GetPoint(range));
            }

            time -= Time.deltaTime;

            yield return null;
        }

        laser.enabled = false;

        yield return null;
    }
}
