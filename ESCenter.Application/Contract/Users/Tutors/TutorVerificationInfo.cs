using ESCenter.Application.Contract.Commons;

namespace ESCenter.Application.Contract.Users.Tutors;

public class TutorVerificationInfoDto : BasicAuditedEntityDto<Guid>
{
    public Guid TutorId { get; set; }
    public string Image { get; set; } = "doc_contract.png";
}