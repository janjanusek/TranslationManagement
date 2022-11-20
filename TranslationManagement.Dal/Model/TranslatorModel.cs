namespace TranslationManagement.Dal.Model;

public class TranslatorModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string HourlyRate { get; set; }
    public string Status { get; set; }
    public string CreditCardNumber { get; set; }

    public TranslatorModel()
    {
        
    }

    public TranslatorModel(int id)
    {
        Id = id;
    }
}