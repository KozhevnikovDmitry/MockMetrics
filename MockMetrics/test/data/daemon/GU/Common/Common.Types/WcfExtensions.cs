using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Common.Types.Extensions
{
    public static class WcfExtensions
    {
        /// <remarks>
        /// http://www.codeproject.com/Tips/197531/Do-not-use-using-for-WCF-Clients
        /// </remarks>
        public static void Using<T>(this T client, Action<T> work)
            where T : ICommunicationObject
        {
            try
            {
                work(client);
                client.Close();
            }
            catch (CommunicationException)
            {
                client.Abort();
            }
            catch (TimeoutException)
            {
                client.Abort();
            }
            catch (Exception)
            {
                client.Abort();
                throw;
            }
        }

        /// <summary>
        /// WCF proxys do not clean up properly if they throw an exception. This method ensures that the service proxy is handeled correctly.
        /// Do not call TService.Close() or TService.Abort() within the action lambda.
        /// </summary>
        /// <typeparam name="TService">The type of the service to use</typeparam>
        /// <param name="action">Lambda of the action to performwith the service</param>
        /// <remarks>
        /// http://nimtug.org/blogs/damien-mcgivern/archive/2009/05/26/wcf-communicationobjectfaultedexception-quot-cannot-be-used-for-communication-because-it-is-in-the-faulted-state-quot-messagesecurityexception-quot-an-error-occurred-when-verifying-security-for-the-message-quot.aspx
        /// </remarks>
        public static void Using<TService>(Action<TService> action)
            where TService : ICommunicationObject, IDisposable, new()
        {
            var service = new TService();
            bool success = false;
            try
            {
                action(service);
                if (service.State != CommunicationState.Faulted)
                {
                    service.Close();
                    success = true;
                }
            }
            finally
            {
                if (!success)
                {
                    service.Abort();
                }
            }
        }
    }
}
