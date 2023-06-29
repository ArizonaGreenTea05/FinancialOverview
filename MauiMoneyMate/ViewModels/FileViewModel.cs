﻿using System.Text;
using BusinessLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiMoneyMate.Resources.Languages;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Storage;

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
        private readonly CommonVariables _commonVariables;

        public FileViewModel(FinancialOverview financialOverview, CommonVariables commonVariables)
        {
            _commonVariables = commonVariables;
            _financialOverview = financialOverview;
            LoadResources();
        }

        [RelayCommand]
        private async Task OpenFile(CancellationToken cancellationToken)
        {
            try
            {
                var folderPickerResult = await FilePicker.PickAsync(PickOptions.Default);
                if (folderPickerResult == null)
                {
                    Toast.Make("File could not be opened");
                    return;
                }

                _financialOverview.LoadData(folderPickerResult.FullPath);
                DataIsSaved = true;
            }
            catch (Exception ex)
            {
                await Toast.Make($"File could not be opened, {ex.Message}").Show(cancellationToken);
            }

        }
        [RelayCommand]
        private async Task SaveFile(CancellationToken cancellationToken)
        {
            DataIsSaved = _financialOverview.SaveData();
            if (DataIsSaved)
                await Toast.Make($"File has been saved: {_financialOverview.DefaultFilePath}").Show(cancellationToken);
            else
                await Toast.Make($"File could not be saved").Show(cancellationToken);
        }

        [RelayCommand]
        private async Task SaveFileAs(CancellationToken cancellationToken)
        {
            try
            {
                var fileLocationResult = await FileSaver.SaveAsync(_financialOverview.DefaultDirectory, _financialOverview.DefaultFilename, Stream.Null, cancellationToken);
                fileLocationResult.EnsureSuccess();
                _financialOverview.SaveData(fileLocationResult.FilePath);
                await Toast.Make($"File has been saved: {fileLocationResult.FilePath}").Show(cancellationToken);
                DataIsSaved = true;
            }
            catch (Exception ex)
            {
                await Toast.Make($"File could not be saved, {ex.Message}").Show(cancellationToken);
            }
        }

        public void OnAppearing()
        {
            DisplaySavingState();
        }

        private bool DataIsSaved
        {
            set
            {
                _commonVariables.DataIsSaved = value;
                DisplaySavingState();
            }
            get => _commonVariables.DataIsSaved;
        }

        private void DisplaySavingState()
        {
            FilePageTitle = FilePageTitle.TrimEnd('*');
            if (!DataIsSaved)
                FilePageTitle += '*';
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
