namespace Game.Core.Modes
{
    /// <summary>
    /// Stable hashing for anything two independent runs - or two independent
    /// clients - must agree on: which arena a match uses, which loadout an
    /// opponent brings. <see cref="string.GetHashCode()"/> cannot be used for
    /// this: it is randomised per process on modern runtimes, so the same match
    /// id would pick a different arena on each side.
    /// </summary>
    public static class DeterministicSeed
    {
        private const uint FnvOffsetBasis = 2166136261;
        private const uint FnvPrime = 16777619;

        /// <summary>FNV-1a over the UTF-16 code units, byte by byte.</summary>
        public static uint Hash(string value)
        {
            var hash = FnvOffsetBasis;
            if (string.IsNullOrEmpty(value))
            {
                return hash;
            }

            for (var i = 0; i < value.Length; i++)
            {
                var unit = value[i];
                hash = (hash ^ (byte)(unit & 0xFF)) * FnvPrime;
                hash = (hash ^ (byte)(unit >> 8)) * FnvPrime;
            }

            return hash;
        }

        /// <summary>Combines several parts into one seed, order-sensitive.</summary>
        public static uint Hash(params string[] parts)
        {
            var hash = FnvOffsetBasis;
            if (parts == null)
            {
                return hash;
            }

            for (var i = 0; i < parts.Length; i++)
            {
                hash ^= Hash(parts[i]);
                hash *= FnvPrime;
            }

            return hash;
        }

        /// <summary>Index in <c>[0, count)</c> for a seed; <c>0</c> when the range is empty.</summary>
        public static int Index(uint seed, int count)
        {
            return count <= 0 ? 0 : (int)(seed % (uint)count);
        }
    }
}
