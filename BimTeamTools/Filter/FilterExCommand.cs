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

        //Create list of families in model (Category - Family - Type)
        IEnumerable<Element> modelElements = new FilteredElementCollector(doc, activeView.Id).WhereElementIsNotElementType().WhereElementIsViewIndependent().ToElements();
        var elemsByCategories = modelElements.GroupBy(i => i.Category.Name);


        ObservableCollection<Node> familyList = new ObservableCollection<Node>();

        foreach (var group in elemsByCategories)
        {
          Node categoryNode = new Node();
          
          categoryNode.Name = group.Key;
          // Сделать уникальные имена семейств
          ObservableCollection<Node> familyNames = new ObservableCollection<Node>();
          foreach (Element e in group)
          {
            Node familyName = new Node();
            familyName.Name = e.Name;
            familyNames.Add(familyName);
          }

          categoryNode.Nodes = familyNames;

          familyList.Add(categoryNode);
        }

        // Create a view model that will be associated to the DataContext of the view.
        FilterViewModel vmod = new FilterViewModel();
        vmod.UIAPP = uiapp;
        vmod.DOC = doc;
        vmod.ModelElements = familyList;

        System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();


        using (FilterControl window = new FilterControl())
        {
          System.Windows.Interop.WindowInteropHelper helper =
            new System.Windows.Interop.WindowInteropHelper(window);
          helper.Owner = proc.MainWindowHandle;

          window.DataContext = vmod;
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
  }
}
