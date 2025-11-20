using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _PROJECT_.GP.Scripts.HUD
{
    public class HudManager : MonoBehaviour
    {
        public CrossHairsManager _crossHairsManager;
        public InfoTip _infoTip;

        public void ShowHideInfo(InputAction.CallbackContext context)
        {
            _infoTip.SwitchInfoState();
        }
    }
}
