using System;
using AlienLabs.AlienAdrenaline.App.Views;

namespace AlienLabs.AlienAdrenaline.App
{
	public interface ApplicationButtonsPresenter
	{
		ApplicationButtonsView View { get; set; }
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
	    event Action CompareSelectedSnapshots;
		event Action SaveAsCurrentSnapshot;
		event Action RecordNewSnapshot;

		event Action<string> StartRecordingSnapshot;
		event Action StopRecordingSnapshot;

		event Action DeleteSnapshotBeingPlayed;
	    event Action ViewProcessData;

		void HideApplicationButtons();
		void ShowGMProfilesButtons();
		void ShowSnapshotListButtons();
		void ShowSnapshotRecordingButtons();
		void ShowPerformanceSnapshotView(int recordingCount);
        void ShowPerformanceSnapshotViewForIncompatibleFile(int recordingCount);
	}
}