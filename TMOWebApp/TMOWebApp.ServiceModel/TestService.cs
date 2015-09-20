using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;

namespace TMOWebApp.ServiceModel
{
    [Route("/datatest")]
    public class TestData : IReturn<TestResponse>
    {
        public string paramName { get; set; }
        public double paramValue { get; set; }
    }
    public class TestResponse
    {
        public string valueName { get; set; }
        public double value { get; set; }
    }


    [Route("/getpeaks")]
    public class PeakFinderInput : IReturn<PeakFinderOutput>
    {
        public List<XICDataPoint> xicData { get; set; }

    }

    public class PeakFinderOutput
    {
        private static PeakFinder _finder;
        public static PeakFinder Finder
        {
            get
            {
                if (_finder == null)
                    _finder = new PeakFinder();

                return _finder;

            }
        }
        public List<PeakData> peaklist { get; set; }
    }

    public class XICDataPoint
    {
        public int scanNumber { get; set; }
        public double intensity { get; set; }
    }

    public class PeakData
    {
        public PeakData(int size)
        {
            intensityValues = new double[size];
            this.startScan = 0;
            this.endScan = 0;

        }

        public PeakData(int startScan, int endScan, double[] intensityValues)
        {
            this.startScan = startScan;
            this.endScan = endScan;
            this.intensityValues = intensityValues;
        }

        public void AddPeak(int index, int scanNum, double intensity)
        {
            if (index > intensityValues.Length - 1)
                return;

            if (scanNum < startScan)
                startScan = scanNum;

            if (scanNum > endScan)
                endScan = scanNum;

            intensityValues[index] = intensity;
        }

        public double startScan { get; set; }
        public double endScan { get; set; }

        public double[] intensityValues { get; set; }
    }

    public class PeakFinder
    {

        public List<PeakData> GetPeaks(List<XICDataPoint> xicPoints)
        {

            if (xicPoints == null || xicPoints.Count == 0)
                return GetDummyPeaks();


            return FindPeaks(xicPoints, 2, 3);
        }

        private bool CheckPeak(List<XICDataPoint> xicPoints, int numPeakPoints, int midCursor)
        {
            if (xicPoints.Count < midCursor + numPeakPoints || midCursor == numPeakPoints)
                return false;

            for (int index = 0; index < numPeakPoints; index++)
            {
                if (xicPoints[midCursor + index + 1].intensity < xicPoints[midCursor - index].intensity &&
                    xicPoints[midCursor - index - 1].intensity < xicPoints[midCursor - index].intensity)
                    continue;
                else
                    return false;
            }

            return true;
        }

        private PeakData buildPeak(List<XICDataPoint> xicPoints, int numPeakPoints, int cursor)
        {
            if (cursor + numPeakPoints >= xicPoints.Count)
                return null;

            PeakData peak = new PeakData(numPeakPoints * 2 + 1);

            var peakIndex = 0;
            //for (int index = cursor - numPeakPoints; index <= cursor + numPeakPoints; index++)
            //{
            //    peak.AddPeak(peakIndex, xicPoints[index].scanNumber, xicPoints[index].intensity);
            //}

            int startPeakCursor = cursor - numPeakPoints;
            for (peakIndex = 0; peakIndex <= 2 * numPeakPoints; peakIndex++)
            {
                peak.AddPeak(peakIndex, xicPoints[startPeakCursor + peakIndex].scanNumber, xicPoints[startPeakCursor + peakIndex].intensity);
            }


            return peak;
        }
        private List<PeakData> FindPeaks(List<XICDataPoint> xicPoints, int numPeakPoints, int startCursor)
        {
            List<PeakData> peakList = new List<PeakData>();

            int cursor = startCursor;
            for (int index = startCursor; index < xicPoints.Count; index++)
            {
                if (cursor < numPeakPoints)
                {
                    cursor++;
                    continue;
                }

                if (CheckPeak(xicPoints, numPeakPoints, cursor))
                {
                    var peakData = buildPeak(xicPoints, numPeakPoints, cursor);
                    if (peakData == null)
                        break;

                    peakList.Add(peakData);
                    cursor = index + numPeakPoints - 1;
                }
                else
                    cursor++;
            }

            return peakList;
        }

        private List<PeakData> GetDummyPeaks()
        {
            List<PeakData> peakList = new List<PeakData>();
            PeakData peak = new PeakData(10, 20, new double[] { 23.4, 24.5, 27.8, 29.7, 23.4, 24.5, 27.8, 29.7, 27.8, 29.7 });
            peakList.Add(peak);
            peak = new PeakData(20, 22, new double[] { 29.7, 23.4, 24.5 });
            peakList.Add(peak);

            return peakList;
        }


    }


}