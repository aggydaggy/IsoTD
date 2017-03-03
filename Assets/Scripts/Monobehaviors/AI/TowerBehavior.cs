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
    GameObject gridTile;
    TileInfo gridTileInfo;
    Tower towerBase;
    bool isUpdating = false;
    TargetPriority targetPriority = TargetPriority.CLOSEST;
    public int kills { get; private set; }

    public void SetInitialValues(Tower tower, GameObject tilePos)
    {
        //Todo, use tile info to calculate the level of the tower initially.
        //Todo, set ammo effects initially.
        //Todo, generate random name?
        towerBase = tower;
        gridTile = tilePos;
        gridTileInfo = gridTile.GetComponent<TileInfo>();
    }

    private void UpdateTarget()
    {
        if (!isUpdating)
        {
            if (target == null) target = null;
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
        }
    }

    private void ShootTarget()
    {
        if(!isUpdating && target != null)
        {
            GameObject bullet = Instantiate(towerBase.BaseBullet,transform.position - towerBase.BulletSpawnOffset, Quaternion.Euler(0f, 0f, 0f));
            bullet.GetComponent<SpriteRenderer>().color = towerBase.ShotColor;
            bullet.GetComponent<BulletBehavior>().SetInitialValues(gameObject, towerBase, target);
        }
    }

	// Use this for initialization
	void Start () {
        InvokeRepeating("UpdateTarget", 0f, .3f);
        InvokeRepeating("ShootTarget", 0f, towerBase.BaseTimeBetweenShots);
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, towerBase.BaseRadius);
    }

    public void GotAKill()
    {
        kills += 1;
    }
}
