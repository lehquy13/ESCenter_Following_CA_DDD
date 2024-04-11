﻿using ESCenter.Domain.Aggregates.Tutors.Errors;
using ESCenter.Domain.Aggregates.Tutors.ValueObjects;
using ESCenter.Domain.Shared.Courses;
using Matt.SharedKernel.Domain.Primitives.Auditing;

namespace ESCenter.Domain.Aggregates.Tutors.Entities;

public class
    ChangeVerificationRequest : AuditedEntity<ChangeVerificationRequestId> 
{
    private List<ChangeVerificationRequestDetail> _changeVerificationRequestDetails = new();
    public TutorId TutorId { get; private set; } = null!;

    public RequestStatus RequestStatus { get; private set; }

    public IReadOnlyCollection<ChangeVerificationRequestDetail> ChangeVerificationRequestDetails
        => _changeVerificationRequestDetails.AsReadOnly();

    private ChangeVerificationRequest()
    {
    }

    public static ChangeVerificationRequest Create(TutorId tutorId, List<string> urls)
    {
        if (urls.Count < 1)
        {
            throw new ArgumentException(TutorDomainError.InvalidImageUrls);
        }

        return new ChangeVerificationRequest()
        {
            Id = ChangeVerificationRequestId.Create(),
            TutorId = tutorId,
            RequestStatus = RequestStatus.Pending,
            _changeVerificationRequestDetails = urls
                .Select(ChangeVerificationRequestDetail.Create)
                .ToList()
        };
    }

    public void Approve()
    {
        RequestStatus = RequestStatus.Approved;
    }

    public void Reject()
    {
        RequestStatus = RequestStatus.Canceled;
    }
}