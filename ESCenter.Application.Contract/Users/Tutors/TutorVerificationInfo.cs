using ESCenter.Application.Contract.Commons.Primitives.Auditings;

namespace ESCenter.Application.Contract.Users.Tutors;

public class TutorVerificationInfoDto : BasicAuditedEntityDto<int>
{
    public int TutorId { get; set; }
    public string Image { get; set; } = "doc_contract.png";
}