using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json.Linq;
using UHFPS.Tools;

namespace UHFPS.Runtime
{
    public enum PowerType { None, Output, Input }
    public enum PartDirection { Up, Down, Left, Right }

    [RequireComponent(typeof(AudioSource))]
    public class ElectricalCircuitPuzzle : PuzzleBase, ISaveable
    {
        [Serializable]
        public sealed class PowerComponent
        {
            public PowerType PowerType;
            public PartDirection PowerDirection;
            public int PowerID;
            public int ConnectPowerID;
        }

        [Serializable]
        public sealed class ComponentFlow
        {
            public ElectricalCircuitComponent Component;
            public int Rotation;
        }

        [Serializable]
        public sealed class PowerInputEvents
        {
            public PowerComponent PowerComponent;
            public UnityEvent<int> OnConnected;
            public UnityEvent<int> OnDisconnected;
        }

        public ushort Rows;
        public ushort Columns;
        public PowerComponent[] PowerFlow;
        public ElectricalCircuitComponent[] CircuitComponents;

        public Transform ComponentsParent;
        public float ComponentsSpacing = 0f;
        public float ComponentsSize = 1f;

        public ComponentFlow[] ComponentsFlow;
        public List<ElectricalCircuitComponent> Components = new();
        public List<PowerInputEvents> InputEvents = new();

        public SoundClip RotateComponent;
        public SoundClip PowerConnected;
        public SoundClip PowerDisconnected;

        public UnityEvent OnConnected;
        public UnityEvent OnDisconnected;
        public bool isConnected;

        private AudioSource audioSource;

        public override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (!SaveGameManager.GameWillLoad)
            {
                PowerAllOutputs();
                CheckAllInputs();
            }
        }

        public void ReinitializeCircuit()
        {
            RemoveAllPowerIDs();
            PowerAllOutputs();
            CheckPowerStates();
            CheckAllInputs();

            audioSource.PlayOneShotSoundClip(RotateComponent);
        }

        public void PowerAllOutputs()
        {
            for (int i = 0; i < PowerFlow.Length; i++)
            {
                PowerComponent powerComponent = PowerFlow[i];
                if (powerComponent.PowerType == PowerType.Output)
                {
                    ElectricalCircuitComponent component = Components[i];
                    PartDirection fromDirection = ToOppositeDirection(powerComponent.PowerDirection);
                    component.SetPowerFlow(fromDirection, powerComponent.PowerID, null);
                }
            }
        }

        public void CheckAllInputs()
        {
            Dictionary<int, List<int>> connectPairs = new();
            Dictionary<PowerComponent, ElectricalCircuitComponent> inputs = new();
            int inputsCount = 0;

            for (int i = 0; i < PowerFlow.Length; i++)
            {
                PowerComponent powerComponent = PowerFlow[i];
                if (powerComponent.PowerType == PowerType.Output)
                {
                    if (!connectPairs.ContainsKey(powerComponent.ConnectPowerID))
                    {
                        connectPairs.Add(powerComponent.ConnectPowerID, new() { powerComponent.PowerID });
                    }
                    else
                    {
                        connectPairs[powerComponent.ConnectPowerID].Add(powerComponent.PowerID);
                    }
                }
                else if (powerComponent.PowerType == PowerType.Input)
                {
                    ElectricalCircuitComponent component = Components[i];
                    inputs.Add(powerComponent, component);
                    inputsCount++;
                }
            }

            int connectedInputs = 0;
            foreach (var input in inputs)
            {
                if (connectPairs.TryGetValue(input.Key.PowerID, out var requiredConnections))
                {
                    PowerInputEvents events = InputEvents.FirstOrDefault(x => x.PowerComponent.PowerID == input.Key.PowerID);
                    PartDirection oppositeDIrection = ToOppositeDirection(input.Key.PowerDirection);
                    var oppositeFlow = input.Value.GetOppositePowerFlow(oppositeDIrection);
                    int reqCount = requiredConnections.Count;

                    if (oppositeFlow != null)
                    {
                        int connected = 0;

                        foreach (var connection in requiredConnections)
                        {
                            if (oppositeFlow.PowerFlows.Contains(connection))
                            {
                                events.OnConnected?.Invoke(connection);
                                connected++;
                            }
                            else
                            {
                                events.OnDisconnected?.Invoke(connection);
                            }
                        }

                        if (connected == reqCount)
                            connectedInputs++;
                    }
                    else
                    {
                        foreach (var connection in requiredConnections)
                        {
                            if(events != null) events.OnDisconnected?.Invoke(connection);
                        }
                    }
                }
            }

            if(connectedInputs == inputsCount)
            {
                if (!SaveGameManager.GameWillLoad)
                    audioSource.PlayOneShotSoundClip(PowerConnected);

                OnConnected?.Invoke();
                isConnected = true;
            }
            else if(isConnected)
            {
                if (!SaveGameManager.GameWillLoad)
                    audioSource.PlayOneShotSoundClip(PowerDisconnected);

                OnDisconnected?.Invoke();
                isConnected = false;
            }
        }

        public void RemoveAllPowerIDs()
        {
            foreach (var component in Components)
            {
                foreach (var flow in component.PowerFlows)
                {
                    flow.PowerFlows.Clear();
                }
            }
        }

        public void CheckPowerStates()
        {
            foreach (var component in Components)
            {
                foreach (var flow in component.PowerFlows)
                {
                    if (!flow.PowerFlows.Any())
                    {
                        component.SetFlowState(flow, false);
                    }
                }
            }
        }

        public int CoordsToIndex(Vector2Int coords)
        {
            return coords.y * Columns + coords.x;
        }

        public bool IsCoordsValid(Vector2Int coords)
        {
            return coords.x >= 0 && coords.x < Columns
                && coords.y >= 0 && coords.y < Rows;
        }

        public static Vector2Int DirectionToVector(PartDirection direction)
        {
            return direction switch
            {
                PartDirection.Up => new Vector2Int(0, -1),
                PartDirection.Down => new Vector2Int(0, 1),
                PartDirection.Left => new Vector2Int(-1, 0),
                PartDirection.Right => new Vector2Int(1, 0),
                _ => Vector2Int.zero,
            };
        }

        public static bool IsOppositeDirection(PartDirection lhs, PartDirection rhs)
        {
            if (lhs == PartDirection.Up && rhs == PartDirection.Down) return true;
            else if (lhs == PartDirection.Down && rhs == PartDirection.Up) return true;
            else if (lhs == PartDirection.Left && rhs == PartDirection.Right) return true;
            else if (lhs == PartDirection.Right && rhs == PartDirection.Left) return true;
            return false;
        }

        public static PartDirection ToOppositeDirection(PartDirection direction)
        {
            return direction switch
            {
                PartDirection.Up => PartDirection.Down,
                PartDirection.Down => PartDirection.Up,
                PartDirection.Left => PartDirection.Right,
                PartDirection.Right => PartDirection.Left,
                _ => direction
            };
        }

        public StorableCollection OnSave()
        {
            StorableCollection saveableBuffer = new StorableCollection();

            for (int i = 0; i < Components.Count; i++)
            {
                saveableBuffer.Add("component_" + i, Components[i].Angle);
            }

            return saveableBuffer;
        }

        public void OnLoad(JToken data)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].Angle = (float)data["component_" + i];
                Components[i].InitializeDirections();
                Components[i].SetComponentAngle();
            }

            PowerAllOutputs();
            CheckAllInputs();
        }
    }
}