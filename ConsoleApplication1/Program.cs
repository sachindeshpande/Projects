using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadWriteCsv;
using Utilities;


namespace ConsoleApplication1
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    CsvRow row = new CsvRow();
        //    CsvFileReader reader = new CsvFileReader("C:\\temp\\XICData.csv");
        //    while (reader.ReadRow(row))
        //    {
        //        Console.Out.WriteLine(row[0] + " -> " +  row[1]);
        //    }

        //    Console.Read();

        //}

        static void Main(string[] args)
        {

            List<XICDataPoint> xicData = new List<XICDataPoint>();

            CsvRow row = new CsvRow();
            CsvFileReader reader = new CsvFileReader("C:\\temp\\XICData.csv");
            while (reader.ReadRow(row))
            {
                Console.Out.WriteLine(row[0] + " -> " + row[1]);
                int scanNumber;
                double intensity;

                if (int.TryParse(row[0], out scanNumber) && double.TryParse(row[1], out intensity))
                    xicData.Add(new XICDataPoint { scanNumber = scanNumber, intensity = intensity });
            }

            PeakFinder finder = new PeakFinder();
            foreach (var peakData in finder.GetPeaks(xicData))
            {
                Console.Out.WriteLine(peakData.startScan + " -> " , peakData.endScan);
                foreach (var intensity in peakData.intensityValues)
                {
                    Console.Out.WriteLine("Intensity = " + intensity);
                }
            }

            Console.Read();

        }

    }
}
