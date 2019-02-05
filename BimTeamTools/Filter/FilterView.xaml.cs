using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BimTeamTools.Filter;
using ComboBox = System.Windows.Controls.ComboBox;


namespace BimTeamTools
{
  /// <summary>
  /// Логика взаимодействия для FilterControl.xaml
  /// </summary>
  public partial class FilterView : IDisposable
  {
    public UIApplication UIAPP = null;
    private Application APP = null;
    private UIDocument UIDOC = null;
    public Document DOC = null;
    public List<Element> selectedElements;

    public FilterView()
    {
      InitializeComponent();
    }

    public void Dispose()
    {
      this.Close();
    }


    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      CheckBox currentCheckBox = (CheckBox)sender;
      CheckBoxId.checkBoxId = currentCheckBox.Uid;
    }

    private void AndMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      orCheckBox.IsChecked = false;
    }
    private void OrMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      andCheckBox.IsChecked = false;
    }

    private IList<ElementFilter> CollectFilters(ComboBox[,] comboBoxs)
    {
      IList < ElementFilter > filters = new List<ElementFilter>();
      for (int i = 0; i < 4; i++)
      {
        string parameter = comboBoxs[i, 0]?.SelectedValue?.ToString();
        string operat = comboBoxs[i, 1]?.SelectedValue?.ToString();

        if (parameter != null && operat != null)
        {
          ElementFilter f = CreateParameterFilter.createParameterFilter(
            DOC,
            (ParameterData)comboBoxs[i, 0].SelectedValue,
            comboBoxs[i, 1].SelectedValue.ToString(),
            comboBoxs[i, 2].SelectedValue.ToString());

          if (f != null)
          {
            filters.Add(f);
          }
        }
      }
      return filters;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

      ComboBox[,] comboBoxs =
      {
        {cbParameter1, cbOperation1, cbValue1},
        {cbParameter2, cbOperation2, cbValue2},
        {cbParameter3, cbOperation3, cbValue3},
        {cbParameter4, cbOperation4, cbValue4}
      };

      IList<ElementFilter> filters = CollectFilters(comboBoxs);

      ICollection<ElementId> ids;
      if (andCheckBox.IsChecked == true)
      {
        LogicalAndFilter filter = new LogicalAndFilter(filters);
        ids = CollectorFromTreeView.collectorFromTreeView(DOC,
          (ObservableCollection<Node>) treeView.ItemsSource).WherePasses(filter).ToElementIds();
      }
      else
      {
        LogicalOrFilter filter = new LogicalOrFilter(filters);
        ids = CollectorFromTreeView.collectorFromTreeView(DOC,
          (ObservableCollection<Node>)treeView.ItemsSource).WherePasses(filter).ToElementIds();
      }

      UIDOC.Selection.SetElementIds(ids);
      UIDOC.ShowElements(ids);

    }

    private void cbParameter1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      cbOperation1.ItemsSource = ListOfOperands.listOfOperands(((ParameterData)cbParameter1.SelectedValue).StorageType);
    }
    private void cbParameter1_OnDropDownOpened(object sender, EventArgs e)
    {
      FilteredElementCollector collector = CollectorFromTreeView.collectorFromTreeView(DOC,
        (ObservableCollection<Node>)treeView.ItemsSource);
      cbParameter1.ItemsSource = GetParamsFromSelectedElements.getParamsFromSelectedElements(collector);
    }
    private void cbParameter2_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      cbOperation2.ItemsSource = ListOfOperands.listOfOperands(((ParameterData)cbParameter2.SelectedValue).StorageType);
    }
    private void cbParameter2_OnDropDownOpened(object sender, EventArgs e)
    {
      FilteredElementCollector collector = CollectorFromTreeView.collectorFromTreeView(DOC,
        (ObservableCollection<Node>)treeView.ItemsSource);
      cbParameter2.ItemsSource = GetParamsFromSelectedElements.getParamsFromSelectedElements(collector);
    }
    private void cbParameter3_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      cbOperation3.ItemsSource = ListOfOperands.listOfOperands(((ParameterData)cbParameter3.SelectedValue).StorageType);
    }
    private void cbParameter3_OnDropDownOpened(object sender, EventArgs e)
    {
      FilteredElementCollector collector = CollectorFromTreeView.collectorFromTreeView(DOC,
        (ObservableCollection<Node>)treeView.ItemsSource);
      cbParameter3.ItemsSource = GetParamsFromSelectedElements.getParamsFromSelectedElements(collector);
    }
    private void cbParameter4_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      cbOperation4.ItemsSource = ListOfOperands.listOfOperands(((ParameterData)cbParameter4.SelectedValue).StorageType);
    }
    private void cbParameter4_OnDropDownOpened(object sender, EventArgs e)
    {
      FilteredElementCollector collector = CollectorFromTreeView.collectorFromTreeView(DOC,
        (ObservableCollection<Node>)treeView.ItemsSource);
      cbParameter4.ItemsSource = GetParamsFromSelectedElements.getParamsFromSelectedElements(collector);
    }
    private void cbValue1_OnDropDownOpened(object sender, EventArgs e)
    {
      FilteredElementCollector collector = CollectorFromTreeView.collectorFromTreeView(DOC,
        (ObservableCollection<Node>)treeView.ItemsSource);
      cbValue1.ItemsSource = ValuesFromParameter.valuesFromParameter(cbParameter1.Text, collector);
    }
    private void cbValue2_OnDropDownOpened(object sender, EventArgs e)
    {
      FilteredElementCollector collector = CollectorFromTreeView.collectorFromTreeView(DOC,
        (ObservableCollection<Node>)treeView.ItemsSource);
      cbValue2.ItemsSource = ValuesFromParameter.valuesFromParameter(cbParameter2.Text, collector);
    }
    private void cbValue3_OnDropDownOpened(object sender, EventArgs e)
    {
      FilteredElementCollector collector = CollectorFromTreeView.collectorFromTreeView(DOC,
        (ObservableCollection<Node>)treeView.ItemsSource);
      cbValue3.ItemsSource = ValuesFromParameter.valuesFromParameter(cbParameter3.Text, collector);
    }
    private void cbValue4_OnDropDownOpened(object sender, EventArgs e)
    {
      FilteredElementCollector collector = CollectorFromTreeView.collectorFromTreeView(DOC,
        (ObservableCollection<Node>)treeView.ItemsSource);
      cbValue4.ItemsSource = ValuesFromParameter.valuesFromParameter(cbParameter4.Text, collector);
    }
  }
}

