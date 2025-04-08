using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NodeController : MonoBehaviour
{
    public void DoFill()
    {
        StartCoroutine(FillAnimation());
    }
    private IEnumerator FillAnimation()
    {
        float timer = 0f;
        while (timer < this.animTime)
        {
            timer += Time.deltaTime;
            this.brigeImg.fillAmount = timer / this.animTime;
            yield return null;
        }
        this.circleImg.fillAmount = 1f;
        yield break;
    }
    public void ClearFill()
    {
        this.brigeImg.fillAmount = (this.circleImg.fillAmount = 0f);
    }
    [SerializeField]
    private Image brigeImg;

    [SerializeField]
    private Image circleImg;

    private readonly float animTime = 0.1f;
}
