using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlatformScript : MonoBehaviour
{
    public float stayTime;
    private float timer;
    private Text timerText;
    private Animator animator;
    private bool isDestroyed = false;

    public void Setup(float stayTime, Text platformTimerText)
    {
        this.stayTime = stayTime;
        this.timerText = platformTimerText;
        timer = stayTime;

        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timerText != null && !isDestroyed)
            {
                timerText.text = timer.ToString("F2");
            }

            yield return null;
        }

        if (!isDestroyed)
        {
            StartCoroutine(ScaleOutAndDestroy());
        }
    }

    public IEnumerator ScaleOutAndDestroy()
    {
        if (animator != null)
        {
            animator.Play("PlatformDisappear");
        }

        yield return new WaitForSeconds(1f);

        if (this != null && gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
