using Game.Economy;
using TMPro;
using UnityEngine;

namespace Game.App
{
    /// <summary>Keeps one header-currency label synchronized with the economy service.</summary>
    [DisallowMultipleComponent]
    public sealed class CurrencyBalanceText : MonoBehaviour
    {
        [SerializeField] private bool _premium;
        private TMP_Text _label;
        private IEconomyService _economy;

        public static void Bind(TMP_Text label, bool premium)
        {
            if (label == null)
            {
                return;
            }

            var binding = label.GetComponent<CurrencyBalanceText>();
            if (binding == null)
            {
                binding = label.gameObject.AddComponent<CurrencyBalanceText>();
            }

            binding._premium = premium;
            binding.RefreshValue();
        }

        private void OnEnable()
        {
            _label = GetComponent<TMP_Text>();
            _economy = AppServices.Economy;
            if (_economy != null)
            {
                _economy.BalanceChanged += RefreshValue;
            }

            RefreshValue();
        }

        private void OnDisable()
        {
            if (_economy != null)
            {
                _economy.BalanceChanged -= RefreshValue;
                _economy = null;
            }
        }

        private void RefreshValue()
        {
            if (_label == null)
            {
                _label = GetComponent<TMP_Text>();
            }

            if (_label == null || _economy == null)
            {
                return;
            }

            _label.text = (_premium ? _economy.PremiumCurrency : _economy.SoftCurrency).ToString();
        }
    }
}
