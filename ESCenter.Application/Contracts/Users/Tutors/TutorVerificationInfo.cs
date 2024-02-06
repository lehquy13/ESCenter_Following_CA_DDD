using ESCenter.Application.Contracts.Commons;

namespace ESCenter.Application.Contracts.Users.Tutors;

public class TutorVerificationInfoDto : BasicAuditedEntityDto<Guid>
{
    public Guid TutorId { get; set; }
    public string Image { get; set; } = "doc_contract.png";
}