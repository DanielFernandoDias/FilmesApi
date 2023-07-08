using AutoMapper;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;

namespace FilmesApi.Profiles
{
    public class FilmeProfile : Profile
    {
        public FilmeProfile()
        {
            CreateMap<CreateFilmeDto, Filme>();
            CreateMap<UpdateFilmeDto, Filme>();
            CreateMap<Filme, UpdateFilmeDto>();
            // No mapeamento de Filme para filme Dto, For Member readFilmeDto quero que mapeie a sessao de filme para sessao de filme Dto
            CreateMap<Filme, ReadFilmeDto>().ForMember(dto => dto.Sessoes, opt => opt.MapFrom(filme
                => filme.Sessoes));
        }
    }
}
