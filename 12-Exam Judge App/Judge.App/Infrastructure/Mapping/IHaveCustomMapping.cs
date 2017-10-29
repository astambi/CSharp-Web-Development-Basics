namespace Judge.App.Infrastructure.Mapping
{
    using AutoMapper;

    public interface IHaveCustomMapping
    {
        void Configure(IMapperConfigurationExpression config);
    }
}
