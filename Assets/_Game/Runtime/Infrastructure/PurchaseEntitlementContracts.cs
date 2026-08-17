using System;
using System.Collections.Generic;

namespace Game.Infrastructure
{
    public readonly struct ReceiptValidationRequest
    {
        public ReceiptValidationRequest(string playerId, string platform, string productId,
            string transactionId, string receiptPayload, DateTime purchasedAtUtc)
        {
            PlayerId = playerId ?? string.Empty;
            Platform = platform ?? string.Empty;
            ProductId = productId ?? string.Empty;
            TransactionId = transactionId ?? string.Empty;
            ReceiptPayload = receiptPayload ?? string.Empty;
            PurchasedAtUtc = purchasedAtUtc.ToUniversalTime();
        }

        public string PlayerId { get; }
        public string Platform { get; }
        public string ProductId { get; }
        public string TransactionId { get; }
        public string ReceiptPayload { get; }
        public DateTime PurchasedAtUtc { get; }
    }

    public readonly struct ReceiptValidationResult
    {
        private ReceiptValidationResult(bool isValid, string transactionId, string message)
        {
            IsValid = isValid;
            TransactionId = transactionId ?? string.Empty;
            Message = message ?? string.Empty;
        }

        public bool IsValid { get; }
        public string TransactionId { get; }
        public string Message { get; }

        public static ReceiptValidationResult Valid(string transactionId) =>
            new ReceiptValidationResult(true, transactionId, string.Empty);

        public static ReceiptValidationResult Invalid(string message) =>
            new ReceiptValidationResult(false, string.Empty, message);
    }

    public interface IReceiptValidator
    {
        ReceiptValidationResult Validate(ReceiptValidationRequest request);
    }

    public readonly struct Entitlement
    {
        public Entitlement(string playerId, string productId, string transactionId, DateTime grantedAtUtc, bool active)
        {
            PlayerId = playerId ?? string.Empty;
            ProductId = productId ?? string.Empty;
            TransactionId = transactionId ?? string.Empty;
            GrantedAtUtc = grantedAtUtc.ToUniversalTime();
            IsActive = active;
        }

        public string PlayerId { get; }
        public string ProductId { get; }
        public string TransactionId { get; }
        public DateTime GrantedAtUtc { get; }
        public bool IsActive { get; }
    }

    public interface IEntitlementStore
    {
        bool HasProcessedTransaction(string transactionId);
        void Grant(Entitlement entitlement);
        IReadOnlyList<Entitlement> GetSnapshot(string playerId);
    }

    public readonly struct PurchaseProcessingResult
    {
        public PurchaseProcessingResult(bool success, bool alreadyProcessed, string transactionId, string message)
        {
            Success = success;
            AlreadyProcessed = alreadyProcessed;
            TransactionId = transactionId ?? string.Empty;
            Message = message ?? string.Empty;
        }

        public bool Success { get; }
        public bool AlreadyProcessed { get; }
        public string TransactionId { get; }
        public string Message { get; }
    }

    /// <summary>Provider-agnostic grant boundary. The store must atomically deduplicate transaction IDs.</summary>
    public sealed class PurchaseEntitlementProcessor
    {
        private readonly IReceiptValidator _validator;
        private readonly IEntitlementStore _store;

        public PurchaseEntitlementProcessor(IReceiptValidator validator, IEntitlementStore store)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public PurchaseProcessingResult Process(ReceiptValidationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.TransactionId) || string.IsNullOrWhiteSpace(request.ProductId))
                return new PurchaseProcessingResult(false, false, request.TransactionId, "Transaction and product IDs are required.");

            if (_store.HasProcessedTransaction(request.TransactionId))
                return new PurchaseProcessingResult(true, true, request.TransactionId, "Transaction was already processed.");

            if (!MonetizationProductCatalog.TryGet(request.ProductId, out _))
                return new PurchaseProcessingResult(false, false, request.TransactionId, "Unknown product.");

            var validation = _validator.Validate(request);
            if (!validation.IsValid || !string.Equals(validation.TransactionId, request.TransactionId, StringComparison.Ordinal))
                return new PurchaseProcessingResult(false, false, request.TransactionId, validation.Message);

            _store.Grant(new Entitlement(request.PlayerId, request.ProductId, request.TransactionId, DateTime.UtcNow, true));
            return new PurchaseProcessingResult(true, false, request.TransactionId, string.Empty);
        }
    }
}
