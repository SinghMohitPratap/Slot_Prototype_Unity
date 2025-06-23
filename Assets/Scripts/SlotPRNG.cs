using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Symbol
{
    public string name;
    public Sprite sprite;
    public int weight; // For randomness
    public int payout; // For 3-match payout
}

public class SlotPRNG : MonoBehaviour
{
  
    public int rows = 3;
    public int columns = 5;
    

    private List<Symbol> symbolPool;

    [Range(0f, 1f)]
    public float targetRTP = 0.9f;
    public float currentRTP = 0f;
    public float totalWagered = 0;
    public float totalPaidOut = 0;

    public Symbol[,] GenerateSpin(float spinBet)
    {
        Symbol[,] result = new Symbol[rows, columns];
        totalWagered += spinBet;

        EnsureSymbolPool();

        // Fill each cell from weighted symbol pool
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                result[row, col] = GetRandomSymbol();
            }
        }

        int win = CalculateWin(result);
        totalPaidOut += win;

        currentRTP = totalWagered > 0 ? totalPaidOut / totalWagered : 0;

        // Optional: adjust future spins based on how far current RTP is from target RTP

        return result;
    }

    private void EnsureSymbolPool()
    {
        symbolPool = new List<Symbol>();
        foreach (Symbol s in SpinReels.Instance.symbol_SO.symbolList)
        {
            for (int i = 0; i < s.weight; i++)
            {
                symbolPool.Add(s);
            }
        }
    }

    private Symbol GetRandomSymbol()
    {
        return symbolPool[Random.Range(0, symbolPool.Count)];
    }

    private int CalculateWin(Symbol[,] grid)
    {
        int win = 0;

        // Simple horizontal match logic for now
        for (int row = 0; row < rows; row++)
        {
            string matchName = grid[row, 0].name;
            bool match = true;

            for (int col = 1; col < columns; col++)
            {
                if (grid[row, col].name != matchName)
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                win += grid[row, 0].payout;
            }
        }

        // You can expand this to vertical, diagonal, 2-of-a-kind, wild logic, etc.
        return win;
    }
}
