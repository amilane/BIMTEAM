using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace BimTeamTools
{
  static class GetParamsFromSelectedElements
  {
    public static SortedDictionary<string, ParameterData> getParamsFromSelectedElements(FilteredElementCollector collector)
    {
      List<object> listOfParamsDictionaries = new List<object>();
      var families = collector.GroupBy(e => e.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString());
      foreach (var f in families)
      {
        Dictionary<string, ParameterData> tempDict = new Dictionary<string, ParameterData>();
        var parameters = f.First().Parameters;
        foreach (Parameter p in parameters)
        {
          ParameterData pd = new ParameterData();
          if (!tempDict.ContainsKey(p.Definition.Name) && p.StorageType != StorageType.ElementId | (p.StorageType == StorageType.ElementId & p.Definition.Name == "Уровень"))
          {
            
            pd.name = p.Definition.Name;
            pd.storageType = p.StorageType;
            pd.id = p.Id;

            tempDict.Add(p.Definition.Name, pd);
          }
        }
        listOfParamsDictionaries.Add(tempDict);
      }

      var DictMerged = (Dictionary<string, ParameterData>)listOfParamsDictionaries.First();
      foreach (Dictionary<string, ParameterData> DictA in listOfParamsDictionaries)
      {
        var DictB = DictA.Where(i => DictMerged.Keys.Contains(i.Key)).ToDictionary(o => o.Key, o => o.Value);
        DictMerged = DictB;
      }

      return new SortedDictionary<string, ParameterData>(DictMerged);


    }
  }
}
