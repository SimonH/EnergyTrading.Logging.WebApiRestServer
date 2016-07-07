using EnergyTrading.WebApi.Common.Server;

namespace EnergyTrading.Logging.WebApiRestServer.Logic
{
    public class SuppliedLoggerFactoryCreator : ICreateLoggerFactories
    {
        public ILoggerFactory CreateInstance()
        {
            return new SimpleLoggerFactory(new NullLogger());
        }
    }
}