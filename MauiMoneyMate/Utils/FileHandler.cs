﻿using CommunityToolkit.Maui.Storage;

namespace MauiMoneyMate.Utils;

public class FileHandler
{
    #region public static Methods

    public static string SaveFileDialog(string defaultDirectory, string defaultFilename)
    {
        return Task.Run(() => SaveFileDialogTask(defaultDirectory, defaultFilename),
            new CancellationToken()).Result;
    }

    public static string OpenFileDialog()
    {
        return Task.Run(OpenFileDialogTask, new CancellationToken()).Result;
    }

    public static string ReadTextFile(string targetFile)
    {
        return Task.Run(() => ReadTextFileTask(targetFile), new CancellationToken()).Result;
    }

    public static bool WriteTextToFile(string text, string targetFile)
    {
        return Task.Run(() => WriteTextToFileTask(text, targetFile), new CancellationToken()).Result;
    }

    #endregion

    #region private Methods

    private static async Task<string> ReadTextFileTask(string targetFile)
    {
        if (!Path.Exists(targetFile)) return null;
        await using var inputStream = File.OpenRead(targetFile);
        using var reader = new StreamReader(inputStream);
        return await reader.ReadToEndAsync();
    }

    private static async Task<bool> WriteTextToFileTask(string text, string targetFile)
    {
        if (null == targetFile) return false;
        if (!Directory.Exists(targetFile)) Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
        var outputStream = File.OpenWrite(targetFile);
        await using var streamWriter = new StreamWriter(outputStream);
        await streamWriter.WriteLineAsync(text);
        return true;
    }

    private static async Task<string> SaveFileDialogTask(string defaultDirectory, string defaultFilename)
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

    private static async Task<string> OpenFileDialogTask()
    {
        try
        {
            var folderPickerResult = await FilePicker.PickAsync(PickOptions.Default);
            return folderPickerResult?.FullPath;
        }
        catch (Exception)
        {
            return null;
        }
    }

    #endregion
}