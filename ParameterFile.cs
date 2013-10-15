using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Dream.IO
{
  public class ParameterFile
  {

    #region Private fields
    string[] _parameterNames;
    double[,] _parameters;
    Hashtable _hash = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
    int _firstYear;
    int _lastYear;
    #endregion

    /// <summary>
    /// Parses a parameter text file with a head, time in first column and a subsequent 
    /// number of paramters in the next columns.
    /// </summary>
    /// <param name="fileName">txt-file</param>
    /// <param name="firstYear">Start Year</param>
    /// <param name="lastYear">End Year</param>
    public ParameterFile(string fileName, int firstYear, int lastYear)
    {
      //Console.WriteLine("Initializing...");

      _firstYear = firstYear;
      _lastYear = lastYear;

      TabFileReader file = null;

      try
      {
        file = new TabFileReader(fileName);
      }
      catch (Exception e)
      {
        throw new Exception("ParameterFile-error: " + e.Message);
      }

      //Console.WriteLine("Reading data from: " + fileName);

      string[] keyWords = file.Keywords;

      _parameterNames = new string[keyWords.Length - 1];
      for (int i = 1; i < keyWords.Length; i++)
      {
        _parameterNames[i - 1] = keyWords[i];
        _hash.Add(_parameterNames[i - 1], i - 1);
      }

      int t;
      int tCount = _firstYear;
      _parameters = new double[lastYear - firstYear + 1, keyWords.Length - 1];

      while (file.ReadLine())
      {
        t = file.GetInt32(keyWords[0]);

        if (t == tCount && t <= _lastYear)
        {
          for (int i = 1; i < keyWords.Length; i++)
          {
            _parameters[t - firstYear, i - 1] = file.GetDouble(keyWords[i]);
          }
          tCount++;
        }
        else if (t > tCount && t <= _lastYear)
        {
          for (int s = tCount; s < t; s++)
            for (int i = 1; i < keyWords.Length; i++)
            {
              if (s - firstYear > 0) _parameters[s - firstYear, i - 1] = _parameters[(s - 1) - firstYear, i - 1];
              else _parameters[0, i - 1] = 0;
            }
          for (int i = 1; i < keyWords.Length; i++)
          {
            _parameters[t - firstYear, i - 1] = file.GetDouble(keyWords[i]);
          }
          tCount = t + 1;
        }
        else tCount++;
      }

      file.Close();

      for (int s = tCount; s <= lastYear; s++)
        for (int i = 1; i < keyWords.Length; i++)
        {
          if (s - firstYear > 0) _parameters[s - firstYear, i - 1] = _parameters[(s - 1) - firstYear, i - 1];
          else _parameters[0, i] = 0;
        }

    }

    /// <summary>
    /// Returns a parameter value, for given year and parameter-name
    /// </summary>
    /// <param name="year">Year</param>
    /// <param name="ParameterName">Parameter name</param>
    /// <returns>System.Double</returns>
    public double GetParameter(int year, string ParameterName)
    {
      if (year < _firstYear || year > _lastYear)
      {
        string timeInterval = "[" + _firstYear.ToString() + "," + _lastYear.ToString() + "]";
        throw new Exception("*** Error. Indexs year is out of bound. Timeinterval is: " + timeInterval);
      }

      try
      {
        return _parameters[year - _firstYear, (int)_hash[ParameterName]];
      }
      catch
      {
        string nameList = null;
        for (int i = 0; i < _parameterNames.Length; i++) { nameList = nameList + " " + _parameterNames[i]; }
        throw new Exception("*** Error. No parameter named \"" + ParameterName + "\" exists. Parameters are: " + nameList);
      }
    }

    /// <summary>
    /// Returns a parameter value, for given year and parameter-index
    /// </summary>
    /// <param name="year">Year</param>
    /// <param name="parameterIndex">Parameter index</param>
    /// <returns>System.Double</returns>
    public double GetParameter(int year, int parameterIndex)
    {
      if (year < _firstYear || year > _lastYear)
      {
        string timeInterval = "[" + _firstYear.ToString() + "," + _lastYear.ToString() + "]";
        throw new Exception("*** Error. Indexs year is out of bound. Timeinterval is: " + timeInterval);
      }

      return _parameters[year - _firstYear, parameterIndex];

    }

    /// <summary>
    /// Returns the names of all the parameters in a string-array
    /// </summary>
    /// <returns>System.string</returns>
    public string[] GetParameterNames()
    {
      return _parameterNames;
    }

  }
}
