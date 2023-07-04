using System;
using System.ComponentModel.DataAnnotations;

namespace Api;

public class ApiSwaggerInfo {
    public const string TestFunctionApiKey = "apikeyquery_auth";
    public const string TestFunctionApiHeaderName = "x-functions-key";
    
    public Version Version { get; init; }
    
    public string Title { get; init; }
    
    public string Description { get; init; }
    
    public Uri TermsOfService { get; init; }
    
    public ApiContact ApiContact { get; init; }
    
    public ApiLicense ApiLicense { get; init; }

    public ApiSwaggerInfo()
    {
        ApiContact = new();
        ApiLicense = new();
    }
}

public class ApiContact {
    
    public string Name { get; init; }
        
    public string Email { get; init; }
    
    public Uri Url { get; init; }
}

public class ApiLicense {
    
    public string Name { get; init; }
    
    public Uri Url { get; init; }
}