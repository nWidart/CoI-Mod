using Mafi;

namespace ExampleMod;

public struct StatsSummery
{
    public PartialQuantity TotalMaintenancePerMonth { get; private set; }
    public Electricity TotalPowerRequired { get; private set; }
    public int TotalWorkersAssigned { get; private set; }

    public void IncrementTotalMaintenancePerMonth(PartialQuantity maintenancePerMonth) => TotalMaintenancePerMonth += maintenancePerMonth;
    public void IncrementTotalPowerRequired(Electricity powerRequired) => TotalPowerRequired += powerRequired;
    public void IncrementTotalWorkersAssigned(int workersNeeded) => TotalWorkersAssigned += workersNeeded;

    public override string ToString() =>
        $"Total maintenance costs/month: {this.TotalMaintenancePerMonth}, power required: {this.TotalPowerRequired}, workers assigned: {this.TotalWorkersAssigned}";
}