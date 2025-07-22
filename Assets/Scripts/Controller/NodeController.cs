using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class NodeController
    {
        private readonly float animTime = 0.1f;
        private readonly Image bridgeImg;
        private readonly Image circleImg;
        private bool isFilling;
        private float timer;

        public NodeController(Image bridge, Image circle)
        {
            bridgeImg = bridge;
            circleImg = circle;
        }

        public void StartFill()
        {
            isFilling = true;
            timer = 0;
        }

        public void Update()
        {
            if (!isFilling) return;

            timer += Time.deltaTime;
            var t = Mathf.Clamp01(timer / animTime);
            bridgeImg.fillAmount = t;

            if (t >= 1f)
            {
                circleImg.fillAmount = 1f;
                isFilling = false;
            }
        }

        public void ClearFill()
        {
            bridgeImg.fillAmount = circleImg.fillAmount = 0f;
            isFilling = false;
        }
    }
}