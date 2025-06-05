using AutoMapper;
using MVCWebApp.Models;

namespace MVCWebApp.Helper.Mapper
{
    public interface IMapModel
    {
        TDestination MapDto<TSource, TDestination>(TSource model);
        TDestination MapDto<TDestination>(object model);
        void Map<TSource, TDestination>(TSource modelS, TDestination modelD);

        //special

        TDestination MapDtoCreateSetUsername<TSource, TDestination>(TSource model, string username)
            where TDestination : ISetUserInfo;
        void Map<TSource, TDestination>(TSource modelS, TDestination modelD, string username)
                where TDestination : ISetUserInfo;

        void Map<TDestination>(TDestination modelD, string username) where TDestination : ISetUpdateInfo;
    }

    public class MapModel(IMapper mapper) : IMapModel
    {
        // Generic MapDto method
        public TDestination MapDto<TSource, TDestination>(TSource model)
        {
            return mapper.Map<TDestination>(model);
        }

        public TDestination MapDto<TDestination>(object model)
        {
            return mapper.Map<TDestination>(model);
        }

        public void Map<TSource, TDestination>(TSource modelS, TDestination modelD)
        {
            mapper.Map(modelS, modelD);
        }

        //special

        public TDestination MapDtoCreateSetUsername<TSource, TDestination>(TSource model, string username)
            where TDestination : ISetUserInfo
        {
            var entity = mapper.Map<TDestination>(model);

            entity.CreatedBy = username;
            entity.CreatedAt = DateTime.UtcNow.AddHours(8);
            return entity;
        }

        public void Map<TSource, TDestination>(TSource modelS, TDestination modelD, string username)
                where TDestination : ISetUserInfo
        {
            mapper.Map(modelS, modelD);

            modelD.ModifiedBy = username;
            modelD.ModifiedAt = DateTime.UtcNow.AddHours(8);
        }

        public void Map<TDestination>(TDestination modelD, string username) where TDestination : ISetUpdateInfo
        {
            modelD.ModifiedBy = username;
            modelD.ModifiedAt = DateTime.UtcNow.AddHours(8);
        }
    }
}
