using Mafi;

namespace ExampleMod;

public struct StatsSummery
{
    public PartialQuantity TotalMaintenancePerMonth { get; set; }
    public Electricity TotalPowerRequired { get; set; }
    public int TotalWorkersAssigned { get; set; }
    
    public void IncrementTotalMaintenancePerMonth(PartialQuantity maintenancePerMonth) => this.TotalMaintenancePerMonth += maintenancePerMonth;
    public void IncrementTotalPowerRequired(Electricity powerRequired) => this.TotalPowerRequired += powerRequired;
    public void IncrementTotalWorkersAssigned() => this.TotalWorkersAssigned++;
}