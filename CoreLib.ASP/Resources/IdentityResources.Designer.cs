﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoreLib.ASP.Resources {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class IdentityResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal IdentityResources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CoreLib.ASP.Resources.IdentityResources", typeof(IdentityResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Учетная запись заблокирована.
        /// </summary>
        public static string AccountLockedOutError {
            get {
                return ResourceManager.GetString("AccountLockedOutError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Пароль был успешно изменен.
        /// </summary>
        public static string ChangePasswordSuccessStatusMessage {
            get {
                return ResourceManager.GetString("ChangePasswordSuccessStatusMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Спасибо, что подтвердили адрес электронной почты.
        /// </summary>
        public static string ConfirmEmailSuccessMessage {
            get {
                return ResourceManager.GetString("ConfirmEmailSuccessMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Произошла ошибка при подтверждении электронной почты.
        /// </summary>
        public static string ConfirmEmailUnsuccessError {
            get {
                return ResourceManager.GetString("ConfirmEmailUnsuccessError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на При подтверждении адреса электронной почты произошла ошибка.
        /// </summary>
        public static string ConfirmEmailUnsuccessMesage {
            get {
                return ResourceManager.GetString("ConfirmEmailUnsuccessMesage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Такого пользователя не существует.
        /// </summary>
        public static string ConfirmEmailUserNotFoundError {
            get {
                return ResourceManager.GetString("ConfirmEmailUserNotFoundError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Подтверждение пароля.
        /// </summary>
        public static string ConfirmPassword {
            get {
                return ResourceManager.GetString("ConfirmPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Текущий пароль.
        /// </summary>
        public static string CurrentPassword {
            get {
                return ResourceManager.GetString("CurrentPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Роль с таким названием уже существует.
        /// </summary>
        public static string DuplicateRoleNameError {
            get {
                return ResourceManager.GetString("DuplicateRoleNameError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Электронная почта.
        /// </summary>
        public static string Email {
            get {
                return ResourceManager.GetString("Email", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Ваша эл. почта была успешно обновлена..
        /// </summary>
        public static string EmailUpdateSuccessStatusMessage {
            get {
                return ResourceManager.GetString("EmailUpdateSuccessStatusMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Идентификатор пользовательской роли не найден.
        /// </summary>
        public static string IdentityRoleNoIdError {
            get {
                return ResourceManager.GetString("IdentityRoleNoIdError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Пользовательская роль не найдена.
        /// </summary>
        public static string IdentityRoleNotFoundError {
            get {
                return ResourceManager.GetString("IdentityRoleNotFoundError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Невозможно произвести операцию с текущим пользователем.
        /// </summary>
        public static string IdentityUserCurrentUserError {
            get {
                return ResourceManager.GetString("IdentityUserCurrentUserError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Идентификатор пользователя не найден.
        /// </summary>
        public static string IdentityUserNoIdError {
            get {
                return ResourceManager.GetString("IdentityUserNoIdError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Пользователь не найден.
        /// </summary>
        public static string IdentityUserNotFoundError {
            get {
                return ResourceManager.GetString("IdentityUserNotFoundError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Невозможно исключить себя из роли &quot;Administrators&quot;.
        /// </summary>
        public static string IdentityUserRolesUpdateCurrentUserSelfRemoveFromAdministratorRoleError {
            get {
                return ResourceManager.GetString("IdentityUserRolesUpdateCurrentUserSelfRemoveFromAdministratorRoleError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Список пользовательских ролей пуст.
        /// </summary>
        public static string IdentityUserRolesUpdateNoRolesError {
            get {
                return ResourceManager.GetString("IdentityUserRolesUpdateNoRolesError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Список пользовательских ролей некорректен.
        /// </summary>
        public static string IdentityUserRolesUpdateRolesNotFoundError {
            get {
                return ResourceManager.GetString("IdentityUserRolesUpdateRolesNotFoundError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Пользовательские роли успешно обновлены.
        /// </summary>
        public static string IdentityUserRolesUpdateSuccess {
            get {
                return ResourceManager.GetString("IdentityUserRolesUpdateSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Ок.
        /// </summary>
        public static string IdentityUserSearchLockoutStatusFalse {
            get {
                return ResourceManager.GetString("IdentityUserSearchLockoutStatusFalse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Заблокирован.
        /// </summary>
        public static string IdentityUserSearchLockoutStatusTrue {
            get {
                return ResourceManager.GetString("IdentityUserSearchLockoutStatusTrue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Необходимо выбрать параметр поиска.
        /// </summary>
        public static string IdentityUserSearchParamIsEmpty {
            get {
                return ResourceManager.GetString("IdentityUserSearchParamIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на По вашему запросу ничего не найдено:(.
        /// </summary>
        public static string IdentityUserSearchResultEmptyError {
            get {
                return ResourceManager.GetString("IdentityUserSearchResultEmptyError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Поле поиска по значению &apos;{0}&apos; имеет неверный формат.
        /// </summary>
        public static string IdentityUserSearchStringFormatError {
            get {
                return ResourceManager.GetString("IdentityUserSearchStringFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Поле поиска не может быть пустым.
        /// </summary>
        public static string IdentityUserSearchStringIsEmpty {
            get {
                return ResourceManager.GetString("IdentityUserSearchStringIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Пользовательские роли.
        /// </summary>
        public static string IdentityUserUserRolesProperty {
            get {
                return ResourceManager.GetString("IdentityUserUserRolesProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Неверный логин для восстановления пароля.
        /// </summary>
        public static string InvalidForgotPasswordAttemptError {
            get {
                return ResourceManager.GetString("InvalidForgotPasswordAttemptError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Неверный логин и/или пароль.
        /// </summary>
        public static string InvalidLoginAttemptError {
            get {
                return ResourceManager.GetString("InvalidLoginAttemptError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Неверный логин для сброса пароля.
        /// </summary>
        public static string InvalidResetPasswordAttemptError {
            get {
                return ResourceManager.GetString("InvalidResetPasswordAttemptError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Новый пароль.
        /// </summary>
        public static string NewPassword {
            get {
                return ResourceManager.GetString("NewPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Пароль.
        /// </summary>
        public static string Password {
            get {
                return ResourceManager.GetString("Password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Пароль должен состоять не менее чем из 8-ми символов и иметь хотя бы одну заглавную, маленькую латинскую букву, цифру и специальный символ.
        /// </summary>
        public static string PasswordValidationError {
            get {
                return ResourceManager.GetString("PasswordValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Номер телефона.
        /// </summary>
        public static string PhoneNumberProperty {
            get {
                return ResourceManager.GetString("PhoneNumberProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на При обновлении вашего профиля что-то пошло не так. Пожалуйста, повторите позднее..
        /// </summary>
        public static string ProfileUpdateErrorStatusMessage {
            get {
                return ResourceManager.GetString("ProfileUpdateErrorStatusMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Ваш профиль был успешно обновлен.
        /// </summary>
        public static string ProfileUpdateSuccessStatusMessage {
            get {
                return ResourceManager.GetString("ProfileUpdateSuccessStatusMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Запомнить меня?.
        /// </summary>
        public static string RememberMe {
            get {
                return ResourceManager.GetString("RememberMe", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Для сброса пароля необходимо подтвердить адрес электронной почты..
        /// </summary>
        public static string ResetPasswordEmailErrorStatusMessage {
            get {
                return ResourceManager.GetString("ResetPasswordEmailErrorStatusMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Для сброса вашего пароля, пожалуйста, нажмите &lt;a href=&apos;{0}&apos;&gt;здесь&lt;/a&gt;..
        /// </summary>
        public static string ResetPasswordMessageText {
            get {
                return ResourceManager.GetString("ResetPasswordMessageText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Сброс пароля личного кабинета.
        /// </summary>
        public static string ResetPasswordMessageTitle {
            get {
                return ResourceManager.GetString("ResetPasswordMessageTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Отсутствует код сброса пароля.
        /// </summary>
        public static string ResetPasswordNoCodeError {
            get {
                return ResourceManager.GetString("ResetPasswordNoCodeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Сообщение о сбросе пароля было отправлено..
        /// </summary>
        public static string ResetPasswordSuccessStatusMessage {
            get {
                return ResourceManager.GetString("ResetPasswordSuccessStatusMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Название.
        /// </summary>
        public static string RoleNameProperty {
            get {
                return ResourceManager.GetString("RoleNameProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Логин или эл. почта.
        /// </summary>
        public static string UserNameOrEmailProperty {
            get {
                return ResourceManager.GetString("UserNameOrEmailProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Имя пользователя.
        /// </summary>
        public static string UserNameProperty {
            get {
                return ResourceManager.GetString("UserNameProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Ваше имя пользователя было успешно обновлено..
        /// </summary>
        public static string UserNameUpdateSuccessStatusMessage {
            get {
                return ResourceManager.GetString("UserNameUpdateSuccessStatusMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Для подтверждения вашей почты, пожалуйста, нажмите &lt;a href=&apos;{0}&apos;&gt;здесь&lt;/a&gt;..
        /// </summary>
        public static string VerificationEmailMessageText {
            get {
                return ResourceManager.GetString("VerificationEmailMessageText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Подтверждение адреса электронной почты.
        /// </summary>
        public static string VerificationEmailMessageTitle {
            get {
                return ResourceManager.GetString("VerificationEmailMessageTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Сообщение о подтверждении электронного адреса было отправлено. Пожалуйста, проверьте ваш почтовый ящик..
        /// </summary>
        public static string VerificationEmailSentSuccessStatusMessage {
            get {
                return ResourceManager.GetString("VerificationEmailSentSuccessStatusMessage", resourceCulture);
            }
        }
    }
}
