using UnityEngine;
using UHFPS.Tools;

namespace UHFPS.Runtime
{
    public class LevelInteract : MonoBehaviour, IInteractStart
    {
        public enum LevelType { NextLevel, WorldState, PlayerData }

        public LevelType LevelLoadType = LevelType.NextLevel;
        public string NextLevelName;

        public bool CustomTransform;
        public Transform TargetTransform;
        public float LookUpDown;

        public void InteractStart()
        {
            if (LevelLoadType == LevelType.PlayerData)
            {
                SaveGameManager.SavePlayer();
                GameManager.Instance.LoadNextLevel(NextLevelName);
            }
            else if (CustomTransform)
            {
                SaveGameManager.SaveGame(TargetTransform.position, new Vector2(TargetTransform.eulerAngles.y, LookUpDown), () =>
                {
                    if (LevelLoadType == LevelType.NextLevel)
                        GameManager.Instance.LoadNextLevel(NextLevelName);
                    else
                        GameManager.Instance.LoadNextWorld(NextLevelName);
                });
            }
            else
            {
                SaveGameManager.SaveGame(() =>
                {
                    if (LevelLoadType == LevelType.NextLevel)
                        GameManager.Instance.LoadNextLevel(NextLevelName);
                    else
                        GameManager.Instance.LoadNextWorld(NextLevelName);
                });
            }
        }

        private void OnDrawGizmos()
        {
            if(CustomTransform && TargetTransform != null)
            {
#if UNITY_EDITOR
                UnityEditor.Handles.color = Color.green.Alpha(0.01f);
                UnityEditor.Handles.DrawSolidDisc(TargetTransform.position, Vector3.up, 1f);
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawWireDisc(TargetTransform.position, Vector3.up, 1f);
#endif
                Gizmos.DrawSphere(TargetTransform.position, 0.05f);
                GizmosE.DrawGizmosArrow(TargetTransform.position, TargetTransform.forward);
            }
        }
    }
}