﻿using Matt.SharedKernel.Application.Contracts.Primitives;

namespace ESCenter.Application.Contract.Courses.Dtos;

public class SubjectDto : EntityDto<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}