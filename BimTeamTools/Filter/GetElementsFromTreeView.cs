using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autodesk.Revit.DB;

namespace BimTeamTools
{
  static class GetElementsFromTreeView
  {
    public static List<List<Element>> getElementsFromTreeView(Document doc, ObservableCollection<Node> nodes,
      List<Element> selectedElements)
    {
      List<List<Element>> allElems = new List<List<Element>>();
      foreach (var category in nodes)
      {
        foreach (var family in category.Children)
        {
          foreach (var familyType in family.Children)
          {
            if (familyType.IsChecked == true)
            {
              var familySymbol = selectedElements.Where(i =>
                i.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString() ==
                String.Format("{0}: {1}", family.Text, familyType.Text)).ToList();
              allElems.Add(familySymbol);
            }
          }
        }
      }


      return allElems;
    }
  }
}
