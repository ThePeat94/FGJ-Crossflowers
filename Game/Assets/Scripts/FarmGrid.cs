using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FarmGrid : MonoBehaviour
    {
        [SerializeField]
        private Field[] m_row0;
        
        [SerializeField]
        private Field[] m_row1;
        
        [SerializeField]
        private Field[] m_row2;
        
        [SerializeField]
        private Field[] m_row3;

        private Field[][] m_grid;

        private void Awake()
        {
            this.m_grid = new Field[4][];

            this.m_grid[0] = this.m_row0;
            this.m_grid[1] = this.m_row1;
            this.m_grid[2] = this.m_row2;
            this.m_grid[3] = this.m_row3;
        }
        
        
    }
}