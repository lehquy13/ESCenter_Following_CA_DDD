using Matt.ResultObject;

namespace ESCenter.Domain.Aggregates.Tutors.Errors;

public static class TutorDomainError
{
    public static string ImageUrlCannotBeEmpty => "Image url cannot be empty";
    public static Error TutorNotFound { get; } = new("TutorNotFound", "Tutor not found");
    public static string InvalidImageUrls => "Can't create change verification request with invalid image urls";
}