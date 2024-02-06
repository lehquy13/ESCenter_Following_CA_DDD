namespace ESCenter.Application.Contract.Users.Tutors;

public record TutorProfileCreateDto(string AcademicLevel, string University, List<int> Majors);