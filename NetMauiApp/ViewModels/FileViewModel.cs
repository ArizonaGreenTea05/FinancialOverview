using BusinessLogic;
using CommunityToolkit.Mvvm.ComponentModel;
using NetMauiApp.Resources.Languages;
using System.Resources;

namespace NetMauiApp.ViewModels
{
    public partial class FileViewModel : ObservableObject
    {
        [ObservableProperty] private string _filePageTitle;

        private readonly ResourceManager _resources = TextResource.ResourceManager;

        public FileViewModel()
        {
            LoadResources();
        }

        private void LoadResources()
        {
            FilePageTitle = _resources.GetString("FilePageTitle") ?? string.Empty;
        }
    }
}
