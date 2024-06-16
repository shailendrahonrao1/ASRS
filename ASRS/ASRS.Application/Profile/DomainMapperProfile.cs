using VM = ASRS.Application.ViewModels;
using DM = ASRS.Core.Models;

namespace ASRS.Application.Profile
{
    using AutoMapper;
    using ExcelDataReader;
    using Microsoft.AspNetCore.Routing.Constraints;
    using System.Data;

    public class DomainMapperProfile : Profile
    {
        public DomainMapperProfile()
        {
            CreateMap<DM.StoreReceipt, VM.StoreReceiptRequest>()
                .ReverseMap();

            CreateMap<DM.StockRelease, VM.StockReleaseRequest>()
                .ReverseMap();

            CreateMap<DM.ItemMaster, VM.ItemMasterRequest>()
                .ReverseMap();

            CreateMap<DM.StoreReceipt,VM.StoreReceiptResponse>()
                .ReverseMap();

            CreateMap<DM.StockRelease, VM.StockReleaseResponse>()
                .ReverseMap();

            CreateMap<IDataReader, VM.StoreReceiptDataTable>()
                .ForMember(dest => dest.CRVNo, opt => opt.MapFrom(src => src.GetString(src.GetOrdinal("CRVNo"))))
                .ForMember(dest => dest.QuantityReceived, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal("QuantityReceived"))))
                .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.GetString(src.GetOrdinal("ItemCode"))))
                .ForMember(dest => dest.LedgerFolioNo, opt => opt.MapFrom(src => src.GetString(src.GetOrdinal("LedgerFolioNo"))))
                .ForMember(dest => dest.ItemSerialNo, opt => opt.MapFrom(src => src.GetString(src.GetOrdinal("ItemSerialNo"))))
                .ForMember(dest => dest.CRVDate, opt => opt.MapFrom(src => src.GetDateTime(src.GetOrdinal("CRVDate"))));

            CreateMap<IDataReader, VM.StockReleaseDataTable>()
                .ForMember(dest => dest.DemandNo, opt => opt.MapFrom(src => src.GetString(src.GetOrdinal("DemandNo"))))
                .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.GetString(src.GetOrdinal("ItemCode"))))
                .ForMember(dest => dest.DemandQuantity, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal("DemandQuantity"))))
                .ForMember(dest => dest.DemandType, opt => opt.MapFrom(src => src.GetString(src.GetOrdinal("DemandType"))))
                .ForMember(dest => dest.ShipName, opt => opt.MapFrom(src => src.GetString(src.GetOrdinal("ShipName"))))
                .ForMember(dest => dest.DemandDate, opt => opt.MapFrom(src => src.GetDateTime(src.GetOrdinal("DemandDate"))));
        }
    }
}
