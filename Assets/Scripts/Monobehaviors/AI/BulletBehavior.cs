using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {

    public GameObject hitParticle;

    GameObject towerParent = null;
    Tower towerParentInfo = null;
    GameObject target = null;


    public void SetInitialValues(GameObject tower, Tower towerInfo, GameObject targetEnemy)
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
        float distanceToMove = towerParentInfo.BaseBulletSpeed * Time.deltaTime;

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
    }
}
