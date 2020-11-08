using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Farm : MonoBehaviour
{
    private static Farm s_instance;
    
    [SerializeField] private Field[] m_fields;

    [SerializeField] private int m_rows;
    [SerializeField] private int m_columns;

    [SerializeField] private GameObject m_fieldPrefab;

    private int m_currentIndex = 0;

    private Field[][] m_fieldGrid;
    private Field[] m_flattenedGrid;

    public static Farm Instance => s_instance ?? FindObjectOfType<Farm>();
    
    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        var fieldRows = this.GetComponentsInChildren<FieldRow>();
        this.m_fieldGrid = new Field[fieldRows.Length][];
        for (int i = 0; i < this.m_fieldGrid.Length; i++)
        {
            this.m_fieldGrid[i] = fieldRows[i].Fields;
        }
        this.m_flattenedGrid = this.m_fieldGrid.SelectMany(f => f).ToArray();
    }

    public void UnlockNextFarm()
    {
        this.m_fields[this.m_currentIndex].Unlock();
        this.m_currentIndex++;
    }

    public List<Field> GetSurroundingFieldsForField(Field field)
    {
        var flatIndex = Array.IndexOf(this.m_flattenedGrid, field);
        var rowIndex = flatIndex / this.m_fieldGrid.Length;
        var columnIndex = flatIndex % this.m_columns;

        var clampedMinRowIndex = Mathf.Clamp(rowIndex - 1, 0, this.m_rows - 1);
        var clampedMaxRowIndex = Mathf.Clamp(rowIndex + 1, 0, this.m_rows - 1);
        var clampedMinColIndex = Mathf.Clamp(columnIndex - 1, 0, this.m_rows - 1);
        var clampedMaxColIndex = Mathf.Clamp(columnIndex + 1, 0, this.m_rows - 1);


        var surroundingFields = new List<Field>();
        for (int row = clampedMinRowIndex; row <= clampedMaxRowIndex; row++)
        {
            for (int col = clampedMinColIndex; col <= clampedMaxColIndex; col++)
            {
                var found = this.m_fieldGrid[row][col];
                if(found != field)
                    surroundingFields.Add(found);
            }
        }

        return surroundingFields;
    }
}