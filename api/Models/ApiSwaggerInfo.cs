using System;
using System.ComponentModel.DataAnnotations;

namespace Api;

public class ApiSwaggerInfo {
    [Required]
    public Version Version { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public Uri TermsOfService { get; set; }
    [Required]
    public ApiContact ApiContact { get; set; }
    [Required]
    public ApiLicense ApiLicense { get; set; }
}

public class ApiContact {
    [Required]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public Uri ContactUrl { get; set; }
}

public class ApiLicense {
    [Required]
    public string Name { get; set; }
    [Required]
    public Uri Url { get; set; }
}