using FindRoommate.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FindRoommate.Infrastructure.Validators
{
    public class CustomPasswordValidator : PasswordValidator<AppUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            IdentityResult result = await base.ValidateAsync(manager, user, password);

            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            if (String.IsNullOrEmpty(password) || password.Length < 6)
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordLength",
                    Description = "Hasło musi mieć conajmniej 6 znaków"
                });
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordLowerCase",
                    Description = "Hasło musi zawierać małą literę"
                });
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordUpperCase",
                    Description = "Hasło musi zawierać dużą literę"
                });
            }

            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordDigit",
                    Description = "Hasło musi zawierać cyfrę"
                });
            }

            if (!Regex.IsMatch(password, @"[!@#$%^&*\(\)_\+\-\={}<>,\.\|""'~`:;\\?\/\[\] ]"))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordNonAlphanumeric",
                    Description = "Hasło musi zawierać znak specjalny"
                });
            }

            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
