using System;
using TestTasks.InternationalTradeTask.Models;

namespace TestTasks.InternationalTradeTask
{
    public class CommodityRepository
    {
        public double GetImportTariff(string commodityName)
        {
            return GetTariff(commodityName, c => c.ImportTarif);
        }

        public double GetExportTariff(string commodityName)
        {
            return GetTariff(commodityName, c => c.ExportTarif);
        }

        private FullySpecifiedCommodityGroup[] _allCommodityGroups = new FullySpecifiedCommodityGroup[]
        {
            new FullySpecifiedCommodityGroup("06", "Sugar, sugar preparations and honey", 0.05, 0)
            {
                SubGroups = new CommodityGroup[]
                {
                    new CommodityGroup("061", "Sugar and honey")
                    {
                        SubGroups = new CommodityGroup[]
                        {
                            new CommodityGroup("0611", "Raw sugar,beet & cane"),
                            new CommodityGroup("0612", "Refined sugar & other prod.of refining,no syrup"),
                            new CommodityGroup("0615", "Molasses", 0, 0),
                            new CommodityGroup("0616", "Natural honey", 0, 0),
                            new CommodityGroup("0619", "Sugars & syrups nes incl.art.honey & caramel"),
                        }
                    },
                    new CommodityGroup("062", "Sugar confy, sugar preps. Ex chocolate confy", 0, 0)
                }
            },
            new FullySpecifiedCommodityGroup("282", "Iron and steel scrap", 0, 0.1)
            {
                SubGroups = new CommodityGroup[]
                {
                    new CommodityGroup("28201", "Iron/steel scrap not sorted or graded"),
                    new CommodityGroup("28202", "Iron/steel scrap sorted or graded/cast iron"),
                    new CommodityGroup("28203", "Iron/steel scrap sort.or graded/tinned iron"),
                    new CommodityGroup("28204", "Rest of 282.0")
                }
            }
        };

        private double GetTariff(string commodityName, Func<ICommodityGroup, double?> tariffSelector)
        {
            double? tariffResult = null;
            var commodity = FindCommodityByName(commodityName, tariffSelector, ref tariffResult);
            if (commodity == null)
            {
                throw new ArgumentException($"Commodity '{commodityName}' not found.");
            }

            return tariffResult ?? 0;
        }

        private ICommodityGroup FindCommodityByName(string commodityName, Func<ICommodityGroup, double?> tariffSelector, ref double? result)
        {
            foreach (var group in _allCommodityGroups)
            {
                var commodity = FindCommodityInGroup(group, commodityName, tariffSelector, ref result);
                if (commodity != null)
                {
                    if (result is null)
                    {
                        result = tariffSelector(group);
                    }
                    return commodity;
                }
            }
            return null;
        }

        private ICommodityGroup FindCommodityInGroup(ICommodityGroup group, string commodityName, Func<ICommodityGroup, double?> tariffSelector, ref double? result)
        {
            if (group.Name.Equals(commodityName, StringComparison.OrdinalIgnoreCase))
            {
                var tariff = tariffSelector(group);
                if (tariff.HasValue && result is null)
                {
                    result = tariff.Value;
                }
                return group;
            }

            if (group.SubGroups != null)
            {
                foreach (var subGroup in group.SubGroups)
                {
                    var commodity = FindCommodityInGroup(subGroup, commodityName, tariffSelector, ref result);
                    if (commodity != null)
                    {
                        var tariff = tariffSelector(commodity);
                        if (tariff.HasValue && result is null)
                        {
                            result = tariff.Value;
                        }
                        return commodity;
                    }
                }
            }

            return null;
        }
    }
}
