using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsList : MonoBehaviour
{
    public GameObject buttonGameObject;
    public GameManager gameManager;
    // Start is called before the first frame update
    
    public void Init()
    {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        float panelWidth = gameObject.GetComponent<RectTransform>().sizeDelta.x;
        // float buttonWidth = buttonGameObject.GetComponent<RectTransform>().sizeDelta.x;
        int buttonNum = SceneManager.sceneCountInBuildSettings - 2;
        float step = panelWidth / (buttonNum + 1);
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings - 1; i++)
        {
            GameObject newButton = Instantiate(buttonGameObject, gameObject.transform, false) as GameObject;
            newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * step, 0) ;
            newButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(i.ToString());
            Button buttonComponent = newButton.GetComponent<Button>();
            var i1 = i;
            buttonComponent.onClick.AddListener((
                delegate { gameManager.LoadLevel(i1); }));
            if (gameManager.IsLevelCompleted(i))
            {
                ColorBlock colors = buttonComponent.colors;
                colors.normalColor = Color.green;
                buttonComponent.colors = colors;
            }
            else if (gameManager.IsLevelOpened(i))
            {
                ColorBlock colors = buttonComponent.colors;
                colors.normalColor = Color.yellow;
                buttonComponent.colors = colors;
            }
            else
            {
                buttonComponent.interactable = false;
            }
        }
    }
}
