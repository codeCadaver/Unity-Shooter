using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer3D : MonoBehaviour
{
    public string sortingLayerName = "";
    public int sortingOrder = 0;

    private Renderer _renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();

        _renderer.sortingLayerName = sortingLayerName;
        _renderer.sortingOrder = sortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
