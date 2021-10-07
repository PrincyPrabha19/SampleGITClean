
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles
{
	public interface ProfileActionCreator
	{
        ProfileAction New(string name, GameModeActionType type);
	    ProfileAction New(string name, GameModeActionType type, bool initialize);
	}
}
