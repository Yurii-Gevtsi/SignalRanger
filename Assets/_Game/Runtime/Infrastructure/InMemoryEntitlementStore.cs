using System;
using System.Collections.Generic;

namespace Game.Infrastructure
{
    /// <summary>Deterministic test/local adapter. Replace with an atomic server-backed store before production activation.</summary>
    public sealed class InMemoryEntitlementStore : IEntitlementStore
    {
        private readonly Dictionary<string, Entitlement> _byTransaction =
            new Dictionary<string, Entitlement>(StringComparer.Ordinal);

        public bool HasProcessedTransaction(string transactionId)
        {
            return !string.IsNullOrWhiteSpace(transactionId) && _byTransaction.ContainsKey(transactionId);
        }

        public void Grant(Entitlement entitlement)
        {
            if (string.IsNullOrWhiteSpace(entitlement.TransactionId))
                throw new ArgumentException("Transaction ID is required.", nameof(entitlement));
            if (!_byTransaction.ContainsKey(entitlement.TransactionId))
                _byTransaction.Add(entitlement.TransactionId, entitlement);
        }

        public IReadOnlyList<Entitlement> GetSnapshot(string playerId)
        {
            var result = new List<Entitlement>();
            foreach (var entitlement in _byTransaction.Values)
            {
                if (string.Equals(entitlement.PlayerId, playerId, StringComparison.Ordinal))
                    result.Add(entitlement);
            }
            return result;
        }
    }
}
