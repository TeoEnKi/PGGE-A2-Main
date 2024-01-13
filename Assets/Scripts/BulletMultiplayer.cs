using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMultiplayer : MonoBehaviourPun
{
    void Start()
    {
        // Destroy the bullet after 10 seconds if it does not hit any object.
        StartCoroutine(Coroutine_Destroy(10.0f));
    }

    void Update()
    {
    }

    IEnumerator Coroutine_Destroy(float duration)
    {
        yield return new WaitForSeconds(duration);
        this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj != null)
        {
            obj.TakeDamage();
        }

        StartCoroutine(Coroutine_Destroy(0.1f));
    }
    [PunRPC]
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
