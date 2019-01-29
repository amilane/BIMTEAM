using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autodesk.Revit.DB;

namespace BimTeamTools
{
  class GetParamsFromSelectedElements
  {
    /*
    public List<string> getParamsFromSelectedElements(Document doc, ObservableCollection<Node> nodes, List<Element> selectedElements)
    {

      List<List<string>> paramsOfFamilies = new List<List<string>>();
      foreach (var category in nodes)
      {
        foreach (var family in category.Children)
        {
          foreach (var familyType in family.Children)
          {
            List<string> parNamesOfFamilyType = new List<string>();
            if (familyType.IsChecked == true)
            {
              var familySymbol = selectedElements.
                Where(i => i.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString() == String.Format("{0}: {1}", family.Text, familyType.Text)).First();

              var parameters = familySymbol.Parameters;
              foreach (Parameter p in parameters)
              {
                parNamesOfFamilyType.Add(p.Definition.Name);
              }
              paramsOfFamilies.Add(parNamesOfFamilyType);
            }
          }
        }
      }

      var commonParameterNames = IntersectAll(paramsOfFamilies);
      return commonParameterNames;

    }

    //Get unique parameters of families
    public List<T> IntersectAll<T>(IEnumerable<IEnumerable<T>> lists)
    {
      HashSet<T> hashSet = null;
      foreach (var list in lists)
      {
        if (hashSet == null)
        {
          hashSet = new HashSet<T>(list);
        }
        else
        {
          hashSet.IntersectWith(list);
        }
      }
      return hashSet == null ? new List<T>() : hashSet.OrderBy(i => i).ToList();
    }
    */

    public SortedDictionary<string, StorageType> getParamsFromSelectedElements(Document doc, ObservableCollection<Node> nodes, List<Element> selectedElements)
    {

      SortedDictionary<string, StorageType> paramsOfFamilies = new SortedDictionary<string, StorageType>();
      foreach (var category in nodes)
      {
        foreach (var family in category.Children)
        {
          foreach (var familyType in family.Children)
          {
            if (familyType.IsChecked == true)
            {
              var familySymbol = selectedElements.
                Where(i => i.get_Parameter(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM).AsValueString() == String.Format("{0}: {1}", family.Text, familyType.Text)).First();

              var parameters = familySymbol.Parameters;
              foreach (Parameter p in parameters)
              {
                if (!paramsOfFamilies.ContainsKey(p.Definition.Name))
                {
                  paramsOfFamilies.Add(p.Definition.Name, p.StorageType);
                }
              }
            }
          }
        }
      }

      return paramsOfFamilies;

    }
    
  }
}
