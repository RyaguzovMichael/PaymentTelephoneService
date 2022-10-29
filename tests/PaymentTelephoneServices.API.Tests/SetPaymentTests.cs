using FluentAssertions;
using PaymentTelephoneServices.API.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace PaymentTelephoneServices.API.Tests;

public class SetPaymentTests : IClassFixture<CustomApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly CustomApplicationFactory<Program> _factory;

    public SetPaymentTests(CustomApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
        _factory = factory;
    }

    public static IEnumerable<object[]> ValidData()
    {
        yield return new object[] { "87778376454", 1 };
        yield return new object[] { "+8 777 837 6454", 1 };
        yield return new object[] { "8 (777) 8376454", 1 };
        yield return new object[] { "8 (777) 837 64 54", 1 };
        yield return new object[] { "+7 (777) 837 64 54", 1 };
        yield return new object[] { "+7 (777) 837 64 54", 1.10m };
        yield return new object[] { "+7 (777) 837 64 54", decimal.MaxValue };
    }

    [Theory]
    [MemberData(nameof(ValidData))]
    public async void SendValidData_AndReturnOk(string phoneNumber, decimal paymentAmount)
    {
        // Arrange
        SetPaymentControllerCommand command = new()
        {
            PaymentAmount = paymentAmount,
            PhoneNumber = phoneNumber
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/Payment/SetPayment", command);
        var dataAsString = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<ResponseVm>(dataAsString);

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.IsSucsess.Should().BeTrue();
        data.ErrorCode.Should().Be(ErrorCodes.Sucsess);
    }

    public static IEnumerable<object[]> InvalidPhoneNumber()
    {
        yield return new object[] { "asdn", 1 };
        yield return new object[] { "87778239847509234376454", 1 };
        yield return new object[] { "8777sadfasd454", 1 };
    }

    [Theory]
    [MemberData(nameof(InvalidPhoneNumber))]
    public async void SendInvalidPhoneNumber_AndReturnErrorCode_InvalidPhoneNumberInput(string phoneNumber, decimal paymentAmount)
    {
        // Arrange
        SetPaymentControllerCommand command = new()
        {
            PaymentAmount = paymentAmount,
            PhoneNumber = phoneNumber
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/Payment/SetPayment", command);
        var dataAsString = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<ResponseVm>(dataAsString);

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.IsSucsess.Should().BeFalse();
        data.ErrorCode.Should().Be(ErrorCodes.InvalidPhoneNumberInput);
    }


    [Theory]
    [InlineData("", 1)]
    public async void SendEmptyPhoneNumber_AndReturnErrorCode_InputDataIsEmpty(string phoneNumber, decimal paymentAmount)
    {
        // Arrange
        SetPaymentControllerCommand command = new()
        {
            PaymentAmount = paymentAmount,
            PhoneNumber = phoneNumber
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/Payment/SetPayment", command);
        var dataAsString = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<ResponseVm>(dataAsString);

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.IsSucsess.Should().BeFalse();
        data.ErrorCode.Should().Be(ErrorCodes.InputDataIsEmpty);
    }

    public static IEnumerable<object[]> InvalidPaymentAmount()
    {
        yield return new object[] { "+7 (777) 837 64 54", 0m };
        yield return new object[] { "+7 (777) 837 64 54", 0.01m };
        yield return new object[] { "+7 (777) 837 64 54", 0.1m };
        yield return new object[] { "+7 (777) 837 64 54", decimal.MinValue };
        yield return new object[] { "+7 (777) 837 64 54", decimal.MinusOne };
    }

    [Theory]
    [MemberData(nameof(InvalidPaymentAmount))]
    public async void SendInvalidPaymentAmount_AndReturnErrorCode_InvalidPaymentAmountInput(string phoneNumber, decimal paymentAmount)
    {
        // Arrange
        SetPaymentControllerCommand command = new()
        {
            PaymentAmount = paymentAmount,
            PhoneNumber = phoneNumber
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/Payment/SetPayment", command);
        var dataAsString = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<ResponseVm>(dataAsString);

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.IsSucsess.Should().BeFalse();
        data.ErrorCode.Should().Be(ErrorCodes.InvalidPaymentAmountInput);
    }

    public static IEnumerable<object[]> UnsupportedMobileOperatorsCodes()
    {
        yield return new object[] { "+7 (203) 837 64 54", 1 };
        yield return new object[] { "+7 (506) 837 64 54", 1 };
    }

    [Theory]
    [MemberData(nameof(UnsupportedMobileOperatorsCodes))]
    public async void SendUnsupportedMobileOperatorsCodes_AndReturnErrorCode_MobileOperatorIsNotSupported(string phoneNumber, decimal paymentAmount)
    {
        // Arrange
        SetPaymentControllerCommand command = new()
        {
            PaymentAmount = paymentAmount,
            PhoneNumber = phoneNumber
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/Payment/SetPayment", command);
        var dataAsString = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<ResponseVm>(dataAsString);

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.IsSucsess.Should().BeFalse();
        data.ErrorCode.Should().Be(ErrorCodes.MobileOperatorIsNotSupported);
    }
}