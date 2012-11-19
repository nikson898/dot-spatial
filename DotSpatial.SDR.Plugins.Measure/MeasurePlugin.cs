﻿using System;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.SDR.Plugins.Measure.Properties;

namespace DotSpatial.SDR.Plugins.Measure
{
    public class MeasurePlugin : Extension
    {
        #region Constants and Fields

        private MapFunctionMeasure _Painter;

        #endregion

        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem(HeaderControl.HomeRootItemKey, "Measure", MeasureTool_Click) { GroupCaption = "Measure Tools", SmallImage = Resources.measure_16x16, LargeImage = Resources.measure_32x32 });
            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        private void MeasureTool_Click(object sender, EventArgs e)
        {
            if (_Painter == null)
                _Painter = new MapFunctionMeasure(App.Map);

            if (!App.Map.MapFunctions.Contains(_Painter))
                App.Map.MapFunctions.Add(_Painter);

            App.Map.FunctionMode = FunctionMode.None;
            App.Map.Cursor = Cursors.Cross;
            _Painter.Activate();
        }
    }
}