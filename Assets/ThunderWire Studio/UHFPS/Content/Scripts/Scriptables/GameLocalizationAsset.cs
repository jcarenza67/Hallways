using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace UHFPS.Scriptable
{
    [CreateAssetMenu(fileName = "GameLocalization", menuName = "UHFPS/Game Localization")]
    public class GameLocalizationAsset : ScriptableObject
    {
        [Serializable]
        public struct Localization
        {
            public string Key;
            [TextArea(1, 6)]
            public string Text;
        }

        [Serializable]
        public struct LocalizationSection
        {
            public string Section;
            public List<Localization> Localizations;
        }

        public List<LocalizationSection> Localizations = new List<LocalizationSection>();

        public void AddSectionKey(string key, string text)
        {
            var split = key.Split('.');
            string section = split[0];
            string newKey = string.Join(".", split.Skip(1));

            if(!Localizations.Any(x => x.Section == section))
            {
                Localizations.Add(new()
                {
                    Section = section,
                    Localizations = new()
                    {
                        new()
                        {
                            Key = newKey,
                            Text = text
                        }
                    }
                });
            }
            else
            {
                foreach (var localizationSection in Localizations)
                {
                    if(localizationSection.Section == section)
                    {
                        localizationSection.Localizations.Add(new()
                        {
                            Key = newKey,
                            Text = text
                        });
                        break;
                    }
                }
            }
        }
    }
}