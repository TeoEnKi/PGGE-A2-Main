using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolBullet : MonoBehaviour
{
    public List<GameObject> photonListBullets = new List<GameObject>();
    [SerializeField] private GameObject bulletPrefab;

    private void Start()
    {
        photonListBullets = FindObjectsOfType<GameObject>().ToList();
    }

    //gets first bullet that is inactive
    public GameObject GetInactivePooledObject()
    {
        foreach(GameObject bullet in photonListBullets)
        {
            //if bullet is not active (not used), return that bullet object to use
            if (!bullet.activeSelf)
            {
                return bullet;
            }
        }
        //if no bullets available is returned, create a new bullet object to use
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.parent = gameObject.transform;
        photonListBullets.Add(newBullet);
        
        return newBullet;
    }
}
