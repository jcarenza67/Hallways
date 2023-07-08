using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace UHFPS.Runtime
{
    public class CutsceneModule : ManagerModule
    {
        private PlayableDirector currentCutscene;

        public override string ToString() => "Cutscene";

        public void PlayCutscene(PlayableDirector cutscene, Action onCutsceneComplete)
        {
            currentCutscene = cutscene;
            GameManager.StartCoroutine(OnPlayPlayerCutscene(onCutsceneComplete));
        }

        public void PlayCutscene(PlayableDirector cutscene, GameObject cutsceneCamera, float fadeSpeed, Action onCutsceneComplete)
        {
            currentCutscene = cutscene;
            GameManager.StartCoroutine(OnPlayCameraCutscene(cutsceneCamera, fadeSpeed, onCutsceneComplete));
        }

        IEnumerator OnPlayPlayerCutscene(Action onCutsceneComplete)
        {
            GameManager.DisableAllGamePanels();
            PlayerPresence.FreezePlayer(true);
            currentCutscene.Play();

            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds((float)currentCutscene.duration);

            GameManager.ShowPanel(GameManager.PanelType.MainPanel);
            PlayerPresence.FreezePlayer(false);
            onCutsceneComplete.Invoke();
            currentCutscene = null;
        }

        IEnumerator OnPlayCameraCutscene(GameObject cutsceneCamera, float fadeSpeed, Action onCutsceneComplete)
        {
            GameManager.DisableAllGamePanels();
            PlayerPresence.FreezePlayer(true);

            yield return PlayerPresence.SwitchCamera(cutsceneCamera, fadeSpeed);
            currentCutscene.Play();

            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds((float)currentCutscene.duration);
            yield return PlayerPresence.SwitchCamera(null, fadeSpeed);

            GameManager.ShowPanel(GameManager.PanelType.MainPanel);
            PlayerPresence.FreezePlayer(false);
            onCutsceneComplete.Invoke();
            currentCutscene = null;
        }
    }
}