using AutoMapper;
using RodizioSmartKernel.Entities;

namespace API.Application.Helpers
{
    /// <summary>
    /// This is the class that will define our Mapping preferences in what is called <see cref="Profile"/>
    /// <para> <see cref="Profile"/>s are named configurations for <see cref="AutoMapper"/></para>
    /// <para> NOTE: This is configuration that maps properties of one class to another. At times specific properties may still need to be defined explicitly</para>
    /// </summary>
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, AdminUser>();


        }

    }
}
