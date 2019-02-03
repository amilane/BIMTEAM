using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.RightsManagement;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BimTeamTools.Filter;


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

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      List<List<object>> combos = new List<List<object>>();
      combos.Add(new List<object>{ (ParameterData)cbParameter1.SelectedValue , cbOperation1.SelectedValue.ToString(), cbValue1.SelectedValue.ToString() });

      ElementParameterFilter filter = CreateParameterFilter.createParameterFilter(DOC,
        (ParameterData)cbParameter1.SelectedValue, cbOperation1.SelectedValue.ToString(),
        cbValue1.SelectedValue.ToString());
      MessageBox.Show(filter.ToString(), "sdsd");
    }

    
    private void cbParameter1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      cbOperation1.ItemsSource = ListOfOperands.listOfOperands(((ParameterData)cbParameter1.SelectedValue).storageType);
    }
    private void cbParameter2_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      cbOperation2.ItemsSource = ListOfOperands.listOfOperands(((ParameterData)cbParameter2.SelectedValue).storageType);
    }
    private void cbParameter1_OnDropDownOpened(object sender, EventArgs e)
    {
      FilteredElementCollector collector = CollectorFromTreeView.collectorFromTreeView(DOC,
        (ObservableCollection<Node>)treeView.ItemsSource);
      SortedDictionary<string, ParameterData> parameterDict = GetParamsFromSelectedElements.getParamsFromSelectedElements(collector);
      cbParameter1.ItemsSource = parameterDict;
    }
    private void cbParameter2_OnDropDownOpened(object sender, EventArgs e)
    {
      FilteredElementCollector collector = CollectorFromTreeView.collectorFromTreeView(DOC,
        (ObservableCollection<Node>)treeView.ItemsSource);
      SortedDictionary<string, ParameterData> parameterDict = GetParamsFromSelectedElements.getParamsFromSelectedElements(collector);
      cbParameter2.ItemsSource = parameterDict;
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
    private void cbValue1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ElementParameterFilter filter = CreateParameterFilter.createParameterFilter(DOC,
        (ParameterData) cbParameter1.SelectedValue, cbOperation1.SelectedValue.ToString(),
        cbValue1.SelectedValue.ToString());
      FilteredElementCollector collector = CollectorFromTreeView.collectorFromTreeView(DOC,
        (ObservableCollection<Node>)treeView.ItemsSource);
      FilteredElementCollector filteredCollector = collector.WherePasses(filter);
      var ids = filteredCollector.ToElementIds();
    }
    private void cbValue2_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

  }
}

