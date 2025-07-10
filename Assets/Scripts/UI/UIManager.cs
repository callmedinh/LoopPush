using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        Dictionary<string, UIBaseView> views = new Dictionary<string, UIBaseView>();
        [SerializeField] private UIBaseView loadingView;
        [SerializeField] private UIBaseView titleView;
        [SerializeField] private UIBaseView levelSelectView;
        [SerializeField] private UIBaseView gameplayView;
        private UIBaseView currentView;

        protected override void Awake()
        {
            base.Awake();
            //add view into hashmap
            views.Add(ViewConstants.LoadingView, loadingView);
            views.Add(ViewConstants.TitleView, titleView);
            views.Add(ViewConstants.LevelView, levelSelectView);
            views.Add(ViewConstants.GameplayView, gameplayView);
            
            foreach (var view in views)
            {
                view.Value.Hide();
            }
            currentView = loadingView;
            currentView.Show();
        }

        public void ShowView(string viewName)
        {
            if (views.TryGetValue(viewName, out var view))
            {
                if (currentView != null) currentView.Hide();
                currentView = view;
                currentView.Show();
            }
        }

        public void ShowPopup(string viewName)
        {
            if (views.TryGetValue(viewName, out var view))
            {
                view.Show();
            }
        }

        public void HidePopup(string viewName)
        {
            if (views.TryGetValue(viewName, out var view))
            {
                view.Hide();
            }
        }
    }
}