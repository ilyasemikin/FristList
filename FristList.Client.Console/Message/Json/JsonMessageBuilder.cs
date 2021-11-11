using System;
using System.Collections;
using System.Collections.Generic;

namespace FristList.Client.Console.Message.Json
{
    public class JsonMessageBuilder
    {
        protected virtual MessageNodeBase BuildNumber(int value)
            => JsonValueMessageNode.CreateNumber(value);

        protected virtual MessageNodeBase BuildString(string value)
            => JsonValueMessageNode.CreateString(value);

        protected virtual MessageNodeBase BuildBoolean(bool value)
            => JsonValueMessageNode.CreateBoolean(value);

        protected virtual MessageNodeBase BuildDateTime(DateTime value)
            => JsonValueMessageNode.CreateDateTime(value);

        protected virtual MessageNodeBase BuildNull()
            => JsonValueMessageNode.CreateNull();
        
        protected virtual MessageNodeBase BuildKeyValue(string name, MessageNodeBase node)
            => new JsonKeyValueMessageNode(name, node);

        protected virtual MessageNodeBase BuildObject(object obj)
        {
            var nodes = new List<MessageNodeBase>();
            
            var type = obj.GetType();
            foreach (var property in type.GetProperties())
            {
                if (!property.CanRead)
                    continue;

                var node = Build(property.GetValue(obj));
                nodes.Add(BuildKeyValue(property.Name, node));
            }

            return new JsonObjectMessageNode(nodes);
        }

        protected virtual MessageNodeBase BuildArray(IEnumerable values)
        {
            var nodes = new List<MessageNodeBase>();
            
            foreach (var obj in values)
                nodes.Add(Build(obj));

            return new JsonArrayMessageNode(nodes);
        }

        public MessageNodeBase Build(object obj)
            => obj switch
            {
                int value => BuildNumber(value),
                string value => BuildString(value),
                bool value => BuildBoolean(value),
                DateTime value => BuildDateTime(value),
                null => BuildNull(),
                IEnumerable value => BuildArray(value),
                _ => BuildObject(obj)
            };
    }
}