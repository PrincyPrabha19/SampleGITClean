using System.ComponentModel;
using System.Threading;
using System.Windows;
using Dominator.Domain.Classes.Factories;
using Dominator.Domain.Enums;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;
using DesignerProperties = Dominator.UI.Classes.Helpers.DesignerProperties;

namespace Dominator.UI.ViewModels
{
    public class MetadataDownloadViewModel : ViewModelBase
    {
        private readonly ILogger logger = LoggerFactory.LoggerInstance;

        private IViewWithDataContextAndVisibility view;
        public IViewWithDataContextAndVisibility View
        {
            get { return view; }
            set
            {
                if (view == value) return;

                view = value;
                if (!DesignerProperties.IsInDesignMode)
                    view.DataContext = this;
            }
        }

        private string dialogText;
        public string DialogText
        {
            get { return dialogText; }
            set { SetProperty(ref dialogText, value, "DialogText"); }
        }

        public MetadataDownloadViewModel()
        {
            initializeCommands();            
        }

        public override void Initialize()
        {
            DialogText = Properties.Resources.MetadataDownloadInProgress;
        }

        public override void Load()
        {
            var metadataSystemProvider = OCMetadataSystemProviderFactory.NewOCMetadataSystemProvider();
            metadataSystemProvider.MetadataDownloadCompleted += metadataSystemProvider_MetadataDownloadCompleted;
            metadataSystemProvider.RetrieveMetadataAsync();
        }

        public bool ShowView()
        {
            var result = (View as Window)?.ShowDialog();
            return result.HasValue && result.Value;
        }

        private void initializeCommands()
        {
        }

        private void metadataSystemProvider_MetadataDownloadCompleted(OCMetadataStatus status)
        {
            if (status == OCMetadataStatus.MetadataDownloadSuccess)
            {
                ((Window) View).DialogResult = true;
                return;
            }

            logger?.WriteError($"MetadataDownloadViewModel.MetadataDownloadCompleted ErrorCode: {OCMetadataStatus.MetadataServiceUnreachable}");

            DialogText = status == OCMetadataStatus.MetadataServiceUnreachable ? 
                Properties.Resources.MetadataServiceUnreachable : Properties.Resources.MetadataDownloadFailed;

            var waitCloseWorker_RunWorkerCompleted = new BackgroundWorker();
            waitCloseWorker_RunWorkerCompleted.DoWork += delegate { Thread.Sleep(3000); };
            waitCloseWorker_RunWorkerCompleted.RunWorkerCompleted += delegate { ((Window)View).DialogResult = true; };
            waitCloseWorker_RunWorkerCompleted.RunWorkerAsync();
        }        
    }
}
