using UnityEngine;
using System.Collections;

public class PlatformParent : MonoBehaviour
{
    public Transform platform;
    private Transform player;
    private CharacterController cc;

    void OnTriggerEnter(Collider other)
    {
        if (player) return;

        if (other.CompareTag("Player"))
        {
            player = other.transform;
            player.parent = platform.transform;
            cc = player.GetComponent<CharacterController>();
            StartCoroutine(CheckStatus());
        }
    }

    IEnumerator CheckStatus()
    {
		yield return new WaitForSeconds(0.3f);	
        while (player)
        {
            if (!cc.isGrounded)
            {
                player.parent = null;
                player = null;
                cc = null;
            }
            yield return null;
        }
    }
}