using ESCenter.Application.Contracts.Commons.Primitives;
using ESCenter.Application.Contracts.Commons.Primitives.Auditings;

namespace ESCenter.Application.Contracts.Users.Tutors;

public class TutorVerificationInfoDto : BasicAuditedEntityDto<int>
{
    public int TutorId { get; set; }
    public string Image { get; set; } = "doc_contract.png";
}