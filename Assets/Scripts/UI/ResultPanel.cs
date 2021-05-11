using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Play()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().SetBool("open", true);
    }

    public void Close()
    {
        GetComponent<Animator>().SetBool("open", false);
        gameObject.SetActive(false);
    }
}
