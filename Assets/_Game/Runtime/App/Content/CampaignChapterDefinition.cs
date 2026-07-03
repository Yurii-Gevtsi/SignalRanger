using System;
using UnityEngine;

namespace Game.App.Content
{
    [CreateAssetMenu(menuName = "Game/Progression/Campaign Chapter Definition", fileName = "CampaignChapterDefinition")]
    public sealed class CampaignChapterDefinition : ScriptableObject
    {
        [SerializeField] private string _chapterId;
        [SerializeField] private string _displayName;
        [SerializeField] private string _summary;
        [SerializeField] private string[] _missionIds;
        [SerializeField] private string[] _teachingFocusIds;
        [SerializeField] private string[] _featuredWeaponIds;

        public string ChapterId => _chapterId;
        public string DisplayName => _displayName;
        public string Summary => _summary;
        public string[] MissionIds => _missionIds ?? Array.Empty<string>();
        public string[] TeachingFocusIds => _teachingFocusIds ?? Array.Empty<string>();
        public string[] FeaturedWeaponIds => _featuredWeaponIds ?? Array.Empty<string>();

        public static CampaignChapterDefinition CreateRuntime(
            string chapterId,
            string displayName,
            string summary,
            string[] missionIds,
            string[] teachingFocusIds = null,
            string[] featuredWeaponIds = null)
        {
            var definition = CreateInstance<CampaignChapterDefinition>();
            definition._chapterId = chapterId ?? string.Empty;
            definition._displayName = displayName ?? string.Empty;
            definition._summary = summary ?? string.Empty;
            definition._missionIds = missionIds ?? Array.Empty<string>();
            definition._teachingFocusIds = teachingFocusIds ?? Array.Empty<string>();
            definition._featuredWeaponIds = featuredWeaponIds ?? Array.Empty<string>();
            return definition;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_chapterId)
                && !string.IsNullOrWhiteSpace(_displayName)
                && !string.IsNullOrWhiteSpace(_summary)
                && _missionIds != null
                && _missionIds.Length > 0;
        }
    }
}
