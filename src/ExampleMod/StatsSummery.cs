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

    public override string ToString() =>
        $"Total maintenance costs/month: {this.TotalMaintenancePerMonth}, power required: {this.TotalPowerRequired}, workers assigned: {this.TotalWorkersAssigned}";
}