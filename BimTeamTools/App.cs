using System;
using System.IO;
using System.Reflection;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;


namespace BimTeamTools
{
  class App : IExternalApplication
  {
    public static int Security() // Метод по защите программы в зависимости от контрольной даты
    {
      DateTime ControlDate = new DateTime(2019, 04, 30, 12, 0, 0); // Формируем контрольную дату в формате: год, месяц, день, часы, минуты, секунды
      DateTime dt = DateTime.Today; // получаем сегодняшнюю дату в таком же формате
      TimeSpan deltaDate = ControlDate - dt; // вычисляем разницу между контрольной и текущей датой
      string deltaDateDays = deltaDate.Days.ToString();
      int deltaDateDaysInt = Convert.ToInt32(deltaDateDays);
      if (deltaDateDaysInt < 1)
      {
        TaskDialog.Show("Истёк срок действия программы BimTeamTools", String.Concat("Срок действия программы BimTeamTools истёк", ".\r\nЧтобы избежать появления данного предупреждения обратитесь к разработчику\r\nили удалите программу BimTeamTools в панели управления - программы и компоненты.\r\nСпасибо за использование программы."));
      }
      return deltaDateDaysInt;
    }

    // Метод получает путь к папке где лежит исполняемая библиотека dll
    public static string GetExeDirectory()
    {
      string codeBase = Assembly.GetExecutingAssembly().CodeBase;
      UriBuilder uri = new UriBuilder(codeBase);
      string path = Uri.UnescapeDataString(uri.Path);
      path = Path.GetDirectoryName(path);
      return path;
    }


    static void AddRibbonPanel(UIControlledApplication application)
    {
      // Create a custom ribbon tab
      string tabName = "BimTeamTools";
      application.CreateRibbonTab(tabName);

      // Add a new ribbon panel
      RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "Общее");

      // Get dll assembly path
      string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

      // create push button for TotalLength
      PushButtonData b1Data = new PushButtonData(
          "cmdTotalLength",
          "Посчитать" + System.Environment.NewLine + "  длину  ",
          thisAssemblyPath,
          "BimTeamTools.TotalLength");

      PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;
      pb1.ToolTip = "Подсчет длины выбранных элементов";
      BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/BimTeamTools;component/Resources/TotalLengthIcon.png"));
      pb1.LargeImage = pb1Image;

    }
    public Result OnShutdown(UIControlledApplication application)
    {
      // do nothing
      return Result.Succeeded;
    }

    public Result OnStartup(UIControlledApplication application)
    {
      // call our method that will load up our toolbar
      AddRibbonPanel(application);
      return Result.Succeeded;
    }


  }
}
