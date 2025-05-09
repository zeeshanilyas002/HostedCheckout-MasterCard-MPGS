using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class verifyTransactionResp
{
    [JsonPropertyName("3dsAcsEci")]
    public string ThreeDsAcsEci { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("authentication")]
    public Authentication Authentication { get; set; }

    [JsonPropertyName("authenticationStatus")]
    public string AuthenticationStatus { get; set; }

    [JsonPropertyName("authenticationVersion")]
    public string AuthenticationVersion { get; set; }

    [JsonPropertyName("chargeback")]
    public Chargeback Chargeback { get; set; }

    [JsonPropertyName("creationTime")]
    public DateTime CreationTime { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("device")]
    public Device Device { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("lastUpdatedTime")]
    public DateTime LastUpdatedTime { get; set; }

    [JsonPropertyName("merchant")]
    public string Merchant { get; set; }

    [JsonPropertyName("merchantAmount")]
    public decimal MerchantAmount { get; set; }

    [JsonPropertyName("merchantCategoryCode")]
    public string MerchantCategoryCode { get; set; }

    [JsonPropertyName("merchantCurrency")]
    public string MerchantCurrency { get; set; }

    [JsonPropertyName("result")]
    public string Result { get; set; }

    [JsonPropertyName("risk")]
    public Risk Risk { get; set; }

    [JsonPropertyName("sourceOfFunds")]
    public SourceOfFunds SourceOfFunds { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("totalAuthorizedAmount")]
    public decimal TotalAuthorizedAmount { get; set; }

    [JsonPropertyName("totalCapturedAmount")]
    public decimal TotalCapturedAmount { get; set; }

    [JsonPropertyName("totalDisbursedAmount")]
    public decimal TotalDisbursedAmount { get; set; }

    [JsonPropertyName("totalRefundedAmount")]
    public decimal TotalRefundedAmount { get; set; }

    [JsonPropertyName("transaction")]
    public List<TransactionModel> Transaction { get; set; }
}
public class Authentication
{
    [JsonPropertyName("3ds")]
    public ThreeDS ThreeDS { get; set; }
}

public class ThreeDS
{
    [JsonPropertyName("acsEci")]
    public string AcsEci { get; set; }

    [JsonPropertyName("authenticationToken")]
    public string AuthenticationToken { get; set; }

    [JsonPropertyName("transactionId")]
    public string TransactionId { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }
}

public class Chargeback
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
}

public class Device
{
    [JsonPropertyName("browser")]
    public string Browser { get; set; }

    [JsonPropertyName("ipAddress")]
    public string IpAddress { get; set; }
}

public class Risk
{
    [JsonPropertyName("response")]
    public RiskResponse Response { get; set; }
}

public class RiskResponse
{
    [JsonPropertyName("review")]
    public Review Review { get; set; }
}

public class Review
{
    [JsonPropertyName("decision")]
    public string Decision { get; set; }

    [JsonPropertyName("rule")]
    public Rule Rule { get; set; }
}

public class Rule
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class SourceOfFunds
{
    [JsonPropertyName("provided")]
    public Provided Provided { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class Provided
{
    [JsonPropertyName("card")]
    public Card Card { get; set; }
}

public class Card
{
    [JsonPropertyName("number")]
    public string Number { get; set; }

    [JsonPropertyName("expiry")]
    public Expiry Expiry { get; set; }

    [JsonPropertyName("brand")]
    public string Brand { get; set; }

    [JsonPropertyName("fundingMethod")]
    public string FundingMethod { get; set; }

    [JsonPropertyName("nameOnCard")]
    public string NameOnCard { get; set; }

    [JsonPropertyName("scheme")]
    public string Scheme { get; set; }

    [JsonPropertyName("storedOnFile")]
    public string StoredOnFile { get; set; }
}


public class Expiry
{
    [JsonPropertyName("month")]
    public string Month { get; set; }

    [JsonPropertyName("year")]
    public string Year { get; set; }
}

public class Number
{
    [JsonPropertyName("bin")]
    public string Bin { get; set; }

    [JsonPropertyName("last4")]
    public string Last4 { get; set; }
}

public class TransactionModel
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("authorizationCode")]
    public string AuthorizationCode { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
