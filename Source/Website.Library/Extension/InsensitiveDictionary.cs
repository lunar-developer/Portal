using System;
using System.Collections.Generic;
using Website.Library.Enum;

namespace Website.Library.Extension
{
    public sealed class InsensitiveDictionary<TValue> : Dictionary<string, TValue>
    {
        public InsensitiveDictionary() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public InsensitiveDictionary(IDictionary<string, TValue> dictionary)
            : base(dictionary, StringComparer.OrdinalIgnoreCase)
        {
        }


        public new TValue this[string key]
        {
            get
            {
                if (ContainsKey(key))
                {
                    return base[key];
                }
                throw new KeyNotFoundException(string.Format(ResourceEnum.KeyNotFound, key));
            }
        }

        /// <summary>
        ///     Return the default value when key is not found. Don't throw exception.
        /// </summary>
        public TValue GetValue(string key)
        {
            return ContainsKey(key) ? base[key] : default(TValue);
        }

        /// <summary>
        ///     Auto append new key into dictionary if that key is not exist.
        ///     Otherwise update the existing key with new value.
        /// </summary>
        public void SetValue(string key, TValue value)
        {
            if (ContainsKey(key))
            {
                base[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }
    }
}