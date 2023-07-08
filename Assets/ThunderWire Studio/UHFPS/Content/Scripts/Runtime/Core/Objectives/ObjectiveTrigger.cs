using Newtonsoft.Json.Linq;
using UnityEngine;

namespace UHFPS.Runtime
{
    public class ObjectiveTrigger : MonoBehaviour, IInteractStart, ISaveable
    {
        public enum TriggerType { Trigger, Interact, Event }
        public enum ObjectiveType { New, Complete, NewAndComplete }

        public TriggerType triggerType = TriggerType.Trigger;
        public ObjectiveType objectiveType = ObjectiveType.New;

        public ObjectiveSelect objectiveToAdd;
        public ObjectiveSelect objectiveToComplete;

        private ObjectiveManager objectiveManager;
        private bool isTriggered;

        private void Awake()
        {
            objectiveManager = ObjectiveManager.Instance;
        }

        public void InteractStart()
        {
            if (triggerType != TriggerType.Interact || triggerType == TriggerType.Event || isTriggered)
                return;

            TriggerObjective();
            isTriggered = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (triggerType != TriggerType.Trigger || triggerType == TriggerType.Event || isTriggered)
                return;

            if (other.CompareTag("Player"))
            {
                TriggerObjective();
                isTriggered = true;
            }
        }

        public void TriggerObjective()
        {
            if (objectiveType == ObjectiveType.New)
            {
                objectiveManager.AddObjective(objectiveToAdd.ObjectiveKey, objectiveToAdd.SubObjectives);
            }
            else if (objectiveType == ObjectiveType.Complete)
            {
                objectiveManager.CompleteObjective(objectiveToComplete.ObjectiveKey, objectiveToComplete.SubObjectives);
            }
            else if(objectiveType == ObjectiveType.NewAndComplete)
            {
                objectiveManager.AddObjective(objectiveToAdd.ObjectiveKey, objectiveToAdd.SubObjectives);
                objectiveManager.CompleteObjective(objectiveToComplete.ObjectiveKey, objectiveToComplete.SubObjectives);
            }
        }

        public StorableCollection OnSave()
        {
            return new StorableCollection()
            {
                { nameof(isTriggered), isTriggered }
            };
        }

        public void OnLoad(JToken data)
        {
            isTriggered = (bool)data[nameof(isTriggered)];
        }
    }
}