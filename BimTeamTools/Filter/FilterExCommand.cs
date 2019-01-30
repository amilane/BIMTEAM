using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

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

        // Create data for TreeView (Category - Family - FamilyType)
        var elemsByCategories = selectedElements.OrderBy(i => i.Category.Name).GroupBy(i => i.Category.Name);
        ObservableCollection<Node> familyList = new ObservableCollection<Node>();

        foreach (var category in elemsByCategories)
        {
          Node categoryNode = new Node();
          categoryNode.Text = category.Key;

          var elemsByFamilyName = category.
            OrderBy(i => i.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString()).
            GroupBy(i => i.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString());
          
          foreach (var family in elemsByFamilyName)
          {
            Node familyNode = new Node();
            familyNode.Text = family.Key;
            familyNode.Parent.Add(categoryNode);
            categoryNode.Children.Add(familyNode);

            var elemsByFamilyType = family.OrderBy(i => i.Name).GroupBy(i => i.Name);
            foreach (var familyType in elemsByFamilyType)
            {
              Node familyTypeNode = new Node();
              familyTypeNode.Text = familyType.Key;
              familyTypeNode.Parent.Add(familyNode);
              familyNode.Children.Add(familyTypeNode);
            }
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
          window.selectedElements = selectedElements;
          window.treeView.ItemsSource = familyList;
          
          //window.cbParameter1.ItemsSource = CheckedNodes(window.treeView);
          
          window.ShowDialog();



          
        }
        return Result.Succeeded;
      }
      catch (Exception ex)
      {
        message = ex.Message;
        return Result.Failed;
      }
    }



    private void FindCheckedNodes(
      List<Node> checked_nodes, ObservableCollection<Node> nodes)
    {
      foreach (Node node in nodes)
      {
        // Add this node.
        if (node.IsChecked == true) checked_nodes.Add(node);

        // Check the node's descendants.
        FindCheckedNodes(checked_nodes, node.Children);
      }
    }

    private List<Node> CheckedNodes(TreeView treeView)
    {
      List<Node> checked_nodes = new List<Node>();
      FindCheckedNodes(checked_nodes, (ObservableCollection<Node>)treeView.ItemsSource);
      return checked_nodes;
    }


  }
}
