using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;

namespace ESCenter.Domain.DomainModels.Tutors;

public record TutorProfileDomainModel(Tutor Tutor, User User);