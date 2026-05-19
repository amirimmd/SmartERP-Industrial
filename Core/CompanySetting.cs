namespace SmartERP.Core;

public class CompanySetting
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string ManagerName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string LogoPath { get; set; } = string.Empty;
    public string Slogan { get; set; } = string.Empty;
    public decimal DefaultTaxRate { get; set; } = 0.09m;
}