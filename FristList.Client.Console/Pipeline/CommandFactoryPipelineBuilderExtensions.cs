using FristList.Client.Console.Pipeline.Base;
using FristList.Data.Dto;

namespace FristList.Client.Console.Pipeline;

public static class CommandFactoryPipelineBuilderExtensions
{
    public static CommandFactoryPipelineBuilder AddAuthorizeHandler(this CommandFactoryPipelineBuilder builder,
        AuthorizeService authorizeService)
        => builder.AddHandler(new AuthorizePipelineHandlerBase(authorizeService));

    public static CommandFactoryPipelineBuilder AddCommonHandler(this CommandFactoryPipelineBuilder builder)
        => builder.AddHandler(new CommonPipelineHandler());
    
    public static CommandFactoryPipelineBuilder AddCreateHandler(this CommandFactoryPipelineBuilder builder, FristListClient client, CategoryStorage categoryStorage)
        => builder.AddHandler(new CreatePipelineHandler(client, categoryStorage));

    public static CommandFactoryPipelineBuilder AddDeleteHandler(this CommandFactoryPipelineBuilder builder, FristListClient client, CategoryStorage categoryStorage)
        => builder.AddHandler(new DeletePipelineHandler(client, categoryStorage));

    public static CommandFactoryPipelineBuilder AddListHandler(this CommandFactoryPipelineBuilder builder, FristListClient client)
        => builder.AddHandler(new ListPipelineHandler(client));
}