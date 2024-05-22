using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnerData
{

}


public class SpawnerManager : MonoBehaviour
{
    //public TextData[] textData;

    // Start is called before the first frame update
    void Start()
    {
        // csvƒtƒ@ƒCƒ‹‚Ì“Ç‚İ‚İ
        TextAsset csv = Resources.Load("test", typeof(TextAsset)) as TextAsset;

        //textData = CSVSerializer.Deserialize<TextData>(csv.text);
    }
}
