using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.IO;


namespace Dream.IO
{
  public class TabFile
  {

    public static string NumberLabel = "Number";
    public static string ExtraColumnValue = "";
    public static string ExtraColumnLabel = "";


    /// <summary>
    /// TabFile.Put coverts a double or integer array to a tab-seperated file  
    /// </summary>
    /// <param name="fileName">Name of tab-seperated output file</param>
    /// <param name="arr">Input array. Have to be a double or integer array</param>
    /// <param name="headerNames">Header names placed in the first row of the tab-file</param>
    public static void Put(string fileName, Array arr, params string[] headerNames)
    {
      string[][] label = new string[arr.Rank][];

      Put(fileName, arr, label, headerNames);

    }

    /// <summary>
    /// TabFile.Put coverts a double or integer array to a tab-seperated file  
    /// </summary>
    /// <param name="fileName">Name of tab-seperated output file</param>
    /// <param name="arr">Input array. Have to be a double or integer array</param>
    /// <param name="label">Ragged array containing the labels. Example: label[2] = new string[]{"Male", "Female"}</param>
    /// <param name="headerNames">Header names placed in the first row of the tab-file</param>
    public static void Put(string fileName, Array arr, string[][] label, params string[] headerNames)
    {

      StreamWriter file = new StreamWriter(fileName, false, Encoding.UTF8);

      if (arr.Rank != headerNames.Length)

        if (arr.Rank != headerNames.Length)
          throw new ApplicationException("TabFile.Put error: Length of headerNames-array should be equal to rank of arr-array");

      string header = "";

      for (int i = 0; i < headerNames.Length; i++)
        header += headerNames[i] + "\t";

      header += NumberLabel;

      if (ExtraColumnLabel != "")
        header += "\t" + ExtraColumnLabel;

      file.WriteLine(header);

      int[] c = new int[arr.Rank];
      int[] indices = new int[arr.Rank];

      Type type = arr.GetValue(indices).GetType();

      c[0] = 1;

      for (int i = 1; i < arr.Rank; i++)
        c[i] = c[i - 1] * arr.GetLength(i - 1);

      int N = c[arr.Rank - 1] * arr.GetLength(arr.Rank - 1);

      string line = "";
      for (int i = 0; i < N; i++)
      {
        for (int j = 0; j < arr.Rank; j++)
          indices[j] = (i / c[j]) % arr.GetLength(j);

        Object obj = arr.GetValue(indices);

        if (type == typeof(int))
        {
          if ((int)obj == 0)
            continue; //print ikke 0-vædier
        }
        else if (type == typeof(double))
        {
          if ((double)obj == 0)
            continue; //print ikke 0-vædier
        }
        else if (type != typeof(bool))
          break; //break hvis array er af ukendt type

        line = "";
        for (int j = 0; j < arr.Rank; j++)
        {
          if (label[j] == null)
            line += indices[j].ToString() + "\t";
          else
            line += label[j][indices[j]] + "\t";
        }

        line += obj.ToString();

        if (ExtraColumnLabel != "")
          line += "\t" + ExtraColumnValue;

        file.WriteLine(line);
      }

      file.Close();

    }


  }
}
