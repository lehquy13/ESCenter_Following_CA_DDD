using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Users;
using Matt.SharedKernel.Domain.Interfaces;
using MediatR;

namespace ESCenter.Application.EventHandlers;

public class RegisteredAsTutorEventHandler(
    IUnitOfWork unitOfWork,
    ISubjectRepository subjectRepository,
    ITutorRepository tutorRepository)
    : INotificationHandler<RegisteredAsTutorDomainEvent>
{
    public async Task Handle(RegisteredAsTutorDomainEvent registeredAsTutorDomainEvent,
        CancellationToken cancellationToken)
    {
        var tutor = Tutor.Create(
            registeredAsTutorDomainEvent.CustomerId,
            registeredAsTutorDomainEvent.AcademicLevel,
            registeredAsTutorDomainEvent.University,
            registeredAsTutorDomainEvent.VerificationInfoDtos,
            false
        );

        // Handle major
        var subjects = await subjectRepository
            .GetListByIdsAsync(registeredAsTutorDomainEvent
                    .Majors
                    .Select(SubjectId.Create)
                    .ToList(),
                cancellationToken);

        var tutorMajors = subjects
            .Select(x => TutorMajor.Create(tutor.Id, x.Id, x.Name))
            .ToList();

        tutor.UpdateAllMajor(tutorMajors);
        
        await tutorRepository.InsertAsync(tutor, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}