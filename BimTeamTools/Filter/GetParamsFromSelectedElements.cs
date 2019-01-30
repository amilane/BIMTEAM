using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autodesk.Revit.DB;

namespace BimTeamTools
{
  static class GetParamsFromSelectedElements
  {
    public static SortedDictionary<string, StorageType> getParamsFromSelectedElements(List<List<Element>> allSelectedElems)
    {
      List<object>listOfParamsDictionaries = new List<object>();
      foreach (var type in allSelectedElems)
      {
        SortedDictionary<string, StorageType> tempDict = new SortedDictionary<string, StorageType>();
        var parameters = type.First().Parameters;
        foreach (Parameter p in parameters)
        {
          if (!tempDict.ContainsKey(p.Definition.Name) && (p.StorageType != StorageType.ElementId | (p.StorageType == StorageType.ElementId & p.Definition.Name == "Уровень")))
          {
            tempDict.Add(p.Definition.Name, p.StorageType);
          }
          listOfParamsDictionaries.Add(tempDict);
        }
      }

      SortedDictionary<string, StorageType> DictMerged = (SortedDictionary<string, StorageType>)listOfParamsDictionaries.First();
      foreach (SortedDictionary<string, StorageType> DictA in listOfParamsDictionaries)
      {
        var DictB = DictMerged.Intersect(DictA).ToDictionary(o => o.Key, o => o.Value);
        DictMerged = new SortedDictionary<string, StorageType>(DictB);
      }

      return DictMerged;
    }
  }
}
