using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum SymbolType
{
    Fruits,
    Animals,
    Wild
}

[System.Serializable]
public class SymbolWeight
{
    public SymbolType symbol;
    public int weight;
}


[CreateAssetMenu]
public class SymbolsScriptableObjectScript : ScriptableObject
{
    [Header("Weighted Symbols")]
    public List<SymbolWeight> symbolWeights;
    public Sprite[] animalSymbols;
    public Sprite[] fruitsSymbols;
    public Sprite[] wildSymbols;
}
