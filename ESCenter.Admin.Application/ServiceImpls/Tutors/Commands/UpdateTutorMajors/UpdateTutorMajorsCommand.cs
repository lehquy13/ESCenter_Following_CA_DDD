using Matt.SharedKernel.Application.Mediators.Commands;

namespace ESCenter.Admin.Application.ServiceImpls.Tutors.Commands.UpdateTutorMajors;

public record UpdateTutorMajorsCommand(Guid TutorId, List<int> MajorIds) : ICommandRequest;