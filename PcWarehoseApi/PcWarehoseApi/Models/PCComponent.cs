namespace PcWarehouseApi.Models;

public class PCComponent
{
    public int PCId { get; set; }
    public PC PC { get; set; } = null!;

    public string ComponentCode { get; set; } = string.Empty;
    public Component Component { get; set; } = null!;

    public int Amount { get; set; }
}