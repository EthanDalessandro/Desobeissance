using System.Collections.Generic;
using UnityEngine;

namespace _PROJECT_.GP.Scripts.ScriptablesObjects
{
    [CreateAssetMenu(fileName = "New BoucleGameplayObjects", menuName = "ScriptableObjects/BoucleGameplayObjects")]
    /// <summary>
    /// ScriptableObject that holds data for a gameplay loop or scene configuration.
    /// Stores a list of objects associated with a specific scene number.
    /// </summary>
    public class BoucleGameplayObjects : ScriptableObject
    {
        public int _sceneNumber;
        [Header("Scene Objects")]
        public List<GameObject> _sceneObjects;
    
    }
}