using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolBullet : MonoBehaviour
{
    PhotonView photonView;

    [HideInInspector] public List<Transform> photonListBullets = new List<Transform>();
    [SerializeField] private GameObject bulletPrefab;
    int maxBulletID;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        foreach (Transform child in transform)
        {
            photonListBullets.Add(child);
        }
        maxBulletID = photonListBullets.Count - 1;
        Debug.Log(maxBulletID + " maxBulletID");
    }

    //gets first bullet that is inactive
    public string GetUnusedBullet()
    {
        foreach(Transform bullet in photonListBullets)
        {
            //if bullet is not active (not used), return that bullet object to use
            if (!bullet.gameObject.activeSelf)
            {
                return bullet.name;
            }
        }
        //if no bullets available is returned, create a new bullet object to use
        GameObject newBullet = PhotonNetwork.Instantiate(bulletPrefab.name,Vector3.zero,Quaternion.identity);
        photonView.RPC("SetParent", RpcTarget.AllBuffered, newBullet.name);
        
        return newBullet.name;
    }
    [PunRPC]
    void SetParent(string bulletName)
    {
        GameObject bullet = GameObject.Find(bulletName);
        bullet.transform.parent = gameObject.transform;
        maxBulletID += 1;
        bullet.name = bulletPrefab.name + " " + maxBulletID;
        photonListBullets.Add(bullet.transform);
    }
}
