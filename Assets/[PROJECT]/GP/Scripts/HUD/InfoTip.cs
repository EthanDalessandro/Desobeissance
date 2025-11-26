using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _PROJECT_.GP.Scripts.HUD
{
    /// <summary>
    /// Displays and toggles informational text on the HUD.
    /// </summary>
    public class InfoTip : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _infoText;

        [Header("Information")]
        [SerializeField, TextArea] private string _info;

        private bool _isActive;

        private void Awake()
        {
            UpdateTextInfo();
        }
        public void SwitchInfoState()
        {
            _isActive = !_isActive;
            _infoText.DOKill();
            int a = _isActive ? 0 : 1;

            _infoText.DOFade(a, 0.25f);
        }
        private void UpdateTextInfo()
        {
            _infoText.text = _info;
        }
    }
}
