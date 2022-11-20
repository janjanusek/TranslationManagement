namespace TranslationManagement.Dal.Model;

public class TranslationJob
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public JobStatus Status { get; set; }
    public string OriginalContent { get; set; }
    public string TranslatedContent { get; set; }
    public double Price { get; set; }

    public TranslationJob()
    {
        
    }

    public TranslationJob(int id, string customerName, JobStatus status, string originalContent, string translatedContent, double price)
    {
        Id = id;
        CustomerName = customerName;
        Status = status;
        OriginalContent = originalContent;
        TranslatedContent = translatedContent;
        Price = price;
    }
    
    public void SetPrice(double pricePerCharacter) => Price = OriginalContent.Length * pricePerCharacter;

    public bool TryChangeStatus(JobStatus status)
    {
        var validChange =
            (Status == JobStatus.New && status == JobStatus.Completed) || Status == JobStatus.Completed || status == JobStatus.New;
        validChange &= status != JobStatus.None;

        if (validChange)
            Status = status;
        
        return validChange;
    }
}