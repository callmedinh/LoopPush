using UnityEngine;

namespace UI
{
    public abstract class UIBaseView : MonoBehaviour
    {
        public virtual void Show()
        {
            this.gameObject.SetActive(true);
            OnShow();
        }

        public virtual void Hide()
        {
            OnHide();
            this.gameObject.SetActive(false);
        }

        public virtual void OnShow()
        {
            
        }

        public virtual void OnHide()
        {
            
        }
    }
}