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
        // csv�t�@�C���̓ǂݍ���
        TextAsset csv = Resources.Load("test", typeof(TextAsset)) as TextAsset;

        //textData = CSVSerializer.Deserialize<TextData>(csv.text);
    }
}
