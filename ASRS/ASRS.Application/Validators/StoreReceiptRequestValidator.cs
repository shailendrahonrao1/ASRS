using ASRS.Application.ViewModels;
using FluentValidation;
using FluentValidation.Validators;
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
            RuleFor(x => x.QuantityStored).NotEmpty()
                .WithMessage("Quantity Stored should not be empty.");

            RuleFor(x => x.QuantityStored).LessThanOrEqualTo(x=>x.QuantityReceived)
                .WithMessage("Quantity Stored should be less than or equal to Quantity Received.");

            RuleFor(x => x.Location).NotEmpty()
                .WithMessage("Location should not be empty.");
        }
    }
}
