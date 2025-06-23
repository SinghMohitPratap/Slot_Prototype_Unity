using System.Collections;
using System.Collections.Generic;
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


    public Sprite[,] resultMatrix = new Sprite[3, 5]; // 3 rows, 5 columns
    public bool boostMatch;
   internal bool isMatchDone;

    private static SpinReels instance;
    public static SpinReels Instance { get { return instance; } private set { }  }

    private SpinReels() { }



    //work needs to be done..
    Sprite[,] symbolMatrix = new Sprite[3, 5];

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

    private void Start()
    {
        isMatchDone = false;


        // 7 Fruit symbols - more frequent
        for (int i = 0; i < symbol_SO.fruitsSymbols.Length; i++)
            for (int j = 0; j < 5; j++) // 5 entries per fruit
                symbolPool.Add(symbol_SO.fruitsSymbols[i]);

        // 8 Animal symbols - medium frequency
        for (int i = 0; i < symbol_SO.animalSymbols.Length; i++)
            for (int j = 0; j < 3; j++) // 3 entries per animal
                symbolPool.Add(symbol_SO.animalSymbols[i]);

        // 1 Wild symbol - very rare
        for (int j = 0; j < symbol_SO.wildSymbols.Length; j++)
            symbolPool.Add(symbol_SO.wildSymbols[j]);

       


        reelsArray = new Reel[reelContainer.childCount];
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
    public void OnClickSpinReelBtn() 
    {
        for (int i = 0; i < reelsArray.Length; i++) 
        {
            reelsArray[i].StartSpin();
        }
    }



}
