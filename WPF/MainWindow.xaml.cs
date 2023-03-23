using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace async_io
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _numberOfFiles;

        public MainWindow()
        {
            InitializeComponent();

            _numberOfFiles = 10;
            FilesDownloadStatuses = new FilesDownloadViewModel();
            for (int i = 0; i < _numberOfFiles; i++)
            {
                FilesDownloadStatuses.Add(new FileProgressViewModel());
            }
            FilesDownloadView.DataContext = FilesDownloadStatuses;
        }

        public FilesDownloadViewModel FilesDownloadStatuses { get; }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            // Start the download...
            if (FilesDownloadStatuses.IsIdle)
            {
                FilesDownloadStatuses.IsIdle = false;

                var fileProcessor = new FileProcessor();
                await fileProcessor.ProcessFiles(_numberOfFiles, new Progress<AggregatedProgress>(UpdateProgressUi));

                FilesDownloadStatuses.IsIdle = true;
            }
        }

        private void UpdateProgressUi(AggregatedProgress progress)
        {
            var specProgress = progress.SpecificProgress;
            FilesDownloadStatuses[specProgress.WorkId].Progress = specProgress.PctComplete;
            FilesDownloadStatuses[specProgress.WorkId].ThreadId = specProgress.ThreadId;
            FilesDownloadStatuses.OverallProgress = FilesDownloadStatuses.Sum(p => p.Progress) / _numberOfFiles;
        }
    }
}
