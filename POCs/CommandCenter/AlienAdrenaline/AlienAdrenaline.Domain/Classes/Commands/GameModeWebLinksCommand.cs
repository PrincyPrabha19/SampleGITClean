using System;
using System.Collections.ObjectModel;
using System.Linq;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Commands
{
    public class GameModeWebLinksCommand : EquatableCommand
    {
        public CommandType CommandType { get { return CommandType.WebLinksAction; } }
        public ProfileService ProfileService { get; set; }        
        public ObservableCollection<string> Urls { get; set; }
        public bool EnableTabbedBrowsing { get; set; }
        public Guid Guid { get; set; }

        public void Execute()
        {
            ProfileService.SetProfileActionInfoWebLinks(Urls, EnableTabbedBrowsing, Guid);
        }

        public bool IsRedundant
        {
            get
            {
                var profileActionInfo = ProfileService.GetProfileActionInfo(Guid) as ProfileActionInfoWebLinksClass;
                return profileActionInfo != null && profileActionInfo.Guid == Guid && profileActionInfo.EnableTabbedBrowsing == EnableTabbedBrowsing &&
                    areEqualCollection(profileActionInfo.Urls);
            }
        }

        private bool areEqualCollection(ObservableCollection<string> urls)
        {
            bool areEqual = true;

            if (urls.Count != Urls.Count)
                return false;

            foreach (var url in Urls)
            {
                areEqual = urls.ToList().Exists(u => String.Compare(u, url, true) == 0);

                if (!areEqual)
                    break;
            }

            return areEqual;
        }

        public virtual bool Equals(EquatableCommand equatableCommand)
        {
            if ((equatableCommand == null) || (GetType() != equatableCommand.GetType()))
                return false;

            if (ReferenceEquals(this, equatableCommand))
                return true;

            var command = equatableCommand as GameModeWebLinksCommand;
            if (command == null)
                return false;

            return CommandType == command.CommandType && Guid == command.Guid;
        }
    }
}