namespace Core6
{
    using System.Collections.Generic;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Support;

    class ForInstallationOnReplacement
    {
        async Task Simple(EndpointConfiguration endpointConfiguration)
        {
            #region Installers

            endpointConfiguration.EnableInstallers();

            // this will run the installers
            await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            #endregion
        }
    }

    class SwitchInstallersWithCommandline
    {
        static EndpointConfiguration endpointConfiguration = new EndpointConfiguration("someEndpoint");

        #region InstallersRunWhenNecessaryCommandLine

        public static void Main(string[] args)
        {
            var runInstallers = args.Length == 1 && args[0] == "/runInstallers";

            if (runInstallers)
            {
                endpointConfiguration.EnableInstallers();
            }
        }

        #endregion
    }

    class SwitchInstallersByMachineNameConvention
    {
        async Task Simple(EndpointConfiguration endpointConfiguration)
        {
            #region InstallersRunWhenNecessaryMachineNameConvention

            if (!RuntimeEnvironment.MachineName.EndsWith("-PROD"))
            {
                endpointConfiguration.EnableInstallers();
            }

            #endregion
        }
    }

    class SwitchInstallersByADMemberShip
    {
        async Task Simple(EndpointConfiguration endpointConfiguration)
        {
            #region InstallersRunWhenNecessaryADMembership

            if (GetGroupNames("MyUser", "MyDomain").All(u => u != "ProdGroup"))
            {
                endpointConfiguration.EnableInstallers();
            }

            #endregion
        }

        #region InstallersRunWhenNecessaryADMembershipQuery
        static List<string> GetGroupNames(string userName, string domain)
        {
            var pc = new PrincipalContext(ContextType.Domain, domain);
            var src = UserPrincipal.FindByIdentity(pc, userName).GetGroups(pc);
            var result = new List<string>();
            src.ToList().ForEach(sr => result.Add(sr.UserPrincipalName));
            return result;
        }
        #endregion
    }
}