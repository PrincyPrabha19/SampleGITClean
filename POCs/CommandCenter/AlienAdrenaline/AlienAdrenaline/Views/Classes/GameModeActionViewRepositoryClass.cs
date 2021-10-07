using System;
using System.Collections.Generic;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Helpers;

namespace AlienLabs.AlienAdrenaline.App.Views.Classes
{
    public class GameModeActionViewRepositoryClass : GameModeActionViewRepository
    {
        private GameModeActionViewFactory viewFactory;
        public GameModeActionViewFactory ViewFactory { set { viewFactory = value; } }

        private readonly IDictionary<int, GameModeActionView> cache = new Dictionary<int, GameModeActionView>();

        public GameModeActionView GetByType(GameModeActionViewType type, Guid id)
        {
            return viewFactory.NewView(type);

            if (!EnumHelper.GetAttributeValue<AllowCachingAttributeClass, bool>(type))
                return viewFactory.NewView(type);

            int hashCode = type.ToString().GetHashCode();
            if (EnumHelper.GetAttributeValue<AllowMultipleAttributeClass, bool>(type))
                hashCode += id.GetHashCode();
            return cache.ContainsKey(hashCode) ? cache[hashCode] : newView(type, hashCode);
        }

        public IList<GameModeActionContentView> GetAllContentViews()
        {
            var activeViews = new List<GameModeActionContentView>();

            foreach (var view in cache.Values) 
                if (view is GameModeActionContentView)
                    activeViews.Add(view as GameModeActionContentView);

            return activeViews;
        }

        public void ClearAllContentViews()
        {
            cache.Clear();
        }

        private GameModeActionView newView(GameModeActionViewType type, int hashCode)
        {
            var view = viewFactory.NewView(type);
            cache.Add(hashCode, view);
            return view;
        }
    }
}