using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace plannerBackEnd.Common.automapper
{
    public class AutomapperProfile : Profile
    {
        /// <summary>
        /// Initialize Mapping process by finding all types that needs 
        /// to be mapped
        /// </summary>
        /// comment 
        public AutomapperProfile()
        {
            this.AllowNullCollections = true;
            this.AllowNullDestinationValues = true;

            var types = Assembly.GetExecutingAssembly().GetExportedTypes();
            registerStandardMappings(types);
            registerCustomMappings(types);

            types = Assembly.GetEntryAssembly().GetExportedTypes();
            registerStandardMappings(types);
            registerCustomMappings(types);
        }

        /// <summary>
        /// Load all types that implement interface <see cref="IMapFrom{T}"/>
        /// and create a map between {T} and them
        /// </summary>
        /// <param name="types"></param>
        private void registerStandardMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMaps<>)
                              && !t.IsAbstract
                              && !t.IsInterface
                        select new
                        {
                            Source = i.GetGenericArguments()[0],
                            Destination = t
                        }).ToArray();

            foreach (var map in maps)
            {
                CreateMap(map.Source, map.Destination, MemberList.None)
                    .ReverseMap();

            }
        }

        /// <summary>
        /// Load all types that implement interface <see cref="ICustomMapping"/>
        /// and register their mapping
        /// </summary>
        /// <param name="types"></param>
        private void registerCustomMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where typeof(ICustomMapping).IsAssignableFrom(t)
                              && !t.IsAbstract
                              && !t.IsInterface
                              && !t.IsGenericType
                        select (ICustomMapping)Activator.CreateInstance(t)).ToArray();

            foreach (var map in maps)
            {
                map.CreateMappings(this);
            }
        }
    }
}
