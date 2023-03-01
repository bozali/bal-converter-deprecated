using AutoMapper;

using Bal.Converter.Common.Media;
using Bal.Converter.FFmpeg.Filters.Audio;
using Bal.Converter.Modules.Conversion.Filters.ViewModels;
using Bal.Converter.Modules.MediaDownloader.ViewModels;

namespace Bal.Converter.Profiles;

public class BalMapperProfile : Profile
{
    public BalMapperProfile()
    {
        this.CreateMap<MediaTagsViewModel, MediaTags>().ReverseMap();
        this.CreateMap<VolumeFilterViewModel, VolumeFilter>().ReverseMap();
    }
}