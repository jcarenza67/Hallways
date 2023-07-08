using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UHFPS.Scriptable;

namespace UHFPS.Runtime
{
    public struct SavedGameInfo
    {
        public string Id;
        public string Scene;
        public DateTime TimeSaved;
        public TimeSpan TimePlayed;
        public string Foldername;
        public string Dataname;
        public Texture2D Thumbnail;
    }

    public class SaveGameReader
    {
        private readonly SerializationAsset serializationAsset;

        public SaveGameReader(SerializationAsset serializationAsset)
        {
            this.serializationAsset = serializationAsset;
        }

        /// <summary>
        /// Get information about all saved games.
        /// </summary>
        public async Task<SavedGameInfo[]> ReadAllSaves()
        {
            string savesPath = serializationAsset.GetSavesPath();
            string saveFolderPrefix = serializationAsset.SaveFolderPrefix;
            string saveInfoPrefix = serializationAsset.SaveInfoName;
            string saveExtension = serializationAsset.SaveExtension;
            IList<SavedGameInfo> saveInfos = new List<SavedGameInfo>();

            if (Directory.Exists(savesPath))
            {
                string[] directories = Directory.GetDirectories(savesPath, $"{saveFolderPrefix}*");
                foreach (var directoryPath in directories)
                {
                    string json;
                    string saveInfoPath = Path.Combine(directoryPath, saveInfoPrefix + saveExtension);

                    if (!File.Exists(saveInfoPath)) 
                        continue;

                    if (serializationAsset.EncryptSaves)
                    {
                        byte[] data = await File.ReadAllBytesAsync(saveInfoPath);
                        json = await SerializableEncryptor.Decrypt(serializationAsset.EncryptionKey, data);
                    }
                    else
                    {
                        json = await File.ReadAllTextAsync(saveInfoPath);
                    }

                    JObject saveInfo = JObject.Parse(json);
                    string thumbnailName = (string)saveInfo["thumbnail"];
                    Texture2D thumbnail = null;

                    if (!string.IsNullOrEmpty(thumbnailName))
                    {
                        string thumbnailPath = Path.Combine(directoryPath, thumbnailName);
                        if (File.Exists(thumbnailPath))
                        {
                            byte[] thumbnailBytes = await File.ReadAllBytesAsync(thumbnailPath);
                            thumbnail = new Texture2D(2, 2);
                            thumbnail.LoadImage(thumbnailBytes);
                        }
                    }

                    saveInfos.Add(new SavedGameInfo()
                    {
                        Id = (string)saveInfo["id"],
                        Scene = (string)saveInfo["scene"],
                        TimeSaved = saveInfo["dateTime"].ToObject<DateTime>(),
                        TimePlayed = TimeSpan.FromSeconds((float)saveInfo["timePlayed"]),
                        Foldername = Path.GetFileName(directoryPath),
                        Dataname = (string)saveInfo["data"],
                        Thumbnail = thumbnail
                    }); ;
                }

                return saveInfos.OrderByDescending(x => x.TimeSaved).ToArray();
            }

            return new SavedGameInfo[0];
        }

        /// <summary>
        /// Get info about the saved game of the specified save.
        /// </summary>
        public async Task<SavedGameInfo> ReadSave(string folderName)
        {
            string savesPath = serializationAsset.GetSavesPath();
            string saveInfoPrefix = serializationAsset.SaveInfoName;
            string saveExtension = serializationAsset.SaveExtension;
            string saveFolderPath = Path.Combine(savesPath, folderName);

            if (Directory.Exists(savesPath) && Directory.Exists(saveFolderPath))
            {
                string saveInfoPath = Path.Combine(saveFolderPath, saveInfoPrefix + saveExtension);
                if (File.Exists(saveInfoPath))
                {
                    string json;

                    if (serializationAsset.EncryptSaves)
                    {
                        byte[] data = await File.ReadAllBytesAsync(saveInfoPath);
                        json = await SerializableEncryptor.Decrypt(serializationAsset.EncryptionKey, data);
                    }
                    else
                    {
                        json = await File.ReadAllTextAsync(saveInfoPath);
                    }

                    JObject saveInfo = JObject.Parse(json);
                    string thumbnailName = (string)saveInfo["thumbnail"];
                    Texture2D thumbnail = null;

                    if (!string.IsNullOrEmpty(thumbnailName))
                    {
                        string thumbnailPath = Path.Combine(saveFolderPath, thumbnailName);
                        if (File.Exists(thumbnailPath))
                        {
                            byte[] thumbnailBytes = await File.ReadAllBytesAsync(thumbnailPath);
                            thumbnail = new Texture2D(2, 2);
                            thumbnail.LoadImage(thumbnailBytes);
                        }
                    }

                    return new SavedGameInfo()
                    {
                        Id = (string)saveInfo["id"],
                        Scene = (string)saveInfo["scene"],
                        TimeSaved = saveInfo["dateTime"].ToObject<DateTime>(),
                        TimePlayed = TimeSpan.FromSeconds((float)saveInfo["timePlayed"]),
                        Dataname = (string)saveInfo["data"],
                        Thumbnail = thumbnail
                    };
                }
            }

            return new SavedGameInfo();
        }

        /// <summary>
        /// Remove all saved games.
        /// </summary>
        public async Task RemoveAllSaves()
        {
            string savesPath = serializationAsset.GetSavesPath();
            string saveFolderPrefix = serializationAsset.SaveFolderPrefix;

            if (Directory.Exists(savesPath))
            {
                string[] directories = Directory.GetDirectories(savesPath, $"{saveFolderPrefix}*");
                if (directories.Length > 0)
                {
                    Task[] deleteTasks = new Task[directories.Length];
                    for (int i = 0; i < directories.Length; i++)
                    {
                        string directoryPath = directories[i];
                        deleteTasks[i] = Task.Run(() => Directory.Delete(directoryPath, true));
                    }
                    await Task.WhenAll(deleteTasks);
                }
            }
        }
    }
}