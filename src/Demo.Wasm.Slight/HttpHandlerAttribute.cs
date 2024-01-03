using System;

namespace Demo.Wasm.Slight
{
    internal class HttpHandlerAttribute: Attribute
    {
        public HttpMethod Method { get; }

        public string Route { get; }

        public HttpHandlerAttribute(HttpMethod method, string route)
        {
            Method = method;
            Route = route;
        }

        public override bool Equals(object? obj) => Equals(obj as HttpHandlerAttribute);

        public bool Equals(HttpHandlerAttribute? attribute)
        {
            if (attribute is null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, attribute))
            {
                return true;
            }

            if (this.GetType() != attribute.GetType())
            {
                return false;
            }

            return (Method == attribute.Method) && (Route == attribute.Route);
        }

        public override int GetHashCode() => (Method, Route).GetHashCode();
    }
}