using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;
using TMOWebApp.ServiceModel;

namespace TMOWebApp.ServiceInterface
{
    public class MyServices : Service
    {
        public object Any(TestData request)
        {
            return new TestResponse { valueName = "Test Value", value = 25 * request.paramValue};
        }

        //public object Any(PeakFinderInput request)
        //{

        //    return new PeakFinderOutput { peaklist = GetPeaks(request) };
        //}

        public object Any(PeakFinderInput request)
        {
            return new PeakFinderOutput { peaklist = PeakFinderOutput.Finder.GetPeaks(request.xicData) };
        }




        //static void main(String[] args)
        //{
        //    PeakFinderInput input = new PeakFinderInput();
        //    input.xicData = new List<XICDataPoint>();


        //}

    }
}