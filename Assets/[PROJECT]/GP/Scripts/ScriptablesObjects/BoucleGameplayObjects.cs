using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BoucleGameplayObjects", menuName = "ScriptableObjects/BoucleGameplayObjects")]
public class BoucleGameplayObjects : ScriptableObject
{
    public int _sceneNumber;
    [Header("Scene Objects")]
    public List<GameObject> _sceneObjects;
    
}