namespace Shared.API;

internal record AuthConfig(string AuthorizationUrl, string TokenUrl, string MetadataUrl, string Audience, string Authority);