// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridHeaderControl.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// Sample implementation of grid toolbar header.
    /// </summary>
    public class GridHeaderControl : HeaderControl
    {
        #region Constants and Fields

        private const string STR_DefaultGroupName = "Default Group";
        private TableLayoutPanel _Panel;
        private MenuStrip _MenuStrip;
        private List<ToolStrip> _Strips;
        private Dictionary<ToolStrip, int> _GridStrips;
        private SplitContainer _SplitContainer;
        private int _SplitterDistance;
        // invalidation flag to only invalidate onmoved if a user moved the splitter
        private bool _SplitterInvalidate = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Initialize(TableLayoutPanel panel)
        {
            this._Panel = panel;
            this._SplitContainer = (SplitContainer)panel.Parent.Controls.Owner.Parent;
            this._SplitContainer.SplitterMoved += Splitter_Moved;
            this._SplitContainer.SplitterMoving += Splitter_Moving;
            this._SplitContainer.DoubleClick += Splitter_DoubleClick;
            this._SplitContainer.Paint += Splitter_Paint;
            this._SplitterDistance = this._SplitContainer.SplitterDistance;

            // create the menu strip.
            MenuStrip strip = new MenuStrip();
            strip.Name = STR_DefaultGroupName;
            strip.Dock = DockStyle.Top;

            // add the menu to the form so that it appears on top of all the toolbars.
            _SplitContainer.Parent.Controls.Add(strip);
            // dict used for sorting all the grid panel strips
            this._GridStrips = new Dictionary<ToolStrip, int>();
            // list for sorting all root level strips
            this._Strips = new List<ToolStrip>();
            this._Strips.Add(strip);
            this._MenuStrip = strip;

            this._MenuStrip.ItemClicked += MenuStrip_ItemClicked;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public override void Add(MenuContainerItem item)
        {
            ToolStripMenuItem menu = new ToolStripMenuItem(item.Caption);
            menu.Name = item.Key;

            var root = this._MenuStrip.Items[item.RootKey] as ToolStripDropDownButton;
            if (root != null)
            {
                root.DropDownItems.Add(menu);
                root.Visible = true;
            }
        }

        /// <summary>
        /// Adds the separator.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Add(SeparatorItem item)
        {
            ToolStripSeparator separator = new ToolStripSeparator();
            ToolStrip strip = this.GetOrCreateStrip(item.GroupCaption);

            if (strip != null)
            {
                strip.Items.Add(separator);
            }
        }

        /// <summary>
        /// Adds the specified root item.
        /// </summary>
        /// <param name="item">
        /// The root item.
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void Add(RootItem item)
        {
            // The root may have already been created.
            var root = this._MenuStrip.Items[item.Key] as ToolStripDropDownButton;
            if (root == null)
            {
                // if not we need to create it.
                CreateToolStripDropDownButton(item);
            }
            else
            {
                // We have already created the RootItem in anticipation of it being needed. (As it was specified by some HeaderItem already)
                // Update the caption and sort order of the root.
                root.Text = item.Caption;
                root.MergeIndex = item.SortOrder;
            }
            RefreshRootItemOrder();
        }

        /// <summary>
        /// This will add a new item that will appear on the standard toolbar or ribbon control.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void Add(SimpleActionItem item)
        {
            ToolStripItem menu;

            if (IsForMenuStrip(item) || item.GroupCaption == ApplicationMenuKey)
            {
                // add it to the menu bar
                menu = new ToolStripMenuItem(item.Caption);
                menu.Image = item.SmallImage;
            }
            else
            {
                // create a tool strip and add it as a button
                menu = new ToolStripButton(item.Caption);
                menu.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                menu.TextImageRelation = TextImageRelation.ImageAboveText;
                menu.ImageAlign = ContentAlignment.MiddleCenter;
                menu.Image = item.LargeImage;
                menu.Text = item.Caption;
                menu.TextAlign = ContentAlignment.BottomCenter;
                menu.AutoSize = false;
                menu.Height = 50;
                menu.Width = 60;

                //  we're grouping all Toggle buttons together into the same group.
                //if (item.ToggleGroupKey != null)
                //{
                //    ToolStripButton button = menu as ToolStripButton;
                //    button.CheckOnClick = true;
                //    button.CheckedChanged += this.button_CheckedChanged;

                //    item.Toggling += (sender, e) =>
                //    {
                //        UncheckButtonsExcept(button);
                //        button.Checked = !button.Checked;
                //    };
                //}
            }

            menu.Name = item.Key;
            menu.Enabled = item.Enabled;
            menu.Visible = item.Visible;
            menu.Click += (sender, e) => item.OnClick(e);

            EnsureNonNullRoot(item);
            var root = this._MenuStrip.Items[item.RootKey] as ToolStripDropDownButton;
            if (root == null)
            {
                // Temporarily create the root.
                root = CreateToolStripDropDownButton(new RootItem(item.RootKey, "AddRootItemWithKey " + item.RootKey));
            }

            if (item.MenuContainerKey == null)
            {
                if (IsForMenuStrip(item) || item.GroupCaption == ApplicationMenuKey || item.GroupCaption == null)
                {
                    root.DropDownItems.Add(menu);
                    root.Visible = true;
                }
                else
                {
                    // find or assemble the strip that the item is attached to
                    int position = item.SortOrder;
                    ToolStrip strip = this.GetOrCreateStrip(item.GroupCaption);
                    if (strip != null)
                    {
                        strip.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                        strip.GripStyle = ToolStripGripStyle.Hidden;
                        strip.Items.Add(menu);
                        strip.Dock = DockStyle.None;
                        strip.Items.Add(menu);
                    }
                    if (String.IsNullOrWhiteSpace(item.ToolTipText) == false)
                    {
                        menu.ToolTipText = item.ToolTipText;
                    }
                    else
                    {
                        menu.ToolTipText = item.Caption;
                    }
                    if (_GridStrips.ContainsValue(position) || position == 0)
                    {
                        position = this.GetNewGridOrderPosition();
                    }
                    // add the strip to our sort tracking dict
                    if (strip != null) _GridStrips.Add(strip, position);
                    RefreshGridControlOrder();
                }
            }
            else
            {
                var subMenu = root.DropDownItems[item.MenuContainerKey] as ToolStripMenuItem;
                if (subMenu != null)
                {
                    subMenu.DropDownItems.Add(menu);
                }
            }
            item.PropertyChanged += SimpleActionItem_PropertyChanged;
        }

        /// <summary>
        /// Adds a combo box style item
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Add(DropDownActionItem item)
        {
            ToolStrip strip = this.GetOrCreateStrip(item.GroupCaption);
            var combo = new ToolStripComboBox(item.Key);

            ParseAllowEditingProperty(item, combo);

            combo.ToolTipText = item.Caption;
            if (item.Width != 0)
            {
                combo.Width = item.Width;
            }

            combo.Items.AddRange(item.Items.ToArray());
            combo.SelectedIndexChanged += delegate
            {
                item.PropertyChanged -= DropDownActionItem_PropertyChanged;
                item.SelectedItem = combo.SelectedItem;
                item.PropertyChanged += DropDownActionItem_PropertyChanged;
            };

            if (strip != null)
            {
                strip.Items.Add(combo);
            }

            item.PropertyChanged += DropDownActionItem_PropertyChanged;
        }

        /// <summary>
        /// Adds the specified textbox item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Add(TextEntryActionItem item)
        {
            ToolStrip strip = this.GetOrCreateStrip(item.GroupCaption);
            var textBox = new ToolStripTextBox(item.Key);
            if (item.Width != 0)
            {
                textBox.Width = item.Width;
            }

            textBox.TextChanged += delegate
            {
                item.PropertyChanged -= TextEntryActionItem_PropertyChanged;
                item.Text = textBox.Text;
                item.PropertyChanged += TextEntryActionItem_PropertyChanged;
            };
            if (strip != null)
            {
                strip.Items.Add(textBox);
            }

            item.PropertyChanged += TextEntryActionItem_PropertyChanged;
        }

        /// <summary>
        /// Remove item from the standard toolbar or ribbon control
        /// </summary>
        /// <param name="key">
        /// The string itemName to remove from the standard toolbar or ribbon control
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void Remove(string key)
        {
            var item = this.GetItem(key);
            if (item != null)
            {
                ToolStrip toolStrip = item.Owner;
                _GridStrips.Remove(toolStrip);
                item.Dispose();
                if (toolStrip.Items.Count == 0)
                {
                    _Strips.Remove(toolStrip);
                    toolStrip.Dispose();
                }
            }
            base.Remove(key);
            RefreshGridControlOrder();
        }

        /// <summary>
        /// Selects the root. (Does nothing.)
        /// </summary>
        /// <param name="key">The key.</param>
        public override void SelectRoot(string key)
        {
            // we won't do anything here.
        }

        #endregion

        #region Methods

        private static void PaintSplitterDots(SplitContainer sc, PaintEventArgs e)
        {
            var control = sc;
            // paint the three dots'
            Point[] points = new Point[3];
            var w = control.Width;
            var h = control.Height;
            var d = control.SplitterDistance;
            var sW = control.SplitterWidth;

            //calculate the position of the points'
            if (control.Orientation == Orientation.Horizontal)
            {
                points[0] = new Point((w / 2), d + (sW / 2));
                points[1] = new Point(points[0].X - 10, points[0].Y);
                points[2] = new Point(points[0].X + 10, points[0].Y);
            }
            else
            {
                points[0] = new Point(d + (sW / 2), (h / 2));
                points[1] = new Point(points[0].X, points[0].Y - 10);
                points[2] = new Point(points[0].X, points[0].Y + 10);
            }

            foreach (Point p in points)
            {
                p.Offset(-2, -2);
                e.Graphics.FillEllipse(SystemBrushes.ControlDark,
                    new Rectangle(p, new Size(3, 3)));

                p.Offset(1, 1);
                e.Graphics.FillEllipse(SystemBrushes.ControlLight,
                    new Rectangle(p, new Size(3, 3)));
            }
        }

        private void RefreshGridControlOrder()
        {
            foreach (KeyValuePair<ToolStrip, int> kvPair in _GridStrips)
            {
                //  now find the proper place on the panel to place the strip/control
                int row = 1;
                if (kvPair.Value > _Panel.ColumnCount)
                {
                    // find the proper row to place this control
                    double drow = kvPair.Value / _Panel.ColumnCount;
                    row = (int)Math.Ceiling(drow);
                }
                var pos_sub = (row - 1) * _Panel.ColumnCount;
                var column = kvPair.Value - pos_sub;

                this._Panel.Controls.Add(kvPair.Key, column - 1, row - 1);
            }
            _SplitContainer.SplitterDistance = _Panel.Width;
        }

        private int GetNewGridOrderPosition()
        {
            // determine the last slot and return it for new control
            int sortOrder = 0;
            var list = _GridStrips.Values.ToList();
            if (list.Count > 0)
            {
                list.Sort();
                sortOrder = list[list.Count -1];
                sortOrder++;
            }
            return sortOrder;
        }

        /// <summary>
        /// Determines whether [is for tool strip]  being that it has an icon. Otherwise it should go on a menu.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is for tool strip] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsForMenuStrip(SimpleActionItem item)
        {
            return item.SmallImage == null;
        }

        private static void ParseAllowEditingProperty(DropDownActionItem item, ToolStripComboBox combo)
        {
            if (item.AllowEditingText)
            {
                combo.DropDownStyle = ComboBoxStyle.DropDown;
            }
            else
            {
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }

        private void RefreshRootItemOrder()
        {
            // Get a list of all the menus
            var pages = new List<ToolStripItem>(this._MenuStrip.Items.Count);
            foreach (ToolStripItem s in this._MenuStrip.Items)
            {
                pages.Add(s);
            }

            // order the list by SortOrder
            var sortedPages = (from entry in pages orderby entry.MergeIndex ascending select entry);

            // Re add all of the items in the new order.
            this._MenuStrip.Items.Clear();
            foreach (var sortedPage in sortedPages)
            {
                this._MenuStrip.Items.Add(sortedPage);
            }
        }

        private void MenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            OnRootItemSelected(e.ClickedItem.Name);
        }

        private void ActionItem_PropertyChanged(ActionItem item, PropertyChangedEventArgs e)
        {
            var guiItem = this.GetItem(item.Key);

            switch (e.PropertyName)
            {
                case "Caption":
                    guiItem.Text = item.Caption;
                    break;

                case "Enabled":
                    guiItem.Enabled = item.Enabled;
                    break;

                case "Visible":
                    guiItem.Visible = item.Visible;
                    break;

                case "ToolTipText":
                    guiItem.ToolTipText = item.ToolTipText;
                    break;

                case "GroupCaption":
                    // todo: change group
                    break;

                case "RootKey":
                    // todo: change root
                    // note, this case will also be selected in the case that we set the Root key in our code.
                    break;

                case "Key":
                default:
                    throw new NotSupportedException(" This Header Control implementation doesn't have an implemenation for or has banned modifying that property.");
            }
        }

        private ToolStrip AddToolStrip(string groupName)
        {
            ToolStrip strip = new ToolStrip();
            strip.Name = groupName;

            this._Strips.Add(strip);
            this._Strips.Remove(_MenuStrip);
            ToolStrip[] strips = this._Strips.ToArray();
            this._Strips.Add(_MenuStrip);
            this._Strips.AddRange(strips);

            return strip;
        }

        private ToolStripDropDownButton CreateToolStripDropDownButton(RootItem item)
        {
            ToolStripDropDownButton menu = new ToolStripDropDownButton(item.Caption);
            menu.Name = item.Key;
            menu.ShowDropDownArrow = false;
            menu.Visible = false;
            menu.MergeIndex = item.SortOrder;
            this._MenuStrip.Items.Add(menu);
            return menu;
        }

        private void DropDownActionItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = sender as DropDownActionItem;
            var guiItem = this.GetItem(item.Key) as ToolStripComboBox;

            switch (e.PropertyName)
            {
                case "AllowEditingText":
                    ParseAllowEditingProperty(item, guiItem);
                    break;

                case "Width":
                    guiItem.Width = item.Width;
                    break;

                case "SelectedItem":
                    guiItem.SelectedItem = item.SelectedItem;
                    break;

                case "ToggleGroupKey":
                    break;

                default:
                    ActionItem_PropertyChanged(item, e);
                    break;
            }
        }

        /// <summary>
        /// Ensure the extensions tab exists.
        /// </summary>
        private void EnsureExtensionsTabExists()
        {
            bool exists = _MenuStrip.Items.ContainsKey(ExtensionsRootKey);
            if (!exists)
            {
                this.Add(new RootItem(ExtensionsRootKey, "Extensions"));
            }
        }

        /// <summary>
        /// Make sure the root key is present or use a default.
        /// </summary>
        /// <param name="item">
        /// </param>
        private void EnsureNonNullRoot(ActionItem item)
        {
            if (item.RootKey == null)
            {
                this.EnsureExtensionsTabExists();
                item.RootKey = ExtensionsRootKey;
            }
        }

        private ToolStripItem GetItem(string key)
        {
            foreach (ToolStrip strip in this._Strips)
            {
                ToolStripItem item = strip.Items.Find(key, true).FirstOrDefault();
                if (item != null)
                {
                    return item;
                }
            }
            return null;
        }

        private ToolStrip GetOrCreateStrip(string groupCaption)
        {
            var query = from s in this._Strips
                        where s.Name == (groupCaption ?? STR_DefaultGroupName)
                        select s;

            ToolStrip strip = query.FirstOrDefault();
            if (strip == null)
            {
                strip = this.AddToolStrip(groupCaption);
            }

            return strip;
        }

        private void SimpleActionItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = sender as SimpleActionItem;
            var guiItem = this.GetItem(item.Key);

            switch (e.PropertyName)
            {
                case "SmallImage":
                    guiItem.Image = item.SmallImage;
                    break;
                case "LargeImage":
                    guiItem.Image = item.LargeImage;
                    break;
                case "MenuContainerKey":
                    break;
                case "ToggleGroupKey":
                    break;
                default:
                    ActionItem_PropertyChanged(item, e);
                    break;
            }
        }

        private void TextEntryActionItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = sender as TextEntryActionItem;
            var guiItem = this.GetItem(item.Key) as ToolStripTextBox;

            switch (e.PropertyName)
            {
                case "Width":
                    guiItem.Width = item.Width;
                    break;
                case "Text":
                    guiItem.Text = item.Text;
                    break;
                default:
                    ActionItem_PropertyChanged(item, e);
                    break;
            }
        }

        /// <summary>
        /// Unchecks all toolstrip buttons except the current button
        /// </summary>
        /// <param name="checkedButton">
        /// The toolstrip button which should
        /// stay checked
        /// </param>
        private void UncheckButtonsExcept(ToolStripButton checkedButton)
        {
            foreach (ToolStrip strip in this._Strips)
            {
                foreach (ToolStripItem item in strip.Items)
                {
                    ToolStripButton buttonItem = item as ToolStripButton;
                    if (buttonItem != null)
                    {
                        if (buttonItem.Name != checkedButton.Name && buttonItem.Checked)
                        {
                            buttonItem.Checked = false;
                        }
                    }
                }
            }
        }

        #endregion

        #region ------------------- Event Handlers

        private void button_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button.Checked)
            {
                this.UncheckButtonsExcept(button);
            }
        }


        private void Splitter_Paint(object sender, PaintEventArgs e)
        {
            PaintSplitterDots((SplitContainer)sender, e);
        }

        private void Splitter_Moved(object sender, EventArgs e)
        {
            if (this._SplitterInvalidate)
            {
                var sc = (SplitContainer)sender;
                var btn = sc.Panel1.Controls[0].Controls[0];  // grab a single control button
                int btnWidth = btn.Width;
                int splitDist = sc.SplitterDistance;
                double dcolCount = splitDist / btnWidth;
                int colCount = (int)Math.Round(dcolCount);

                if (splitDist % btnWidth != 0)
                {
                    int dist = colCount * btnWidth;
                    sc.SplitterDistance = dist;
                }
                else
                {
                    this._SplitterDistance = sc.SplitterDistance;
                    this._Panel.ColumnCount = colCount;
                    this._SplitterInvalidate = false;
                    RefreshGridControlOrder();
                }
            }
        }

        private void Splitter_Moving(object sender, EventArgs e)
        {
            // simple flag to only do the splitter invalidation after
            // a user has moved the splitter, ignores load events etc.
            this._SplitterInvalidate = true;
        }

        private void Splitter_DoubleClick(object sender, EventArgs e)
        {
            SplitContainer sc = (SplitContainer)sender;
            int dist = sc.SplitterDistance;

            if (dist != 0)
            {
                this._SplitterDistance = dist;
                sc.SplitterDistance = 0;
            }
            else
            {
                sc.SplitterDistance = this._SplitterDistance;
            }
        }

        #endregion

    }
}