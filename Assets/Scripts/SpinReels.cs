using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SpinReels : MonoBehaviour
{
    [SerializeField]
    Transform reelContainer;
    Reel[] reelsArray;
    public float spinSpeed = 1000f;
    public float spinDuration = 2f;

    public GameObject symbolPrefab;
    public int visibleCount = 3; // How many symbols visible
    public List<Sprite> symbolPool;
    public SymbolsScriptableObjectScript symbol_SO;


   
    public bool boostMatch;
    internal bool isMatchDone;

    private static SpinReels instance;
    public static SpinReels Instance { get { return instance; } private set { }  }

    private SpinReels() { }

    public SlotPRNG prng_instance;

    //work needs to be done..
    Symbol[,] symbolMatrix;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
    public float spinBet;
    private void Start()
    {
     
       

        reelsArray = new Reel[reelContainer.childCount];

        symbolPool = symbol_SO.symbolList.Select(x => x.sprite).ToList();
        for (int i = 0; i < reelsArray.Length; i++) 
        {
            reelsArray[i] = reelContainer.GetChild(i).GetComponent<Reel>();
            reelContainer.GetChild(i).GetComponent<Reel>().spinSpeed= spinSpeed;
            reelContainer.GetChild(i).GetComponent<Reel>().spinDuration= spinDuration;
            reelContainer.GetChild(i).GetComponent<Reel>().symbols = symbolPool.ToArray();
            reelContainer.GetChild(i).GetComponent<Reel>().symbolPrefab = symbolPrefab;
            reelContainer.GetChild(i).GetComponent<Reel>().visibleCount= visibleCount;
   
        }
        
    }


    void SetSpinValues() 
    {
        symbolMatrix = prng_instance.GenerateSpin(spinBet);
      
        for (int i = 0; i < reelsArray.Length; i++)
        {           
            reelContainer.GetChild(i).GetComponent<Reel>().SetColResults(SetReelResult(i));
        }
    }

    Sprite[] SetReelResult(int i) 
    {    
        Sprite[] rows = new Sprite[symbolMatrix.GetLength(0)];
        for (int j = 0; j < symbolMatrix.GetLength(0); j++)
        {
            rows[j] = symbolMatrix[j, i].sprite;
        }
        return rows;
    }

    public void OnClickSpinReelBtn() 
    {
        SetSpinValues();
        for (int i = 0; i < reelsArray.Length; i++) 
        {
            reelsArray[i].StartSpin();
        }
    }



}
