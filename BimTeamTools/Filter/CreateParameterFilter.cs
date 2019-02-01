using System;
using Autodesk.Revit.DB;

namespace BimTeamTools.Filter
{
  static class CreateParameterFilter
  {
    public static ElementParameterFilter createParameterFilter(FilteredElementCollector collector, ParameterData parameter, string operation, string ruleString)
    {
      ElementId parameterId = parameter.id;
      ParameterValueProvider fvp = new ParameterValueProvider(parameterId);
      StorageType storageType = parameter.storageType;
      FilterRule fRule = null;
      FilterInverseRule fInvRule = null;
      ElementParameterFilter filter = null;


      switch (storageType)
      {
        case StorageType.String:
          FilterStringRuleEvaluator fsre = null;
          switch (operation)
          {
            case "равно":
              fsre = new FilterStringEquals();
              fRule = new FilterStringRule(fvp, fsre, ruleString, true);
              filter = new ElementParameterFilter(fRule);
              break;
            case "не равно":
              fsre = new FilterStringEquals();
              fRule = new FilterStringRule(fvp, fsre, ruleString, true);
              fInvRule = new FilterInverseRule(fRule);
              filter = new ElementParameterFilter(fInvRule);
              break;
            case "содержит":
              fsre = new FilterStringContains();
              fRule = new FilterStringRule(fvp, fsre, ruleString, true);
              filter = new ElementParameterFilter(fRule);
              break;
            case "не содержит":
              fsre = new FilterStringContains();
              fRule = new FilterStringRule(fvp, fsre, ruleString, true);
              fInvRule = new FilterInverseRule(fRule);
              filter = new ElementParameterFilter(fInvRule);
              break;
            case "начинается с":
              fsre = new FilterStringBeginsWith();
              fRule = new FilterStringRule(fvp, fsre, ruleString, true);
              filter = new ElementParameterFilter(fRule);
              break;
            case "не начинается с":
              fsre = new FilterStringBeginsWith();
              fRule = new FilterStringRule(fvp, fsre, ruleString, true);
              fInvRule = new FilterInverseRule(fRule);
              filter = new ElementParameterFilter(fInvRule);
              break;
            case "заканчивается на":
              fsre = new FilterStringEndsWith();
              fRule = new FilterStringRule(fvp, fsre, ruleString, true);
              filter = new ElementParameterFilter(fRule);
              break;
            case "не заканчивается на":
              fsre = new FilterStringEndsWith();
              fRule = new FilterStringRule(fvp, fsre, ruleString, true);
              fInvRule = new FilterInverseRule(fRule);
              filter = new ElementParameterFilter(fInvRule);
              break;
          }
          break;
        case StorageType.Double:
          FilterNumericRuleEvaluator fnre = null;
          double ruleValue = Convert.ToDouble(ruleString);
          switch (operation)
          {
            case "равно":
              fnre = new FilterNumericEquals();
              fRule = new FilterDoubleRule(fvp, fnre, ruleValue, 0.0);
              break;
          }
          break;








      }









      
      
      
      
    }
  }
}
