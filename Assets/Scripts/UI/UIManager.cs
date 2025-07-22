using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private UIBaseView loadingView;
        [SerializeField] private UIBaseView titleView;
        [SerializeField] private UIBaseView levelSelectView;
        [SerializeField] private UIBaseView gameplayView;
        private readonly Dictionary<ScreenType, UIBaseView> views = new();
        private UIBaseView currentView;

        protected override void Awake()
        {
            base.Awake();
            //add view into hashmap
            views.Add(ScreenType.Loading, loadingView);
            views.Add(ScreenType.Title, titleView);
            views.Add(ScreenType.LevelSelect, levelSelectView);
            views.Add(ScreenType.Gameplay, gameplayView);

            foreach (var view in views) view.Value.Hide();
            currentView = loadingView;
            currentView.Show();
        }

        public void ShowView(ScreenType viewType)
        {
            if (views.TryGetValue(viewType, out var view))
            {
                if (currentView != null) currentView.Hide();
                currentView = view;
                currentView.Show();
            }
        }

        public void ShowPopup(ScreenType viewType)
        {
            if (views.TryGetValue(viewType, out var view)) view.Show();
        }

        public void HidePopup(ScreenType viewType)
        {
            if (views.TryGetValue(viewType, out var view)) view.Hide();
        }
    }

    public enum ScreenType
    {
        Loading,
        Title,
        LevelSelect,
        Gameplay
    }
}