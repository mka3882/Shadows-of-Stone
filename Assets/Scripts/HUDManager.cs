using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Vector2 keyStartPos;
    public GameObject hudKey;
    int numKey = 0;
    int offset = 55;
    private ArrayList keys = new ArrayList();
    public Slider burstBar;
    private float currentEnergy;
    private float maxEnergy;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        keyStartPos = new Vector2(-365, 5);
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            currentEnergy = player.Energy;
            maxEnergy = player.MaxEnergy;

            burstBar.maxValue = maxEnergy;
            burstBar.minValue = 0;
            burstBar.value = currentEnergy;
        }
    }

    public void AcquireKey()
    {
        Vector2 pos = keyStartPos;
        keyStartPos.x += numKey * offset;
        GameObject key = Instantiate(hudKey);
        key.transform.parent = transform;
        key.GetComponent<RectTransform>().parent = transform;
        key.GetComponent<RectTransform>().localPosition = keyStartPos;
        keys.Add(key);
        numKey++;
       // key.transform.position = keyStartPos;
    }

    public void RemoveKey()
    {
        if (keys.Count > 0) {
            GameObject key = (GameObject)keys[keys.Count - 1];
            keys.RemoveAt(keys.Count - 1);
            Destroy(key);
        }
    }

    public bool HasKey()
    {
        if(keys.Count > 0)
        {
            return true;
        }
        return false;
    }
}
