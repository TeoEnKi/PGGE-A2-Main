using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMultiplayer : MonoBehaviourPun
{
    //disable bullet after a few seconds
    void OnEnable()
    {
        // Destroy the bullet after 10 seconds if it does not hit any object.
        StartCoroutine(Coroutine_Disable(10.0f));
    }

    IEnumerator Coroutine_Disable(float duration)
    {
        yield return new WaitForSeconds(duration);
        this.GetComponent<PhotonView>().RPC("DisableBullet", RpcTarget.AllBuffered);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj != null)
        {
            obj.TakeDamage();
        }

        StartCoroutine(Coroutine_Disable(0.1f));
    }

    [PunRPC]
    public void DisableBullet()
    {
        gameObject.SetActive(false);
    }
}
