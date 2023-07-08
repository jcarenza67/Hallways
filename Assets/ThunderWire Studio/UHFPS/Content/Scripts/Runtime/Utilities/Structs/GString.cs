using System;
using System.Reactive.Subjects;
using System.Text.RegularExpressions;
using UHFPS.Input;

namespace UHFPS.Runtime
{
    /// <summary>
    /// Represents a string that can be localized (GameLocalization) or normal.
    /// </summary>
    [Serializable]
    public sealed class GString
    {
        public const char EXCLUDE_CHAR = '*';

        public string GlocText;
        public string NormalText;
        public Subject<string> OnTextChange = new();

        public string Value
        {
            get
            {
                if(string.IsNullOrEmpty(NormalText)) 
                    return GlocText;

                return NormalText;
            }

            set
            {
                NormalText = value;
            }
        }

        public static implicit operator string(GString gstring)
        {
            if(gstring == null || string.IsNullOrEmpty(gstring.Value))
                return string.Empty;

            return gstring.Value;
        }

        public static implicit operator GString(string value)
        {
            GString result = new();
            result.NormalText = value;
            return result;
        }

        /// <summary>
        /// Subscribe to listening for a localized string changes.
        /// </summary>
        public void SubscribeGloc(Action<string> onUpdate = null)
        {
#if UHFPS_LOCALIZATION
            if (GlocText.Length > 0 && GlocText[0] == EXCLUDE_CHAR)
            {
                NormalText = GlocText[1..];
                return;
            }

            GlocText.SubscribeGloc(text => 
            {
                NormalText = text;
                onUpdate?.Invoke(text);
                OnTextChange.OnNext(text);
            });
#endif
        }

        /// <summary>
        /// Subscribe to listening for a localized string changes. The result of the localized text may contain actions in the format "[action]" to subscribe to listen for changes to the action binding path.
        /// </summary>
        public void SubscribeGlocMany(Action<string> onUpdate = null)
        {
#if UHFPS_LOCALIZATION
            if (GlocText.Length > 0 && GlocText[0] == EXCLUDE_CHAR)
            {
                NormalText = GlocText;
                return;
            }

            GlocText.SubscribeGlocMany(text =>
            {
                NormalText = text;
                onUpdate?.Invoke(text);
                OnTextChange.OnNext(text);
            });
#endif
        }

        /// <summary>
        /// Subscribe to listening for a binding path changes.
        /// </summary>
        public void ObserveBindingPath()
        {
            Regex regex = new Regex(@"\[(.*?)\]");
            Match match = regex.Match(NormalText);

            if (match.Success)
            {
                string actionName = match.Groups[1].Value;
                string text = NormalText;

                InputManagerE.ObserveGlyphPath(actionName, 0, glyph =>
                {
                    NormalText = regex.Replace(text, glyph);
                });
            }
        }

        public override string ToString() => Value;
    }
}