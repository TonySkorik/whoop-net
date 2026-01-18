namespace WhoopNet.Helpers.NetstandardPolyfill;

#if NETSTANDARD2_0

internal static class Netstandard20Extensions
{
    extension<T>(IEnumerable<T> collection)
    {
        public HashSet<T> ToHashSet() => [.. collection];
    }

    extension<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
    {
        public bool TryAdd(TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                return false;
            }

            dictionary.Add(key, value);
            return true;
        }
    }

    extension<TKey, TValue>(KeyValuePair<TKey, TValue> target)
    {
        public void Deconstruct(
            out TKey key,
            out TValue value)
        {
            key = target.Key;
            value = target.Value;
        }
    }

    extension(string target)
    {
        public bool Contains(string value, StringComparison comparisonType) => target.IndexOf(value, comparisonType) >= 0;
    }
}

#endif
