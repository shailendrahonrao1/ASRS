using ASRS.Application.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Application.Validators
{
    public class ImportAsrsRequestValidators : AbstractValidator<ImportAsrsRequest>
    {
        public ImportAsrsRequestValidators()
        {
            RuleFor(f => f.file).NotEmpty()
                .WithMessage("Please select file");

            RuleFor(f => f.file.FileName).Must(ValidateFileUpload).When(f => f.file != null)
                .WithMessage("Unsupported file format");
        }

        private bool ValidateFileUpload(string fileName)
        {
            var allowedExtensions = new string[] { ".xls", ".xlsx" };

            if (!allowedExtensions.Contains(Path.GetExtension(fileName.ToLower())))
            {
                return false;
            }
            return true;
        }
    }
}
