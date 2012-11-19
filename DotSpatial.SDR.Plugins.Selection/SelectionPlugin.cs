// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuBarPlugin.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Data;
using DotSpatial.SDR.Plugins.Selection.Properties;
using Msg = DotSpatial.SDR.Plugins.Selection.MessageStrings;
using DotSpatial.Topology;

namespace DotSpatial.SDR.Plugins.Selection
{
    public class SelectionPlugin : Extension
    {
        #region Constants and Fields

        private const string HomeMenuKey = HeaderControl.HomeRootItemKey;

        #endregion

        #region Public Methods

        public override void Activate()
        {
            IHeaderControl header = App.HeaderControl;
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Select, SelectionTool_Click) { GroupCaption = "Selection", SmallImage = Resources.select_16x16, LargeImage = Resources.select_32x32 });
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Deselect, DeselectAll_Click) { GroupCaption = "Deselect", SmallImage = Resources.deselect_16x16, LargeImage = Resources.deselect_32x32 });
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Identify, IdentifierTool_Click) { GroupCaption = "Identify", SmallImage = Resources.info_rhombus_16x16, LargeImage = Resources.info_rhombus_32x32 });
            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        #endregion

        /// <summary>
        /// Identifier Tool
        /// </summary>
        private void IdentifierTool_Click(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.Info;
        }

        /// <summary>
        /// Select or deselect Features
        /// </summary>
        private void SelectionTool_Click(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.Select;
        }

        /// <summary>
        /// Deselect all features in all layers
        /// </summary>
        private void DeselectAll_Click(object sender, EventArgs e)
        {
            IEnvelope env = new Envelope();
            App.Map.MapFrame.ClearSelection(out env);
            
            //foreach (IMapLayer layer in App.Map.MapFrame.GetAllLayers())
            //{
            //    IMapFeatureLayer mapFeatureLayer = layer as IMapFeatureLayer;
            //    {
            //        mapFeatureLayer.UnSelectAll();
            //    }
            //}
        }
    }
}