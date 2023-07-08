using System.IO;
using UnityEngine;
using UnityEditor;
using UHFPS.Scriptable;
using UHFPS.Runtime;

namespace UHFPS.Editors
{
    public class ScriptableCreator : Editor
    {
        public const string ROOT_PATH = "UHFPS";
        public const string TOOLS_PATH = "Tools/" + ROOT_PATH;
        public const string TEMPLATES_PATH = "C# UHFPS Templates";

        public const string GAMEMANAGER_PATH = "Setup/GAMEMANAGER";
        public const string PLAYER_PATH = "Setup/HEROPLAYER";

        public static string TemplatesPath
        {
            get
            {
                string projectPath = FindAssetPath(ROOT_PATH);
                if (projectPath == "Assets")
                    projectPath = Path.Combine(projectPath, ROOT_PATH);

                return Path.Combine(projectPath, "ScriptTemplates");
            }
        }

        public static string ScriptablesPath
        {
            get
            {
                string projectPath = FindAssetPath(ROOT_PATH);
                if (projectPath == "Assets")
                    projectPath = Path.Combine(projectPath, ROOT_PATH);

                return Path.Combine(projectPath, "Scriptables");
            }
        }

        public static string GameScriptablesPath
        {
            get
            {
                string scriptablesGamePath = Path.Combine(ScriptablesPath, "Game");

                if (!Directory.Exists(scriptablesGamePath))
                {
                    Directory.CreateDirectory(scriptablesGamePath);
                    AssetDatabase.Refresh();
                }

                return scriptablesGamePath;
            }
        }

        public static string FindAssetPath(string searchPattern)
        {
            string[] result = AssetDatabase.FindAssets(searchPattern);

            if (result.Length > 0)
                return AssetDatabase.GUIDToAssetPath(result[0]);

            return "Assets";
        }

        public static T CreateAssetFile<T>(string AssetName) where T : ScriptableObject
        {
            var asset = CreateInstance<T>();
            ProjectWindowUtil.CreateAsset(asset, Path.Combine(GameScriptablesPath, $"New {AssetName}.asset"));
            return asset;
        }

        [MenuItem("Assets/Create/" + TEMPLATES_PATH + "/PlayerState Script", false, 120)]
        static void CreatePlayerStateScript()
        {
            string templatePath = Path.Combine(TemplatesPath, "PlayerState_Template.txt").Replace("\\", "/");
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "New PlayerState.cs");
        }

        [MenuItem("Assets/Create/" + TEMPLATES_PATH + "/AIState Script", false, 120)]
        static void CreateAIStateScript()
        {
            string templatePath = Path.Combine(TemplatesPath, "AIState_Template.txt").Replace("\\", "/");
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "New AIState.cs");
        }

        [MenuItem(TOOLS_PATH + "/Setup Scene")]
        static void SetupScene()
        {
            // load GameManager and Player prefab from Resources
            GameObject gameManagerPrefab = Resources.Load<GameObject>(GAMEMANAGER_PATH);
            GameObject playerPrefab = Resources.Load<GameObject>(PLAYER_PATH);

            // add GameManager and Player to scene
            GameObject gameManager = PrefabUtility.InstantiatePrefab(gameManagerPrefab) as GameObject;
            GameObject player = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

            // get required components
            Camera mainCameraBrain = gameManager.GetComponentInChildren<Camera>();
            PlayerPresenceManager playerPresence = gameManager.GetComponent<PlayerPresenceManager>();
            PlayerManager playerManager = player.GetComponent<PlayerManager>();

            // assign missing references
            player.transform.position = new Vector3(0, 0, 0);
            playerManager.MainCamera = mainCameraBrain;
            playerPresence.Player = player;
        }
    }
}