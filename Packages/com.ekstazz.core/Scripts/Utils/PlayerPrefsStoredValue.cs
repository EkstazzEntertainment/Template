namespace Ekstazz.Utils
{
    using System;
    using UnityEngine;

    public class PlayerPrefsStoredValue<T>
    {
        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                Save(value);
                onValueChanged(value);
            }
        }

        private string FullKey => $"local:{key}";

        private readonly string key;
        private readonly Action<T> onValueChanged;
        private T value;

        public PlayerPrefsStoredValue(string key, T defaultValue) : this(key, _ => { }, defaultValue)
        {
        }

        public PlayerPrefsStoredValue(string key, Action<T> onValueChanged, T defaultValue)
        {
            this.key = key;
            this.onValueChanged = onValueChanged;
            value = (T) Restore(defaultValue);

            onValueChanged(value);
        }

        private void Save(T value)
        {
            PlayerPrefs.SetString(FullKey, value.ToString());
        }

        private object Restore(T defaultValue)
        {
            if (!PlayerPrefs.HasKey(FullKey))
            {
                Save(defaultValue);
                return defaultValue;
            }
            var stringValue = PlayerPrefs.GetString(FullKey);
            if (TryParseString(stringValue, out var result))
            {
                return result;
            }
            throw new Exception($"{typeof(T)} is not supported in PlayerPrefsStoredValue");
        }

        private bool TryParseString(string stringValue, out T value)
        {
            try
            {
                return ParseString(stringValue, out value);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to convert type {typeof(T)} to string");
                Debug.LogException(e);
                value = default;
                return false;
            }
        }

        private bool ParseString(string stringValue, out T value)
        {
            var type = typeof(T);
            if (type.IsEnum)
            {
                value = (T) Enum.Parse(type, stringValue);
                return true;
            }
            value = (T) Convert.ChangeType(stringValue, type);
            return true;
        }
    }
}
