using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class NodeController
    {
        private Image bridgeImg;
        private Image circleImg;
        private float animTime = 0.1f;
        private float timer = 0f;
        private bool isFilling = false;

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
            float t = Mathf.Clamp01(timer / animTime);
            bridgeImg.fillAmount = t;

            if (t >= 1f)
            {
                circleImg.fillAmount = 1f;
                isFilling = false;
            }
        }
       
        public void ClearFill()
        {
            bridgeImg.fillAmount = (this.circleImg.fillAmount = 0f);
            isFilling = false;
        }

    }
}
