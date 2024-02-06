﻿using ESCenter.Application.Contract.Commons;
using ESCenter.Domain.Shared.Courses;

namespace ESCenter.Application.Contract.Courses.Dtos;

public class CourseRequestDto : BasicAuditedEntityDto<Guid>
{
    public Guid TutorId { get; set; }
    public string TutorName { get; set; } = string.Empty;
    public string TutorPhone { get; set; } = string.Empty;
    public string TutorEmail { get; set; } = string.Empty;
    public Guid CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending;
}