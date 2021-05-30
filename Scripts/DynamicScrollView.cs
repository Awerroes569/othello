using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DynamicScrollView : MonoBehaviour
{
    public GameObject Prefab;
    public Transform Container;
    public List<string> files = new List<string>();

    void Start()
    {
        files = SaveData.GetFilesOfType();

        for (int i = 0; i < files.Count; i++)
        {
            Debug.Log("ok nr "+i);
            GameObject go = Instantiate(Prefab);
            go.GetComponentInChildren<Text>().text = files[i];
            go.transform.SetParent(Container);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            int buttonIndex = i;
            go.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(buttonIndex));
        }
        Canvas.ForceUpdateCanvases();
    }



    public void OnButtonClick(int index)
    {
        string file = files[index];

        Generals.nameOfLoadedFile = file;

        Debug.Log(file);
        // Process file here...
        SceneManager.LoadScene("HH_load");
    }
}
