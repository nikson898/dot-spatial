using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Controls.Header
{
    public partial class GridControlPanel : UserControl
    {
        #region ---------------- Class Variables

        private TableLayoutPanel _tableLayoutPanel;
        private SplitContainer _splitContainer;

        #endregion

        #region ---------------- Public Properties

        public TableLayoutPanel TableLayoutPanel
        {
            get
            {
                return _tableLayoutPanel;
            }
            set
            {
                if (value == null) return;
                _tableLayoutPanel = value;
            }
        }

        #endregion

        #region ---------------- Public Methods

        public GridControlPanel()
        {
            InitializeComponent();
            _tableLayoutPanel = this.tableLayoutPanel1;
        }

        public void AddControl(ToolStrip ctrl, short order)
        {
            //var sortOrder = order;
            //if (sortOrder == null)
            //{
            //    sortOrder = (short)_tableLayoutPanel.Controls.Count;
            //}
            //AddControl(ctrl, sortOrder);
        }

        public void AddControl(ToolStrip ctrl)
        {
            ctrl.Anchor = (AnchorStyles.Top | AnchorStyles.Left);



            if (this._tableLayoutPanel.Controls.Count == 0)
            {
                // set the split container now from the parents of the control
                _splitContainer = (SplitContainer)this.Parent.Controls.Owner.Parent;
                // go ahead and generate a column and row to store the tool
                TableLayoutPanelCellPosition pos = new TableLayoutPanelCellPosition(0,0);
                this._tableLayoutPanel.SetCellPosition(ctrl, pos);
               
                this._tableLayoutPanel.Controls.Add(ctrl);
                // go ahead and set the size of the splitcontainer now
                SplitContainer splitContainer = (SplitContainer)this.Parent.Controls.Owner.Parent;
                splitContainer.SplitterDistance = _tableLayoutPanel.Width;
            }
            else
            {


            }
            
        }

        #endregion

    }
}
