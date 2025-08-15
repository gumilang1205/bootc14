using FluentValidation;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Validator
{
    /// <summary>
    /// StudentValidator demonstrates advanced FluentValidation techniques
    /// This replaces all the Data Annotations we removed from the Student model
    /// </summary>
    public class StudentValidator : AbstractValidator<StudentDto>
    {
        /// <summary>
        /// Constructor defining validation rules for Student model
        /// Notice how much more readable this is compared to Data Annotations
        /// </summary>
        public StudentValidator()
        {
            // Name validation - required field with length constraints
            RuleFor(student => student.Name)
                .NotEmpty()
                .WithMessage("Student name is required.")
                .Length(2, 100)
                .WithMessage("Name must be between 2 and 100 characters.")
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Name can only contain letters and spaces");

            // Email validation - required with university domain
            RuleFor(student => student.Email)
                .NotEmpty()
                .WithMessage("Email address is required.")
                .EmailAddress()
                .WithMessage("Please enter a valid email address.")
                .Must(BeUniversityEmail)
                .WithMessage("Email must use university domain (@university.edu).");

        }

        /// <summary>
        /// Custom validation method for gender
        /// Much cleaner than complex regular expressions in attributes
        /// </summary>

        /// <summary>
        /// Custom validation method for university email domain
        /// </summary>
        private bool BeUniversityEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            return email.EndsWith("@university.edu", StringComparison.OrdinalIgnoreCase);
        }
    }
}