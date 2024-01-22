using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMultiplayer : MonoBehaviourPun
{
    //disable bullet after a few seconds of it being active 
    void OnEnable()
    {
        // Destroy the bullet after 10 seconds if it does not hit any object.
        StartCoroutine(Coroutine_Disable(10.0f));
    }

    //delay before bullet disables
    IEnumerator Coroutine_Disable(float duration)
    {
        yield return new WaitForSeconds(duration);
        this.GetComponent<PhotonView>().RPC("DisableBullet", RpcTarget.AllBuffered);
    }

    //set bullet object to inactive 0.1 seconds after the bullet hits and object
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj != null)
        {
            obj.TakeDamage();
        }

        StartCoroutine(Coroutine_Disable(0.1f));
    }

    //set bullet object to inactive
    [PunRPC]
    public void DisableBullet()
    {
        gameObject.SetActive(false);
    }
}
