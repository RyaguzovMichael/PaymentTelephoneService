using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Localization;

namespace PaymentTelephoneServices.API.Services;

public class GlobalErrorsStringLocalizer
{
    private readonly Dictionary<string, Dictionary<string, string?>> _resources;

    public GlobalErrorsStringLocalizer()
    {
        var serializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true, 
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
        };
        
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "GlobalExceptionHandlerLocale.json");
        string json = File.ReadAllText(path);
        _resources = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string?>>>(json, serializerOptions ) 
                     ?? new Dictionary<string, Dictionary<string, string?>>();
    }

    public LocalizedString this[string name]
    {
        get
        {
            var currentCulture = CultureInfo.CurrentCulture;
            string? value = null;
            if (_resources.ContainsKey(currentCulture.Name))
            {
                if (_resources[currentCulture.Name].ContainsKey(name))
                {
                    value = _resources[currentCulture.Name][name];
                }
            }
            else
            {
                if (_resources["default"].ContainsKey(name))
                {
                    value = _resources["default"][name];
                }
            }
            return value is null ? new LocalizedString(name, name, true) : new LocalizedString(name, value, false);
        }
    }
}