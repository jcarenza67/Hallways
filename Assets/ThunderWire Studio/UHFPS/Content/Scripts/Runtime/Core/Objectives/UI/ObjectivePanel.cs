using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Disposables;
using UnityEngine;
using UHFPS.Tools;
using TMPro;

namespace UHFPS.Runtime
{
    public class ObjectivePanel : MonoBehaviour
    {
        public Transform subObjectivesPanel;
        public TMP_Text titleText;

        [Header("Timing")]
        public float hideObjectiveAfter = 2f;
        public float hideSubObjectiveAfter = 2f;

        [Header("Speed")]
        public float objectiveFadeInSpeed = 2f;
        public float objectiveFadeOutSpeed = 2f;

        private readonly Dictionary<string, GameObject> subObjectives = new();
        private readonly CompositeDisposable disposables = new();

        private ObjectiveManager objectiveManager;
        private ObjectiveManager.ObjectiveData objectiveData;
        private string objectiveKey;

        private bool subObjectiveAnimCompleted = false;

        private void OnDestroy()
        {
            disposables.Dispose();
        }

        public void SetObjective(ObjectiveManager manager, ObjectiveManager.ObjectiveData objectiveData)
        {
            objectiveManager = manager;
            this.objectiveData = objectiveData;

            objectiveKey = objectiveData.Objective.ObjectiveKey;
            titleText.text = objectiveData.Objective.ObjectiveTitle;

            // subscribe listening to localization changes
            objectiveData.Objective.ObjectiveTitle.OnTextChange.Subscribe(text => titleText.text = text).AddTo(disposables);

            // event when all objectives will be completed
            objectiveData.IsCompleted.Subscribe(completed =>
            {
                if (completed)
                {
                    StartCoroutine(HideAndDestroyBaseObjective(objectiveData.ObjectiveObject));
                    disposables.Dispose();
                }
            }).AddTo(disposables);

            // event when sub objective will be added
            objectiveData.AddSubObjective.Subscribe(data =>
            {
                CreateSubObjective(data, false);
            }).AddTo(disposables);

            // event when sub objective will be removed
            objectiveData.RemoveSubObjective.Subscribe(key =>
            {
                StartCoroutine(HideAndDestroy(subObjectives[key]));
            }).AddTo(disposables);

            // add starting objectives
            foreach (var subObj in objectiveData.SubObjectives)
            {
                CreateSubObjective(subObj.Value, true);
            }

            StartCoroutine(FadeObjective(gameObject, true));
        }

        private void CreateSubObjective(ObjectiveManager.SubObjectiveData data, bool isInitial)
        {
            GameObject objective = Instantiate(objectiveManager.SubObjectivePrefab, Vector3.zero, Quaternion.identity, subObjectivesPanel);
            SubObjectivePanel subObjective = objective.GetComponent<SubObjectivePanel>();
            CompositeDisposable subDisposables = new();

            data.SubObjectiveObject = objective;
            string subObjectiveText = data.SubObjective.ObjectiveText;

            // subscribe listening to localization changes
            data.SubObjective.ObjectiveText.OnTextChange
                .Subscribe(text => FormatObjectiveText(text, data.CompleteCount.Value))
                .AddTo(subDisposables);

            // initialize objective text
            subObjective.ObjectiveText.text = FormatObjectiveText(subObjectiveText, data.CompleteCount.Value);

            // event when sub objective will be completed
            data.IsCompleted.Subscribe(completed =>
            {
                if (completed)
                {
                    StartCoroutine(CompleteSubObjective(subObjective, data.SubObjective.SubObjectiveKey));
                    subDisposables.Dispose();
                }
            }).AddTo(subDisposables);

            // event when sub objective complete count will be changed
            data.CompleteCount.Subscribe(count =>
            {
                subObjective.ObjectiveText.text = FormatObjectiveText(subObjectiveText, count);
            }).AddTo(subDisposables);

            // add sub objective to sub objectives dictionary
            subObjectives.Add(data.SubObjective.SubObjectiveKey, objective);

            // fade objective in
            if(!isInitial) StartCoroutine(FadeObjective(objective, true));
            else
            {
                CanvasGroup canvasGroup = objective.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 1f;
            }
        }

        private string FormatObjectiveText(string text, ushort count)
        {
            return text.RegexReplaceTag('[', ']', "count", count.ToString());
        }

        IEnumerator FadeObjective(GameObject objective, bool fadeIn)
        {
            CanvasGroup canvasGroup = objective.GetComponent<CanvasGroup>();

            if (fadeIn)
            {
                while(!Mathf.Approximately(canvasGroup.alpha, 1f))
                {
                    canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, Time.deltaTime * objectiveFadeInSpeed);
                    yield return null;
                }

                canvasGroup.alpha = 1f;
            }
            else
            {
                while (!Mathf.Approximately(canvasGroup.alpha, 0f))
                {
                    canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, Time.deltaTime * objectiveFadeOutSpeed);
                    yield return null;
                }

                canvasGroup.alpha = 0f;
            }
        }

        IEnumerator HideAndDestroy(GameObject objective)
        {
            yield return FadeObjective(objective, false);
            objectiveManager.RemoveObjective(objectiveKey);
            Destroy(objective);
        }

        IEnumerator HideAndDestroyBaseObjective(GameObject objective)
        {
            yield return new WaitUntil(() => subObjectiveAnimCompleted);
            yield return new WaitForSeconds(hideObjectiveAfter);
            yield return HideAndDestroy(objective);
        }

        IEnumerator CompleteSubObjective(SubObjectivePanel subObjective, string key)
        {
            yield return subObjective.CompleteObjective();

            if (subObjectives.Count > 1)
            {
                yield return new WaitForSeconds(hideSubObjectiveAfter);
                yield return subObjective.HideObjective();
                subObjectives.Remove(key);
                Destroy(subObjective.gameObject);
                objectiveData.SubObjectives.Remove(key);
            }
            else
            {
                subObjectiveAnimCompleted = true;
            }
        }
    }
}