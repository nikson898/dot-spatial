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
using DotSpatial.SDR.Plugins.Navigate.Properties;
using Msg = DotSpatial.SDR.Plugins.Navigate.MessageStrings;
using DotSpatial.Topology;

namespace DotSpatial.SDR.Plugins.Navigate
{
    public class NavigatePlugin : Extension
    {
        #region Constants and Fields

        private const string HomeMenuKey = HeaderControl.HomeRootItemKey;

        private SimpleActionItem _ZoomNext;
        private SimpleActionItem _ZoomPrevious;
        private SimpleActionItem _ZoomToLayer;

        #endregion

        #region Public Methods

        public override void Activate()
        {
            IHeaderControl header = App.HeaderControl;

            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Pan, PanTool_Click) { GroupCaption = "Pan", ToolTipText = "Pan", SmallImage = Resources.hand_16x16, LargeImage = Resources.hand_32x32 });
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Zoom_In, ZoomIn_Click) { GroupCaption = "Zoom_In", ToolTipText = "Zoom In", SmallImage = Resources.zoom_in_16x16, LargeImage = Resources.zoom_in_32x32 });
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Zoom_Out, ZoomOut_Click) { GroupCaption = "Zoom_Out", ToolTipText = "Zoom Out", SmallImage = Resources.zoom_out_16x16, LargeImage = Resources.zoom_out_32x32 });
            header.Add(new SimpleActionItem(HomeMenuKey, Msg.Zoom_To_Extents, ZoomToMaxExtents_Click) { GroupCaption = "Zoom Max Extents", ToolTipText = "Extents", SmallImage = Resources.zoom_extend_16x16, LargeImage = Resources.zoom_extend_32x32 });
            _ZoomPrevious = new SimpleActionItem(HomeMenuKey, Msg.Zoom_Previous, ZoomPrevious_Click) { GroupCaption = "Zoom Previous", ToolTipText = Msg.Zoom_Previous_Tooltip, SmallImage = Resources.zoom_to_previous_16, LargeImage = Resources.zoom_to_previous, Enabled = false };
            header.Add(_ZoomPrevious);
            _ZoomNext = new SimpleActionItem(HomeMenuKey, Msg.Zoom_Next, ZoomNext_Click) { GroupCaption = "Zoom Next", ToolTipText = Msg.Zoom_Next_Tooltip, SmallImage = Resources.zoom_to_next_16, LargeImage = Resources.zoom_to_next, Enabled = false };
            header.Add(_ZoomNext);
            _ZoomToLayer = new SimpleActionItem(HomeMenuKey, Msg.Zoom_To_Layer, ZoomToLayer_Click) { GroupCaption = "Zoom To Layer", SmallImage = Resources.zoom_layer_16x16, LargeImage = Resources.zoom_layer_32x32, Enabled = false };
            header.Add(_ZoomToLayer);

            App.Map.MapFrame.ViewExtentsChanged += MapFrame_ViewExtentsChanged;
            App.Map.Layers.LayerSelected += Layers_LayerSelected;

            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        #endregion

        #region Methods

        void Layers_LayerSelected(object sender, Symbology.LayerSelectedEventArgs e)
        {
            _ZoomToLayer.Enabled = App.Map.Layers.SelectedLayer != null;
        }

        private void MapFrame_ViewExtentsChanged(object sender, ExtentArgs e)
        {
            var mapFrame = sender as MapFrame;
            _ZoomNext.Enabled = mapFrame.CanZoomToNext();
            _ZoomPrevious.Enabled = mapFrame.CanZoomToPrevious();
        }

        /// <summary>
        /// Move (Pan) the map
        /// </summary>
        private void PanTool_Click(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.Pan;
        }

        /// <summary>
        /// Zoom In
        /// </summary>
        private void ZoomIn_Click(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.ZoomIn;
        }

        /// <summary>
        /// Zoom to previous extent
        /// </summary>
        private void ZoomNext_Click(object sender, EventArgs e)
        {
            App.Map.MapFrame.ZoomToNext();
        }

        /// <summary>
        /// Zoom Out
        /// </summary>
        private void ZoomOut_Click(object sender, EventArgs e)
        {
            App.Map.FunctionMode = FunctionMode.ZoomOut;
        }

        /// <summary>
        /// Zoom to previous extent
        /// </summary>
        private void ZoomPrevious_Click(object sender, EventArgs e)
        {
            App.Map.MapFrame.ZoomToPrevious();
        }

        /// <summary>
        /// Zoom to the currently selected layer
        /// </summary>
        private void ZoomToLayer_Click(object sender, EventArgs e)
        {
            var layer = App.Map.Layers.SelectedLayer;
            if (layer != null)
            {
                ZoomToLayer(layer);
            }
        }

        /// <summary>
        /// Zoom to maximum extents
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ZoomToMaxExtents_Click(object sender, EventArgs e)
        {
            App.Map.ZoomToMaxExtent();
        }

        private void ZoomToLayer(IMapLayer layerToZoom)
        {
            const double eps = 1e-7;
            IEnvelope layerEnvelope = layerToZoom.Extent.ToEnvelope();
            if (layerEnvelope.Width > eps && layerEnvelope.Height > eps)
            {
                layerEnvelope.ExpandBy(layerEnvelope.Width / 10, layerEnvelope.Height / 10); // work item #84
            }
            else
            {
                double zoomInFactor = 0.05; //fixed zoom-in by 10% - 5% on each side
                double newExtentWidth = App.Map.ViewExtents.Width * zoomInFactor;
                double newExtentHeight = App.Map.ViewExtents.Height * zoomInFactor;
                layerEnvelope.ExpandBy(newExtentWidth, newExtentHeight);
            }

            App.Map.ViewExtents = layerEnvelope.ToExtent();
        }

        #endregion
    }
}