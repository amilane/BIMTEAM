using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
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


    public FilterView()
    {
      InitializeComponent();
    }

    public void Dispose()
    {
      this.Close();
    }

    private List<string> selectedNames = new List<string>();
    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      CheckBox currentCheckBox = (CheckBox)sender;
      CheckBoxId.checkBoxId = currentCheckBox.Uid;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var CurrentNodes = treeView.ItemsSource as ObservableCollection<Node>;
      string lol = "";
      foreach (var i in CurrentNodes)
      {
        var familyNodes = i.Children;
        foreach (var fam in familyNodes)
        {
          if (fam.IsChecked == true)
          {
            lol += fam.Text;
          }
        }
        
      }

      MessageBox.Show(lol, "sdsd");
    }

    private void cbParameter1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var CurrentNodes = treeView.ItemsSource as ObservableCollection<Node>;
      List<string> lol = new List<string>();
      foreach (var i in CurrentNodes)
      {
        var familyNodes = i.Children;
        foreach (var fam in familyNodes)
        {
          if (fam.IsChecked == true)
          {
            lol.Add(fam.Text);
          }
        }
      }

      ComboBox cb = (ComboBox) sender;
      cb.ItemsSource = lol;





    }

  }
}

