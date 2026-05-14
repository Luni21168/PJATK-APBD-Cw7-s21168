namespace PcWarehouseApi.DTOs;

public class PcWithComponentsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Weight { get; set; }
    public int Warranty { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Stock { get; set; }

    public List<PcAssignedComponentDto> Components { get; set; } = new();
}

public class PcAssignedComponentDto
{
    public int Amount { get; set; }
    public ComponentDetailsDto Component { get; set; } = new();
}

public class ComponentDetailsDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ManufacturerDto Manufacturer { get; set; } = new();
    public ComponentTypeDto Type { get; set; } = new();
}

public class ManufacturerDto
{
    public int Id { get; set; }
    public string Abbreviation { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateOnly FoundationDate { get; set; }
}

public class ComponentTypeDto
{
    public int Id { get; set; }
    public string Abbreviation { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}