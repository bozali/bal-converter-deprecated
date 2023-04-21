using System.Collections.ObjectModel;
using AutoMapper;

using Bal.Converter.Common.Media;
using Bal.Converter.FFmpeg.Filters.Audio;
using Bal.Converter.Modules.Conversion.Filters.Volume;
using Bal.Converter.Modules.MediaDownloader.ViewModels;
using Bal.Converter.YouTubeDl;

namespace Bal.Converter.Profiles;

public class BalMapperProfile : Profile
{
    public BalMapperProfile()
    {
        this.CreateMap<MediaTagsViewModel, MediaTags>().ReverseMap();
        this.CreateMap<VideoViewModel, Video>().ReverseMap();
        this.CreateMap<PlaylistViewModel, Playlist>().ReverseMap();

        this.CreateMap<VolumeFilterViewModel, VolumeFilter>().ReverseMap();
    }
}