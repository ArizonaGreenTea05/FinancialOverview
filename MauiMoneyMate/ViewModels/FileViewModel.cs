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

        private readonly ResourceManager _resources = TextResource.ResourceManager;

        public FileViewModel()
        {
            LoadResources();
        }

        private void LoadResources()
        {
            FilePageTitle = _resources.GetString("FilePageTitle") ?? string.Empty;
            OpenFileText = _resources.GetString("OpenFile") ?? string.Empty;
            SaveFileText = _resources.GetString("SaveFile") ?? string.Empty;
            SaveFileAsText = _resources.GetString("SaveFileAs") ?? string.Empty;
            OpenFileFontSize = Convert.ToInt32(_resources.GetString("OpenFileFontSize"));
            OpenFileFontSize = 0 == OpenFileFontSize ? 17 : OpenFileFontSize;
            SaveFileFontSize = Convert.ToInt32(_resources.GetString("SaveFileFontSize"));
            SaveFileFontSize = 0 == SaveFileFontSize ? 17 : SaveFileFontSize;
            SaveFileAsFontSize = Convert.ToInt32(_resources.GetString("SaveFileAsFontSize"));
            SaveFileAsFontSize = 0 == SaveFileAsFontSize ? 17 : SaveFileAsFontSize;
        }
    }
}
