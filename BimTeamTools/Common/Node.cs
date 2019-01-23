using System.Collections.ObjectModel;

namespace BimTeamTools
{
  public class Node
  {
    public string Name { get; set; }
    public ObservableCollection<Node> Nodes { get; set; }
  }
}
