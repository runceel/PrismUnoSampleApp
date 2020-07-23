using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.Infrastructures
{
    public static class RegionNames
    {
        public static string TopMenuRegion { get; } = nameof(TopMenuRegion);
        public static string DetailsRegion { get; } = nameof(DetailsRegion);
        public static string MasterRegion { get; } = nameof(MasterRegion);
    }

    public class RegionNamesForXAML
    {
        public string TopMenuRegion => RegionNames.TopMenuRegion;
        public string DetailsRegion => RegionNames.DetailsRegion;
        public string MasterRegion => RegionNames.MasterRegion;
    }
}
