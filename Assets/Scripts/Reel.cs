using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Reel : MonoBehaviour
{
    public RectTransform content;
    internal float spinSpeed = 1000f;
    internal float spinDuration = 2f;
    internal Sprite[] symbols;
    internal GameObject symbolPrefab;
    internal int visibleCount = 3; // How many symbols visible

    private bool isSpinning = false;


    private void Start()
    {
        InitReel();
    }
    public void InitReel()
    {
        // Clear old
        foreach (Transform child in content)
            Destroy(child.gameObject);

        // Add extra symbols to create loop illusion
        for (int i = 0; i < visibleCount + 5; i++)
        {
            GameObject go = Instantiate(symbolPrefab, content);
            go.GetComponent<Image>().sprite = symbols[Random.Range(0, symbols.Length)];
        }
        float symbolHeight = ((RectTransform)content.GetChild(0)).rect.height;
        float offset = content.anchoredPosition.y % symbolHeight;
        content.anchoredPosition -= new Vector2(0, offset);
    }


    

    public void StartSpin()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinRoutine());
        }
    }

    IEnumerator SpinRoutine()
    {
        isSpinning = true;
        float elapsedTime = 0f;

        float symbolHeight = ((RectTransform)content.GetChild(0)).rect.height;
        int symbolCount = content.childCount;

        while (elapsedTime < spinDuration)
        {
            float moveY = spinSpeed * Time.deltaTime;
            content.anchoredPosition += new Vector2(0, -moveY);

            // Looping logic: when the content position passes the height of one symbol
            if (Mathf.Abs(content.anchoredPosition.y) >= symbolHeight)
            {
                Debug.Log("anchored Position " + content.anchoredPosition.y);
                content.anchoredPosition += new Vector2(0, symbolHeight);

                // Move bottom-most symbol to the top
                Transform bottom = content.GetChild(0);
                bottom.SetAsLastSibling();
                bottom.GetComponent<Image>().sprite = symbols[Random.Range(0, symbols.Length)];
            }
        
            elapsedTime += Time.deltaTime;
            yield return null;
        }
     
        float offset = content.anchoredPosition.y % symbolHeight;
        content.anchoredPosition -= new Vector2(0, offset);
    }

    
}
