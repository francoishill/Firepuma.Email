namespace Firepuma.Email.Client.Services.Results;

public class EnqueueEmailResult
{
    public bool IsSuccessful { get; set; }

    public FailureReason? FailedReason { get; set; }
    public string[] FailedErrors { get; set; }

    private EnqueueEmailResult(
        bool isSuccessful,
        FailureReason? failedReason,
        string[] failedErrors)
    {
        IsSuccessful = isSuccessful;
        FailedReason = failedReason;
        FailedErrors = failedErrors;
    }

    public static EnqueueEmailResult Success()
    {
        return new EnqueueEmailResult(true, null, null);
    }

    public static EnqueueEmailResult Failed(FailureReason reason, params string[] errors)
    {
        return new EnqueueEmailResult(false, reason, errors);
    }

    public enum FailureReason
    {
        InputValidationFailed,
    }
}