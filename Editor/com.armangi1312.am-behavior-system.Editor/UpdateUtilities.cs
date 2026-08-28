using AMBehaviorSystem.Editor.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace AMBehaviorSystem.Editor
{
    public static class UpdateUtilities
    {
        private const string SessionKey = "AMBS_CheckedUpdate";

        private const string PackageName = "com.armangi1312.am-behavior-system";
        private const string GitUrl = "https://github.com/Armangi1312/AMBehaviorSystem.git";

        private const string RemotePackageUrl = "https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/package.json";
        private const string LocalPackagePath = "Packages/com.armangi1312.am-behavior-system/package.json";

        private const string ChangeLogUrl = "https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/CHANGELOG.md";

        private static readonly HttpClient httpClient = new();

        private static RemoveRequest removeRequest;
        private static AddRequest addRequest;

        public static Version CurrentVersion { get; private set; }
        public static Version LatestVersion { get; private set; }

        public static IReadOnlyDictionary<Version, string> ChangeLogs { get; private set; }

        [Serializable]
        private struct Package
        {
            public string version;
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            if (SessionState.GetBool(SessionKey, false) || !AMBSSettings.instance.UpdateCheck) return;
            SessionState.SetBool(SessionKey, true);

            CacheVersions();
            CacheChangeLog();
        }

        private static async void CacheVersions()
        {
            try
            {
                string localJson = File.ReadAllText(LocalPackagePath);
                Package localInfo = JsonUtility.FromJson<Package>(localJson);

                string remoteJson = await httpClient.GetStringAsync(RemotePackageUrl);
                Package remoteInfo = JsonUtility.FromJson<Package>(remoteJson);

                if (!Version.TryParse(localInfo.version, out Version currentVersion) || !Version.TryParse(remoteInfo.version, out Version latestVersion)) return;

                CurrentVersion = currentVersion;
                LatestVersion = latestVersion;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[UpdateChecker] Failed to check version: {exception.GetType().Name} - {exception.Message}\n{exception.StackTrace}");
            }
        }

        private static async void CacheChangeLog()
        {
            try
            {
                string fullChangeLog = await httpClient.GetStringAsync(ChangeLogUrl);

                MatchCollection headerMatches = Regex.Matches(fullChangeLog, @"^#{1,2}\s*(\d+\.\d+\.\d+).*$", RegexOptions.Multiline);
                Dictionary<Version, string> sections = new();

                for(int i = 0; i < headerMatches.Count; i++)
                {
                    Version version = new(headerMatches[i].Groups[1].Value);
                    int startIndex = headerMatches[i].Index + headerMatches[i].Length;
                    int endIndex = i + 1 < headerMatches.Count ? headerMatches[i + 1].Index : fullChangeLog.Length;

                    string body = fullChangeLog[startIndex..endIndex].Trim('-', ' ', '\n', '\r');

                    sections[version] = Regex.Replace(body, @"^-\s*", "", RegexOptions.Multiline);
                }
                ChangeLogs = sections;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[UpdateChecker] Failed to check changelog: {exception.Message}");
            }
        }

        public static bool IsUpdateAvailable()
        {
            if (CurrentVersion == null || LatestVersion == null) return false;

            return CurrentVersion < LatestVersion;
        }

        public static void UpdatePackage()
        {
            removeRequest = Client.Remove(PackageName);
            EditorApplication.update += WaitForRemove;
        }

        private static void WaitForRemove()
        {
            if (!removeRequest.IsCompleted) return;

            EditorApplication.update -= WaitForRemove;

            if (removeRequest.Status == StatusCode.Success)
            {
                Debug.Log("[UpdateChecker] Package removed. Installing latest version...");
                addRequest = Client.Add(GitUrl);
                EditorApplication.update += WaitForAdd;
            }
            else
            {
                Debug.LogError($"[UpdateChecker] Failed to remove package: {removeRequest.Error.message}");
            }
        }

        private static void WaitForAdd()
        {
            if (!addRequest.IsCompleted) return;

            EditorApplication.update -= WaitForAdd;

            if (addRequest.Status == StatusCode.Success)
            {
                Debug.Log("[UpdateChecker] Update complete!");
                EditorUtility.DisplayDialog(
                    "Update Complete",
                    "The package has been updated to the latest version.",
                    "OK"
                );
            }
            else
            {
                Debug.LogError($"[UpdateChecker] Failed to install package: {addRequest.Error.message}");
            }
        }
    }
}