using Microsoft.Extensions.DependencyInjection;
using System;

namespace Membership.Core.Data
{
    public static class DependencyInjection
    {
        public static void AddMembershipDatabases(this IServiceCollection services)
        {
            services
                .AddSingleton<IOAuthClientRepository, InMemoryOAuthClientRepository>(CreateOAuthClientInMemoryRepository)
                .AddSingleton<IOAuthTokenRepository, InMemoryOAuthTokenRepository>()
                .AddSingleton<IUserRepository, InMemoryUserRepository>(CreateUserInMemoryRepository)
                .AddSingleton<IDeactivationRepository, InMemoryDeactivationRepository>()
                ;
        }

        /// <summary>
        /// Factory method that creates User repository 
        /// and add some sample users into it.
        /// </summary>
        static InMemoryUserRepository CreateUserInMemoryRepository(IServiceProvider arg)
        {
            var r = new InMemoryUserRepository();
            r.AddUser(new UserEntry {
                Id = Guid.Parse("6005ef6d-11fc-494c-9555-4c91af9d3579"),
                ClientId = "esiStOMporitandRogYRaTRacstigHAnTERStATH",
                FirstName = "Nam",
                LastName = "Duong",
                Email = "nam.duong.887@gmail.com",
                Password = new SHA256HashedString("123456", "sal"),
                PasswordSalt = "sal",
                Username = "nam",
                IsAdmin = true
            });
            r.AddUser(new UserEntry
            {
                Id = Guid.Parse("dbd3346d-b52c-4f8a-a957-aee814700483"),
                ClientId = "iONUMbRovELoPHOmFoRGouNDRUstrAtERBactOLd",
                FirstName = "Foo",
                LastName = "Bar",
                Email = "foo.bar@gmail.com",
                Password = new SHA256HashedString("abcdefg", "zl2"),
                PasswordSalt = "zl2",
                Username = "bar"
            });
            return r;
        }

        /// <summary>
        /// Factory method that creates OAuthClient repository and adds
        /// some sample data into it.
        /// </summary>
        static InMemoryOAuthClientRepository CreateOAuthClientInMemoryRepository(
            IServiceProvider arg)
        {
            // For demonstration purpose,
            // We'll add a couple of example tenant applications (OAuth clients) here
            var repository = new InMemoryOAuthClientRepository();
            AddSampleClients(repository);
            return repository;
        }

        static void AddSampleClients(InMemoryOAuthClientRepository repository)
        {
            repository.Add(new OAuthClientEntry
            {
                Id = "esiStOMporitandRogYRaTRacstigHAnTERStATH",
                Secret = new SHA256HashedString("$lGh6QlOIPRgd87DWeOUo5Y!H!%GOPsI0YdFxDN1"),
                Name = "Tenant A"
            });

            repository.Add(new OAuthClientEntry
            {
                Id = "iONUMbRovELoPHOmFoRGouNDRUstrAtERBactOLd",
                Secret = new SHA256HashedString("lGsxh03Qw^qXb@8ot*oa5NCS5qtjZIU^$gK&T0!B"),
                Name = "Tenant B"
            });

            repository.Add(new OAuthClientEntry
            {
                Id = "diChoRtMAsterNaMartYphinGTaCeTOlDEnArdER",
                Secret = new SHA256HashedString("l^W!Q$P!oCe%RX5R4k8Ejbk6DdW#wiIPSIGH*Uq*"),
                Name = "Tenant C"
            });
        }
    }
}
