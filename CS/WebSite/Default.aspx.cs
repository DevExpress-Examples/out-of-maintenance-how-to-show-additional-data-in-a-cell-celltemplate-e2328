using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.XtraPivotGrid.Data;
using DevExpress.XtraPivotGrid;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ASPxPivotGrid1.CellTemplate = new CellTemplate(ASPxPivotGrid1.Fields["fieldCompanyName"], ASPxPivotGrid1.Fields["fieldProductName"]);
    }

  
    class CellTemplate : ITemplate
    {
        PivotGridField mainField, sourceField;
        public CellTemplate(PivotGridField mainField, PivotGridField sourceField)
        {
            this.mainField = mainField;
            this.sourceField = sourceField;
        }  
        public void InstantiateIn(Control container)
        {
            PivotGridCellTemplateContainer cell = container as PivotGridCellTemplateContainer;

            if (cell.RowField != null && cell.RowField.ID == mainField.ID)
            {
                cell.Controls.Add(CreateCellTable(cell.Item));
            }
            else
            {
                Label templateLable = new Label();
                templateLable.Text = cell.Item.Text;
                cell.Controls.Add(templateLable);
            }

        }

        private Control CreateCellTable(PivotGridCellTemplateItem cell)
        {
            Dictionary<object, decimal> values = GetSummaryValues(cell.CreateDrillDownDataSource(), sourceField, cell.DataField);
            
            Table table = new Table();
            foreach (KeyValuePair<object,decimal> pair in values)
            {
                TableRow tr = new TableRow();
                table.Controls.Add(tr);
                TableCell tc1 = new TableCell();
                tc1.Style.Add(HtmlTextWriterStyle.Padding, "0px");
                tc1.Text = pair.Key.ToString();
                tr.Controls.Add(tc1);

                TableCell tc2 = new TableCell();
                tc2.Style.Add(HtmlTextWriterStyle.Padding, "0px");
                tc2.Text = pair.Value.ToString();
                tr.Controls.Add(tc2);                
            }
            return table;
        }

        public static Dictionary<object, decimal> GetSummaryValues(PivotDrillDownDataSource ds, PivotGridField valueField, PivotGridField dataField)
        {
            Dictionary<object, decimal> dict = new Dictionary<object, decimal>();
            for (int i = 0; i < ds.RowCount; i++)
            {
                object currentObject = ds[i][valueField];
                decimal currentValue = Convert.ToDecimal(ds[i][dataField]);
                if (dict.ContainsKey(currentObject))
                    dict[currentObject] += currentValue;
                else
                    dict.Add(currentObject, currentValue);
            }
            return dict;
        }
    }



}
