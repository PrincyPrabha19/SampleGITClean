using System;

namespace AlienLabs.AlienAdrenaline.App.Views
{
	public interface ApplicationButtonsView
	{
		bool IsSnapshotSelected { get; set; }
        bool AreMultipleSnapshotsSelected { get; set; }
        bool IsProcessDataSelected { get; set; }
        bool AreMultipleProcessDataSelected { get; set; }

		event Action SaveCurrentGMProfile;
		event Action SaveAsCurrentGMProfile;
		event Action CancelCurrentGMProfile;
		event Action DeleteCurrentGMProfile;
		event Action DeleteCurrentSnapshot;
		event Action ViewCurrentSnapshot;
		event Action SaveAsCurrentSnapshot;
		event Action RecordCurrentSnapshot;
		event Action StartStopRecordingSnapshot;
		event Action DeleteSnapshotBeingPlayed;
	    event Action ViewProcessData;

		void HideApplicationButtons();
		void ShowGMProfilesButtons();
		void ShowSnapshotListButtons();
		void ShowSnapshotRecordingButtons();
		void ShowPerformanceSnapshotView();
		void SetPlayingVisualState(bool playing);
		void SetIsDeleteSnapshotEnabledStatus(bool enabled);
		//void SetIsPlaySnapshotEnabledStatus(bool enabled);

		void SetRecordingVisualState(bool recording);
	}
}