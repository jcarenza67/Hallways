using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using UnityEngine;
using UHFPS.Input;
using UHFPS.Scriptable;
using ThunderWire.Attributes;

namespace UHFPS.Runtime
{
    [InspectorHeader("Game Localization")]
    public class GameLocalization : Singleton<GameLocalization>
    {
        public GameLocalizationAsset LocalizationAsset;
        public CompositeDisposable Disposables = new();

        public Lazy<IDictionary<string, string>> GlocDictionary { get; } = new(() =>
        {
            Dictionary<string, string> glocDict = new();

            foreach (var section in Instance.LocalizationAsset.Localizations)
            {
                if (string.IsNullOrEmpty(section.Section))
                    continue;

                string sectionName = section.Section.Replace(" ", "");
                foreach (var loc in section.Localizations)
                {
                    if (string.IsNullOrEmpty(loc.Key) || string.IsNullOrEmpty(loc.Text))
                        continue;

                    string keyName = loc.Key.Replace(" ", "");
                    string key = sectionName + "." + keyName;

                    if (glocDict.ContainsKey(key))
                    {
                        Debug.LogError($"[GameLocalization] Key with the same name has already been added. Key: {key}");
                        continue;
                    }

                    glocDict.Add(sectionName + "." + keyName, loc.Text);
                }
            }

            return glocDict;
        });

        private readonly BehaviorSubject<string> OnLanguageChange = new("English");

        public IObservable<string> ObserveGloc(string key)
        {
            return OnLanguageChange.Select(lang =>
            {
                // React to a language change, the lang parameter is the current language.
                // In this case, it always gets the English text from the GlocDictionary.

                if (GlocDictionary.Value.TryGetValue(key, out string text))
                    return text;

                return null;
            });
        }

        public void ChangeLanguage(string language)
        {
            OnLanguageChange.OnNext(language);
        }
    }

    public static class GameLocalizationE
    {
        /// <summary>
        /// Get gloc and return a formatted string with the input glyph.
        /// </summary>
        /// <param name="format">Format in which to return the string. <br>Example: "{0} [gloc.key]"</br></param>
        /// <remarks>Useful if you want to get a formatted string in the Update() function.</remarks>
        public static string WithGloc(this InputManager.BindingPath bindingPath, string format)
        {
            Regex regex = new Regex(@"\[(.*?)\]");
            Match match = regex.Match(format);
            if (!match.Success) throw new ArgumentNullException("Could not find the gloc key in [] brackets.");

            string glocKey = regex.Match(format).Groups[1].Value;
            string newFormat = regex.Replace(format, "{1}");

            if (GameLocalization.Instance.GlocDictionary.Value.TryGetValue(glocKey, out string text))
            {
                string glyphFormat = string.Format(newFormat, "{0}", text);
                return bindingPath.Format(glyphFormat);
            }

            throw new NullReferenceException($"Could not find gloc with key '{glocKey}'");
        }

        /// <summary>
        /// Subscribe to listening for a binding path with localized string changes.
        /// </summary>
        /// <param name="key">Game localization key in format "section.key".</param>
        /// <param name="format">Format of the resulting text. Example: "{0} {1}"</param>
        /// <param name="onUpdate">Action when binding path or localization is changed and text is updated.</param>
        public static void SubscribeGlyphGloc(this InputManager.BindingPath bindingPath, string key, string format, Action<string> onUpdate)
        {
            string bindingGlyph = bindingPath.glyphPath;
            string glocText = key;

            // observe binding glyph changes
            bindingPath.ObserveGlyphPath(glyph =>
            {
                bindingGlyph = glyph;
                string formattedString = string.Format(format, bindingGlyph, glocText);
                onUpdate?.Invoke(formattedString);
            });

            // observe gloc string changes
            SubscribeGloc(key, text =>
            {
                glocText = text;
                string formattedString = string.Format(format, bindingGlyph, glocText);
                onUpdate?.Invoke(formattedString);
            });
        }

        /// <summary>
        /// Subscribe to listening for a binding path with localized string changes.
        /// </summary>
        /// <param name="format">Format of the resulting text. Example: "{0} [gloc.key]"</param>
        /// <param name="onUpdate">Action when binding path or localization is changed and text is updated.</param>
        public static void SubscribeGlyphGloc(this InputManager.BindingPath bindingPath, string format, Action<string> onUpdate)
        {
            Regex regex = new Regex(@"\[(.*?)\]");
            Match match = regex.Match(format);
            if (!match.Success) throw new ArgumentNullException("Could not find the gloc key in [] brackets.");

            string glocKey = regex.Match(format).Groups[1].Value;
            string newFormat = regex.Replace(format, "{1}");

            string bindingGlyph = bindingPath.glyphPath;
            string glocText = glocKey;

            // observe binding glyph changes
            bindingPath.ObserveGlyphPath(glyph =>
            {
                bindingGlyph = glyph;
                string formattedString = string.Format(newFormat, bindingGlyph, glocText);
                onUpdate?.Invoke(formattedString);
            });

            // observe gloc string changes
            SubscribeGloc(glocKey, text =>
            {
                glocText = text;
                string formattedString = string.Format(newFormat, bindingGlyph, glocText);
                onUpdate?.Invoke(formattedString);
            });
        }

        /// <summary>
        /// Subscribe to listening for a localized string changes. The result of the localized text may contain actions in the format "[action]" to subscribe to listen for changes to the action binding path. 
        /// </summary>
        /// <param name="key">Game localization key in format "section.key".</param>
        /// <param name="onUpdate">Action when localization is changed and text is updated.</param>
        /// <remarks>Useful if you have text that you want to localize, but you also want to display actions in it. For example: "Press [action1] or [action2] to do something."</remarks>
        public static void SubscribeGlocMany(this string key, Action<string> onUpdate, bool observeBinding = true)
        {
            ReactiveDisposable disposables = new();

            // observe gloc string changes
            SubscribeGloc(key, text =>
            {
                if (string.IsNullOrEmpty(text))
                    return;

                // dispose old subscribed binding changes
                disposables.Dispose();

                bool bindingPathSubscribed = false;
                Regex regex = new Regex(@"\[(.*?)\]");
                MatchCollection matches = regex.Matches(text);
                string[] bindingGlyphs = new string[matches.Count];
                string formatText = text;

                if (matches.Count > 0)
                {
                    var matchesArray = matches.ToArray();
                    foreach (Match match in matchesArray)
                    {
                        string group = match.Groups[0].Value;
                        string action = match.Groups[1].Value;

                        if (!InputManager.HasReference)
                        {
                            Debug.LogError("Reference to InputManager was not found!");
                            continue;
                        }

                        var bindingPath = InputManager.GetBindingPath(action);
                        if (bindingPath == null) continue;

                        int index = Array.IndexOf(matchesArray, match);
                        formatText = formatText.Replace(group, "{" + index + "}");

                        // observe binding path changes
                        if (observeBinding)
                        {
                            disposables.Add(bindingPath.GlyphPathObservable.Subscribe(glyphPath =>
                            {
                                bindingGlyphs[index] = glyphPath;
                                if (bindingPathSubscribed)
                                {
                                    string formattedString = string.Format(formatText, bindingGlyphs);
                                    onUpdate?.Invoke(formattedString);
                                }
                            }));
                        }
                        else
                        {
                            bindingGlyphs[index] = bindingPath.glyphPath;
                        }
                    }

                    bindingPathSubscribed = true;
                }

                string formattedString = string.Format(formatText, bindingGlyphs);
                onUpdate?.Invoke(formattedString);
            });
        }

        /// <summary>
        /// Subscribe to listening for a localized string changes.
        /// </summary>
        /// <param name="key">Game localization key in format "section.key"</param>
        /// <param name="onUpdate">Action when localization is changed and text is updated.</param>
        public static void SubscribeGloc(this string key, Action<string> onUpdate, bool updateWhenNull = true)
        {
#if UHFPS_LOCALIZATION
            if (!GameLocalization.HasReference || string.IsNullOrEmpty(key))
            {
                onUpdate?.Invoke(key);
                return;
            }

            GameLocalization localization = GameLocalization.Instance;
            CompositeDisposable disposables = localization.Disposables;

            if (localization.GlocDictionary.Value.ContainsKey(key))
            {
                disposables.Add(localization.ObserveGloc(key).Subscribe(text =>
                {
                    onUpdate?.Invoke(text);
                }));
            }
            else if(updateWhenNull)
            {
                onUpdate?.Invoke(key);
            }
#else
            onUpdate?.Invoke(key);
#endif
        }
    }
}