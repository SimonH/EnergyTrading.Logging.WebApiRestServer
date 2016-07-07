using System;
using EnergyTrading.Configuration;
using EnergyTrading.Extensions;
using EnergyTrading.WebApi.Common.Faults;
using EnergyTrading.WebApi.Common.Server;

namespace EnergyTrading.Logging.WebApiRestServer.Logic
{
    public static class LoggerProvider
    {
        private static IConfigurationManager configurationManager;
        private static bool initialized;
        private static readonly object lockObject = new object();

        public static IConfigurationManager ConfigurationManager
        {
            get { return configurationManager ?? (configurationManager = new AppConfigConfigurationManager()); }
            set { configurationManager = value; }
        }

        public static ILogger GetLogger(string loggerType)
        {
            Initialise();
            return LoggerFactory.GetLogger(loggerType);
        }

        private static ICreateLoggerFactories GetFactoryCreator()
        {
            var creatorTypeString = ConfigurationManager.GetAppSettingValue("ICreateLoggerFactoriesType");
            if (string.IsNullOrWhiteSpace(creatorTypeString))
            {
                throw new InvalidOperationException("Cannot get value from ICreateLoggerFactoriesType AppSetting on Server");
            }
            var creatorType = creatorTypeString.ToType();
            var instance = Activator.CreateInstance(creatorType);
            var interfaceInstance = instance as ICreateLoggerFactories;
            if (interfaceInstance == null)
            {
                throw new InvalidOperationException("object created from ICreateLoggerFactoriesType AppSetting is not an instance of ICreateLoggerFactories");
            }
            return interfaceInstance;
        }

        private static void Initialise()
        {
            lock (lockObject)
            {
                if (!initialized)
                {
                    try
                    {
                        var factoryCreator = GetFactoryCreator();
                        var factory = factoryCreator.CreateInstance();
                        LoggerFactory.SetProvider(() => factory);
                        LoggerFactory.Initialize();
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(new Fault {ErrorMessage = exception.Message});
                    }
                    initialized = true;
                }
            }
        }
    }
}