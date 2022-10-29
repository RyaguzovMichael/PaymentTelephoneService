using Microsoft.Extensions.Logging;
using PaymentTelephoneServices.Infrastructure.Services.Interfaces;

namespace PaymentTelephoneServices.Infrastructure.DependencyInjection.Options;

internal class MobileOperatorServiceAggregatorOptions
{
    public Dictionary<string, Type> CodeOperatorServicePairs { get; set; }

	public MobileOperatorServiceAggregatorOptions(Action<Dictionary<string, Type>> configure, ILogger<MobileOperatorServiceAggregatorOptions> logger)
	{
        CodeOperatorServicePairs = new Dictionary<string, Type>();
        Dictionary<string, Type> keyValuePairs = new();
        configure.Invoke(keyValuePairs);

        foreach (var keyValuePair in keyValuePairs)
        {
            if(keyValuePair.Value.GetInterfaces().Contains(typeof(IMobileOperatorService)))
            {
                if (!CodeOperatorServicePairs.TryAdd(keyValuePair.Key, keyValuePair.Value))
                {
                    logger.LogWarning($"The pair: {keyValuePair.Key} : {keyValuePair.Value.Name} don't added in {nameof(MobileOperatorServiceAggregatorOptions)}.\n" +
                                      $"Reason: {keyValuePair.Key} is presented in options");
                }
            }
            else
            {
                logger.LogWarning($"The pair: {keyValuePair.Key} : {keyValuePair.Value.Name} don't added in {nameof(MobileOperatorServiceAggregatorOptions)}.\n" +
                                  $"Reason: {keyValuePair.Value.Name} not implemented interface: {nameof(IMobileOperatorService)}");
            }
        }
    }
}
