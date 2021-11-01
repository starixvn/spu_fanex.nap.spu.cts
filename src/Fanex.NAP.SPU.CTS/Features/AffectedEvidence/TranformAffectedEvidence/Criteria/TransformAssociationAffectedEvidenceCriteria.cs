namespace Fanex.NAP.SPU.CTS.Features.AffectedEvidence.Criteria
{
    using Fanex.Data.Repository;

    public class TransformAssociationAffectedEvidenceCriteria : NonQueryCommand
    {
        public override string GetSettingKey()
        {
            return "CTS_DC_CustEvidence_InsertFromAssociation";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}