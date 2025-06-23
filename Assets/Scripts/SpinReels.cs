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
    Symbol[,] resultMatrix;
    private void Start()
    {
        resultMatrix = new Symbol[prng_instance.rows, prng_instance.columns];



        reelsArray = new Reel[reelContainer.childCount];

     
        for (int i = 0; i < reelsArray.Length; i++) 
        {
            reelsArray[i] = reelContainer.GetChild(i).GetComponent<Reel>();
            reelContainer.GetChild(i).GetComponent<Reel>().spinSpeed= spinSpeed;
            reelContainer.GetChild(i).GetComponent<Reel>().spinDuration= spinDuration;
           
            reelContainer.GetChild(i).GetComponent<Reel>().symbolPrefab = symbolPrefab;
            reelContainer.GetChild(i).GetComponent<Reel>().visibleCount= visibleCount;
   
        }
       // SetSpinValues();
    }


    void SetSpinValues() 
    {
        symbolMatrix = prng_instance.GenerateSpin(spinBet);
        
        for (int i = 0; i < reelsArray.Length; i++)
        {
            Symbol[] reelArray = new Symbol[symbolMatrix.GetLength(0)];
            for (int j = 0; j < reelArray.Length; j++)
            {
                reelArray[j] = symbolMatrix[j, i];
            }
            reelContainer.GetChild(i).GetComponent<Reel>().symbols = reelArray;
            reelContainer.GetChild(i).GetComponent<Reel>().InitReel();
        }
    }


    public void OnClickSpinReelBtn() 
    {
        SetSpinValues();
        counter = 0;
        for (int i = 0; i < reelsArray.Length; i++) 
        {
            reelsArray[i].StartSpin();
        }
    }
    int counter;
    public void CreateResultMatrix(Symbol[] reelData,int columnIndex) 
    {
        Debug.Log($"Index Value: {columnIndex}");
        for (int i = 0; i < reelData.Length; i++) 
        {
            resultMatrix[i,columnIndex] = reelData[i];       
        }
        counter++;
        if (counter == prng_instance.columns) 
        {
            prng_instance.CalculateWin(resultMatrix);
        }
    }
}
