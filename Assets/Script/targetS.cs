using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetS : MonoBehaviour
{
    float sin;
    float timeElaps;
    public bool bas;

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!bas)
        {
            timeElaps += Time.deltaTime * 0.08f;
            sin = Mathf.Abs(Mathf.Sin(timeElaps));
            transform.position = new Vector3(Mathf.Lerp(7, -7, sin), 3.24f, 0);
        }

    }

}
