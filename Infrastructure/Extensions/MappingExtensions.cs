using Meb.Core.Infrastructure.Mapper;

namespace Intra.Api.Infrastructure.Extensions
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        public static void UpdateFromSource<TMain>(this TMain main, object source)
        {
            var sourceProperties = source.GetType().GetProperties();
            var mainType = main.GetType();
            foreach (var sourceProperty in sourceProperties)
            {
                var pi = mainType.GetProperty(sourceProperty.Name);
                if (pi != null)
                {
                    var sourceValue = sourceProperty.GetValue(source);
                    pi.SetValue(main, sourceValue, null);
                }
            }
        }
    }
}