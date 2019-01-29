using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      
      //MessageBox.Show(lol.ToString(), "sdsd");
    }

    
    private void cbParameter1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      textBox.Text = cbParameter1.SelectedValue.ToString();
    }
    private void cbParameter1_OnDropDownOpened(object sender, EventArgs e)
    {

      GetParamsFromSelectedElements getParams = new GetParamsFromSelectedElements();
      SortedDictionary<string, StorageType> parameterDict = getParams.getParamsFromSelectedElements(DOC, (ObservableCollection<Node>)treeView.ItemsSource, selectedElements);
      cbParameter1.ItemsSource = parameterDict;
      


    }
    





  }
}

