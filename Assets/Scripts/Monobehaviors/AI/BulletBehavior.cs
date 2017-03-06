using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {

    public GameObject hitParticle;
    public GameObject splashParticle;

    GameObject towerParent = null;
    TowerBehavior towerParentInfo = null;
    GameObject target = null;


    public void SetInitialValues(GameObject tower, TowerBehavior towerInfo, GameObject targetEnemy)
    {
        towerParent = tower;
        towerParentInfo = towerInfo;
        target = targetEnemy;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (target == null)
        {
            target = null;
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.transform.position - transform.position;
        float distanceToMove = towerParentInfo.bulletSpeed* Time.deltaTime;

        if (direction.magnitude <= distanceToMove)
        {
            DamageEnemy();
        }
        else
        {
            transform.Translate(direction.normalized * distanceToMove, Space.World);
        }

    }

    void DamageEnemy()
    {
        Destroy(gameObject);
        Instantiate(hitParticle, target.transform.position + Vector3.up * 2, Quaternion.Euler(0f, 0f, 0f));
        target.GetComponent<EnemyBehavior>().TakeHit(towerParentInfo, towerParent);

        //Check for splash damage
        if (towerParentInfo.towerBase.DoesShotAoeSpread)
        {
            Collider[] collisions = Physics.OverlapSphere(target.transform.position, towerParentInfo.aoeSpreadRadius);
            List<EnemyBehavior> enemiesSplashed = collisions.Where(x => x.GetComponent<EnemyBehavior>() != null && x.gameObject != target).Select(x => x.GetComponent<EnemyBehavior>()).ToList();
            for (int i = 0; i < enemiesSplashed.Count; i++)
            {
                enemiesSplashed[i].TakeSplashHit(towerParentInfo, towerParent);
                Instantiate(splashParticle, enemiesSplashed[i].transform.position + Vector3.up * 2, Quaternion.Euler(0f, 0f, 0f));
            }
        }
    }
}
