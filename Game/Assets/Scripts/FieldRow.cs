using System;
using UnityEngine;

public class FieldRow : MonoBehaviour
{
    public Field[] Fields { get; private set; }

    private void Awake()
    {
        this.Fields = this.GetComponentsInChildren<Field>();
    }
}