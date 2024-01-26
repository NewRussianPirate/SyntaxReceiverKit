using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SyntaxReceiverKit
{
    //Base for receivers that collect data as Dictionary

    public abstract class CombinedReceiverBase<TKey, TValues> : SyntaxReceiver
    {
        public Dictionary<TKey, List<TValues>> CollectedSymbols { get; protected set; }
    }

    public abstract class CombinedReceiver<T> : CombinedReceiverBase<string, T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void AddIfKeyExists(string key, T value)
        {
            foreach(var pair in CollectedSymbols)
            {
                if(pair.Key == key)
                    pair.Value.Add(value);
            }
        }

        public CombinedReceiver(params string[] typeNames)
        {
            if (typeNames == null) throw new ArgumentNullException(nameof(typeNames));
            if (typeNames.Length == 0) throw new ArgumentException($"{nameof(typeNames)} is empty.");
            CollectedSymbols = new();
            foreach (var i in typeNames)
                CollectedSymbols.Add(i, new());
        }
    }
}
