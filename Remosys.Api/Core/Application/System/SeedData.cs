using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper;
using Remosys.Common.Mongo;

namespace Remosys.Api.Core.Application.System
{
    public class SeedData
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IMongoRepository<Role> _roleRepository;

        public SeedData(IMongoRepository<User> userRepository, IMongoRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task SeedAllAsync(CancellationToken cancellation)
        {
            #region SeedData

            var adminRole = new Role
            {
                Name = "Admin",
                Description = "System Admin Role",
                IsVital = true,
            };
            if (!_roleRepository.GetAll().Any())
            {
                await _roleRepository.AddAsync(adminRole);

                await _roleRepository.AddAsync(new Role
                {
                    Name = "Manager",
                    Description = "Mange User",
                    IsVital = true,
                });

                await _roleRepository.AddAsync(new Role
                {
                    Name = "User",
                    Description = "System Admin Role",
                    IsVital = true,
                });
            }

            if (!_userRepository.GetAll().Any())
            {
                await _userRepository.AddAsync(new User
                {
                    FirstName = "نیما",
                    LastName = "نصرتی",
                    Email = "nimanosrati93@gmail.com",
                    Password = PasswordManagement.HashPass("nima1234!"),
                    ActiveCode = Guid.NewGuid().ToString("N"),
                    Mobile = "09107602786",
                    IsEmailConfirm = true,
                    IsMobileConfirm = true,
                    RegisterDate = DateTime.Now,
                    ModifiedDate = DateTime.Now.AddDays(2),
                    Roles = new List<Role>
                    {
                        adminRole
                    }
                });
            }

            #endregion
        }
    }
}