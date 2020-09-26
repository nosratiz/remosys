using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Remosys.Api.Core.Application.Files.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.Environment;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Files.Command
{
    public class CreateFileCommand : IRequest<Result<FileDto>>
    {
        public IFormFile Files { get; set; }
        public string Type { get; set; }
    }

    public class CreateFileCommandHandler : IRequestHandler<CreateFileCommand, Result<FileDto>>
    {
        private readonly IMongoRepository<UserFile> _fileRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWebHostEnvironment _env;

        public CreateFileCommandHandler(IMongoRepository<UserFile> fileRepository, IMapper mapper, ICurrentUserService currentUserService, IWebHostEnvironment env)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _env = env;
        }

        public async Task<Result<FileDto>> Handle(CreateFileCommand request, CancellationToken cancellationToken)
        {
            var tempPath = Path.Combine(ApplicationStaticPath.Documents, request.Files.FileName);
            var fileName = Path.GetFileNameWithoutExtension(tempPath);
            var extension = Path.GetExtension(tempPath);
            var newName = $"{_currentUserService.FullName}-{ Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(_env.ContentRootPath, ApplicationStaticPath.Documents, newName);

            #region Save To File

            await using var fileStream = new FileStream(filePath, FileMode.Create);

            await request.Files.CopyToAsync(fileStream, cancellationToken);


            #endregion

            #region Save To Database

            var userFile = new UserFile
            {
                Type = request.Type,
                Name = fileName,
                Size = request.Files.Length,
                Url = $"{ApplicationStaticPath.Clients.Document}/{newName}",
                MediaType = request.Files.ContentType,
                Path = Path.Combine(ApplicationStaticPath.Documents, newName),
                UserId = Guid.Parse(_currentUserService.UserId),
                FullName = _currentUserService.FullName,
                CreateDate = DateTime.Now
            };

            await _fileRepository.AddAsync(userFile);


            #endregion

            return Result<FileDto>.SuccessFul(_mapper.Map<FileDto>(userFile));

        }
    }
}