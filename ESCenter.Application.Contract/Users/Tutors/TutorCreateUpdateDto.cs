using ESCenter.Application.Contract.Commons.Primitives;
using ESCenter.Application.Contract.Users.BasicUsers;

namespace ESCenter.Application.Contract.Users.Tutors;

public class TutorCreateUpdateDto : EntityDto<Guid>
{
    public UserForCreateDto UserForCreateDto { get; set; } = null!;
    public TutorProfileCreateDto TutorForCreateUpdateDto { get; set; } = null!;
}