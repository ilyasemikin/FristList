using FristList.Data.Dto.Base;
using FristList.Data.Models.Base;

namespace FristList.Data;

public interface IModelToDtoMapper
{
    DtoObjectBase Map(ModelObjectBase modelObject);

    TDtoObjectBase Map<TDtoObjectBase>(ModelObjectBase modelObject) where TDtoObjectBase : DtoObjectBase
        => (TDtoObjectBase)Map(modelObject);
}