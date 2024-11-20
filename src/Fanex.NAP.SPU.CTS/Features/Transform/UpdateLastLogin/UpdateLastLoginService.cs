namespace Fanex.NAP.SPU.CTS.Features.UpdateLastLogin
{
    public interface IUpdateLastLoginService
    {
        void UpdateLastLoginTime(int batchSize);
    }

    public class UpdateLastLoginService : IUpdateLastLoginService
    {
        private readonly IUpdateLastLoginDataAccess dataAccess;

        public UpdateLastLoginService(IUpdateLastLoginDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public void UpdateLastLoginTime(int batchSize)
        {
            dataAccess.TransformUpdatingLastLoginTime(batchSize);
        }
    }
}
