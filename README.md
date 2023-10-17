# CoreLib

A bunch of helper classes for development. Below you will find a description of the library you are interested in:

## CoreLib.ASP

This library contains action filter attributes such as:
- **CanonicalRedirectActionFilter** - permanently redirects requested page to its canonical name.
- **DeleteFileAfterDownloadActionFilter** - deletes the file after downloading.
- **GoogleReCaptchaValidationActionFilter** and **GoogleReCaptchaValidationPageFilter** - for Google ReCaptcha validation.
- **ThrottleActionFilter** - limits the execution of the target action in different ways.

Other interesting helper methods in this library:
- Helper methods that check whether target controller action or page is currently displayed.
- Helper methods that allows partial views to have their own scripts section.
- A tag helper that adds a placeholder attribute with `DisplayNameAttribute` content to input element.
- The rule that rewrites urls to lowercase.

## CoreLib.ASP.Identity.Ru

Russian localization of error messages for the `Microsoft.AspNetCore.Identity.UI` package.

## CoreLib.ASP.Extensions.YouTube

This library allows you to work with YouTube channels to get information about videos, e.g.:
- Search for a channel by its id.
- Get all videos from a channel.
- Get all videos from a playlist.
- Get all uploaded videos of a channel.
- Get the latest channel videos.

## CoreLib.CORE

The core library of helper classes and types extensions. Here are some interesting things:
- **SearchTypeHelper** - a class that allows to search for a type by its name in the current or provided assembly. Can be used for `SerializationBinder`.
- Predicates builder.
- **StringExtendedComparer** - a helper class that is used to correctly sort strings containing numbers(natural sort).
- **ConcurrentSemaphore** - very useful class for concurrent operations.

This library also contains some useful validation attributes:
- **CompareToAttribute**
- **DateValidationAttribute**
- **RequiredIfAttribute**
- **RangeLengthAttribute**

## CoreLib.CORE.Extensions.Compression

A simple library that allows to pack a file or folder into a zip archive.

## CoreLib.CORE.Extensions.Images

A library that allows to perform various operations with images using `System.Drawing.Common` or `SkiaSharp`:
- Change quality.
- Crop to circle.
- Cut.
- Reduce size.
- Resize.

## CoreLib.CORE.Extensions.Json

This library contains various converters for serialization with `System.Text.Json`:
- **CustomDateTimeConverter** - allows to serialize and deserialize `DateTime` or `DateTimeOffset` in a specified format.
- **IpAddressConverter** - allows to serialize and deserialize the `IPAddress` type.
- **JsonCamelCaseStringEnumConverter**, **JsonLowerCaseStringEnumConverter**, **JsonUpperCaseStringEnumConverter**, **JsonStringEnumMemberConverter** - various converters for enums.
- **UnixTimestampConverter** - allows to serialize and deserialize `DateTime` into a unix timestamp.

## CoreLib.CORE.Extensions.Json.Newtonsoft

This library contains some converters for serialization from the previous library, but for `Newtonsoft.Json`.
It also contains a deserializer that uses the **SearchTypeHelper** class from the `CoreLib.CORE` library above to search type in assemblies by name.

## CoreLib.CORE.Extensions.Ru

This library contains some russian-specific helpers e.g.:
- An extension method that converts an `int` value to its string representation in words.
- Address and document authority formatters.
- Various russian-specific regular expressions.
- An extension method for transliterating strings.

## CoreLib.Core.Extensions.Ru.GostEncryption

This library contains an implementation of the `CryptoService` class from the `CoreLib.CORE` library, which supports GOST encryption(grasshopper).

## CoreLib.Core.Extensions.Ru.NameCasesGenerator

This library is a wrapper around the `NameCaseLib.Core` for generating russian name cases.

## CoreLib.Core.Extensions.WEB

This library contains a helper class that handles http request cancellation using timeout and `CancellationToken`.

## CoreLib.OPENXML

A library that allows to perform various operations with OpenXml documents e.g.:
- Find and replace text in word document.
- Find text and set it bold in word document.
- Get all cells from the specified row in excel document.
- Autosize cells in excel document.

## CoreLib.STANDALONE

This library contains various helpers for developing standalone applications(WPF, Xamarin, MAUI):
- Various converters for data bindings.
- Implementation of **ObservableDictionary**.
- **ViewModelBase** abstract class as base for the MVVM pattern.
- Some helper wrapper classes for easier data binding.