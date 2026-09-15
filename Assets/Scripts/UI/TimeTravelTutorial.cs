using UnityEngine;
using System.Collections;

public class TimeTravelTutorial : TutorialTrigger
{
    [Header("Desse tutorial")]
    public float pushForce = 5f;
    public CharacterController player;

    protected override void OnTutorialCompleted()
    {
        StartCoroutine(PushPlayer());
    }

    private IEnumerator PushPlayer()
    {
        tip.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.5f);
        Vector3 pushDirection = player.transform.forward;
        player.Move(pushDirection * pushForce);
        Destroy(tip.gameObject);
        Destroy(gameObject);
    }
}