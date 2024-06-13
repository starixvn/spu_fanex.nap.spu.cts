namespace Fanex.NAP.SPU.CTS.Tests.Infrastructure.Consts
{
    public struct AssociationDetectionConsts
    {
        public struct JsonString
        {
            public const string GetAssociationCust = "[{\"O\":321,\"D\":123,\"A\":1},{\"O\":322,\"D\":124,\"A\":1},{\"O\":323,\"D\":125,\"A\":1},{\"O\":324,\"D\":126,\"A\":1},{\"O\":325,\"D\":127,\"A\":1}]";

            public const string GetAssociationCustLv1 = "[{\"O\":321,\"D\":123},{\"O\":322,\"D\":124},{\"O\":323,\"D\":125},{\"O\":324,\"D\":126},{\"O\":325,\"D\":127}]";
        }

        public struct PrivateMethodName
        {
            public const string GetAssociationCustByAgent = "GetAssociationCustByAgent";

            public const string GetAssociationCustByPrimary = "GetAssociationCustByPrimary";

            public const string GetAssociationCustByPrimaryLv1 = "GetAssociationCustByPrimaryLv1";

            public const string GetAssociationBySharedMatches = "GetAssociationBySharedMatches";
        }
    }
}
