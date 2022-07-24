using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonBehaviour<GameManager>
{
    public float RowMoveSpeed = 1f;
    public float RowPositionOffset = 1.5f;

    public enum PlatformShape
    {
        CIRCLE,
        TRIANGLE,
        SQUARE
    }
    public const int PlatformShapeCount = 3;

    public UnityEvent<PlatformShape> OnShapeChange = new UnityEvent<PlatformShape>();
    private static PlatformShape safeShape;
    public static PlatformShape Shape
    {
        get { return safeShape; }
        set
        {
            safeShape = value;
            GameManager.Instance.OnShapeChange.Invoke(safeShape);
        }
    }

    public UnityEvent OnRowMove = new UnityEvent();

    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RowMove());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PickShape()
    {
        Shape = (PlatformShape) Random.Range(0, PlatformShapeCount);
    }

    private IEnumerator RowMove()
    {
        while(!isGameOver)
        {
            yield return new WaitForSeconds(RowPositionOffset / RowMoveSpeed + 0.1f);
            OnRowMove.Invoke();
            yield return new WaitForSeconds(RowPositionOffset / RowMoveSpeed + 0.1f);
        }
    }
}
