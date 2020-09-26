using System.IO;
using FluentValidation;
using Microsoft.Extensions.Options;
using Remosys.Api.Core.Application.Files.Command;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Options;

namespace Remosys.Api.Core.Validator.Files
{
    public class CreateFileCommandValidator : AbstractValidator<CreateFileCommand>
    {
        private readonly FileExtensions _fileExtensions;

        public CreateFileCommandValidator(IOptionsMonitor<FileExtensions> fileExtensions)
        {
            _fileExtensions = fileExtensions.CurrentValue;

            RuleFor(dto => dto)
                .Must(ValidExtension).WithMessage(ResponseMessage.FileExtensionNotSupported);
        }

        private bool ValidExtension(CreateFileCommand uploadFileCommand)
        {

            if (uploadFileCommand.Files == null || uploadFileCommand.Files.Length == 0)
                return false;

            var fileExtension = Path.GetExtension(uploadFileCommand.Files.FileName);

            if (!_fileExtensions.ValidFormat.Exists(x => x == fileExtension))
                return false;


            return true;
        }
    }
}