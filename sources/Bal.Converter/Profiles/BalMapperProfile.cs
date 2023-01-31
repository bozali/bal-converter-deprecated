using AutoMapper;

using Bal.Converter.Common.Media;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.Modules.Downloads.ViewModels;
using Bal.Converter.Modules.MediaDownloader.ViewModels;

namespace Bal.Converter.Profiles ;

public class BalMapperProfile : Profile
{
    public BalMapperProfile()
    {
        this.CreateMap<MediaTagsViewModel, MediaTags>().ReverseMap();
    }
}