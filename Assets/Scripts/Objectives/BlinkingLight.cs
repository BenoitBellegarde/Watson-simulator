using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    private Light blinkingLight;
    public float duration = 0.5f;     // The total of seconds the flash wil last
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        blinkingLight = GetComponent<Light>();

        StartCoroutine(FlashNow());

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FlashNow()
    {
        while (true)
        {
            blinkingLight.enabled = !blinkingLight.enabled;
            yield return new WaitForSeconds(duration);
        }

    }
}
