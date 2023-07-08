using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UHFPS.Tools;
using ThunderWire.Attributes;
using Newtonsoft.Json.Linq;

namespace UHFPS.Runtime
{
    [InspectorHeader("Interactable Light")]
    public class InteractableLight : MonoBehaviour, IInteractStart, ISaveable
    {
        [Serializable]
        public struct LightEventsStruct
        {
            public UnityEvent OnLightOn;
            public UnityEvent OnLightOff;
        }

        public bool IsSwitchedOn;

        [Header("Light")]
        public List<Light> LightComponents = new();

        [Header("Emission")]
        public RendererMaterial LightMaterial;
        [Space]
        public string EmissionKeyword = "_EMISSION";
        public bool EnableEmission = true;

        [Header("Sounds")]
        public SoundClip LightSwitchOn;
        public SoundClip LightSwitchOff;

        [Space, Boxed("AnimationWindowEvent Icon", title = "Light Events")]
        public LightEventsStruct LightEvents = new();

        private void Awake()
        {
            SetLightState(IsSwitchedOn);
        }

        public void InteractStart()
        {
            if(IsSwitchedOn = !IsSwitchedOn)
            {
                SetLightState(true);
                GameTools.PlayOneShot3D(transform.position, LightSwitchOn, "Lamp On");
                LightEvents.OnLightOn?.Invoke();
            }
            else
            {
                SetLightState(false);
                GameTools.PlayOneShot3D(transform.position, LightSwitchOff, "Lamp Off");
                LightEvents.OnLightOff?.Invoke();
            }
        }

        public void SetLightState(bool state)
        {
            LightComponents.ForEach(x => x.enabled = state);

            if (LightMaterial.IsAssigned && EnableEmission)
            {
                if (state) LightMaterial.ClonedMaterial.EnableKeyword(EmissionKeyword);
                else LightMaterial.ClonedMaterial.DisableKeyword(EmissionKeyword);
            }

            IsSwitchedOn = state;
        }

        public StorableCollection OnSave()
        {
            return new StorableCollection()
            {
                { "lightState", IsSwitchedOn }
            };
        }

        public void OnLoad(JToken data)
        {
            bool lightState = (bool)data["lightState"];
            SetLightState(lightState);
        }
    }
}