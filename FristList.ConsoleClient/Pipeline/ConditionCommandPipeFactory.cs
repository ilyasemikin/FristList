using FristList.ConsoleClient.CommandModels;
using FristList.ConsoleClient.Commands;

namespace FristList.ConsoleClient.Pipeline;

public class ConditionCommandPipeFactory : ICommandPipeFactory
{
    private readonly Func<bool> _condition;
    private readonly ICommandPipeFactory _trueCommandPipeFactory;
    private readonly ICommandPipeFactory _falseCommandPipeFactory;

    public ConditionCommandPipeFactory(Func<bool> condition, ICommandPipeFactory trueCommandPipeFactory, ICommandPipeFactory falseCommandPipeFactory)
    {
        _condition = condition;
        _trueCommandPipeFactory = trueCommandPipeFactory;
        _falseCommandPipeFactory = falseCommandPipeFactory;
    }

    public ICommand<CommandModelBase> GetCommand(Parameters parameters)
    {
        return _condition()
            ? _trueCommandPipeFactory.GetCommand(parameters)
            : _falseCommandPipeFactory.GetCommand(parameters);
    }
}