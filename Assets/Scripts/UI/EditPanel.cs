using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditPanel : MonoBehaviour
{
    public Button moveButton;
    public Button rotateButton;
    public Button snapButton;
    public Button deleteButton;

    private void SetColor(ref ColorBlock colors, Color color)
    {
        colors.normalColor = color;
        colors.highlightedColor = color;
        colors.selectedColor = color;
        colors.pressedColor = color;    
    }
    
    public void SetMoveMode()
    {
        ColorBlock colors = moveButton.colors;
        SetColor(ref colors, Color.yellow); 
        moveButton.colors = colors;
        colors = rotateButton.colors;
        SetColor(ref colors, Color.white); 
        rotateButton.colors = colors;
    }
    
    public void SetRotateMode()
    {
        ColorBlock colors = rotateButton.colors;
        SetColor(ref colors, Color.yellow); 
        rotateButton.colors = colors;
        colors = moveButton.colors;
        SetColor(ref colors, Color.white); 
        moveButton.colors = colors;
    }

    public void SetSnap(bool doSnap)
    {
        if (doSnap)
        {
            ColorBlock colors = snapButton.colors;
            SetColor(ref colors, Color.green); 
            snapButton.colors = colors;
        }
        else
        {
            ColorBlock colors = snapButton.colors;
            SetColor(ref colors, Color.white);  
            snapButton.colors = colors;
        }
    }

    public void Setup(bool doSnap, bool allowDelete)
    {
        SetMoveMode();
        SetSnap(doSnap);
        if (!allowDelete) deleteButton.interactable = false;
        
    } 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
