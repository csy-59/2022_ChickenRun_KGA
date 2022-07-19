using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameManager.PlatformShape shape = GameManager.PlatformShape.CIRCLE;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        GameManager.Instance.OnShapeChange.AddListener(Sink);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Sink(GameManager.PlatformShape selectedShape)
    {

    }
}
