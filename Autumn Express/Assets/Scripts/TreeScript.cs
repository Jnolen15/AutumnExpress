using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public GameObject sprite;

    private GameObject lookPoint;

    void Start()
    {
        lookPoint = GameObject.FindGameObjectWithTag("LookPoint");
        RandomizeTree();
    }

    void Update()
    {
        LookTo(lookPoint.transform);
    }

    // Performs LookAt locked to the Y axis
    private void LookTo(Transform target)
    {
        Vector3 targetPos = new Vector3(target.position.x, sprite.transform.localPosition.y, target.position.z);
        sprite.transform.LookAt(targetPos, Vector3.up);
    }

    private void RandomizeTree()
    {
        var rand = Random.Range(1, 11);
        if (rand > 2) // Stays as tree
        {
            var randx = Random.Range(0.5f, 0.7f);
            var randy = Random.Range(0.5f, 0.7f);
            sprite.transform.localScale = new Vector3(randx, randy, 0);
        }
        else // Becomes bush
        {
            var randx = Random.Range(0.1f, 0.2f);
            var randy = Random.Range(0.1f, 0.2f);
            sprite.transform.localScale = new Vector3(randx, randy, 0);
            sprite.transform.localPosition = new Vector3(0, -0.7f, 0);
        }
    }
}
