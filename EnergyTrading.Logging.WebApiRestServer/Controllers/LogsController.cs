using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EnergyTrading.Contracts.Logging;
using EnergyTrading.Logging.WebApiRestServer.Logic;
using EnergyTrading.WebApi.Common.Faults;

namespace EnergyTrading.Logging.WebApiRestServer.Controllers
{
    public class LogsController : ApiController
    {
        [HttpPost]
        [Route("api/logs")]
        public HttpResponseMessage PostJob(LogMessage logMessage)
        {
            if (logMessage == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new Fault {ErrorMessage = "A posted LogMessage cannot be null"});
            }

            // TODO - Implement EnergyTrading.WebApi.Common.Server.ICreateLoggerFactories with a default constructor, put the type in an AppSetting named 'ICreateLoggerFactoriesType' and make your library available at runtime
            //        By Default the original code uses SuppliedLoggerFactoryCreator - an implementation that returns a SimpleLoggerFactory with a NullLogger
            try
            {
                new LogMessageHandler().AddLogMessage(logMessage);
            }
            catch (Exception exception)
            {
                var faultException = exception as FaultException;
                if (faultException != null)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, faultException.Fault);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Fault { ErrorMessage = "Unknown Error" });
                }
            }


            // TODO - or remove the code between the TODO comments and implement your own handling for log messages here. 

            return Request.CreateResponse(HttpStatusCode.Created, logMessage);
        }

    }
}