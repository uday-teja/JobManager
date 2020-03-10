using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Model.Model
{
    public class TaskValidator : AbstractValidator<Task>
    {
        public TaskValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Requried");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Requried");
        }
    }
}