using System.Text;
using BusinessLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiMoneyMate.Resources.Languages;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Core;
using System.Threading;

namespace MauiMoneyMate.ViewModels
{
    public partial class FileViewModel : ObservableObject
    {

        public enum FileDialogType
        {
            Save,
            Open
        }

        [ObservableProperty] private string _filePageTitle;

        [ObservableProperty] private string _openFileText;

        [ObservableProperty] private string _saveFileText;

        [ObservableProperty] private string _saveFileAsText;

        [ObservableProperty] private int _openFileFontSize;

        [ObservableProperty] private int _saveFileFontSize;

        [ObservableProperty] private int _saveFileAsFontSize;

        private readonly FinancialOverview _financialOverview;
        private const string FileFilterForXmlFiles = "XML files (.xml)|*.xml";
        
        public FileViewModel(ref FinancialOverview financialOverview)
        {
            _financialOverview = financialOverview;
            LoadResources();
        }

        [RelayCommand]
        async Task OpenFile(CancellationToken cancellationToken)
        {
            var pickOptions = PickOptions.Default;
            try
            {
                var folderPickerResult = await FilePicker.PickAsync(pickOptions);
                if (folderPickerResult == null)
                {
                    Toast.Make("File could not be opened");
                    return;
                }

                _financialOverview.LoadData(folderPickerResult.FullPath);
            }
            catch (Exception ex)
            {
                await Toast.Make($"File could not be opened, {ex.Message}").Show(cancellationToken);
            }

        }
        [RelayCommand]
        private async Task SaveFile(CancellationToken cancellationToken)
        {
            _financialOverview.SaveData();
        }

        [RelayCommand]
        private async Task SaveFileAs(CancellationToken cancellationToken)
        {
            using var stream = new MemoryStream(Encoding.Default.GetBytes("Hello from the Community Toolkit!"));
            try
            {
                var fileLocationResult = await FileSaver.SaveAsync(_financialOverview.DefaultDirectory, _financialOverview.DefaultFilename, stream, cancellationToken);
                fileLocationResult.EnsureSuccess();
                _financialOverview.SaveData(fileLocationResult.FilePath);
                await Toast.Make($"File is saved: {fileLocationResult.FilePath}").Show(cancellationToken);
            }
            catch (Exception ex)
            {
                await Toast.Make($"File is not saved, {ex.Message}").Show(cancellationToken);
            }
        }

        private void LoadResources()
        {
            FilePageTitle = TextResource.FilePageTitle ?? string.Empty;
            OpenFileText = TextResource.OpenFile ?? string.Empty;
            SaveFileText = TextResource.SaveFile ?? string.Empty;
            SaveFileAsText = TextResource.SaveFileAs ?? string.Empty;
            OpenFileFontSize = Convert.ToInt32(TextResource.OpenFileFontSize);
            OpenFileFontSize = 0 == OpenFileFontSize ? 17 : OpenFileFontSize;
            SaveFileFontSize = Convert.ToInt32(TextResource.SaveFileFontSize);
            SaveFileFontSize = 0 == SaveFileFontSize ? 17 : SaveFileFontSize;
            SaveFileAsFontSize = Convert.ToInt32(TextResource.SaveFileAsFontSize);
            SaveFileAsFontSize = 0 == SaveFileAsFontSize ? 17 : SaveFileAsFontSize;
        }
    }
}
