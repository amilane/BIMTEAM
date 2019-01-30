using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace BimTeamTools.Filter
{
  static class ValuesFromParameter
  {
    public static List<string> valuesFromParameter(string parameterName, List<List<Element>> allSelectedElems)
    {
      List<string> values = new List<string>();
      foreach (var type in allSelectedElems)
      {
        foreach (var element in type)
        {
          Parameter p = element.LookupParameter(parameterName);
          string value = "";
          StorageType storageType = p.StorageType;
          switch (storageType)
          {
            case StorageType.Double:
            case StorageType.Integer:
            case StorageType.ElementId:
              value = p.AsValueString();
              break;
            case StorageType.String:
              value = p.AsString();
              break;
          }

          if (value != "")
          {
            values.Add(value);
          }
        }
      }

      List<string> uniqueValues = values.Distinct().ToList();
      return uniqueValues;
    }
  }
}
