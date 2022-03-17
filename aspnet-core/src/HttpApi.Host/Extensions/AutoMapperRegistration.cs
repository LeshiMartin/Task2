namespace HttpApi.Host.Extensions;

public static class AutoMapperRegistration
{
  public static void RegisterAutoMapper ( this WebApplicationBuilder builder )
  {
    builder.Services.AddAutoMapper (typeof (SystemConstants).Assembly);
  }
}
