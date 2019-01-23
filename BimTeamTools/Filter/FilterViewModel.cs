using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BimTeamTools
{
  class FilterViewModel : INotifyPropertyChanged
  {

    public UIApplication UIAPP = null;
    private Application APP = null;
    private UIDocument UIDOC = null;
    public Document DOC = null;


    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName]string prop = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public ICommand RetrieveParametersValuesCommand {
      get {
        return new DelegateCommand(GenerateParametersAndValues);
      }
    }

    //private List<string> _Elems;
    //public List<string> Elems {
    //  get {
    //    return _Elems;
    //  }
    //  set {
    //    _Elems = value;
    //  }
    //}


    private ObservableCollection<Node> _ModelElements;
    public ObservableCollection<Node> ModelElements {
      get {
        return _ModelElements;
      }
      set {
        _ModelElements = value;
      }
    }


    // действие по кнопке OK
    public void GenerateParametersAndValues()
    {

    }


  }
}
