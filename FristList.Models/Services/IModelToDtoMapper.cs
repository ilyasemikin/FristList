using FristList.Data.Dto.Base;
using FristList.Models.Base;

namespace FristList.Models.Services;

public interface IModelToDtoMapper
{
    DtoObjectBase Map(ModelObjectBase modelObject);

    TDtoObjectBase Map<TDtoObjectBase>(ModelObjectBase modelObject) where TDtoObjectBase : DtoObjectBase
        => (TDtoObjectBase)Map(modelObject);
}