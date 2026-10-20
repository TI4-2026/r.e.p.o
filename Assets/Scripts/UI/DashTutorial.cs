using UnityEngine;

public class DashTutorial : TutorialTrigger
{
    protected override void OnTutorialCompleted()
    {
        Destroy(tip.gameObject);
    }
}
