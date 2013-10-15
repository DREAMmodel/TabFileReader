using System;

namespace Dream.IO
{
  class Program
  {
    [STAThread]
    static void Main(string[] args)
    {
      TabFileReader myTabFileReader = new TabFileReader(@"C:\tmp\test.tab"); //initialize tabfilereader
      int linenr = 0;
      while (myTabFileReader.ReadLine())
      {
        int a = myTabFileReader.GetInt32("firstcol");
        int b = myTabFileReader.GetInt32("secondcol");
        double c = myTabFileReader.GetDouble("thirdcol");

        Console.WriteLine("\rReading data, line #"+(++linenr)+" Firstcol: "+a+" Secondcol: "+b+" Thridcol: "+c);
        Console.Write("Press any key to read next line of data...");
        Console.ReadKey();
      }
      myTabFileReader.Close();

      Console.WriteLine("\rEnd of file. " + linenr + " lines in file. Press any key to exit...");
      Console.ReadKey();

    }
  }
}
