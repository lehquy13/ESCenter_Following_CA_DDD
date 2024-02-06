using ESCenter.Application.Contract.Users.BasicUsers;
using Matt.SharedKernel.Application.Contracts.Primitives;

namespace ESCenter.Application.Contract.Users.Tutors;

public class TutorCreateUpdateDto : EntityDto<Guid>
{
    public UserForCreateDto UserForCreateDto { get; set; } = null!;
    public TutorProfileCreateDto TutorForCreateUpdateDto { get; set; } = null!;
}

public record TutorProfileCreateDto(string AcademicLevel, string University, List<int> Majors);
