using System;

namespace BimTeamTools
{
  /// <summary>
  /// Логика взаимодействия для FilterControl.xaml
  /// </summary>
  public partial class FilterControl : IDisposable
  {
    public FilterControl()
    {
      InitializeComponent();
    }

    public void Dispose()
    {
      this.Close();
    }

  }
}
