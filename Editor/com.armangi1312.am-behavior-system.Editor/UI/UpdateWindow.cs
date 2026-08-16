using AMBehaviorSystem.Editor.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AMBehaviorSystem.Editor.UI
{
    [InitializeOnLoad]
    public class UpdateWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset visualTreeAsset = default;

        private const string SessionKey = "AMBS_ShowedUpdateWindow";

        private const float WindowWidth = 600f;
        private const float WindowHeight = 350f;

        static UpdateWindow()
        {
            if (SessionState.GetBool(SessionKey, false) || !AMBSSettings.instance.UpdateCheck) return;
            SessionState.SetBool(SessionKey, true);

            EditorApplication.delayCall += DelayCall;
        }

        private static void DelayCall()
        {
            EditorApplication.delayCall -= DelayCall;

            ShowWindowByRequest();
        }

        [MenuItem("Window/AM Behavior System/Update")]
        public static void ShowWindowByMenu()
        {
            UpdateWindow window = GetWindow<UpdateWindow>();
            window.titleContent = new GUIContent("Update");
        }

        public static void ShowWindowByRequest()
        {
            UpdateWindow window = CreateInstance<UpdateWindow>();

            window.minSize = new Vector2(WindowWidth, WindowHeight);
            window.titleContent = new GUIContent("Update");

            Rect mainWindowRect = EditorGUIUtility.GetMainWindowPosition();

            float centerX = mainWindowRect.x + (mainWindowRect.width - WindowWidth) * 0.5f;
            float centerY = mainWindowRect.y + (mainWindowRect.height - WindowHeight) * 0.5f;

            window.position = new Rect(centerX, centerY, WindowWidth, WindowHeight);

            window.ShowModalUtility();
        }

        public void CreateGUI()
        {
            if (visualTreeAsset == null)
            {
                Debug.LogError("[UpdateWindow] VisualTreeAsset is not assigned.");
                return;
            }

            VisualElement root = rootVisualElement;

            VisualElement tree = visualTreeAsset.Instantiate();
            root.Add(tree);

            Button cancelButton = root.Q<Button>("CancelButton");
            Button updateButton = root.Q<Button>("UpdateButton");

            if (cancelButton == null || updateButton == null)
            {
                Debug.LogError("[UpdateWindow] Required buttons not found in UXML.");
                return;
            }

            Label currentVersionLabel = root.Q<Label>("CurrentVersionLabel");
            Label latestVersionLabel = root.Q<Label>("LatestVersionLabel");

            Label changelogLabel = root.Q<Label>("ChangelogLabel");

            if (currentVersionLabel == null || latestVersionLabel == null || changelogLabel == null)
            {
                Debug.LogError("[UpdateWindow] Required buttons not found in UXML.");
                return;
            }

            currentVersionLabel.text = UpdateUtilities.CurrentVersion?.ToString() ?? "Unknown";
            latestVersionLabel.text = UpdateUtilities.LatestVersion?.ToString() ?? "Unknown";

            changelogLabel.text = UpdateUtilities.ChangeLog ?? "Cannot load changelog";

            cancelButton.clicked += OnCancelButtonClicked;
            updateButton.clicked += OnUpdateButtonClicked;
        }

        private void OnCancelButtonClicked()
        {
            Close();
        }

        private void OnUpdateButtonClicked()
        {
            UpdateUtilities.UpdatePackage();

            Close();
        }
    }
}