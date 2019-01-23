using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace BimTeamTools
{
  [Transaction(TransactionMode.Manual)]
  public class TotalLength : IExternalCommand
  {
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {

      // Get application and document objects
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp?.ActiveUIDocument;
      Document doc = uidoc?.Document;
      try
      {
        List<Element> selectedElements = new List<Element>();
        ICollection<ElementId> ids = uidoc.Selection.GetElementIds();

        double length, lengthMeters;
        string lengthRound;

        using (Transaction t = new Transaction(doc))
        {
          t.Start("TotalLength");
          message = "";
          if (ids != null && ids.Count > 0)
          {
            foreach (ElementId eid in ids)
            {
              selectedElements.Add(doc.GetElement(eid));
            }
          }

          var groupsByCategory = selectedElements.GroupBy(i => i.Category.Name);


          foreach (var group in groupsByCategory)
          {
            length = 0;
            foreach (var e in group)
            {
              Parameter parLength = e.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
              if (parLength != null)
              {
                length += parLength.AsDouble();
              }
            }
            lengthMeters = UnitUtils.ConvertFromInternalUnits(length, DisplayUnitType.DUT_METERS);
            lengthRound = Math.Round(lengthMeters, 2).ToString();

            if (length > 0)
            {
              message += String.Format("{0} - {1} м \n", group.Key, lengthRound);
            }
          }

          t.Commit();
        }


        TaskDialog.Show("Результат", message);
        return Result.Succeeded;
      }
      // This is where we "catch" potential errors and define how to deal with them
      catch (Autodesk.Revit.Exceptions.OperationCanceledException)
      {
        // If user decided to cancel the operation return Result.Canceled
        return Result.Cancelled;
      }
      catch (Exception ex)
      {
        // If something went wrong return Result.Failed
        message = ex.Message;
        return Result.Failed;
      }

    }
  }
}
