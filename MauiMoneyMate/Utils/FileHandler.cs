using CommunityToolkit.Maui.Storage;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace MauiMoneyMate.Utils;

public class FileHandler
{
    #region public static Methods

    public static string SaveFileDialog(string defaultDirectory, string defaultFilename)
    {
        return Task.Run(() => SaveFileDialogAsync(defaultDirectory, defaultFilename),
            new CancellationToken()).Result;
    }

    public static string OpenFileDialog()
    {
        return OpenFileDialog(new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            { { DevicePlatform.WinUI, new[] { ".xml", ".finance" } } }));
    }

    public static string OpenFileDialog(FilePickerFileType fileTypes)
    {
        return Task.Run(() => OpenFileDialogAsync(fileTypes), new CancellationToken()).Result;
    }

    public static string ReadTextFile(string targetFile)
    {
        return Task.Run(() => ReadTextFileAsync(targetFile), new CancellationToken()).Result;
    }

    public static bool WriteTextToFile(List<string> contents, string targetFile)
    {
        return WriteTextToFile(string.Join('\n', contents), targetFile);
    }

    public static bool WriteTextToFile(string text, string targetFile)
    {
        if (null == targetFile) return false;
        if (!Directory.Exists(targetFile)) Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
        File.WriteAllText(targetFile, text);
        return true;
    }

    #endregion

    #region private Methods

    private static async Task<string> ReadTextFileAsync(string targetFile)
    {
        if (!Path.Exists(targetFile)) return null;
        await using var inputStream = File.OpenRead(targetFile);
        using var reader = new StreamReader(inputStream);
        return await reader.ReadToEndAsync();
    }

    private static async Task<string> SaveFileDialogAsync(string defaultDirectory, string defaultFilename)
    {
        try
        {
            var fileLocationResult =
                await FileSaver.SaveAsync(defaultDirectory, defaultFilename, Stream.Null, new CancellationToken());
            fileLocationResult.EnsureSuccess();
            return fileLocationResult?.FilePath;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static async Task<string> OpenFileDialogAsync(FilePickerFileType fileTypes)
    {
        try
        {
            var folderPickerResult = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = fileTypes
            });
            return folderPickerResult?.FullPath;
        }
        catch (Exception)
        {
            return null;
        }
    }

    #endregion
}