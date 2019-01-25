using System;
using System.Collections;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;

namespace BimTeamTools
{
  [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
  [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
  public class FilterExCommand : IExternalCommand
  {
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
      try
      {
        UIApplication uiapp = commandData.Application;
        UIDocument uidoc = uiapp.ActiveUIDocument;
        Application app = uiapp.Application;
        Document doc = uidoc.Document;

        View activeView = doc.ActiveView;

        // List of selected elements
        List<Element> selectedElements = new List<Element>();
        ICollection<ElementId> ids = uidoc.Selection.GetElementIds();

        if (ids != null && ids.Count > 0)
        {
          foreach (ElementId eid in ids)
          {
            selectedElements.Add(doc.GetElement(eid));
          }
        }

        //Create data for TreeView
        var elemsByCategories = selectedElements.OrderBy(i => i.Category.Name).GroupBy(i => i.Category.Name);
        ObservableCollection<Node> familyList = new ObservableCollection<Node>();

        foreach (var category in elemsByCategories)
        {
          Node categoryNode = new Node();
          categoryNode.Text = category.Key;

          var elemsByFamilyName = category.OrderBy(i => i.Name).GroupBy(i => i.Name);
          
          foreach (var family in elemsByFamilyName)
          {
            Node familyNode = new Node();
            familyNode.Text = family.Key;
            familyNode.Parent.Add(categoryNode);
            categoryNode.Children.Add(familyNode);
          }
          
          familyList.Add(categoryNode);
        }

       

        System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();


        using (FilterView window = new FilterView())
        {
          System.Windows.Interop.WindowInteropHelper helper =
            new System.Windows.Interop.WindowInteropHelper(window);
          helper.Owner = proc.MainWindowHandle;

          window.DOC = doc;
          window.treeView.ItemsSource = familyList;
          window.ShowDialog();

          var CurrentNodes = window.treeView.ItemsSource as ObservableCollection<Node>;
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

          window.textBox.Text = lol;



        }
        return Result.Succeeded;
      }
      catch (Exception ex)
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
}
