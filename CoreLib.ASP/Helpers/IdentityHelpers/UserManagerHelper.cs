#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.ASP.Resources;
using CoreLib.CORE.CustomObjects;
using CoreLib.CORE.Helpers.CryptoHelpers;
using CoreLib.CORE.Helpers.StringHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

#endregion

namespace CoreLib.ASP.Helpers.IdentityHelpers
{
    public class UserManagerHelper<T, T1> where T : IdentityUser<string>, new() where T1 : IdentityRole<string>, new()
    {
        private readonly RoleManager<T1> _roleManager;
        private readonly UserManager<T> _userManager;

        public UserManagerHelper(UserManager<T> userManager, RoleManager<T1> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task UpdateIdentityUserRolesAsync(string userId, HttpContext currentContext,
            Dictionary<string, bool> roles)
        {
            if (userId.IsNullOrEmptyOrWhiteSpace())
                throw new ArgumentNullException(nameof(userId),
                    IdentityResources.ResourceManager.GetString("IdentityUserNoIdError"));
            if (roles == null)
                throw new ArgumentNullException(nameof(userId),
                    IdentityResources.ResourceManager.GetString("IdentityUserRolesUpdateNoRolesError"));
            var item = await _userManager.FindByIdAsync(userId);
            if (item == null)
                throw new NullReferenceException(
                    IdentityResources.ResourceManager.GetString("IdentityUserNotFoundError"));
            if (!roles.All(ur => _roleManager.Roles.Select(r => r.Name).Contains(ur.Key)))
                throw new ArgumentNullException(nameof(userId),
                    IdentityResources.ResourceManager.GetString("IdentityUserRolesUpdateRolesNotFoundError"));

            if (currentContext.User.Identity.Name == item.UserName && !roles["Administrators"])
                throw new ValidationException(
                    IdentityResources.ResourceManager.GetString(
                        "IdentityUserRolesUpdateCurrentUserSelfRemoveFromAdministratorRoleError"));

            var userRoles = await _userManager.GetRolesAsync(item);

            var addedRoles = roles.Where(r => r.Value).Select(r => r.Key).Except(userRoles);
            var removedRoles = userRoles.Except(roles.Where(r => r.Value).Select(r => r.Key));

            if (addedRoles.Any())
            {
                var addToRolesResult =
                    await _userManager.AddToRolesAsync(item, addedRoles);
                if (!addToRolesResult.Succeeded)
                    throw new ExtendedValidationException(addToRolesResult.Errors.Select(e => e.Description));
            }

            if (removedRoles.Any())
            {
                var removeFromRolesResult =
                    await _userManager.RemoveFromRolesAsync(item, removedRoles);
                if (!removeFromRolesResult.Succeeded)
                    throw new ExtendedValidationException(removeFromRolesResult.Errors.Select(e => e.Description));
            }

            if (currentContext.User.Identity.Name != item.UserName)
                await _userManager.UpdateSecurityStampAsync(item);
        }

        public async Task<bool> ToggleIdentityUserLockoutAsync(string userId, HttpContext currentContext)
        {
            if (userId.IsNullOrEmptyOrWhiteSpace())
                throw new ArgumentNullException(nameof(userId),
                    IdentityResources.ResourceManager.GetString("IdentityUserNoIdError"));
            var item = await _userManager.FindByIdAsync(userId);
            if (item == null)
                throw new NullReferenceException(
                    IdentityResources.ResourceManager.GetString("IdentityUserNotFoundError"));
            if (currentContext.User.Identity.Name == item.UserName)
                throw new ValidationException(
                    IdentityResources.ResourceManager.GetString("IdentityUserCurrentUserError"));
            var setLockoutEnabledResult = await _userManager.SetLockoutEnabledAsync(item, !item.LockoutEnabled);
            if (!setLockoutEnabledResult.Succeeded)
                throw new ExtendedValidationException(setLockoutEnabledResult.Errors.Select(e => e.Description));

            if (item.LockoutEnabled && (item.LockoutEnd != null && item.LockoutEnd.Value != DateTimeOffset.MaxValue ||
                                        item.LockoutEnd == null))
            {
                var setLockoutEndDateResult = await _userManager.SetLockoutEndDateAsync(item, DateTimeOffset.MaxValue);
                if (!setLockoutEndDateResult.Succeeded)
                    throw new ExtendedValidationException(setLockoutEndDateResult.Errors.Select(e => e.Description));
                await _userManager.UpdateSecurityStampAsync(item);
            }

            return item.LockoutEnabled;
        }

        public async Task DeleteIdentityUserAsync(string userId, HttpContext currentContext)
        {
            if (userId.IsNullOrEmptyOrWhiteSpace())
                throw new ArgumentNullException(nameof(userId),
                    IdentityResources.ResourceManager.GetString("IdentityUserNoIdError"));
            var item = await _userManager.FindByIdAsync(userId);
            if (item == null)
                throw new NullReferenceException(
                    IdentityResources.ResourceManager.GetString("IdentityUserNotFoundError"));
            if (currentContext.User.Identity.Name == item.UserName)
                throw new ValidationException(IdentityResources.ResourceManager.GetString("IdentityUserCurrentUserError"));
            var deleteUserResult = await _userManager.DeleteAsync(item);
            if(!deleteUserResult.Succeeded)
                throw new ExtendedValidationException(deleteUserResult.Errors.Select(e => e.Description));
        }

        public async Task<string> ResetIdentityUserPasswordAsync(string userId, HttpContext currentContext)
        {
            if (userId.IsNullOrEmptyOrWhiteSpace())
                throw new ArgumentNullException(nameof(userId),
                    IdentityResources.ResourceManager.GetString("IdentityUserNoIdError"));
            var item = await _userManager.FindByIdAsync(userId);
            if (item == null)
                throw new NullReferenceException(
                    IdentityResources.ResourceManager.GetString("IdentityUserNotFoundError"));
            if (currentContext.User.Identity.Name == item.UserName)
                throw new ValidationException(
                    IdentityResources.ResourceManager.GetString("IdentityUserCurrentUserError"));

            var newPassword = PasswordGenerators.GenerateGuidPassword();
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(item);
            var resetPasswordResult = await _userManager.ResetPasswordAsync(item, passwordResetToken, newPassword);
            if (!resetPasswordResult.Succeeded)
                throw new ExtendedValidationException(resetPasswordResult.Errors.Select(e => e.Description));
            await _userManager.UpdateSecurityStampAsync(item);
            return newPassword;
        }

        private static Tuple<int, List<T>> GenerateIdentityUserPage(
            IQueryable<T> users, int pageIndex, int pageSize)
        {
            return new Tuple<int, List<T>>(users.Count(), users.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        public async Task<Tuple<int, List<T>>> GetIdentityUsersAsync(int searchParamIndex, string searchString,
            int pageNumber, int pageSize)
        {
            if (searchString.IsNullOrEmptyOrWhiteSpace())
                throw new ValidationException(
                    IdentityResources.ResourceManager.GetString("IdentityUserSearchStringIsEmpty"));

            SearchType searchType = 0;
            if (Enum.IsDefined(typeof(SearchType), searchParamIndex)) searchType = (SearchType) searchParamIndex;

            Tuple<int, List<T>> searchResult;
            var pageIndex = pageNumber - 1;
            switch (searchType)
            {
                default:
                case SearchType.None:
                    throw new ValidationException(
                        IdentityResources.ResourceManager.GetString("IdentityUserSearchParamIsEmpty"));
                case SearchType.UserName:
                    searchResult =
                        GenerateIdentityUserPage(_userManager.Users.Where(u => u.UserName.Contains(searchString)),
                            pageIndex, pageSize);
                    break;
                case SearchType.Email:
                    if (!searchString.IsEmailAddress())
                        throw new ValidationException(string.Format(
                            IdentityResources.ResourceManager.GetString("IdentityUserSearchStringFormatError"),
                            IdentityResources.ResourceManager.GetString("Email")));
                    searchResult = GenerateIdentityUserPage(_userManager.Users.Where(u => u.Email == searchString),
                        pageIndex, pageSize);
                    break;
                case SearchType.PhoneNumber:
                    if (!searchString.IsRussianPhoneNumber())
                        throw new ValidationException(string.Format(
                            IdentityResources.ResourceManager.GetString("IdentityUserSearchStringFormatError"),
                            IdentityResources.ResourceManager.GetString("PhoneNumberProperty")));
                    searchResult =
                        GenerateIdentityUserPage(_userManager.Users.Where(u => u.PhoneNumber == searchString),
                            pageIndex, pageSize);
                    break;
                case SearchType.UserRole:
                    searchResult = GenerateIdentityUserPage(
                        (await _userManager.GetUsersInRoleAsync(searchString)).AsQueryable(), pageIndex, pageSize);
                    break;
            }

            if (searchResult.Item1 == 0)
                throw new ValidationException(
                    IdentityResources.ResourceManager.GetString("IdentityUserSearchResultEmptyError"));


            return searchResult;
        }

        private enum SearchType
        {
            None,
            UserName,
            Email,
            PhoneNumber,
            UserRole
        }
    }
}