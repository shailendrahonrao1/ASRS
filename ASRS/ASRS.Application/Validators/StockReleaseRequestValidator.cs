using ASRS.Application.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Application.Validators
{
    public class StockReleaseRequestValidator : AbstractValidator<StockReleaseRequest>
    {
        public StockReleaseRequestValidator()
        {
            RuleFor(x => x.IssuedQuantity).NotEmpty()
            .WithMessage("Issued Quantity should not be empty.");

            RuleFor(x => x.IssuedQuantity).LessThanOrEqualTo(x => x.DemandQuantity)
                .WithMessage("Issued Quantity should be less than or equal to Demand Quantity.");

            RuleFor(x => x.Location).NotEmpty()
                .WithMessage("Location should not be empty.");
        }
    }
}
