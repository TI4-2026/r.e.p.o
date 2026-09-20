using UnityEngine;

public class DashTutorial : TutorialTrigger
{
    protected override void OnTutorialCompleted()
    {
        Time.timeScale = 1f;
        Destroy(tip.gameObject);
    }
}
