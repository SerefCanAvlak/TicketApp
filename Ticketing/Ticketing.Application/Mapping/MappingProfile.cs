using AutoMapper;
using Ticketing.Application.Features.Events.Dtos;
using Ticketing.Application.Features.Events.Queries;
using Ticketing.Application.Features.SeatLock.Dtos;
using Ticketing.Application.Features.Seats.Dtos;
using Ticketing.Application.Features.Ticket.Dtos;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enum;

namespace Ticketing.Application.Mapping
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateEventDto, Event>()
                .ForMember(dest => dest.State,
                    opt => opt.MapFrom(src => EventStateEnum.FromName(src.State, true)));

            CreateMap<UpdateEventDto, Event>()
                .ForMember(dest => dest.State,
                    opt => opt.MapFrom(src => EventStateEnum.FromName(src.State, true)));

            CreateMap<EventDto, Event>()
                .ForMember(dest => dest.State,
                    opt => opt.MapFrom(src => EventStateEnum.FromName(src.State, true)));

            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.State,
                    opt => opt.MapFrom(src => src.State.Name));

            CreateMap<Seat, SeatDto>().ReverseMap();
            CreateMap<SeatLock, SeatLockDto>().ReverseMap();

            CreateMap<Ticket, TicketDto>()
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.Name));

            CreateMap<TicketDto, Ticket>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => TicketStateEnum.FromName(src.State, true)));
        }
    }
}
