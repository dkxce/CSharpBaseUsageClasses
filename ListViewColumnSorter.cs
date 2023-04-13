using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace dkxce
{
    public class ListViewColumnSorter : IComparer
    {
        private ListView lv = null;
        private int column = 1;
        private SortOrder order = SortOrder.Ascending;
        private IComparer customComparer = null;

        public bool Ascending { get { return order == SortOrder.Ascending; } }
        public int Column { get { return column; } }
        public IComparer CustomComparer { get { return customComparer; } set { customComparer = value; } }

        public ListViewColumnSorter(ListView lv)
        {
            this.lv = lv;
            this.lv.ListViewItemSorter = this;
            this.lv.ColumnClick += (delegate (object sender, ColumnClickEventArgs e){ Sort(e.Column); });
        }        

        public int Compare(object x, object y)
        {
            // if (lv.Sorting != SortOrder.Ascending && lv.Sorting != SortOrder.Descending) return 0;

            ListViewItem listviewX = (ListViewItem)x;
            ListViewItem listviewY = (ListViewItem)y;      
            if(customComparer == null)
                return (Ascending ? 1 : -1) * string.Compare(listviewX.SubItems[column].Text, listviewY.SubItems[column].Text);
            else
                return (Ascending ? 1 : -1) * customComparer.Compare(listviewX.SubItems[column].Text, listviewY.SubItems[column].Text);
        }        

        public void Sort(int column = 0)
        {
            if (this.column != column) order = SortOrder.Descending;
            else order = order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            this.column = column;
            DoSort();
        }

        public void Sort(int column = 0, bool asc = true)
        {
            order = asc ? SortOrder.Ascending : SortOrder.Descending;
            this.column = column;
            DoSort();
        }

        private void DoSort()
        {
            this.lv.Sort();
            for (int i = 0; i < this.lv.Columns.Count; i++)
                this.lv.Columns[i].Text = Regex.Replace(this.lv.Columns[i].Text, "[↑↓]", "").Trim();
            if (column >= 0)
                this.lv.Columns[column].Text += order == SortOrder.Ascending ? " ↑" : " ↓";
        }
    }
    
    public class NumberCaseInsensitiveComparer : CaseInsensitiveComparer
    {
        public new int Compare(object x, object y)
        {
            if (x == null && y == null) return 0;
            else if (x == null && y != null) return -1;
            else if (x != null && y == null) return 1;
            
            if ((x is System.String) && IsDecimalNumber((string)x) && (y is System.String) && IsDecimalNumber((string)y))
            {
                try
                {
                    decimal xx = Decimal.Parse(((string)x).Trim());
                    decimal yy = Decimal.Parse(((string)y).Trim());
                    return decimal.Compare(xx, yy);
                }
                catch { return -1; };
            }
            else return base.Compare(x, y);
        }

        private string GetNumberDecimalSeparator()
        {
            return System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        }

        private bool IsDecimalNumber(string strNumber)
        {
            string regex = @"^-?(\d+|(\d{1,3}((,|\.)\d{3})*))((,|\.)\d+)?$";
            Regex wholePattern = new Regex(regex);
            return wholePattern.IsMatch(strNumber);
        }
    }
}
