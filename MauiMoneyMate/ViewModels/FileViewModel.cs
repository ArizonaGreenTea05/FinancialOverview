using BusinessLogic;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Resources;
using MauiMoneyMate.Resources.Languages;

namespace MauiMoneyMate.ViewModels
{
    public partial class FileViewModel : ObservableObject
    {
        [ObservableProperty] private string _filePageTitle;

        [ObservableProperty] private string _openFileText;

        [ObservableProperty] private string _saveFileText;

        [ObservableProperty] private string _saveFileAsText;

        [ObservableProperty] private int _openFileFontSize;

        [ObservableProperty] private int _saveFileFontSize;

        [ObservableProperty] private int _saveFileAsFontSize;

        public FileViewModel()
        {
            LoadResources();
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
