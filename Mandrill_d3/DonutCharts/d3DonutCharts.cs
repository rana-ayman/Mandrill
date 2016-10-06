﻿using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;

namespace D3jsLib.DonutChart
{
    public class DonutChartStyle : ChartStyle
    {
        public Color HoverColor { get; set; }
        public List<string> Colors { get; set; }
        public bool Labels { get; set; }
        public string TotalLabel { get; set; }
        public int Margin { get; set; }
    }

    public class DonutChartDataPoint
    {
        public string name { get; set; }
        public double val { get; set; }
    }

    public class DonutChartData
    {
        public List<DonutChartDataPoint> Data { get; set; }
    }

    public class DonutChartModel : ChartModel
    {
        public string HoverColor { get; set; }
        public string Data { get; set; }
        public bool Colors { get; set; }
        public string DomainColors { get; set; }
        public bool Labels { get; set; }
        public string TotalLabel { get; set; }
        public int Margin { get; set; }
    }

    public class DonutChart : Chart
    {
        public DonutChartData Data;
        public DonutChartStyle Style;
        public string UniqueName { get; set; }

        public DonutChart(DonutChartData data, DonutChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            DonutChartModel model = new DonutChartModel();
            model.Width = this.Style.Width.ToString();
            model.Height = this.Style.Height.ToString();
            model.HoverColor = ChartsUtilities.ColorToHexString(this.Style.HoverColor);
            model.Labels = this.Style.Labels;
            model.DivId = "div" + counter.ToString();
            model.TotalLabel = this.Style.TotalLabel;
            model.Margin = this.Style.Margin;

            // set grid address
            model.GridRow = this.Style.GridRow.ToString();
            model.GridColumn = this.Style.GridColumn.ToString();

            // always round up for the grid size so chart is smaller then container
            model.SizeX = System.Math.Ceiling(this.Style.Width / 100d).ToString();
            model.SizeY = System.Math.Ceiling(this.Style.Width / 100d).ToString();

            if (this.Style.Colors != null)
            {
                string domainColors = new JavaScriptSerializer().Serialize(this.Style.Colors);
                model.DomainColors = domainColors;
                model.Colors = true;
            }
            else
            {
                model.Colors = false;
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(this.Data.Data);
            model.Data = jsData;

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempDonut" + counter.ToString();
            DonutChartModel model = this.ChartModel as DonutChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.DonutCharts.DonutChartScript.html", templateName);
            return colString;
        }
    }
}