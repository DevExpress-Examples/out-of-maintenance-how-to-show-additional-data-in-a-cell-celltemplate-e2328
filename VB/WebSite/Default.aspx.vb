Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxPivotGrid
Imports DevExpress.XtraPivotGrid.Data
Imports DevExpress.XtraPivotGrid

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		ASPxPivotGrid1.CellTemplate = New CellTemplate(ASPxPivotGrid1.Fields("fieldCompanyName"), ASPxPivotGrid1.Fields("fieldProductName"))
	End Sub


	Private Class CellTemplate
		Implements ITemplate
		Private mainField, sourceField As PivotGridField
		Public Sub New(ByVal mainField As PivotGridField, ByVal sourceField As PivotGridField)
			Me.mainField = mainField
			Me.sourceField = sourceField
		End Sub
		Public Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
			Dim cell As PivotGridCellTemplateContainer = TryCast(container, PivotGridCellTemplateContainer)

			If cell.RowField IsNot Nothing AndAlso cell.RowField.ID = mainField.ID Then
				cell.Controls.Add(CreateCellTable(cell.Item))
			Else
				Dim templateLable As New Label()
				templateLable.Text = cell.Item.Text
				cell.Controls.Add(templateLable)
			End If

		End Sub

		Private Function CreateCellTable(ByVal cell As PivotGridCellTemplateItem) As Control
			Dim values As Dictionary(Of Object, Decimal) = GetSummaryValues(cell.CreateDrillDownDataSource(), sourceField, cell.DataField)

			Dim table As New Table()
			For Each pair As KeyValuePair(Of Object,Decimal) In values
				Dim tr As New TableRow()
				table.Controls.Add(tr)
				Dim tc1 As New TableCell()
				tc1.Style.Add(HtmlTextWriterStyle.Padding, "0px")
				tc1.Text = pair.Key.ToString()
				tr.Controls.Add(tc1)

				Dim tc2 As New TableCell()
				tc2.Style.Add(HtmlTextWriterStyle.Padding, "0px")
				tc2.Text = pair.Value.ToString()
				tr.Controls.Add(tc2)
			Next pair
			Return table
		End Function

		Public Shared Function GetSummaryValues(ByVal ds As PivotDrillDownDataSource, ByVal valueField As PivotGridField, ByVal dataField As PivotGridField) As Dictionary(Of Object, Decimal)
			Dim dict As New Dictionary(Of Object, Decimal)()
			For i As Integer = 0 To ds.RowCount - 1
				Dim currentObject As Object = ds(i)(valueField)
				Dim currentValue As Decimal = Convert.ToDecimal(ds(i)(dataField))
				If dict.ContainsKey(currentObject) Then
					dict(currentObject) += currentValue
				Else
					dict.Add(currentObject, currentValue)
				End If
			Next i
			Return dict
		End Function
	End Class



End Class
