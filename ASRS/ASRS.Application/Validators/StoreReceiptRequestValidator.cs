using ASRS.Application.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Application.Validators
{
    public class StoreReceiptRequestValidator : AbstractValidator<StoreReceiptRequest>
    {
        public StoreReceiptRequestValidator()
        {
            
        }
    }
}
