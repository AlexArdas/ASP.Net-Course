using System.Runtime.CompilerServices;
using CST.Common.Models.Domain;
using CST.Common.Repositories;
using Bogus;
using CST.Common.Models.Enums;
using Gender = CST.Common.Models.Enums.Gender;
using LocationType = CST.Common.Models.Enums.LocationType;

[assembly: InternalsVisibleTo("CST.BusinessLogic.Tests")]
namespace CST.BusinessLogic.Services
{
    internal class SampleDataService : ISampleDataService
    {
        private const string UsualUserString = "UsualUser";

        private readonly IRepository<LocationDomainEntity> _locationRepository;
        private readonly IRepository<RoleDomainEntity> _roleRepository;
        private readonly IRepository<UserDomainEntity> _userRepository;
        private readonly IRepository<UserRoleDomainEntity> _userRoleRepository;
        private readonly IRepository<RequestDomainEntity> _requestRepository;
        private readonly IRepository<RequestFormDomainEntity> _requestFormRepository;

        public SampleDataService(IRepository<LocationDomainEntity> locationRepository
            , IRepository<RoleDomainEntity> roleRepository
            , IRepository<UserDomainEntity> userRepository
            , IRepository<UserRoleDomainEntity> userRoleRepository
            , IRepository<RequestDomainEntity> requestRepository
            , IRepository<RequestFormDomainEntity> requestFormRepository)
        {
            _locationRepository = locationRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _requestRepository = requestRepository;
            _requestFormRepository = requestFormRepository;
        }

        public async Task PopulateSampleDataAsync()
        {
            var location = GenerateLocation();
            var users = new Dictionary<string, UserDomainEntity>
            {
                { UsualUserString, GenerateUser(null, null, location.Id) },
            };

            await _locationRepository.AddAsync(location);
            await _userRepository.AddRangeAsync(users.Values);
            await _requestFormRepository.AddAsync(GenerateRequestForm());
        }

        private LocationDomainEntity GenerateLocation()
        {
            var location = new Faker<LocationDomainEntity>()
                .RuleFor(l => l.Id, Guid.NewGuid())
                .RuleFor(l => l.Name, f => f.Address.City())
                .RuleFor(l => l.ExternalId, Guid.NewGuid().ToString())
                .RuleFor(l => l.Type, f => f.PickRandom<LocationType>())
                .RuleFor(l => l.Timezone, f => f.Random.Int(1 - 10));
            return location;
        }
        private UserDomainEntity GenerateUser(string email, string fullName, Guid locationId)
        {
            var user = new Faker<UserDomainEntity>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Email, f => email ?? f.Internet.Email())
                .RuleFor(u => u.FullName, f => fullName ?? f.Name.FullName())
                .RuleFor(u => u.NativeName, f => fullName ?? f.Name.FullName())
                .RuleFor(u => u.LocationId, locationId)
                .RuleFor(u => u.DoB, f => f.Date.Past(20).ToUniversalTime())
                .RuleFor(u => u.JobTitle, f => f.Name.JobTitle())
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                .RuleFor(u => u.AvatarUri, f => f.Internet.Avatar())
                .RuleFor(u => u.MccOpenedAt, f => f.Date.Past().ToUniversalTime())
                .RuleFor(u => u.JobLevel, f => f.Name.JobType())
                .RuleFor(u => u.JobFunctionBase, f => f.Name.JobArea())
                .RuleFor(u => u.PrimarySkill, f => f.Name.JobDescriptor())
                .RuleFor(u => u.FireDate, f => f.Date.Past().ToUniversalTime())
                .RuleFor(u => u.ExternalId, Guid.NewGuid().ToString())
                .RuleFor(u => u.TimeZone, f => f.Random.Int(1 - 10));
            return user;
        }

        private RoleDomainEntity GenerateRole(RoleNames roleName)
        {
            var role = new Faker<RoleDomainEntity>()
                .RuleFor(r => r.Id, Guid.NewGuid())
                .RuleFor(r => r.Name, roleName)
                .RuleFor(r => r.Description, f => f.Name.JobDescriptor());
            return role;
        }

        private RequestDomainEntity GenerateRequest(string email)
        {
            var requestMessage = new Faker<RequestMessageDomainEntity>()
                .RuleFor(rm => rm.Id, Guid.NewGuid())
                .RuleFor(rm => rm.Body, "Autotest RequestMessage Body")
                .RuleFor(rm => rm.CreatedOn, DateTime.UtcNow)
                .RuleFor(rm => rm.ModifiedOn, DateTime.UtcNow);
            var request = new Faker<RequestDomainEntity>()
                .RuleFor(r => r.Id, Guid.NewGuid())
                .RuleFor(r => r.AssigneeId, f => null)
                .RuleFor(r => r.RequestStatus, RequestStatus.Created)
                .RuleFor(r => r.CreatedOn, f => DateTime.UtcNow)
                .RuleFor(r => r.RequesterEmail, email)
                .RuleFor(r => r.RequestMessage, new List<RequestMessageDomainEntity> { requestMessage });
            return request;
        }

        private RequestFormDomainEntity GenerateRequestForm()
        {
            var requestForm = new Faker<RequestFormDomainEntity>()
                .RuleFor(r => r.Id, new Guid("00000000-0000-0000-0000-000000000001"))
                .RuleFor(r => r.Name, f => f.Name.FullName())
                .RuleFor(r => r.Description, f => f.Name.JobDescriptor())
                .RuleFor(r => r.From, f => f.Name.FullName())
                .RuleFor(r => r.Recipients, f => f.Name.FullName())
                .RuleFor(r => r.Customer, f => f.Name.FullName())
                .RuleFor(r => r.ExpectedSendDate, f => f.Date.Future().ToUniversalTime())
                .RuleFor(r => r.LinkToFilesAtOnedrive, f => f.Internet.UrlRootedPath())
                .RuleFor(r => r.RequesterEmail, f => f.Internet.Email())
                .RuleFor(r => r.CreatedOn, f => f.Date.Past().ToUniversalTime());
            return requestForm;
        }
    }
}
