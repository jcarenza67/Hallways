using System;
using System.Reflection;
using UnityEngine;

namespace UHFPS.Runtime
{
    [Serializable]
    public sealed class ReflectionField
    {
        public enum ReflectionType { Field, Property, Method };

        public ReflectionType ReflectType;
        public MonoBehaviour Instance;
        public string ReflectName;

        private FieldInfo fieldInfo = null;
        private PropertyInfo propertyInfo = null;
        private MethodInfo methodInfo = null;

        public bool Value
        {
            get => ReflectType switch
            {
                ReflectionType.Field => (bool)fieldInfo.GetValue(Instance),
                ReflectionType.Property => (bool)propertyInfo.GetValue(Instance),
                ReflectionType.Method => (bool)methodInfo.Invoke(Instance, new object[0]),
                _ => throw new NullReferenceException()
            };

            set
            {
                try
                {
                    if (ReflectType == ReflectionType.Field)
                    {
                        fieldInfo.SetValue(Instance, value);
                    }
                    else if (ReflectType == ReflectionType.Property)
                    {
                        propertyInfo.SetValue(Instance, value);
                    }
                    else
                    {
                        methodInfo.Invoke(Instance, new object[] { value });
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

        public void Initialize()
        {
            switch (ReflectType)
            {
                case ReflectionType.Field:
                    fieldInfo = Instance.GetType().GetField(ReflectName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    break;
                case ReflectionType.Property:
                    propertyInfo = Instance.GetType().GetProperty(ReflectName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    break;
                case ReflectionType.Method:
                    methodInfo = Instance.GetType().GetMethod(ReflectName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    break;
            }
        }
    }
}