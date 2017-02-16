using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TargetPriority
{
    CLOSEST,
    FURTHEST,
    STRONGEST,
    WEAKEST
}

public class TowerBehavior : MonoBehaviour {

    GameObject target;
    Tower towerBase;
    bool isUpdating = false;
    TargetPriority targetPriority = TargetPriority.FURTHEST;
    int kills;

    public void SetInitialValues(Tower tower)
    {
        //Todo, use tile info to calculate the level of the tower initially.
        //Todo, set ammo effects initially.
        //Todo, generate random name?
        towerBase = tower;
    }

    private void UpdateTarget()
    {
        if (!isUpdating)
        {
            GameObject previousTarget = target;
            GameObject closest = null;
            GameObject furthest = null;
            GameObject strongest = null;
            GameObject weakest = null;
            Collider[] collidersInRange = Physics.OverlapSphere(transform.position, towerBase.BaseRadius);
            List<GameObject> objects = collidersInRange.Where(x => x.gameObject.GetComponent<EnemyBehavior>() != null).Select(x => x.gameObject).ToList();
            List<GameObject> objectsByDistance = objects.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();
            List<GameObject> objectsByHealth = objects.OrderBy(x => x.GetComponent<EnemyBehavior>().currentHealth).ToList();
            closest = objectsByDistance.FirstOrDefault();
            furthest = objectsByDistance.LastOrDefault();
            weakest = objectsByHealth.FirstOrDefault();
            strongest = objectsByHealth.LastOrDefault();

            switch(targetPriority)
            {
                case TargetPriority.CLOSEST:
                    target = closest;
                    break;
                case TargetPriority.FURTHEST:
                    target = furthest;
                    break;
                case TargetPriority.STRONGEST:
                    target = strongest;
                    break;
                case TargetPriority.WEAKEST:
                    target = weakest;
                    break;
            }

            if (target != null && target != previousTarget)
            {
                target.GetComponent<SpriteRenderer>().color = Color.green;
                if (target != previousTarget)
                {
                    previousTarget.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }

	// Use this for initialization
	void Start () {
        InvokeRepeating("UpdateTarget", 0f, .3f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, towerBase.BaseRadius);
    }
}
