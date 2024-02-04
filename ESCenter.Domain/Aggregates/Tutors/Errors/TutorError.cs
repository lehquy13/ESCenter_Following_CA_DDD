namespace ESCenter.Domain.Aggregates.Tutors.Errors;

public static class TutorError
{
    public static string ImageUrlCannotBeEmpty => "Image url cannot be empty";
    public static string InvalidImageUrls => "Can't create change verification request with invalid image urls";
}