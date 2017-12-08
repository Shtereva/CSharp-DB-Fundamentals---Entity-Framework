using System.Linq;
using AutoMapper;
using Stations.DataProcessor.Dto.export;
using Stations.Models;

namespace Stations.App
{
	public class StationsProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public StationsProfile()
		{
		    CreateMap<Train, TrainDto>()
                .ForMember(dto => dto.DelayedTimes, cfg => cfg.MapFrom(d => d.Trips.Count(x => x.Status == TripStatus.Delayed)))
                .ForMember(dto => dto.MaxDelayedTime, cfg => cfg.MapFrom(t => t.Trips.Max(x => x.TimeDifference)));


            CreateMap<CustomerCard, CustomerCardDto>();
		}
	}
}
