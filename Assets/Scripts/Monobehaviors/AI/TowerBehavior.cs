using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TargetPriority
{
    CLOSEST,
    FURTHEST,
    STRONGEST,
    WEAKEST,
    ALL
}

public class TowerBehavior : MonoBehaviour {

    List<GameObject> targets = new List<GameObject>();
    GameObject gridTile;
    TileInfo gridTileInfo;
    public Tower towerBase { get; private set; }
    bool isUpdating = false;
    TargetPriority targetPriority = TargetPriority.CLOSEST;
    public int kills { get; private set; }
    public int level { get; private set; }
    public float radius { get; private set; }
    public float bulletSpeed { get; private set; }
    public float aoeSpreadRadius { get; private set; }
    public double aoeDamagePercentage { get; private set; }
    public float timeBetweenShots { get; private set; }
    public double damage { get; private set; }
    public int costToUpgrade { get; private set; }

    const float waitAfterChange = 3f;

    public void SetInitialValues(Tower tower, GameObject tilePos)
    {
        //Todo, use tile info to calculate the level of the tower initially.
        //Todo, set ammo effects initially.
        //Todo, generate random name?
        towerBase = tower;
        gridTile = tilePos;
        gridTileInfo = gridTile.GetComponent<TileInfo>();
        if (towerBase.TargetsRadius)
        {
            targetPriority = TargetPriority.ALL;
        }
        level = 1;
        SetStatsForLevel();
    }

    public void Upgrade()
    {
        level += 1;
        SetStatsForLevel();
        StartCoroutine(WaitAfterChange());
    }

    IEnumerator WaitAfterChange()
    {
        if (isUpdating) yield break;
        isUpdating = true;
        Color originalColor = gameObject.GetComponent<SpriteRenderer>().material.color;
        gameObject.GetComponent<SpriteRenderer>().material.color = Color.Lerp(originalColor, Color.red, .5f);
        yield return new WaitForSeconds(waitAfterChange);
        isUpdating = false;
        gameObject.GetComponent<SpriteRenderer>().material.color = originalColor;
    }

    private void SetStatsForLevel()
    {
        radius = towerBase.BaseRadius + (towerBase.BaseRadius * (float)(towerBase.RadiusIncreasePercent * (level-1)));
        bulletSpeed = towerBase.BaseBulletSpeed;
        aoeSpreadRadius = towerBase.BaseAoeSpreadRadius;
        aoeDamagePercentage = towerBase.BaseAoeDamagePercentage;
        timeBetweenShots = towerBase.BaseTimeBetweenShots - (towerBase.BaseTimeBetweenShots * (float)(towerBase.TimeBetweenShotsIncreasePercent * (level-1)));
        damage = towerBase.BaseDamage + (towerBase.BaseDamage * (float)(towerBase.DamageIncreasePercent * (level-1)));
        costToUpgrade = (int)(towerBase.BaseUpgradeCost + (towerBase.BaseUpgradeCost * (towerBase.CostIncreasePercent * ((level - 1)))));
    }

    private void UpdateTarget()
    {
        if (!isUpdating)
        {
            targets.Clear();
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
                    targets.Add(closest);
                    break;
                case TargetPriority.FURTHEST:
                    targets.Add(furthest);
                    break;
                case TargetPriority.STRONGEST:
                    targets.Add(strongest);
                    break;
                case TargetPriority.WEAKEST:
                    targets.Add(weakest);
                    break;
                case TargetPriority.ALL:
                    targets.AddRange(objectsByDistance);
                    break;
            }
        }
    }

    private void ShootTarget()
    {
        if(!isUpdating && targets.Count > 0)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                GameObject bullet = Instantiate(towerBase.BaseBullet, transform.position - towerBase.BulletSpawnOffset, Quaternion.Euler(0f, 0f, 0f));
                bullet.GetComponent<SpriteRenderer>().color = towerBase.ShotColor;
                bullet.GetComponent<BulletBehavior>().SetInitialValues(gameObject, this, targets[i]);
            }
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
