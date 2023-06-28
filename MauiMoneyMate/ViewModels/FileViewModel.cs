using BusinessLogic;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiMoneyMate.Resources.Languages;
using CommunityToolkit.Mvvm.Input;

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
        private void OpenFile()
        {
            var filepath = GetFilepathFromUser(_financialOverview.DefaultDirectory, _financialOverview.DefaultFilename, ".xml",
                FileFilterForXmlFiles, FileDialogType.Open);
            if (null != filepath) _financialOverview.LoadData(filepath);
        }

        [RelayCommand]
        private void SaveFile()
        {
            _financialOverview.SaveData();
        }

        [RelayCommand]
        private void SaveFileAs()
        {
            var filepath = GetFilepathFromUser(_financialOverview.DefaultDirectory, _financialOverview.DefaultFilename, ".xml",
                FileFilterForXmlFiles, FileDialogType.Save);
            if (null != filepath) _financialOverview.SaveData(filepath);
        }

        private static string GetFilepathFromUser(string defaultDirectory, string defaultFilename, string defaultExtension, string filter, FileDialogType type)
        {
            FileDialog dialog;
            if (type == FileDialogType.Save)
                dialog = new SaveFileDialog();
            else
                dialog = new OpenFileDialog();
            dialog.Title = $@"Select {defaultExtension}";
            dialog.InitialDirectory = Directory.Exists(defaultDirectory) ? defaultDirectory : Directory.GetCurrentDirectory();
            dialog.FileName = defaultFilename;
            dialog.DefaultExt = defaultExtension;
            dialog.Filter = filter;
            if (dialog.ShowDialog() == DialogResult.Cancel) return null;
            return dialog.FileName;
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
