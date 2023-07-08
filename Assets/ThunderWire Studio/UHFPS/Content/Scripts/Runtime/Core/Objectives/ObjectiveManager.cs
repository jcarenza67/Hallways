using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Collections.Generic;
using UnityEngine;
using UHFPS.Scriptable;
using ThunderWire.Attributes;
using Newtonsoft.Json.Linq;

namespace UHFPS.Runtime
{
    [InspectorHeader("Objective Manager")]
    public class ObjectiveManager : Singleton<ObjectiveManager>, ISaveableCustom
    {
        #region Structures
        public struct ObjectiveCache
        {
            public Objective Objective;
            public Dictionary<string, SubObjective> SubObjectives;

            public ObjectiveCache(Objective objective)
            {
                Objective = objective;
                SubObjectives = new();

                objective.ObjectiveTitle.SubscribeGloc();
                foreach (var subObjective in Objective.SubObjectives)
                {
                    subObjective.ObjectiveText.SubscribeGloc();
                    SubObjectives.Add(subObjective.SubObjectiveKey, subObjective);
                }
            }
        }

        public sealed class ObjectiveData
        {
            public GameObject ObjectiveObject;

            public Objective Objective;
            public Dictionary<string, SubObjectiveData> SubObjectives;

            public BehaviorSubject<bool> IsCompleted;
            public Subject<SubObjectiveData> AddSubObjective;
            public Subject<string> RemoveSubObjective;

            public ObjectiveData(Objective objective)
            {
                Objective = objective;
                SubObjectives = new();

                IsCompleted = new BehaviorSubject<bool>(false);
                AddSubObjective = new Subject<SubObjectiveData>();
                RemoveSubObjective = new Subject<string>();
            }
        }

        public sealed class SubObjectiveData
        {
            public GameObject SubObjectiveObject;
            public SubObjective SubObjective;

            public BehaviorSubject<ushort> CompleteCount;
            public BehaviorSubject<bool> IsCompleted;

            public SubObjectiveData(SubObjective subObjective)
            {
                SubObjective = subObjective;
                CompleteCount = new BehaviorSubject<ushort>(0);
                IsCompleted = new BehaviorSubject<bool>(false);
            }
        }
        #endregion

        public ObjectivesAsset ObjectivesAsset;

        [Header("References")]
        public CanvasGroup ObjectivesGroup;
        public Transform ObjectivesPanel;
        public GameObject ObjectivePrefab;
        public GameObject SubObjectivePrefab;

        private readonly Dictionary<string, ObjectiveCache> objectivesCache = new();
        private readonly Dictionary<string, ObjectiveData> activeObjectives = new();
        private ObjectiveEvent[] objectiveEvents;

        private void Awake()
        {
            foreach (var objective in ObjectivesAsset.Objectives)
            {
                objectivesCache.Add(objective.ObjectiveKey, new ObjectiveCache(objective));
            }

            objectiveEvents = FindObjectsOfType<ObjectiveEvent>();
        }

        public void AddObjective(string objectiveKey, string[] subObjectives)
        {
            if (string.IsNullOrEmpty(objectiveKey) || subObjectives.Length <= 0)
            {
                Debug.LogError("Cannot add null or empty objective!");
                return;
            }

            // check if the objective already exists and add sub objectives to it
            if (activeObjectives.ContainsKey(objectiveKey))
            {
                AddSubObjective(objectiveKey, subObjectives);
                return;
            }

            // create objective object
            GameObject objectiveGo = Instantiate(ObjectivePrefab, Vector3.zero, Quaternion.identity, ObjectivesPanel);
            ObjectivePanel objectivePanel = objectiveGo.GetComponent<ObjectivePanel>();

            // create objective data
            ObjectiveData objectiveData = CreateObjectiveData(objectiveKey, subObjectives, true);
            objectiveData.ObjectiveObject = objectiveGo;

            // set objective
            objectivePanel.SetObjective(this, objectiveData);
            activeObjectives.Add(objectiveKey, objectiveData);
        }

        public void AddSubObjective(string objectiveKey, string[] subObjectives)
        {
            if (subObjectives.Length <= 0) return;

            if (activeObjectives.TryGetValue(objectiveKey, out ObjectiveData data))
            {
                ObjectiveEvent[] events = GetObjectiveEvents(objectiveKey);

                foreach (var sub in GetSubObjectives(objectiveKey, subObjectives))
                {
                    if (string.IsNullOrEmpty(sub.SubObjectiveKey))
                        continue;

                    if (!data.SubObjectives.ContainsKey(sub.SubObjectiveKey))
                    {
                        SubObjectiveData subObjectiveData = new(sub);
                        SetSubObjectiveEvents(subObjectiveData, events, true);
                        data.SubObjectives.Add(sub.SubObjectiveKey, subObjectiveData);
                        data.AddSubObjective.OnNext(subObjectiveData);
                    }
                }
            }
        }

        public void CompleteObjective(string objectiveKey, params string[] subObjectives)
        {
            if (activeObjectives.TryGetValue(objectiveKey, out ObjectiveData data))
            {
                foreach (var sub in subObjectives)
                {
                    if(data.SubObjectives.TryGetValue(sub, out SubObjectiveData subData))
                    {
                        ushort count = subData.CompleteCount.Value;
                        subData.CompleteCount.OnNext(++count);

                        if (count >= subData.SubObjective.CompleteCount)
                            subData.IsCompleted.OnNext(true);
                    }
                }

                if(data.SubObjectives.All(x => x.Value.IsCompleted.Value))
                    data.IsCompleted.OnNext(true);
            }
        }

        public void RemoveObjective(string objectiveKey)
        {
            activeObjectives.Remove(objectiveKey);
        }

        private ObjectiveData CreateObjectiveData(string objectiveKey, string[] subObjectives, bool sendEvent = true)
        {
            // get objective cache
            var objectiveCache = GetObjective(objectiveKey);
            if (!objectiveCache.HasValue) return null;

            // create objective data
            ObjectiveData objectiveData = new(objectiveCache.Value.Objective);

            // get objective events
            ObjectiveEvent[] events = GetObjectiveEvents(objectiveKey);

            // add starting sub objectives to objective
            foreach (var sub in GetSubObjectives(objectiveKey, subObjectives))
            {
                if (string.IsNullOrEmpty(sub.SubObjectiveKey))
                    continue;

                SubObjectiveData subObjectiveData = new(sub);
                SetSubObjectiveEvents(subObjectiveData, events, false);
                objectiveData.SubObjectives.Add(sub.SubObjectiveKey, subObjectiveData);
            }

            // set objective events
            SetObjectiveEvents(objectiveData, events, sendEvent);

            // return objective data 
            return objectiveData;
        }

        private void SetObjectiveEvents(ObjectiveData objectiveData, ObjectiveEvent[] events, bool sendAddEvent)
        {
            foreach (var evt in events)
            {
                objectiveData.IsCompleted.Subscribe(x =>
                {
                    if (x) evt.OnObjectiveCompleted?.Invoke();
                });

                if (sendAddEvent) evt.OnObjectiveAdded?.Invoke();
            }
        }

        private void SetSubObjectiveEvents(SubObjectiveData subObjectiveData, ObjectiveEvent[] events, bool sendAddEvent)
        {
            foreach (var evt in events)
            {
                if (evt.Objective.CompareSub(subObjectiveData.SubObjective.SubObjectiveKey))
                {
                    subObjectiveData.IsCompleted.Subscribe(x =>
                    {
                        if (x) evt.OnSubObjectiveCompleted?.Invoke();
                    });

                    subObjectiveData.CompleteCount.Subscribe(x =>
                    {
                        evt.OnSubObjectiveCountChanged?.Invoke(x);
                    });

                    if (sendAddEvent) evt.OnSubObjectiveAdded?.Invoke();
                }
            }
        }

        private ObjectiveCache? GetObjective(string key)
        {
            if (objectivesCache.TryGetValue(key, out var objective))
                return objective;

            Debug.LogError($"Objective '{key}' not found!");
            return null;
        }

        private SubObjective[] GetSubObjectives(string objKey, params string[] subKeys)
        {
            ObjectiveCache objective = GetObjective(objKey).GetValueOrDefault();
            SubObjective[] subObjectives = new SubObjective[subKeys.Length];

            for (int i = 0; i < subKeys.Length; i++)
            {
                string key = subKeys[i];
                if (objective.SubObjectives.TryGetValue(key, out var subObjective))
                    subObjectives[i] = subObjective;
                else Debug.LogError($"Sub objective '{key}' not found!");
            }

            return subObjectives;
        }

        private ObjectiveEvent[] GetObjectiveEvents(string objectiveKey)
        {
            IList<ObjectiveEvent> events = new List<ObjectiveEvent>();
            foreach (var evt in objectiveEvents)
            {
                if(evt.Objective.IsValid && evt.Objective.CompareObj(objectiveKey))
                    events.Add(evt);
            }

            return events.ToArray();
        }

        public StorableCollection OnCustomSave()
        {
            StorableCollection objectives = new StorableCollection();

            foreach (var obj in activeObjectives.Values)
            {
                StorableCollection subObjectivesBuffer = new StorableCollection();
                StorableCollection objectiveBuffer = new StorableCollection
                {
                    { "isCompleted", obj.IsCompleted.Value }
                };

                foreach (var sub in obj.SubObjectives.Values)
                {
                    subObjectivesBuffer.Add(sub.SubObjective.SubObjectiveKey, new StorableCollection
                    {
                        { "completeCount", sub.CompleteCount.Value },
                        { "isCompleted", sub.IsCompleted.Value }
                    });
                }

                objectiveBuffer.Add("subObjectives", subObjectivesBuffer);
                objectives.Add(obj.Objective.ObjectiveKey, objectiveBuffer);
            }

            return objectives;
        }

        public void OnCustomLoad(JToken data)
        {
            JObject objectives = (JObject)data;

            foreach (var obj in objectives.Properties())
            {
                JToken objective = obj.Value;
                JObject subObjectives = (JObject)objective["subObjectives"];
                bool isCompleted = (bool)objective["isCompleted"];

                // get sub objective keys and objective data
                string[] subObjectiveKeys = subObjectives.Properties().Select(x => x.Name).ToArray();
                ObjectiveData objectiveData = CreateObjectiveData(obj.Name, subObjectiveKeys, false);

                if (objectiveData != null)
                {
                    // create objective object
                    GameObject objectiveGo = Instantiate(ObjectivePrefab, Vector3.zero, Quaternion.identity, ObjectivesPanel);
                    ObjectivePanel objectivePanel = objectiveGo.GetComponent<ObjectivePanel>();

                    // set objective variables
                    objectiveData.ObjectiveObject = objectiveGo;
                    objectiveData.IsCompleted = new BehaviorSubject<bool>(isCompleted);

                    foreach (var sub in subObjectives.Properties())
                    {
                        JToken subObjective = sub.Value;
                        ushort completeCount = (ushort)subObjective["completeCount"];
                        bool isSubCompleted = (bool)subObjective["isCompleted"];

                        // set subobjective variables
                        if (objectiveData.SubObjectives.TryGetValue(sub.Name, out var subData))
                        {
                            subData.CompleteCount = new BehaviorSubject<ushort>(completeCount);
                            subData.IsCompleted = new BehaviorSubject<bool>(isSubCompleted);
                        }
                    }

                    // set objective
                    objectivePanel.SetObjective(this, objectiveData);
                    activeObjectives.Add(obj.Name, objectiveData);
                }
            }
        }
    }
}